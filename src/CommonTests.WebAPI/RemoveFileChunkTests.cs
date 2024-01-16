namespace CommonTests.WebAPI;

#region << Using >>

using CRUD.WebAPI;
using Xunit;

#endregion

public class RemoveFileChunkTests : FileChunksStorageTest
{
    #region Constructors

    public RemoveFileChunkTests(IFileChunksUploadStorageService fileChunksUploadStorage) : base(fileChunksUploadStorage) { }

    #endregion

    [Fact]
    public async Task Should_remove_file_chunks()
    {
        var uid = Guid.NewGuid().ToString();

        var content = await Guid.NewGuid().ToString().GetFileAsync();

        await this.FileChunksUploadStorage.AddChunkAsync(uid, 0, content, false, null, CancellationToken.None);

        this.FileChunksUploadStorage.Remove(uid);

        Assert.Empty(this.FileChunksUploadStorage.Statuses);
    }
}