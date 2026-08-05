$root = "D:\Projects\Visual Studio\HJPlatform"

Get-ChildItem "$root\src","$root\tests" -Recurse -Filter "*.csproj" |
Select-String "PackageReference" |
ForEach-Object {
    Write-Host "$($_.Path): $($_.Line.Trim())"
}