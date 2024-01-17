namespace CRUD.WebAPI;

public interface IChunksStorageService
{
    #region Properties

    /// <summary>
    ///     TimeSpan to define max interval between DateTime.UtcNow and
    ///     respective UpDt to consider an Item as expired
    /// </summary>
    public TimeSpan Expiration { get; init; }

    public IReadOnlyDictionary<string, ChunksItemDto> Items { get; }

    #endregion

    /// <summary>
    ///     Removes all Items where (DateTime.UtcNow - UpDt) is less then or equal to Expiration
    /// </summary>
    public void RemoveExpiredItems();

    /// <summary>
    ///     Adds a chunk to the Item by UID and by index
    /// </summary>
    /// <param name="uid">Unique key of the Item</param>
    /// <param name="order">Ordering value of the chunk in the Item</param>
    /// <param name="chunk"></param>
    /// <returns></returns>
    public void AddChunk(string uid, int order, byte[] chunk);

    /// <summary>
    ///     Removes Item by UID
    /// </summary>
    /// <param name="uid"></param>
    public void RemoveItem(string uid);

    /// <summary>
    ///     Returns the Item chunks as single bytes array
    ///     which consisted of ordered by order value chunks and
    ///     then removes it from the Items
    /// </summary>
    /// <param name="uid"></param>
    /// <returns></returns>
    public byte[] PopItem(string uid);
}