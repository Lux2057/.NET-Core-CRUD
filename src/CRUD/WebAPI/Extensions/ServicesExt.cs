namespace CRUD.WebAPI;

#region << Using >>

using Microsoft.Extensions.DependencyInjection;

#endregion

public static class ServicesExt
{
    /// <summary>
    ///     Adds ChunksStorage dependencies
    /// </summary>
    /// <param name="services"></param>
    /// <param name="expiration">
    ///     TimeSpan to define max interval between DateTime.UtcNow and
    ///     respective UpDt to consider chunks collection as expired
    /// </param>
    public static void AddChunksStorage(this IServiceCollection services,
                                        TimeSpan expiration)
    {
        services.AddSingleton<IChunksStorageService>(new ChunksStorageService
                                                     {
                                                             Expiration = expiration
                                                     });
    }
}