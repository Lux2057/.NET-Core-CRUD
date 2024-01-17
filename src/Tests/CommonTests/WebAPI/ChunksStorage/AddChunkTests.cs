namespace CommonTests.WebAPI;

#region << Using >>

using System.Text;
using CRUD.WebAPI;
using Xunit;

#endregion

public class AddChunkTests : ChunksStorageTest
{
    #region Constructors

    public AddChunkTests(IChunksStorageService chunksStorage)
            : base(chunksStorage) { }

    #endregion

    [Fact]
    public Task Should_add_chunks()
    {
        var uid = Guid.NewGuid().ToString();
        var content = Guid.NewGuid().ToString();

        for (var index = 0; index < content.Length; index++)
        {
            var chunk = Encoding.UTF8.GetBytes(content[index].ToString());

            this.ChunksStorage.AddChunk(uid: uid,
                                        order: index,
                                        chunk: chunk);

            Assert.Equal(index + 1, this.ChunksStorage.Items[uid].Chunks.Count);
            Assert.Equal(1, this.ChunksStorage.Items[uid].Chunks[index]);
        }

        this.ChunksStorage.RemoveItem(uid);

        return Task.CompletedTask;
    }

    [Fact]
    public Task Should_throw_exception_for_incorrect_index()
    {
        var uid = Guid.NewGuid().ToString();

        Assert.Throws<IndexOutOfRangeException>(() =>
                                                {
                                                    var content = Encoding.UTF8.GetBytes(Guid.NewGuid().ToString());

                                                    this.ChunksStorage.AddChunk(uid, 0, content);
                                                    this.ChunksStorage.AddChunk(uid, 0, content);
                                                });

        this.ChunksStorage.RemoveItem(uid);

        return Task.CompletedTask;
    }
}