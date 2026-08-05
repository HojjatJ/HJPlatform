$root = "D:\Projects\Visual Studio\HJPlatform"

$folders = @(
    "$root\src\HJ.Server.Infrastructure\Persistence\Entities",
    "$root\src\HJ.Server.Infrastructure\Persistence\Configurations"
)


foreach($folder in $folders)
{
    if(!(Test-Path $folder))
    {
        New-Item -ItemType Directory -Path $folder -Force | Out-Null
    }
}


Write-Host "Persistence folders created."