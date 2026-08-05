$root = "D:\Projects\Visual Studio\HJPlatform"

Write-Host "Starting Optimization model refactor..."

# Rename Domain folder
$oldPath = Join-Path $root "src\HJ.Server.Domain\Optimization"
$newPath = Join-Path $root "src\HJ.Server.Domain\Processing"

if (Test-Path $oldPath) {
    if (-not (Test-Path $newPath)) {
        Rename-Item $oldPath $newPath
    }
}

# Rename entity
$files = @(
    "src\HJ.Server.Domain\Processing\OptimizationBatch.cs",
    "src\HJ.Server.Infrastructure\Persistence\Configurations\OptimizationBatchConfiguration.cs",
    "src\HJ.Server.Infrastructure\Persistence\Migrations\20260804234940_InitialCreate.cs",
    "src\HJ.Server.Infrastructure\Persistence\Migrations\20260804234940_InitialCreate.Designer.cs",
    "src\HJ.Server.Infrastructure\Persistence\Migrations\20260805012316_AddProductManagement.Designer.cs",
    "src\HJ.Server.Infrastructure\Persistence\Migrations\HJDbContextModelSnapshot.cs"
)

foreach ($file in $files) {

    $path = Join-Path $root $file

    if (Test-Path $path) {

        (Get-Content $path -Raw) `
            -replace "OptimizationBatch", "ProcessingJob" `
            -replace "HJ\.Server\.Domain\.Optimization", "HJ.Server.Domain.Processing" `
            | Set-Content $path -Encoding UTF8

        Write-Host "Updated: $file"
    }
}

# Rename configuration file
$configOld = Join-Path $root "src\HJ.Server.Infrastructure\Persistence\Configurations\OptimizationBatchConfiguration.cs"
$configNew = Join-Path $root "src\HJ.Server.Infrastructure\Persistence\Configurations\ProcessingJobConfiguration.cs"

if (Test-Path $configOld) {
    Rename-Item $configOld $configNew
}

Write-Host "Completed."