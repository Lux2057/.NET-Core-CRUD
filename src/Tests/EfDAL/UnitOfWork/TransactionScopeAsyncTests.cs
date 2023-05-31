namespace EfTests.DAL;

#region << Using >>

using System.Data;
using CRUD.DAL;
using Microsoft.EntityFrameworkCore;

#endregion

public class TransactionScopeAsyncTests : EfUnitOfWorkTest
{
    #region Constructors

    public TransactionScopeAsyncTests(IScopedUnitOfWork scopedUnitOfWork, TestDbContext context)
            : base(scopedUnitOfWork, context) { }

    #endregion

    [Fact]
    public void Should_open_and_close_a_transaction_once()
    {
        var transactionId1 = this.ScopedUnitOfWork.BeginTransactionScope(IsolationLevel.ReadCommitted);
        var transactionId2 = this.ScopedUnitOfWork.BeginTransactionScope(IsolationLevel.ReadUncommitted);

        Assert.NotNull(this.context.Database.CurrentTransaction);
        Assert.Equal(transactionId1, this.context.Database.CurrentTransaction.TransactionId.ToString());
        Assert.Equal(string.Empty, transactionId2);

        this.ScopedUnitOfWork.EndTransactionScope();
        this.ScopedUnitOfWork.EndTransactionScope();

        Assert.True(this.context.Database.CurrentTransaction == null);
    }

    [Fact]
    public async Task Should_rollback_changes_in_a_transaction()
    {
        this.ScopedUnitOfWork.BeginTransactionScope(IsolationLevel.ReadCommitted);

        var text = Guid.NewGuid().ToString();

        await this.ScopedUnitOfWork.Repository.AddAsync(new TestEntity { Text = text });

        var entities = await this.ScopedUnitOfWork.Repository.Get<TestEntity>().ToArrayAsync();

        Assert.Single(entities);
        Assert.Equal(text, entities.Single().Text);

        this.ScopedUnitOfWork.RollbackCurrentTransactionScope();

        entities = await this.ScopedUnitOfWork.Repository.Get<TestEntity>().ToArrayAsync();

        Assert.Empty(entities);
    }

    [Fact]
    public async Task Should_ignore_rolling_back_of_a_closed_transaction()
    {
        var transactionId = this.ScopedUnitOfWork.BeginTransactionScope(IsolationLevel.ReadCommitted);

        var text = Guid.NewGuid().ToString();

        await this.ScopedUnitOfWork.Repository.AddAsync(new TestEntity { Text = text });

        this.ScopedUnitOfWork.EndTransactionScope();

        Assert.Null(this.context.Database.CurrentTransaction);

        this.ScopedUnitOfWork.RollbackCurrentTransactionScope();

        var entities = await this.ScopedUnitOfWork.Repository.Get<TestEntity>().ToArrayAsync();

        Assert.Single(entities);
        Assert.Equal(text, entities.Single().Text);
    }
}