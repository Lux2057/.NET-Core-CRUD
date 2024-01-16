namespace CRUD.WebAPI;

public class FileChunksUploadStorageStatusDto
{
    #region Properties

    public string UID { get; set; }

    public DateTime UpdDt { get; set; }

    /// <summary>
    ///     Key: index, Value: length
    /// </summary>
    public Dictionary<int, int> Chunks { get; set; }

    #endregion
}