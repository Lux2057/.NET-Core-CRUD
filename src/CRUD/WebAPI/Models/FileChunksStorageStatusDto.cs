namespace CRUD.WebAPI;

public class FileChunksStorageStatusDto
{
    #region Properties

    public string UID { get; set; }

    public DateTime UpdDt { get; set; }

    public Dictionary<int, int> Chunks { get; set; }

    #endregion
}