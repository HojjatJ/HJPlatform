using Microsoft.EntityFrameworkCore;

namespace HJ.Server.Infrastructure.Persistence;

public class HJServerDbContext : DbContext
{
    public HJServerDbContext(
        DbContextOptions<HJServerDbContext> options)
        : base(options)
    {
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
