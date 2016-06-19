$ErrorActionPreference = 'Stop'

Task default -Depends Compile

Properties {
    $projectName = "toolkit"
    $base_directory = Resolve-Path .
    $build_directory = "$base_directory\build"
    $release_directory = "$build_directory\release"
    $package_directory = "$base_directory\packages"
  
    $build_configuration = "Release"
    $solution_file = "$base_directory\$projectName.sln"

    $lasttag = Invoke-Command -ScriptBlock { git describe --tags --abbrev=0 }
    $version = if ($lasttag -eq $null) { "0.0.0" } else { $lasttag }
}

Task VsVar32 {
    $base_dir = "C:\Program Files"

    if (Test-Path "C:\Program Files (x86)") {
        $base_dir = "C:\Program Files (x86)"
    }
    
    $vs14_dir = "$base_dir\Microsoft Visual Studio 14.0"
    $vs12_dir = "$base_dir\Microsoft Visual Studio 12.0"
    $vs11_dir = "$base_dir\Microsoft Visual Studio 11.0"
    $vsvar32 = "\Common7\Tools\vsvars32.bat"
    
    $batch_file = ""
    
    if (Test-Path "$vs11_dir\$vsvar32") {
        $batch_file = "$vs11_dir\$vsvar32"
    }
    
    if (Test-Path "$vs12_dir\$vsvar32") {
        $batch_file = "$vs12_dir\$vsvar32"
    }
    
    if (Test-Path "$vs14_dir\$vsvar32") {
        $batch_file = "$vs14_dir\$vsvar32"
    }
    
    if ($batch_file) {
        $cmd = "`"$batch_file`" & set"
        cmd /c "$cmd" | Foreach-Object `
        {
            $p, $v = $_.split('=')
            Set-Item -path env:$p -value $v
        }
    } else {
        Write-Warning "Vsvar32.bat was not found!"
    }
}

Task Clean -depends VsVar32 {
    Remove-Item -Force -Recurse $build_directory -ErrorAction SilentlyContinue | Out-Null
    exec { msbuild /m /p:Configuration="$build_configuration" /t:clean "$solution_file" }
}

Task Init -depends Clean {
    New-Item $build_directory -ItemType Directory | Out-Null
    New-Item $release_directory -ItemType Directory | Out-Null
}

Task Version -depends Init {
    if (Test-Path "$build_directory\CommonAssemblyInfo.cs") { 
        return
    }

    Write-Output "MARKING THIS BUILD AS VERSION $version"

    Set-Content $build_directory\CommonAssemblyInfo.cs @"
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: AssemblyVersionAttribute("$version")]
[assembly: AssemblyFileVersionAttribute("$version")]
[assembly: AssemblyInformationalVersionAttribute("$version")]
[assembly: AssemblyCopyrightAttribute("(c) Julian Easterling $(Get-Date -Format "yyyy")")]
[assembly: AssemblyCompanyAttribute("")]
[assembly: AssemblyConfigurationAttribute("$build_configuration")]
"@
}

Task PackageClean {
    Remove-Item -Force -Recurse $package_directory -ErrorAction SilentlyContinue | Out-Null
}

Task PackageRestore -depends Init {
    exec { nuget restore "$solution_file" }

    # In a CI environment, there really isn't any value to the packages' PDB files and it confuses the code coverage task
    Get-ChildItem -Path "$package_directory" -Filter "*.pdb" -Recurse | Remove-Item -Force
}

Task Compile -depends Version, PackageRestore {
    exec { 
        msbuild /m /p:BuildInParralel=true /p:Platform="Any CPU" `
            /p:Configuration="$build_configuration" `
            /p:OutDir="$release_directory"\\ "$solution_file" 
    }
}

Task Test -depends UnitTest

Task xUnit {
    if ((Get-ChildItem -Path $package_directory -Filter "xunit.runner.console.*").Count -eq 0) {
        Push-Location $package_directory
        exec { nuget install xunit.runner.console }
        Pop-Location
    }

    $xunit = Get-ChildItem -Path $package_directory -Filter "xunit.runner.console.*" `
        | select -Last 1 -ExpandProperty FullName
    $global:xunit = "$xunit\tools\xunit.console.exe"
}

Task UnitTest -depends Compile, xUnit {
    if (Test-Path $xunit) {
        exec { 
            & $xunit "$release_directory\UnitTests.dll" -noshadow
        }
    } else {
        Write-Error "xUnit console runner must be available to run tests."
    }
}

Task Coverage -Depends Compile, xUnit {
    if ((Get-ChildItem -Path $package_directory -Filter "OpenCover.*").Count -eq 0) {
        Push-Location $package_directory
        exec { nuget install OpenCover }
        Pop-Location
    }

    if ((Get-ChildItem -Path $package_directory -Filter "ReportGenerator.*").Count -eq 0) {
        Push-Location $package_directory
        exec { nuget install ReportGenerator }
        Pop-Location
    }

    if (-not (Test-Path "$build_directory\coverage")) {
        New-Item -Path "$build_directory\coverage" -ItemType Directory | Out-Null
    }
    
    $OpenCover = Get-ChildItem -Path $package_directory -Filter "OpenCover.*" `
        | select -Last 1 -ExpandProperty FullName
    $OpenCover = "$opencover\tools\opencover.console.exe"
    
    $ReportGenerator = Get-ChildItem -Path $package_directory -Filter "ReportGenerator.*" `
        | select -Last 1 -ExpandProperty FullName
    $ReportGenerator = "$ReportGenerator\tools\ReportGenerator.exe"

    $targetArgs = "`"$release_directory\UnitTests.dll`" -noshadow"
    $filter = "`"+[*]* -[UnitTests]*`""

    exec {
        & $OpenCover -target:$xunit `
                     -targetargs:$targetArgs `
                     -output:"$build_directory\coverage\coverage.xml" `
                     -register:user `
                     -excludebyattribute:"System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute" `
                     -excludebyfile:"*\*Designer.cs;*\*.g.cs;*.*.g.i.cs" `
                     -filter:$filter `
                     -hideskipped:All -skipautoprops -mergebyhash `
                     -returntargetcode
    }

    exec {
        & $ReportGenerator "$build_directory\coverage\coverage.xml" "$build_directory\coverage"
    }
}

Task CI-BuildAndTest -depends Coverage {
	# For CI builds, I want to pass these values to TeamCity for the code coverage
	$coverage = [xml](Get-Content -Path "$build_directory\coverage\coverage.xml")
	$coverageSummary = $coverage.CoverageSession.Summary

	# Write class coverage
	Write-Host "##teamcity[buildStatisticValue key='CodeCoverageAbsCCovered' value='$($coverageSummary.visitedClasses)']"
	Write-Host "##teamcity[buildStatisticValue key='CodeCoverageAbsCTotal' value='$($coverageSummary.numClasses)']"
	Write-Host ("##teamcity[buildStatisticValue key='CodeCoverageC' value='{0:N2}']" `
        -f (($coverageSummary.visitedClasses / $coverageSummary.numClasses)*100))

	# Report method coverage
	Write-Host "##teamcity[buildStatisticValue key='CodeCoverageAbsMCovered' value='$($coverageSummary.visitedMethods)']"
	Write-Host "##teamcity[buildStatisticValue key='CodeCoverageAbsMTotal' value='$($coverageSummary.numMethods)']"
	Write-Host ("##teamcity[buildStatisticValue key='CodeCoverageM' value='{0:N2}']" `
        -f (($coverageSummary.visitedMethods / $coverageSummary.numMethods)*100))
		
	# Report branch coverage
	Write-Host "##teamcity[buildStatisticValue key='CodeCoverageAbsBCovered' value='$($coverageSummary.visitedBranchPoints)']"
	Write-Host "##teamcity[buildStatisticValue key='CodeCoverageAbsBTotal' value='$($coverageSummary.numBranchPoints)']"
	Write-Host "##teamcity[buildStatisticValue key='CodeCoverageB' value='$($coverageSummary.branchCoverage)']"

	# Report statement coverage
	Write-Host "##teamcity[buildStatisticValue key='CodeCoverageAbsSCovered' value='$($coverageSummary.visitedSequencePoints)']"
	Write-Host "##teamcity[buildStatisticValue key='CodeCoverageAbsSTotal' value='$($coverageSummary.numSequencePoints)']"
	Write-Host "##teamcity[buildStatisticValue key='CodeCoverageS' value='$($coverageSummary.sequenceCoverage)']"
}

Task Package -depends Test {
    foreach ($package in (Get-ChildItem -Path $base_directory -Filter "*.nuspec")) {
        exec { 
            nuget.exe pack "$($package.FullName)" -Version $version -o "$build_directory"
        }
    }
}