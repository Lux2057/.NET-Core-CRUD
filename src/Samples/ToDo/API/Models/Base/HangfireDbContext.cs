namespace Samples.ToDo.API;

#region << Using >>

using Hangfire.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

#endregion

public class HangfireDbContext : DbContext
{
    #region Constructors

    public HangfireDbContext(DbContextOptions<HangfireDbContext> options)
            : base(options)
    {
        Database.EnsureDeleted();
        Database.EnsureCreated();
    }

    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.OnHangfireModelCreating();
    }
}