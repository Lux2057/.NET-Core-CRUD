namespace CommonTests.WebAPI;

#region << Using >>

using System.Text;
using CRUD.WebAPI;
using Xunit;

#endregion

public class RemoveItemTests : ChunksStorageTest
{
    #region Constructors

    public RemoveItemTests(IChunksStorageService chunksStorage) : base(chunksStorage) { }

    #endregion

    [Fact]
    public Task Should_remove_item()
    {
        var uid = Guid.NewGuid().ToString();

        var content = Encoding.UTF8.GetBytes(Guid.NewGuid().ToString());

        this.ChunksStorage.AddChunk(uid, 0, content);

        this.ChunksStorage.RemoveItem(uid);

        Assert.Empty(this.ChunksStorage.Items);

        return Task.CompletedTask;
    }

    [Fact]
    public Task Should_throw_key_not_found_exception()
    {
        Assert.Throws<KeyNotFoundException>(() => this.ChunksStorage.RemoveItem(Guid.NewGuid().ToString()));

        return Task.CompletedTask;
    }
}