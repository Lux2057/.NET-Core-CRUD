namespace WebAPI.API;

#region << Using >>

using CRUD.DAL.EntityFramework;
using CRUD.Logging.EntityFramework;
using Microsoft.EntityFrameworkCore;
using WebAPI.API.Entities;

#endregion

public sealed class ApiDbContext : DbContext, IEfDbContext
{
    #region Constructors

    public ApiDbContext(DbContextOptions<ApiDbContext> options)
            : base(options)
    {
        Database.EnsureCreated();
    }

    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(Program).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LogMapping).Assembly);
    }
}