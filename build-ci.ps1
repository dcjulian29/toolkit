[System.Net.ServicePointManager]::SecurityProtocol = 3072 -bor 768 -bor 192 -bor 48

$TOOLS_DIR = Join-Path $PSScriptRoot "tools"
$NUGET_EXE = Join-Path $TOOLS_DIR "nuget.exe"

if ((Test-Path $PSScriptRoot) -and (-not (Test-Path $TOOLS_DIR))) {
    Write-Output "Creating tools directory..."
    New-Item -Path $TOOLS_DIR -Type Directory | Out-Null
}

if (-not (Test-Path $NUGET_EXE)) {
    Write-Output "Trying to find nuget.exe in PATH..."
    $existingPaths = $Env:Path -Split ';' `
        | Where-Object { (![string]::IsNullOrEmpty($_)) -and (Test-Path $_ -PathType Container) }
    $NUGET_EXE_IN_PATH = Get-ChildItem -Path $existingPaths -Filter "nuget.exe" | Select -First 1
    if (($NUGET_EXE_IN_PATH -ne $null) -and (Test-Path $NUGET_EXE_IN_PATH.FullName)) {
        Write-Output "Found in PATH at $($NUGET_EXE_IN_PATH.FullName)."
        $NUGET_EXE = $NUGET_EXE_IN_PATH.FullName
    }
}

if (-not (Test-Path $NUGET_EXE)) {
    Write-Output "Downloading NuGet.exe..."
    Invoke-WebRequest -Uri $NUGET_URL -OutFile $NUGET_EXE
}

$ENV:NUGET_EXE = $NUGET_EXE

dotnet tool restore
dotnet cake

exit $LASTEXITCODE
