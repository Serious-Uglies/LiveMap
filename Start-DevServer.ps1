param (
  [string] $Branch = "OpenBeta",
  [string] $WriteDir = "DCS.server"
)

$StableRegistryKey = "HKCU:\Software\Eagle Dynamics\DCS World"
$OpenBetaRegistryKey = "HKCU:\Software\Eagle Dynamics\DCS World OpenBeta"

if ($Branch -ne "OpenBeta" -and $Branch -ne "Stable") {
  Write-Host -ForegroundColor Red "Unknown branch '$Branch'."
  exit 1
}

$InstallPath = "" 

if ($Branch -eq "OpenBeta") {

  if (-not (Test-Path $OpenBetaRegistryKey)) {
    Write-Host -ForegroundColor Red "Could not locate the installation directory of the DCS World OpenBeta."
    exit 1
  }

  $InstallPath = Get-ItemPropertyValue $OpenBetaRegistryKey -Name "Path"
}

if ($Branch -eq "Stable" ) {

  if (-not (Test-Path $StableRegistryKey)) {
    Write-Host -ForegroundColor Red "Could not locate the installation directory of the DCS World Stable."
    exit 1
  }

  $InstallPath = Get-ItemPropertyValue $StableRegistryKey -Name "Path"
}

$DcsExecutablePath = Join-Path $InstallPath "bin\DCS.exe"

if (-not (Test-Path $DcsExecutablePath)) {
    Write-Host -ForegroundColor Red "Could not find the path '$DcsExecutablePath'."
    exit 1
}

Write-Host "Launching DCS World $Branch in dedicated server mode (write dir: $WriteDir)."
Write-Host

& $DcsExecutablePath --server --norender -w $WriteDir