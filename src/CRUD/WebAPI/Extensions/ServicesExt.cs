namespace CRUD.WebAPI;

#region << Using >>

using Microsoft.Extensions.DependencyInjection;

#endregion

public static class ServicesExt
{
    /// <summary>
    ///     Adds FileChunksStorage dependencies
    /// </summary>
    /// <param name="services"></param>
    /// <param name="expiration">TimeSpan to define max interval between chunks uploads of a single file</param>
    public static void AddFileChunksStorage(this IServiceCollection services, TimeSpan expiration)
    {
        services.AddSingleton<IFileChunksStorageService>(new FileChunksStorageService
                                                         {
                                                                 Expiration = expiration
                                                         });
    }
}