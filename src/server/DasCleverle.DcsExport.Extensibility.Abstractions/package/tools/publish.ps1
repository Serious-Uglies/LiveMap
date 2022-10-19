param (
  [string] $Configuration,
  [string] $BinSourcePath,
  [string] $OutputPath
)

Add-Type -Path "Tomlyn.dll"

$file = Get-Item $Configuration
$binSource = Join-Path $file.Directory $BinSourcePath
$assetSource = Join-Path $file.Directory "assets"

$assetDest = Join-Path $OutputPath "assets"

$content = Get-Content $file -Raw
$toml = [Tomlyn.Toml]::ToModel($content)

$entryAssembly = $toml['entry_assembly']
$dependencies = $toml['dependencies']
$assets = $toml['assets']

function Copy-ExtensionFile {
  param (
    [string] $Path,
    [string] $BasePath = $file.Directory,
    [string] $Destination = $OutputPath
  )

  $FullDestinationPath = (Join-Path $Destination $Path)

  if (-not (Test-Path $FullDestinationPath)) {
    New-Item $FullDestinationPath -Force -ItemType "File" | Out-Null
  }

  Copy-Item -Path (Join-Path $BasePath $Path) -Destination $FullDestinationPath -Recurse -Force
  Write-Host "Published file '$Path'"
}

if ($null -eq $entryAssembly) {
  Write-Host "Required field 'entry_assembly' is missing from extension configuration"
  exit 1
}

if (Test-Path $OutputPath) {
  Remove-Item $OutputPath -Recurse -Force | Out-Null
}

New-Item $OutputPath -ItemType "Directory" | Out-Null

Copy-ExtensionFile -Path $file.Name
Copy-ExtensionFile -Path "$entryAssembly.dll" -BasePath $binSource
Copy-ExtensionFile -Path "$entryAssembly.pdb" -BasePath $binSource


if ($null -ne $dependencies) {
  foreach ($dependency in $dependencies) {
    Copy-ExtensionFile -Path "$dependency.dll" -BasePath $binSource
    Copy-ExtensionFile -Path "$dependency.pdb" -BasePath $binSource
  }
}

if ($null -ne $assets) {
  if (-not (Test-Path $assetDest)) {
    New-Item $assetDest -ItemType "Directory" | Out-Null
  }

  foreach ($asset in $assets) {
    Copy-ExtensionFile -Path (Join-Path "assets" $asset)
  }
}