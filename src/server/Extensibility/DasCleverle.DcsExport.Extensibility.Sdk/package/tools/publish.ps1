param (
  [string] $Configuration,
  [string] $BinSourcePath,
  [string] $OutputPath
)

$configFile = Get-Item $Configuration

function Copy-ExtensionFile {
  param (
    [string] $Path,
    [string] $BasePath = $configFile.Directory,
    [string] $Destination = $OutputPath
  )

  $FullDestinationPath = (Join-Path $Destination $Path)

  if (-not (Test-Path $FullDestinationPath)) {
    New-Item $FullDestinationPath -Force -ItemType "File" | Out-Null
  }

  Copy-Item -Path (Join-Path $BasePath $Path) -Destination $FullDestinationPath -Recurse -Force
  Write-Host "Published file '$Path'"
}

Add-Type -Path "Tomlyn.dll"

$content = Get-Content $configFile -Raw
$toml = [Tomlyn.Toml]::ToModel($content)

$entryAssembly = $toml['entry_assembly']
$dependencies = $toml['dependencies']
$assets = $toml['assets']
$lua = $toml['lua']

if ($null -eq $entryAssembly) {
  Write-Host "Required field 'entry_assembly' is missing from extension configuration"
  exit 1
}

if (Test-Path $OutputPath) {
  Remove-Item $OutputPath -Recurse -Force | Out-Null
}

$binSource = Join-Path $configFile.Directory $BinSourcePath

Copy-ExtensionFile -Path $configFile.Name
Copy-ExtensionFile -Path "$entryAssembly.dll" -BasePath $binSource
Copy-ExtensionFile -Path "$entryAssembly.pdb" -BasePath $binSource

if ($null -ne $dependencies) {
  foreach ($dependency in $dependencies) {
    Copy-ExtensionFile -Path "$dependency.dll" -BasePath $binSource
    Copy-ExtensionFile -Path "$dependency.pdb" -BasePath $binSource
  }
}

if ($null -ne $assets) {
  foreach ($asset in $assets) {
    Copy-ExtensionFile -Path (Join-Path "assets" $asset)
  }
}

if ($null -ne $lua) {
  foreach ($script in $lua) {
    Copy-ExtensionFile -Path (Join-Path "lua" $script)
  }
}