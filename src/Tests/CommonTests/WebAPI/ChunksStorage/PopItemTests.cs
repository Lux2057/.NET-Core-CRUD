namespace CommonTests.WebAPI;

#region << Using >>

using System.Text;
using CRUD.WebAPI;
using Xunit;

#endregion

public class PopItemTests : ChunksStorageTest
{
    #region Constructors

    public PopItemTests(IChunksStorageService chunksStorage) : base(chunksStorage) { }

    #endregion

    [Fact]
    public Task Should_pop_item()
    {
        var uid = Guid.NewGuid().ToString();
        var content = Guid.NewGuid().ToString();

        for (var index = 0; index < content.Length; index++)
        {
            var chunk = Encoding.UTF8.GetBytes(content[index].ToString());

            this.ChunksStorage.AddChunk(uid: uid,
                                        order: index,
                                        chunk: chunk);
        }

        var item = this.ChunksStorage.PopItem(uid);

        Assert.Empty(this.ChunksStorage.Items);
        Assert.Equal(content, Encoding.UTF8.GetString(item));

        return Task.CompletedTask;
    }

    [Fact]
    public Task Should_throw_key_not_found_exception()
    {
        Assert.Throws<KeyNotFoundException>(() => this.ChunksStorage.PopItem(Guid.NewGuid().ToString()));

        return Task.CompletedTask;
    }
}