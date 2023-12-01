namespace Linq2DbTests.DAL;

#region << Using >>

using System.Data;
using CRUD.DAL.Abstractions;
using CRUD.DAL.Linq2Db;
using Linq2DbTests.Shared;
using LinqToDB;

#endregion

public class TransactionScopeTests : UnitOfWorkTest
{
    #region Constructors

    public TransactionScopeTests(IUnitOfWork unitOfWork,
                                 ILinq2DbRepository repository,
                                 TestDataConnection connection)
            : base(unitOfWork, repository, connection) { }

    #endregion

    [Fact]
    public void Should_open_and_close_a_transaction_scope_once()
    {
        this.Connection.TryCreateTable<TestEntity>(nameof(TestEntity), true);

        this.UnitOfWork.OpenTransaction(IsolationLevel.ReadCommitted);
        var scope1Id = this.UnitOfWork.OpenedTransactionId;
        this.UnitOfWork.OpenTransaction(IsolationLevel.ReadUncommitted);
        var scope2Id = this.UnitOfWork.OpenedTransactionId;

        Assert.NotNull(this.Connection.Transaction);
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
        this.Connection.TryCreateTable<TestEntity>(nameof(TestEntity), true);

        this.UnitOfWork.OpenTransaction(IsolationLevel.ReadCommitted);

        var text = Guid.NewGuid().ToString();

        await Repository.CreateAsync(new TestEntity { Text = text });

        var entities = Repository.Read<TestEntity>().ToArray();

        Assert.Single(entities);
        Assert.Equal(text, entities.Single().Text);

        this.UnitOfWork.RollbackTransaction();

        entities = Repository.Read<TestEntity>().ToArray();

        Assert.Empty(entities);
    }

    [Fact]
    public async Task Should_ignore_rolling_back_of_a_closed_transaction()
    {
        this.Connection.TryCreateTable<TestEntity>(nameof(TestEntity), true);

        this.UnitOfWork.OpenTransaction(IsolationLevel.ReadCommitted);

        var text = Guid.NewGuid().ToString();

        await Repository.CreateAsync(new TestEntity { Text = text });

        this.UnitOfWork.CloseTransaction();

        Assert.False(this.UnitOfWork.IsTransactionOpened);

        this.UnitOfWork.RollbackTransaction();

        var entities = Repository.Read<TestEntity>().ToArray();

        Assert.Single(entities);
        Assert.Equal(text, entities.Single().Text);
    }

    [Fact]
    public async Task Should_execute_several_transactions()
    {
        this.Connection.TryCreateTable<TestEntity>(nameof(TestEntity), true);

        var text1 = Guid.NewGuid().ToString();
        var text2 = Guid.NewGuid().ToString();

        this.UnitOfWork.OpenTransaction(IsolationLevel.ReadCommitted);

        var entity = new TestEntity { Text = text1 };
        await Repository.CreateAsync(entity);

        entity.Text = text2;

        await Repository.UpdateAsync(entity);

        this.UnitOfWork.CloseTransaction();

        var entitiesInDb = await this.Connection.GetTable<TestEntity>().ToArrayAsync();

        Assert.Single(entitiesInDb);
        Assert.Equal(text2, entitiesInDb[0].Text);

        this.UnitOfWork.OpenTransaction(IsolationLevel.ReadCommitted);

        await Repository.CreateAsync(new TestEntity { Text = text2 });

        entity.Text = text1;

        await Repository.UpdateAsync(entity);

        this.UnitOfWork.RollbackTransaction();

        entitiesInDb = await this.Connection.GetTable<TestEntity>().ToArrayAsync();

        Assert.Single(entitiesInDb);
        Assert.Equal(text2, entitiesInDb[0].Text);

        this.UnitOfWork.OpenTransaction(IsolationLevel.ReadCommitted);

        entitiesInDb = await this.Connection.GetTable<TestEntity>().ToArrayAsync();

        entitiesInDb[0].Text = text1;

        await Repository.UpdateAsync(entitiesInDb);

        await Repository.CreateAsync(new TestEntity { Text = text1 });

        this.UnitOfWork.CloseTransaction();

        entitiesInDb = await this.Connection.GetTable<TestEntity>().ToArrayAsync();

        Assert.Equal(2, entitiesInDb.Length);
        Assert.True(entitiesInDb.All(x => x.Text == text1));
    }
}