namespace NhTests.Logging;

#region << Using >>

using CRUD.CQRS;
using CRUD.Logging.Common;
using JetBrains.Annotations;

#endregion

public class GetLogsQuery : QueryBase<LogEntity[]>
{
    #region Nested Classes

    [UsedImplicitly]
    public class Handler : QueryHandlerBase<GetLogsQuery, LogEntity[]>
    {
        #region Constructors

        public Handler(IServiceProvider serviceProvider) : base(serviceProvider) { }

        #endregion

        protected override async Task<LogEntity[]> Execute(GetLogsQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(Repository.Get<LogEntity>().ToArray());
        }
    }

    #endregion
}