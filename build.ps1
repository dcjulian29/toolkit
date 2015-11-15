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
        exec { & $xunit "$release_directory\UnitTests.dll" }
    } else {
        Write-Error "xUnit console runner must be available to run tests."
    }
}

Task Package -depends Test {
    foreach ($package in (Get-ChildItem -Path $base_directory -Filter "*.nuspec")) {
        exec { 
            nuget.exe pack "$($package.FullName)" -Version $version -o "$build_directory"
        }
    }
}