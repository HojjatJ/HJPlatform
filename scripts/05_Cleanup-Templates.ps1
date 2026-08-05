$root = "D:\Projects\Visual Studio\HJPlatform"

$files = @(
    "$root\src\HJ.Server.Application\Class1.cs",
    "$root\src\HJ.Server.Domain\Class1.cs",
    "$root\src\HJ.Server.Infrastructure\Class1.cs",
    "$root\src\HJ.Server.Contracts\Class1.cs",
    "$root\src\HJ.Server.Foundation\Class1.cs",
    "$root\src\HJ.Server.SDK\Class1.cs"
)

foreach ($file in $files) {

    if (Test-Path $file) {

        Remove-Item $file -Force

        Write-Host "Removed: $file"
    }
    else {

        Write-Host "Not found: $file"
    }
}


Write-Host ""
Write-Host "Template cleanup completed."