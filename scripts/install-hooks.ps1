$ErrorActionPreference = 'Stop'
$root = Split-Path -Parent $PSScriptRoot
Push-Location $root
try {
    git config core.hooksPath .githooks
    Write-Host "core.hooksPath = .githooks (ok)"
    git update-index --chmod=+x .githooks/pre-commit 2>$null
    Write-Host "Hooks installed. Encoding check active for *.cs / *.axaml"
}
finally {
    Pop-Location
}
