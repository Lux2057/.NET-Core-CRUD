﻿namespace EfTests.CQRS;

#region << Using >>

using CRUD.CQRS;
using EfTests.Shared;
using FluentValidation;

#endregion

public class ReadDispatcherTests : ReadDispatcherTest
{
    #region Constructors

    public ReadDispatcherTests(TestDbContext context, IReadDispatcher dispatcher)
            : base(context, dispatcher) { }

    #endregion

    [Fact]
    public async Task Should_return_entities_from_db()
    {
        var text = Guid.NewGuid().ToString();
        await this.context.Set<TestEntity>().AddRangeAsync(new[]
                                                           {
                                                                   new TestEntity
                                                                   {
                                                                           Text = text
                                                                   },
                                                                   new TestEntity
                                                                   {
                                                                           Text = text
                                                                   },
                                                                   new TestEntity
                                                                   {
                                                                           Text = text
                                                                   }
                                                           });

        await this.context.SaveChangesAsync();

        var dtos = await this.dispatcher.QueryAsync(new GetTestEntitiesByIdsQueryBase(Array.Empty<int>()));

        Assert.Equal(3, dtos.Length);
        Assert.True(dtos.All(x => x.Text == text));
    }

    [Fact]
    public async Task Should_throw_validation_exception()
    {
        await Assert.ThrowsAsync<ValidationException>(async () => await this.dispatcher.QueryAsync(new GetTestEntitiesByIdsQueryBase(null)));
    }

    [Fact]
    public async Task Should_throw_test_exception()
    {
        await Assert.ThrowsAsync<Exception>(async () => await this.dispatcher.QueryAsync(new TestThrowingExceptionQueryBase()));
    }
}