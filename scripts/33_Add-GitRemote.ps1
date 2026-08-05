$root = "D:\Projects\Visual Studio\HJPlatform"

Set-Location $root


$remote = "https://github.com/HojjatJ/HJPlatform"


git remote add origin $remote


git remote -v


Write-Host "Remote added."