namespace NhTests.DAL;

#region << Using >>

using System.Data;
using CRUD.DAL.Abstractions;
using NHibernate;
using NhTests.Shared;

#endregion

public class TransactionScopeTests : EfUnitOfWorkTest
{
    #region Properties

    private ISessionFactory SessionFactory { get; }

    #endregion

    #region Constructors

    public TransactionScopeTests(IUnitOfWork unitOfWork, ISession session, ISessionFactory sessionFactory)
            : base(unitOfWork, session)
    {
        SessionFactory = sessionFactory;
    }

    #endregion

    [Fact]
    public void Should_open_and_close_a_transaction_scope_once()
    {
        this.UnitOfWork.OpenTransaction(IsolationLevel.ReadCommitted);
        var scope1Id = this.UnitOfWork.OpenedTransactionId;
        this.UnitOfWork.OpenTransaction(IsolationLevel.ReadUncommitted);
        var scope2Id = this.UnitOfWork.OpenedTransactionId;

        Assert.NotNull(this.session.GetCurrentTransaction());
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

        var entities = this.UnitOfWork.Repository.Get<TestEntity>().ToArray();

        Assert.Single(entities);
        Assert.Equal(text, entities.Single().Text);

        this.UnitOfWork.RollbackTransaction();

        entities = this.UnitOfWork.Repository.Get<TestEntity>().ToArray();

        Assert.Empty(entities);
    }

    [Fact]
    public async Task Should_ignore_rolling_back_of_a_closed_transaction()
    {
        this.UnitOfWork.OpenTransaction(IsolationLevel.ReadCommitted);

        var text = Guid.NewGuid().ToString();

        await this.UnitOfWork.Repository.AddAsync(new TestEntity { Text = text });

        this.UnitOfWork.CloseTransaction();

        Assert.Null(this.session.GetCurrentTransaction());

        this.UnitOfWork.RollbackTransaction();

        var entities = this.UnitOfWork.Repository.Get<TestEntity>().ToArray();

        Assert.Single(entities);
        Assert.Equal(text, entities.Single().Text);
    }

    [Fact]
    public async Task Should_execute_several_transactions()
    {
        var text1 = Guid.NewGuid().ToString();
        var text2 = Guid.NewGuid().ToString();

        this.UnitOfWork.OpenTransaction(IsolationLevel.ReadCommitted);

        var entity = new TestEntity { Text = text1 };
        await this.UnitOfWork.Repository.AddAsync(entity);

        entity.Text = text2;

        await this.UnitOfWork.Repository.UpdateAsync(entity);

        this.UnitOfWork.CloseTransaction();

        var entitiesInDb = await SessionFactory.GetEntitiesAsync<TestEntity>();

        Assert.Single(entitiesInDb);
        Assert.Equal(text2, entitiesInDb[0].Text);

        this.UnitOfWork.OpenTransaction(IsolationLevel.ReadCommitted);

        await this.UnitOfWork.Repository.AddAsync(new TestEntity { Text = text2 });

        entity.Text = text1;

        await this.UnitOfWork.Repository.UpdateAsync(entity);

        this.UnitOfWork.RollbackTransaction();

        entitiesInDb = await SessionFactory.GetEntitiesAsync<TestEntity>();

        Assert.Single(entitiesInDb);
        Assert.Equal(text2, entitiesInDb[0].Text);

        this.UnitOfWork.OpenTransaction(IsolationLevel.ReadCommitted);

        entitiesInDb = await SessionFactory.GetEntitiesAsync<TestEntity>();

        entitiesInDb[0].Text = text1;

        await this.UnitOfWork.Repository.UpdateAsync(entitiesInDb);

        await this.UnitOfWork.Repository.AddAsync(new TestEntity { Text = text1 });

        this.UnitOfWork.CloseTransaction();

        entitiesInDb = await SessionFactory.GetEntitiesAsync<TestEntity>();

        Assert.Equal(2, entitiesInDb.Length);
        Assert.True(entitiesInDb.All(x => x.Text == text1));
    }
}