# build.ps1

# Set script directory
$PSScriptRoot = Split-Path -Parent $MyInvocation.MyCommand.Definition
Set-Location $PSScriptRoot

# Download Cake bootstrapper if missing
if (-Not (Test-Path "./tools")) {
    mkdir tools | Out-Null
}

if (-Not (Test-Path "./build.cake")) {
    Write-Host "Error: build.cake not found." -ForegroundColor Red
    exit 1
}

# Download Cake tool if not available
if (-Not (Test-Path "./tools/cake/Cake.dll")) {
    Write-Host "Downloading Cake..."
    dotnet tool install Cake.Tool --tool-path ./tools/cake
}

# Run Cake script
Write-Host "Running build.cake..." -ForegroundColor Cyan
./tools/cake/dotnet-cake build.cake

# Run AppBackend
Write-Host "Starting AppBackend..." -ForegroundColor Green
Push-Location "./AppBackend"
dotnet run
Pop-Location

# Run AppFrontend
Write-Host "Starting AppFrontend..." -ForegroundColor Green
Push-Location "./AppFrontend"
npm run dev
Pop-Location
