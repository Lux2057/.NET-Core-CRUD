namespace Linq2Db.CQRS.Specific;

#region << Using >>

using CRUD.CQRS;
using JetBrains.Annotations;
using Linq2DbTests.Shared;

#endregion

public class GetTestEntitiesTestQuery : QueryBase<TestEntity[]>
{
    #region Properties

    public string TableName { get; }

    #endregion

    #region Constructors

    public GetTestEntitiesTestQuery(string tableName)
    {
        TableName = tableName;
    }

    #endregion

    #region Nested Classes

    [UsedImplicitly]
    class Handler : CRUD.CQRS.Linq2Db.QueryHandlerBase<GetTestEntitiesTestQuery, TestEntity[]>
    {
        #region Constructors

        public Handler(IServiceProvider serviceProvider) : base(serviceProvider) { }

        #endregion

        protected override async Task<TestEntity[]> Execute(GetTestEntitiesTestQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(Repository.Read<TestEntity>(tableName: request.TableName).ToArray());
        }
    }

    #endregion
}