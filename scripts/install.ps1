param(
	$Url,
	$Name
)

Add-Type -Assembly "System.IO.Compression.FileSystem"

if (Test-Path "$PWD/$Name" -PathType Container) {
	Write-Host "-- $Name already installed"
	return
}

$cache = "$PWD/.download-cache"
if (!(Test-Path $cache -PathType Container)) {
	New-Item -ItemType Directory $cache | Out-Null
}

$fileName = [System.IO.Path]::GetFileName($Url);
if (!(Test-Path "$cache/$fileName" -PathType Leaf)) {
	Write-Host "-- Downloading $Name [$fileName]..."
	Invoke-WebRequest $Url -Method "GET" -Outfile "$cache/$fileName"
} else {
	Write-Host "-- $Name [$fileName] loaded from cache"
}

$extractedDirName = ([System.IO.Compression.ZipFile]::OpenRead("$cache/$fileName").Entries.FullName)[0]

Write-Host "-- Extracting $Name [$fileName]..."
Expand-Archive "$cache/$fileName" -DestinationPath $PWD -Force
Rename-Item $extractedDirName $Name -ErrorAction SilentlyContinue
