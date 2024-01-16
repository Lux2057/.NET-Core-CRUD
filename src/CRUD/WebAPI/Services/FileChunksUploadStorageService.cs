namespace CRUD.WebAPI;

#region << Using >>

using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;

#endregion

public class FileChunksUploadStorageService : IFileChunksUploadStorageService
{
    #region Properties

    public TimeSpan Expiration { get; init; } = TimeSpan.FromMinutes(30);

    public FileChunksUploadStorageStatusDto[] Statuses =>
            this._storageDict
                .Select(r => new FileChunksUploadStorageStatusDto
                             {
                                     UID = r.Key,
                                     UpdDt = r.Value.UpdDt,
                                     Chunks = r.Value.Chunks.ToDictionary(x => x.Key, x => x.Value.Length)
                             }).ToArray();

    private readonly ConcurrentDictionary<string, ChunkStorage> _storageDict = new();

    #endregion

    #region Nested Classes

    class ChunkStorage
    {
        #region Properties

        public DateTime UpdDt { get; set; }

        public readonly ConcurrentDictionary<int, byte[]> Chunks = new();

        #endregion
    }

    #endregion

    #region Interface Implementations

    public void CleanExpired()
    {
        foreach (var storage in this._storageDict.Where(r => r.Value.UpdDt + Expiration < DateTime.UtcNow))
            this._storageDict.Remove(storage.Key, out _);
    }

    public async Task AddChunkAsync(string uid,
                                    int index,
                                    IFormFile chunk,
                                    bool isLast,
                                    Action<byte[]> uploadCompletedCallback,
                                    CancellationToken cancellationToken)
    {
        byte[] data;
        await using (var inStream = chunk.OpenReadStream())
        {
            data = new byte[chunk.Length];
            var bs = await inStream.ReadAsync(data, 0, data.Length, cancellationToken);
        }

        ChunkStorage storage;
        if (this._storageDict.ContainsKey(uid))
        {
            storage = this._storageDict[uid];
        }
        else
        {
            storage = new ChunkStorage();
            this._storageDict.GetOrAdd(uid, storage);
        }

        if (storage.Chunks.ContainsKey(index))
            throw new IndexOutOfRangeException($"Chunk #{uid} already has entry with index {index}");

        storage.UpdDt = DateTime.UtcNow;
        storage.Chunks.GetOrAdd(index, data);

        if (!isLast)
            return;

        var file = storage.Chunks.OrderBy(r => r.Key).SelectMany(c => c.Value).ToArray();

        this._storageDict.Remove(uid, out storage);

        uploadCompletedCallback?.Invoke(file);
    }

    public void Remove(string uid)
    {
        if (!this._storageDict.ContainsKey(uid))
            return;

        this._storageDict.Remove(uid, out var val);
    }

    #endregion
}