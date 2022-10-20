function Install {
	param(
		$Url,
		$Name
	)

	$cache = "$PSScriptRoot/.download-cache"
	if (![System.IO.Directory]::Exists($cache)) {
		New-Item -ItemType Directory $cache | Out-Null
	}

	if (![System.IO.File]::Exists("$cache/$Name")) {
		Write-Host "Downloading $Name ..."
		Invoke-WebRequest $Url -Method "GET" -Outfile $cache/$Name
	}

	Write-Host "Extracting $Name ..."
	Expand-Archive $cache/$Name -DestinationPath $PSScriptRoot
}

Install -Name "SDL-release-2.24.1.zip" -Url "https://github.com/libsdl-org/SDL/archive/refs/tags/release-2.24.1.zip"
