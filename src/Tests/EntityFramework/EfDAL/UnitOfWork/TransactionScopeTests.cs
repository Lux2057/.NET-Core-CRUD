namespace EfTests.DAL;

#region << Using >>

using System.Data;
using CRUD.DAL.Abstractions;
using Microsoft.EntityFrameworkCore;

#endregion

public class TransactionScopeTests : EfUnitOfWorkTest
{
    #region Constructors

    public TransactionScopeTests(IScopedUnitOfWork scopedUnitOfWork, TestDbContext context)
            : base(scopedUnitOfWork, context) { }

    #endregion

    [Fact]
    public void Should_open_and_close_a_transaction_scope_once()
    {
        this.ScopedUnitOfWork.OpenScope(IsolationLevel.ReadCommitted);
        var scope1Id = this.ScopedUnitOfWork.OpenedScopeId;
        this.ScopedUnitOfWork.OpenScope(IsolationLevel.ReadUncommitted);
        var scope2Id = this.ScopedUnitOfWork.OpenedScopeId;

        Assert.NotNull(this.context.Database.CurrentTransaction);
        Assert.Equal(scope1Id, scope2Id);

        this.ScopedUnitOfWork.CloseScope();
        scope1Id = this.ScopedUnitOfWork.OpenedScopeId;
        this.ScopedUnitOfWork.CloseScope();
        scope2Id = this.ScopedUnitOfWork.OpenedScopeId;

        Assert.False(this.ScopedUnitOfWork.IsOpened);
        Assert.Equal(scope1Id, scope2Id);
    }

    [Fact]
    public async Task Should_rollback_changes_in_a_transaction()
    {
        this.ScopedUnitOfWork.OpenScope(IsolationLevel.ReadCommitted);

        var text = Guid.NewGuid().ToString();

        await this.ScopedUnitOfWork.Repository.AddAsync(new TestEntity { Text = text });

        var entities = await this.ScopedUnitOfWork.Repository.Get<TestEntity>().ToArrayAsync();

        Assert.Single(entities);
        Assert.Equal(text, entities.Single().Text);

        this.ScopedUnitOfWork.RollbackAndCloseScope();

        entities = await this.ScopedUnitOfWork.Repository.Get<TestEntity>().ToArrayAsync();

        Assert.Empty(entities);
    }

    [Fact]
    public async Task Should_ignore_rolling_back_of_a_closed_transaction()
    {
        this.ScopedUnitOfWork.OpenScope(IsolationLevel.ReadCommitted);

        var text = Guid.NewGuid().ToString();

        await this.ScopedUnitOfWork.Repository.AddAsync(new TestEntity { Text = text });

        this.ScopedUnitOfWork.CloseScope();

        Assert.Null(this.context.Database.CurrentTransaction);

        this.ScopedUnitOfWork.RollbackAndCloseScope();

        var entities = await this.ScopedUnitOfWork.Repository.Get<TestEntity>().ToArrayAsync();

        Assert.Single(entities);
        Assert.Equal(text, entities.Single().Text);
    }
}