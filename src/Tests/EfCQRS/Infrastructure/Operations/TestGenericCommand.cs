﻿namespace EfTests.CQRS;

#region << Using >>

using CRUD.CQRS;

#endregion

public class TestGenericCommand<T> : CommandBase where T : TestEntity
{
    #region Constants

    public const string TestText = "TEST GENERIC COMMAND";

    #endregion

    #region Nested Classes

    public class Handler : CommandHandlerBase<TestGenericCommand<T>>
    {
        #region Constructors

        public Handler(IServiceProvider serviceProvider) : base(serviceProvider) { }

        #endregion

        protected override async Task Execute(TestGenericCommand<T> command, CancellationToken cancellationToken)
        {
            await Repository<TestEntity>().AddAsync(new TestEntity
                                                    {
                                                            Text = TestText
                                                    }, cancellationToken);
        }
    }

    #endregion
}