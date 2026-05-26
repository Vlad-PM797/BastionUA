param(
    [Parameter(Mandatory = $true)]
    [string]$ExePath,

    [Parameter(Mandatory = $true)]
    [string]$OutputPath,

    [int]$Width = 1280,
    [int]$Height = 720,
    [int]$TimeoutSeconds = 90
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

if (-not (Test-Path $ExePath)) {
    throw "Executable not found: $ExePath"
}

$outputDirectory = Split-Path -Parent $OutputPath
if ($outputDirectory) {
    New-Item -ItemType Directory -Path $outputDirectory -Force | Out-Null
}

$launchArgs = @(
    "-screen-fullscreen", "0",
    "-screen-width", "$Width",
    "-screen-height", "$Height",
    "-bastionScreenshotPath", $OutputPath
)

$process = Start-Process -FilePath $ExePath -ArgumentList $launchArgs -PassThru
try {
    Wait-Process -Id $process.Id -Timeout $TimeoutSeconds
}
catch {
    if (-not $process.HasExited) {
        Stop-Process -Id $process.Id -Force -ErrorAction SilentlyContinue
    }

    throw
}

if (-not (Test-Path $OutputPath)) {
    throw "Screenshot file was not created: $OutputPath"
}

$fileInfo = Get-Item $OutputPath
if ($fileInfo.Length -lt 50000) {
    throw "Screenshot file looks too small ($($fileInfo.Length) bytes): $OutputPath"
}

Write-Output $OutputPath
