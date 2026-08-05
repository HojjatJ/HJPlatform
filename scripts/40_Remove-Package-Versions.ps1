$root = "D:\Projects\Visual Studio\HJPlatform"

$projects = Get-ChildItem "$root\src","$root\tests" -Recurse -Filter "*.csproj"

foreach ($project in $projects) {

    $content = Get-Content $project.FullName -Raw

    $content = $content -replace ' Version="[^"]+"', ''

    Set-Content $project.FullName $content -Encoding UTF8

    Write-Host "Updated: $($project.FullName)"
}

Write-Host "Package versions removed from projects."