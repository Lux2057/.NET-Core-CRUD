namespace EfTests.DAL;

#region << Using >>

using System.Data;
using CRUD.DAL.Abstractions;
using Microsoft.EntityFrameworkCore;

#endregion

public class TransactionScopeTests : EfUnitOfWorkTest
{
    #region Constructors

    public TransactionScopeTests(IUnitOfWork unitOfWork, TestDbContext context)
            : base(unitOfWork, context) { }

    #endregion

    [Fact]
    public void Should_open_and_close_a_transaction_scope_once()
    {
        this.UnitOfWork.OpenTransaction(IsolationLevel.ReadCommitted);
        var scope1Id = this.UnitOfWork.OpenedTransactionId;
        this.UnitOfWork.OpenTransaction(IsolationLevel.ReadUncommitted);
        var scope2Id = this.UnitOfWork.OpenedTransactionId;

        Assert.NotNull(this.context.Database.CurrentTransaction);
        Assert.Equal(scope1Id, scope2Id);

        this.UnitOfWork.CloseTransaction();
        scope1Id = this.UnitOfWork.OpenedTransactionId;
        this.UnitOfWork.CloseTransaction();
        scope2Id = this.UnitOfWork.OpenedTransactionId;

        Assert.False(this.UnitOfWork.IsTransactionOpened);
        Assert.Equal(scope1Id, scope2Id);
    }

    [Fact]
    public async Task Should_rollback_changes_in_a_transaction()
    {
        this.UnitOfWork.OpenTransaction(IsolationLevel.ReadCommitted);

        var text = Guid.NewGuid().ToString();

        await this.UnitOfWork.Repository.AddAsync(new TestEntity { Text = text });

        var entities = await this.UnitOfWork.Repository.Get<TestEntity>().ToArrayAsync();

        Assert.Single(entities);
        Assert.Equal(text, entities.Single().Text);

        this.UnitOfWork.RollbackTransaction();

        entities = await this.UnitOfWork.Repository.Get<TestEntity>().ToArrayAsync();

        Assert.Empty(entities);
    }

    [Fact]
    public async Task Should_ignore_rolling_back_of_a_closed_transaction()
    {
        this.UnitOfWork.OpenTransaction(IsolationLevel.ReadCommitted);

        var text = Guid.NewGuid().ToString();

        await this.UnitOfWork.Repository.AddAsync(new TestEntity { Text = text });

        this.UnitOfWork.CloseTransaction();

        Assert.Null(this.context.Database.CurrentTransaction);

        this.UnitOfWork.RollbackTransaction();

        var entities = await this.UnitOfWork.Repository.Get<TestEntity>().ToArrayAsync();

        Assert.Single(entities);
        Assert.Equal(text, entities.Single().Text);
    }
}