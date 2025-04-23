# build.ps1

# -----------------Build‑cake stuff-----------------

$PSScriptRoot = Split-Path -Parent $MyInvocation.MyCommand.Definition
Set-Location $PSScriptRoot

if (-not (Test-Path ./tools)) { mkdir tools | Out-Null }

if (-not (Test-Path ./build.cake)) {
    Write-Host "Error: build.cake not found." -ForegroundColor Red
    exit 1
}

if (-not (Test-Path ./tools/cake/Cake.dll)) {
    Write-Host "Downloading Cake..."
    dotnet tool install Cake.Tool --tool-path ./tools/cake
}

Write-Host "Running build.cake..." -ForegroundColor Cyan
./tools/cake/dotnet-cake build.cake
