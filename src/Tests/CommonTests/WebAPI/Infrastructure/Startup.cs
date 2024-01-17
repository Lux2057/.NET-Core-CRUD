namespace CommonTests.WebAPI;

#region << Using >>

using CRUD.WebAPI;
using Microsoft.Extensions.DependencyInjection;

#endregion

public class Startup
{
    #region Constants

    public static TimeSpan ChunksStorageExpiration = TimeSpan.FromMilliseconds(100);

    #endregion

    public virtual void ConfigureServices(IServiceCollection services)
    {
        services.AddChunksStorage(ChunksStorageExpiration);
    }
}