namespace Linq2Db.CQRS.Specific;

#region << Using >>

using CRUD.CQRS;
using FluentValidation;
using JetBrains.Annotations;
using Linq2DbTests.Shared;

#endregion

internal class GetTestEntitiesByIdsQueryBase : QueryBase<TestEntityDto[]>
{
    #region Properties

    public string[] Ids { get; }

    public string TableName { get; }

    #endregion

    #region Constructors

    public GetTestEntitiesByIdsQueryBase(IEnumerable<string> ids, string tableName)
    {
        TableName = tableName;
        Ids = ids?.ToArray();
    }

    #endregion

    #region Nested Classes

    [UsedImplicitly]
    class Validator : AbstractValidator<GetTestEntitiesByIdsQueryBase>
    {
        #region Constructors

        public Validator()
        {
            RuleFor(r => r.Ids).NotNull();
        }

        #endregion
    }

    [UsedImplicitly]
    class Handler : CRUD.CQRS.Linq2Db.QueryHandlerBase<GetTestEntitiesByIdsQueryBase, TestEntityDto[]>
    {
        #region Constructors

        public Handler(IServiceProvider serviceProvider) : base(serviceProvider) { }

        #endregion

        protected override async Task<TestEntityDto[]> Execute(GetTestEntitiesByIdsQueryBase request, CancellationToken cancellationToken)
        {
            var hasIds = request.Ids!.Any();

            var entities = Repository.Read<TestEntity>(tableName: request.TableName)
                                     .Where(r => !hasIds || request.Ids!.Contains(r.Id)).ToArray();

            return await Task.FromResult(Mapper.Map<TestEntityDto[]>(entities));
        }
    }

    #endregion
}