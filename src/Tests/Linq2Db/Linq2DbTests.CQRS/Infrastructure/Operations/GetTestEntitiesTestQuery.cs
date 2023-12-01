namespace Linq2Db.CQRS;

#region << Using >>

using CRUD.CQRS;
using JetBrains.Annotations;
using Linq2DbTests.Shared;

#endregion

public class GetTestEntitiesTestQuery : QueryBase<TestEntity[]>
{
    #region Nested Classes

    [UsedImplicitly]
    class Handler : QueryHandlerBase<GetTestEntitiesTestQuery, TestEntity[]>
    {
        #region Constructors

        public Handler(IServiceProvider serviceProvider) : base(serviceProvider) { }

        #endregion

        protected override async Task<TestEntity[]> Execute(GetTestEntitiesTestQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(Repository.Read<TestEntity>().ToArray());
        }
    }

    #endregion
}