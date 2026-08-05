$root = "D:\Projects\Visual Studio\HJPlatform"

$folders = @(
    "$root\src",
    "$root\tests",
    "$root\docs",
    "$root\tools",
    "$root\src\HJ.Server.Api",
    "$root\src\HJ.Server.Application",
    "$root\src\HJ.Server.Domain",
    "$root\src\HJ.Server.Infrastructure",
    "$root\src\HJ.Server.Contracts",
    "$root\src\HJ.Server.Foundation"
)

foreach ($folder in $folders) {
    if (!(Test-Path $folder)) {
        New-Item -ItemType Directory -Path $folder | Out-Null
        Write-Host "Created: $folder"
    }
    else {
        Write-Host "Exists:  $folder"
    }
}

Write-Host ""
Write-Host "HJPlatform folder structure created successfully."