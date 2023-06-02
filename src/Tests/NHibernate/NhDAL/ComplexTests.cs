namespace NhTests.DAL;

#region << Using >>

using System.Data;
using CRUD.DAL.Abstractions;
using NHibernate;

#endregion

public class ComplexTests : DbTest
{
    #region Properties

    private IUnitOfWork UnitOfWork { get; }

    ISessionFactory SessionFactory { get; }

    #endregion

    #region Constructors

    public ComplexTests(IUnitOfWork unitOfWork, ISessionFactory sessionFactory)
    {
        UnitOfWork = unitOfWork;
        SessionFactory = sessionFactory;
    }

    #endregion

    [Fact]
    public async Task Should()
    {
        throw new NotImplementedException();

        var text1 = Guid.NewGuid().ToString();
        var text2 = Guid.NewGuid().ToString();

        UnitOfWork.OpenTransaction(IsolationLevel.ReadCommitted);

        var entity = new TestEntity { Text = text1 };
        await UnitOfWork.Repository.AddAsync(entity);

        entity.Text = text2;

        await UnitOfWork.Repository.UpdateAsync(entity);

        UnitOfWork.CloseTransaction();

        TestEntity[] entitiesInDb;
        using (var session = SessionFactory.OpenSession())
        {
            entitiesInDb = session.Query<TestEntity>().ToArray();
        }

        Assert.Single(entitiesInDb);
        Assert.Equal(text2, entitiesInDb[0].Text);

        UnitOfWork.OpenTransaction(IsolationLevel.ReadCommitted);

        entity = UnitOfWork.Repository.Get<TestEntity>().Single();

        entity.Text = text1;

        await UnitOfWork.Repository.UpdateAsync(entity);

        UnitOfWork.RollbackTransaction();
    }
}