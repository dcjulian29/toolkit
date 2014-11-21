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

  $build_number = Invoke-Command -ScriptBlock { git rev-list HEAD --count }
  $version = "$(Get-Date -Format 'yyyy.MM.dd').$build_number"
}

Task Clean {
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

Task Compile -depends Version {
    exec { nuget restore "$solution_file" }
    exec { 
        msbuild /m /p:BuildInParralel=true /p:Platform="Any CPU" `
            /p:Configuration="$build_configuration" `
            /p:OutDir="$release_directory"\\ "$solution_file" 
    }
}

Task Test -depends UnitTest

Task UnitTest -depends Compile {
    if ((Get-ChildItem -Path $package_directory -Filter "xunit.runners.*").Count -eq 0) {
        Push-Location $package_directory
        exec { nuget install xunit.runners }
        Pop-Location
    }

    $xunit = Get-ChildItem -Path $package_directory -Filter "xunit.runners.*" `
        | select -Last 1 -ExpandProperty FullName
    $xunit = "$xunit\tools\xunit.console.clr4.exe"

    if (Test-Path $xunit) {
        exec { & $xunit "$release_directory\UnitTests.dll" }
    } else {
        Write-Error "xUnit console runner must be available to run tests."
    }
}

Task Package -depends Test {
    exec { nuget pack ToolKit.nuspec -o "$build_directory" }
}