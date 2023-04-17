namespace EfTests.DAL;

#region << Using >>

using System.Data;
using CRUD.DAL;
using Microsoft.EntityFrameworkCore;
using Tests.Models;

#endregion

public class EfTransactionScopeAsyncTests : EfUnitOfWorkTest
{
    #region Constructors

    public EfTransactionScopeAsyncTests(IUnitOfWork unitOfWork, TestDbContext context)
            : base(unitOfWork, context) { }

    #endregion

    [Fact]
    public async Task Should_open_and_close_a_transaction_once()
    {
        var transactionId1 = await this.unitOfWork.BeginTransactionScopeAsync(IsolationLevel.ReadCommitted);
        var transactionId2 = await this.unitOfWork.BeginTransactionScopeAsync(IsolationLevel.ReadUncommitted);

        Assert.NotNull(this.context.Database.CurrentTransaction);
        Assert.Equal(transactionId1, this.context.Database.CurrentTransaction.TransactionId.ToString());
        Assert.Equal(string.Empty, transactionId2);

        await this.unitOfWork.EndTransactionScopeAsync(transactionId1);
        await this.unitOfWork.EndTransactionScopeAsync(transactionId2);

        Assert.True(this.context.Database.CurrentTransaction == null);
    }

    [Fact]
    public async Task Should_rollback_changes_in_a_transaction()
    {
        await this.unitOfWork.BeginTransactionScopeAsync(IsolationLevel.ReadCommitted);

        var text = Guid.NewGuid().ToString();

        await this.unitOfWork.ReadWriteRepository<TestEntity>().AddAsync(new TestEntity { Text = text });

        var entities = await this.unitOfWork.ReadRepository<TestEntity>().Get().ToArrayAsync();

        Assert.Single(entities);
        Assert.Equal(text, entities.Single().Text);

        await this.unitOfWork.RollbackCurrentTransactionScopeAsync();

        entities = await this.unitOfWork.ReadRepository<TestEntity>().Get().ToArrayAsync();

        Assert.Empty(entities);
    }

    [Fact]
    public async Task Should_ignore_rolling_back_of_a_closed_transaction()
    {
        var transactionId = await this.unitOfWork.BeginTransactionScopeAsync(IsolationLevel.ReadCommitted);

        var text = Guid.NewGuid().ToString();

        await this.unitOfWork.ReadWriteRepository<TestEntity>().AddAsync(new TestEntity { Text = text });

        await this.unitOfWork.EndTransactionScopeAsync(transactionId);

        Assert.Null(this.context.Database.CurrentTransaction);

        await this.unitOfWork.RollbackCurrentTransactionScopeAsync();

        var entities = await this.unitOfWork.ReadRepository<TestEntity>().Get().ToArrayAsync();

        Assert.Single(entities);
        Assert.Equal(text, entities.Single().Text);
    }
}