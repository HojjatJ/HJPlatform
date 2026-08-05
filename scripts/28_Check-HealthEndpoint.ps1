$root = "D:\Projects\Visual Studio\HJPlatform"

Write-Host "Searching health endpoint..."

Get-ChildItem "$root\src" -Recurse -Include "*.cs" |
Select-String "Health" |
ForEach-Object {
    Write-Host "$($_.Path):$($_.Line)"
}