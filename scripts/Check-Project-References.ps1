$projects = Get-ChildItem -Path . -Filter *.csproj -Recurse

foreach ($project in $projects) {
    Write-Host ""
    Write-Host "=============================="
    Write-Host $project.FullName
    Write-Host "=============================="

    Select-String -Path $project.FullName `
        -Pattern "<ProjectReference|<PackageReference"
}