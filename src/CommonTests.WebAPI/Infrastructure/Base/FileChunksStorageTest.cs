namespace CommonTests.WebAPI;

#region << Using >>

using CRUD.WebAPI;

#endregion

public abstract class FileChunksStorageTest : TestBase
{
    #region Properties

    protected readonly IFileChunksUploadStorageService FileChunksUploadStorage;

    #endregion

    #region Constructors

    protected FileChunksStorageTest(IFileChunksUploadStorageService fileChunksUploadStorage)
    {
        this.FileChunksUploadStorage = fileChunksUploadStorage;
    }

    #endregion
}