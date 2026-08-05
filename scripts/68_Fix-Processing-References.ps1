$root = "D:\Projects\Visual Studio\HJPlatform"

$file = Join-Path $root "src\HJ.Server.Infrastructure\Persistence\HJDbContext.cs"

(Get-Content $file -Raw) `
    -replace "using HJ\.Server\.Domain\.Optimization;", "using HJ.Server.Domain.Processing;" `
    -replace "DbSet<OptimizationBatch>", "DbSet<ProcessingJob>" `
    -replace "OptimizationBatch", "ProcessingJob" `
    | Set-Content $file -Encoding UTF8

Write-Host "HJDbContext updated."