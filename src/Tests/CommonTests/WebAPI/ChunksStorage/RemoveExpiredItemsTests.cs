namespace CommonTests.WebAPI;

#region << Using >>

using System.Text;
using CRUD.WebAPI;
using Xunit;

#endregion

public class RemoveExpiredItemsTests : ChunksStorageTest
{
    #region Constructors

    public RemoveExpiredItemsTests(IChunksStorageService chunksStorage) : base(chunksStorage) { }

    #endregion

    [Fact]
    public Task Should_clean_expired_items()
    {
        var uid = Guid.NewGuid().ToString();

        var content = Encoding.UTF8.GetBytes(Guid.NewGuid().ToString());

        this.ChunksStorage.AddChunk(uid, 0, content);

        Thread.Sleep(Startup.ChunksStorageExpiration);

        this.ChunksStorage.RemoveExpiredItems();

        Assert.Empty(this.ChunksStorage.Items);

        return Task.CompletedTask;
    }
}