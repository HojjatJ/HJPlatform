$root = "D:\Projects\Visual Studio\HJPlatform"

$folder = "$root\src\HJ.Server.Infrastructure\Persistence"


if(!(Test-Path $folder))
{
    New-Item -ItemType Directory -Path $folder -Force | Out-Null
}


@"
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;


namespace HJ.Server.Infrastructure.Persistence;


public class HJDbContextFactory : IDesignTimeDbContextFactory<HJDbContext>
{
    public HJDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<HJDbContext>();

        optionsBuilder.UseNpgsql(
            "Host=localhost;Port=5432;Database=hjplatform;Username=postgres;Password=postgres");


        return new HJDbContext(optionsBuilder.Options);
    }
}
"@ | Set-Content `
"$folder\HJDbContextFactory.cs" `
-Encoding UTF8


Write-Host "DesignTime factory created."