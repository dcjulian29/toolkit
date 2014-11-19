@powershell.exe -NoProfile -ExecutionPolicy Bypass -Command "& {Invoke-Psake .\build.ps1 %1; if ($psake.build_success) { exit 0 } else { exit 1 } }"
