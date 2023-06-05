namespace NhTests.CQRS;

#region << Using >>

using CRUD.CQRS;
using FluentValidation;
using NHibernate;
using NhTests.Shared;

#endregion

public class ReadDispatcherTests : ReadDispatcherTest
{
    #region Constructors

    public ReadDispatcherTests(ISessionFactory sessionFactory, IReadDispatcher dispatcher)
            : base(sessionFactory, dispatcher) { }

    #endregion

    [Fact]
    public async Task Should_return_entities_from_db()
    {
        var text = Guid.NewGuid().ToString();
        await SessionFactory.AddEntitiesAsync(new[]
                                              {
                                                      new TestEntity { Text = text },
                                                      new TestEntity { Text = text },
                                                      new TestEntity { Text = text }
                                              });

        var dtos = await Dispatcher.QueryAsync(new GetTestEntitiesByIdsQueryBase(Array.Empty<int>()));

        Assert.Equal(3, dtos.Length);
        Assert.True(dtos.All(x => x.Text == text));
    }

    [Fact]
    public async Task Should_throw_validation_exception()
    {
        await Assert.ThrowsAsync<ValidationException>(async () => await Dispatcher.QueryAsync(new GetTestEntitiesByIdsQueryBase(null)));
    }

    [Fact]
    public async Task Should_throw_test_exception()
    {
        await Assert.ThrowsAsync<Exception>(async () => await Dispatcher.QueryAsync(new TestThrowingExceptionQueryBase()));
    }
}