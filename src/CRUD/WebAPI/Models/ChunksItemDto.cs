namespace CRUD.WebAPI;

public record ChunksItemDto
{
    #region Properties

    /// <summary>
    ///     Last update DateTime
    /// </summary>
    public DateTime UpdDt { get; init; }

    /// <summary>
    ///     Key: index, Value: length
    /// </summary>
    public IReadOnlyDictionary<int, int> Chunks { get; init; }

    #endregion
}