﻿namespace EfTests.Logging;

#region << Using >>

using CRUD.DAL.EntityFramework;
using CRUD.Logging.EntityFramework;
using Microsoft.EntityFrameworkCore;

#endregion

public sealed class TestDbContext : DbContext, IEfDbContext
{
    #region Constructors

    public TestDbContext(DbContextOptions<TestDbContext> options)
            : base(options)
    {
        Database.EnsureDeleted();
        Database.EnsureCreated();
    }

    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LogMapping).Assembly);
    }
}