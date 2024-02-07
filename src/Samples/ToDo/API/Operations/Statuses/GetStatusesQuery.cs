namespace Samples.ToDo.API;

#region << Using >>

using AutoMapper.QueryableExtensions;
using CRUD.CQRS;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Samples.ToDo.Shared;

#endregion

public class GetStatusesQuery : QueryBase<StatusDto[]>
{
    #region Properties

    public int UserId { get; }

    public string SearchTerm { get; }

    #endregion

    #region Constructors

    public GetStatusesQuery(int userId,
                            string searchTerm)
    {
        UserId = userId;
        SearchTerm = searchTerm?.Trim();
    }

    #endregion

    #region Nested Classes

    [UsedImplicitly]
    class Handler : QueryHandlerBase<GetStatusesQuery, StatusDto[]>
    {
        #region Constructors

        public Handler(IServiceProvider serviceProvider) : base(serviceProvider) { }

        #endregion

        protected override async Task<StatusDto[]> Execute(GetStatusesQuery request, CancellationToken cancellationToken)
        {
            return await Repository.Read(new IsDeletedProp.FindBy.EqualTo<StatusEntity>(false) &&
                                         new UserIdProp.FindBy.EqualTo<StatusEntity>(request.UserId) &&
                                         new NameProp.FindBy.ContainedTerm<StatusEntity>(request.SearchTerm))
                                   .ProjectTo<StatusDto>(Mapper.ConfigurationProvider)
                                   .ToArrayAsync(cancellationToken);
        }
    }

    #endregion
}