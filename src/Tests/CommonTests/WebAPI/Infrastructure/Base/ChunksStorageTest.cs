namespace CommonTests.WebAPI;

#region << Using >>

using CRUD.WebAPI;

#endregion

public abstract class ChunksStorageTest : TestBase
{
    #region Properties

    protected readonly IChunksStorageService ChunksStorage;

    #endregion

    #region Constructors

    protected ChunksStorageTest(IChunksStorageService chunksStorage)
    {
        this.ChunksStorage = chunksStorage;
    }

    #endregion
}