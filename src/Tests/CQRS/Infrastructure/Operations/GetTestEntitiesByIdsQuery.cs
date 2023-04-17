namespace Tests.CQRS;

#region << Using >>

using CRUD.CQRS;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Tests.Models;

#endregion

public class GetTestEntitiesByIdsQuery : IQuery<TestEntityDto[]>
{
    #region Properties

    public int[] Ids { get; init; }

    #endregion

    #region Nested Classes

    public class Validator : AbstractValidator<GetTestEntitiesByIdsQuery>
    {
        #region Constructors

        public Validator()
        {
            RuleFor(r => r.Ids).NotNull();
        }

        #endregion
    }

    class Handler : QueryHandlerBase<GetTestEntitiesByIdsQuery, TestEntityDto[]>
    {
        #region Constructors

        public Handler(IServiceProvider serviceProvider) : base(serviceProvider) { }

        #endregion

        protected override async Task<TestEntityDto[]> Execute(GetTestEntitiesByIdsQuery request, CancellationToken cancellationToken)
        {
            var hasIds = request?.Ids?.Any() == true;

            var entities = await Repository<TestEntity>().Get().Where(r => !hasIds || request.Ids.Contains(r.Id)).ToArrayAsync(cancellationToken);

            return this.Mapper.Map<TestEntityDto[]>(entities);
        }
    }

    #endregion
}