namespace CommonTests.WebAPI;

#region << Using >>

using CRUD.WebAPI;
using Xunit;

#endregion

public class CleanExpiredFileUploadsTests : FileChunksStorageTest
{
    #region Constructors

    public CleanExpiredFileUploadsTests(IFileChunksUploadStorageService fileChunksUploadStorage) : base(fileChunksUploadStorage) { }

    #endregion

    [Fact]
    public async Task Should_clean_expired()
    {
        var uid = Guid.NewGuid().ToString();

        var content = await Guid.NewGuid().ToString().GetFileAsync();

        await this.FileChunksUploadStorage.AddChunkAsync(uid, 0, content, false, null, CancellationToken.None);

        Thread.Sleep(Startup.FileChunksStorageExpiration);

        this.FileChunksUploadStorage.CleanExpired();

        Assert.Empty(this.FileChunksUploadStorage.Statuses);
    }
}