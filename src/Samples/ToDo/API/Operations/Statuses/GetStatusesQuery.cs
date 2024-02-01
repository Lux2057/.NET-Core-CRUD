namespace Samples.ToDo.API;

#region << Using >>

using AutoMapper.QueryableExtensions;
using CRUD.CQRS;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

#endregion

public class GetStatusesQuery : QueryBase<StatusDto[]>
{
    #region Properties

    public int UserId { get; }

    #endregion

    #region Constructors

    public GetStatusesQuery(int userId)
    {
        UserId = userId;
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
            return await Repository.Read(new UserIdProp.FindBy.EqualTo<StatusEntity>(request.UserId))
                                   .ProjectTo<StatusDto>(Mapper.ConfigurationProvider)
                                   .ToArrayAsync(cancellationToken);
        }
    }

    #endregion
}