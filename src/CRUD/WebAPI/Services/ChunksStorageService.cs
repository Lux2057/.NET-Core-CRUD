namespace CRUD.WebAPI;

#region << Using >>

using System.Collections.Concurrent;

#endregion

public class ChunksStorageService : IChunksStorageService
{
    #region Properties

    public TimeSpan Expiration { get; init; } = TimeSpan.FromMinutes(30);

    public IReadOnlyDictionary<string, ChunksItemDto> Items =>
            this._storageDict
                .ToDictionary(r => r.Key,
                              r => new ChunksItemDto
                                   {
                                           UpdDt = r.Value.UpdDt,
                                           Chunks = r.Value.Chunks
                                                     .OrderBy(x => x.Key)
                                                     .ToDictionary(x => x.Key, x => x.Value.Length)
                                   });

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

    public void RemoveExpiredItems()
    {
        foreach (var storage in this._storageDict.Where(r => r.Value.UpdDt + Expiration < DateTime.UtcNow))
            this._storageDict.Remove(storage.Key, out _);
    }

    public void AddChunk(string uid,
                         int order,
                         byte[] chunk)
    {
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

        if (storage.Chunks.ContainsKey(order))
            throw new IndexOutOfRangeException($"Item #{uid} already has a chunk with index #{order}");

        storage.UpdDt = DateTime.UtcNow;
        storage.Chunks.GetOrAdd(order, chunk);
    }

    public void RemoveItem(string uid)
    {
        if (!this._storageDict.ContainsKey(uid))
            throw new KeyNotFoundException();

        this._storageDict.Remove(uid, out var val);
    }

    public byte[] PopItem(string uid)
    {
        if (!this._storageDict.ContainsKey(uid))
            throw new KeyNotFoundException();

        var item = this._storageDict[uid].Chunks.OrderBy(r => r.Key).SelectMany(c => c.Value).ToArray();

        this._storageDict.Remove(uid, out _);

        return item;
    }

    #endregion
}