$root = "D:\Projects\Visual Studio\HJPlatform"

$migrationsPath = "$root\src\HJ.Server.Infrastructure\Persistence\Migrations"


if(Test-Path $migrationsPath)
{
    Get-ChildItem $migrationsPath | Select-Object Name
}
else
{
    Write-Host "Migration folder not found."
}