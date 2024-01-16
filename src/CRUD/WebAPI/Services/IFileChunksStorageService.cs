namespace CRUD.WebAPI;

#region << Using >>

using Microsoft.AspNetCore.Http;

#endregion

/// <summary>
///     A service to support split to chunks files upload
/// </summary>
public interface IFileChunksStorageService
{
    #region Properties

    public TimeSpan Expiration { get; init; }

    public FileChunksStorageStatusDto[] Statuses { get; }

    #endregion

    public void CleanExpired();

    public Task AddChunkAsync(string uid,
                              int index,
                              IFormFile chunk,
                              bool isLast,
                              Action<byte[]> uploadCompletedCallback,
                              CancellationToken cancellationToken);
}