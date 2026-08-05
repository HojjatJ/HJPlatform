$root = "D:\Projects\Visual Studio\HJPlatform"

$file = "$root\src\HJ.Server.Api\Program.cs"


$content = Get-Content $file -Raw


# اضافه کردن using

if (!$content.Contains("using HJ.Server.Infrastructure.DependencyInjection;"))
{
    $content = 
"using HJ.Server.Infrastructure.DependencyInjection;
" + $content
}


# اضافه کردن ثبت سرویس

$target = @"
builder.Services
    .AddFastEndpoints()
    .SwaggerDocument();
"@


$replacement = @"
builder.Services
    .AddFastEndpoints()
    .SwaggerDocument();


// Infrastructure

builder.Services.AddHJInfrastructure(
    builder.Configuration);
"@


$content = $content.Replace(
    $target,
    $replacement
)


Set-Content `
    -Path $file `
    -Value $content `
    -Encoding UTF8



# اضافه کردن ConnectionString

$appsettings = "$root\src\HJ.Server.Api\appsettings.json"


if (Test-Path $appsettings)
{
    $json = Get-Content $appsettings -Raw | ConvertFrom-Json

    if ($null -eq $json.ConnectionStrings)
    {
        $json | Add-Member -MemberType NoteProperty -Name ConnectionStrings -Value @{
            Default = "Host=localhost;Database=HJPlatform;Username=postgres;Password=postgres"
        }
    }

    $json | ConvertTo-Json -Depth 10 |
        Set-Content $appsettings -Encoding UTF8
}


Write-Host "Infrastructure connected."