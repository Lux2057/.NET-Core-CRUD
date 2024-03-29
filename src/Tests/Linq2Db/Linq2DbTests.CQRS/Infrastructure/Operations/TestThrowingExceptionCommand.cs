﻿namespace Linq2Db.CQRS;

#region << Using >>

using CRUD.CQRS;

#endregion

#region << Using >>

#endregion

internal class TestThrowingExceptionCommand : CommandBase
{
    #region Nested Classes

    class Handler : CommandHandlerBase<TestThrowingExceptionCommand>
    {
        #region Constructors

        public Handler(IServiceProvider serviceProvider) : base(serviceProvider) { }

        #endregion

        protected override Task Execute(TestThrowingExceptionCommand command, CancellationToken cancellationToken)
        {
            throw new Exception("Test exception");
        }
    }

    #endregion
}