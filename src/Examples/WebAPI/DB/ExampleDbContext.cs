namespace Examples.WebAPI
{
    #region << Using >>

    using CRUD.DAL.EntityFramework;
    using CRUD.Logging.EntityFramework;
    using Microsoft.EntityFrameworkCore;

    #endregion

    public sealed class ExampleDbContext : DbContext, IEfDbContext
    {
        #region Constructors

        public ExampleDbContext(DbContextOptions<ExampleDbContext> options)
                : base(options)
        {
            Database.EnsureCreated();
        }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ExampleEntity).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(LogMapping).Assembly);
        }
    }
}