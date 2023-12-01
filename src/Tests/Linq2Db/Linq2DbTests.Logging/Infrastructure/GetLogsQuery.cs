namespace Linq2DbTests.Logging;

#region << Using >>

using CRUD.CQRS;
using CRUD.Logging.Linq2Db;
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
            return await Task.FromResult(Repository.Read<LogEntity>().ToArray());
        }
    }

    #endregion
}