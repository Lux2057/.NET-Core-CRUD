namespace CommonTests.WebAPI;

#region << Using >>

using CRUD.WebAPI;
using Xunit;

#endregion

public class AddFileChunksToStorageTests : FileChunksStorageTest
{
    #region Constructors

    public AddFileChunksToStorageTests(IFileChunksUploadStorageService fileChunksUploadStorage)
            : base(fileChunksUploadStorage) { }

    #endregion

    [Fact]
    public async Task Should_upload_a_file()
    {
        var uid = Guid.NewGuid().ToString();
        var content = Guid.NewGuid().ToString();

        var resultBytes = Array.Empty<byte>();
        for (var index = 0; index < content.Length; index++)
        {
            var chunk = await content[index].ToString().GetFileAsync();

            var isLast = index == content.Length - 1;
            await this.FileChunksUploadStorage.AddChunkAsync(uid: uid,
                                                       index: index,
                                                       chunk: chunk,
                                                       isLast: isLast,
                                                       file => resultBytes = file,
                                                       CancellationToken.None);

            if (isLast)
            {
                Assert.Empty(this.FileChunksUploadStorage.Statuses);
            }
            else
            {
                Assert.Single(this.FileChunksUploadStorage.Statuses);
                Assert.Equal(index + 1, this.FileChunksUploadStorage.Statuses[0].Chunks.Count);
            }
        }

        var result = System.Text.Encoding.UTF8.GetString(resultBytes);

        Assert.Equal(content, result);
    }

    [Fact]
    public async Task Should_throw_exception_for_incorrect_index()
    {
        var uid = Guid.NewGuid().ToString();

        await Assert.ThrowsAsync<IndexOutOfRangeException>(async () =>
                                                           {
                                                               var content = await Guid.NewGuid().ToString().GetFileAsync();

                                                               await this.FileChunksUploadStorage.AddChunkAsync(uid, 0, content, false, null, CancellationToken.None);
                                                               await this.FileChunksUploadStorage.AddChunkAsync(uid, 0, content, false, null, CancellationToken.None);
                                                           });

        this.FileChunksUploadStorage.Remove(uid);

        await Assert.ThrowsAsync<IndexOutOfRangeException>(async () =>
                                                           {
                                                               var content = await Guid.NewGuid().ToString().GetFileAsync();

                                                               await this.FileChunksUploadStorage.AddChunkAsync(uid, 0, content, false, null, CancellationToken.None);
                                                               await this.FileChunksUploadStorage.AddChunkAsync(uid, 0, content, true, null, CancellationToken.None);
                                                           });

        this.FileChunksUploadStorage.Remove(uid);
    }
}