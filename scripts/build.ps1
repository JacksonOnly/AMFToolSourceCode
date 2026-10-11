param([ValidateSet('Release','Debug')][string]$Configuration='Release')
$ErrorActionPreference='Stop'
$root=Split-Path -Path $PSScriptRoot -Parent
Push-Location -LiteralPath $root
try {
    $project=Join-Path $root 'src\AMFTool.csproj'
    $logDir=Join-Path $root 'logs\build'
    New-Item -ItemType Directory -Path $logDir -Force | Out-Null
    $log=Join-Path $logDir ('build-'+$Configuration.ToLowerInvariant()+'.log')
    & dotnet build $project -c $Configuration -t:Rebuild -v minimal *> $log
    $result=$LASTEXITCODE
    Get-Content -LiteralPath $log -Tail 6
    if($result -ne 0){throw "Compilation failed (exit $result). See $log"}
    $exe=Join-Path $root ('artifacts\bin\'+$Configuration+'\net48\AMFTool.exe')
    if(-not(Test-Path -LiteralPath $exe)){throw 'Output executable is missing'}
    Write-Output "BUILD_OK: $exe"
} finally { Pop-Location }
