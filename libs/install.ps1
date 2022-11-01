function Install {
	param(
		$Url,
		$Name
	)

	if (Test-Path $Name -PathType Container) {
		Write-Host "$Name already installed"
		return
	}

	$cache = "$PSScriptRoot/.download-cache"
	if (!(Test-Path $cache -PathType Container)) {
		New-Item -ItemType Directory $cache | Out-Null
	}

	$fileName = [System.IO.Path]::GetFileName($Url);
	if (!(Test-Path "$cache/$Name" -PathType Leaf)) {
		Write-Host "Downloading $Name [$fileName]..."
		Invoke-WebRequest $Url -Method "GET" -Outfile "$cache/$fileName"
	} else {
		Write-Host "$Name [$fileName] loaded from cache"
	}

	Write-Host "Extracting $Name [$fileName]..."
	Expand-Archive "$cache/$fileName" -DestinationPath $PSScriptRoot -Force
}

Install -Name "SDL-release-2.24.1" -Url "https://github.com/libsdl-org/SDL/archive/refs/tags/release-2.24.1.zip"
Install -Name "glm" -Url "https://github.com/g-truc/glm/releases/download/0.9.9.8/glm-0.9.9.8.zip"
Install -Name "geGL-master" -Url "https://github.com/dormon/geGL/archive/refs/heads/master.zip"
Install -Name "CLI11-2.3.1" -Url "https://github.com/CLIUtils/CLI11/archive/refs/tags/v2.3.1.zip" 
