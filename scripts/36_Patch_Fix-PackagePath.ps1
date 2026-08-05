$root = "D:\Projects\Visual Studio\HJPlatform"

$file = "$root\scripts\36_Create-Central-Package-Version.ps1"

$content = Get-Content $file -Raw

$content = $content.Replace(
    'D:\Projects\ Visual Studio\HJPlatform',
    'D:\Projects\Visual Studio\HJPlatform'
)

Set-Content $file $content

Write-Host "Package path fixed."