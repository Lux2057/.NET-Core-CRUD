namespace Samples.ToDo.API;

#region << Using >>

using AutoMapper.QueryableExtensions;
using CRUD.CQRS;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

#endregion

public class GetTagsQuery : QueryBase<TagDto[]>
{
    #region Properties

    public string SearchTerm { get; }

    #endregion

    #region Constructors

    public GetTagsQuery(string searchTerm)
    {
        SearchTerm = searchTerm.Trim().ToLower();
    }

    #endregion

    #region Nested Classes

    [UsedImplicitly]
    class Handler : QueryHandlerBase<GetTagsQuery, TagDto[]>
    {
        #region Constructors

        public Handler(IServiceProvider serviceProvider) : base(serviceProvider) { }

        #endregion

        protected override async Task<TagDto[]> Execute(GetTagsQuery request, CancellationToken cancellationToken)
        {
            return await Repository.Read(new IsDeletedProp.FindBy.EqualTo<TagEntity>(false) &&
                                         new NameProp.FindBy.ContainedTerm<TagEntity>(request.SearchTerm))
                                   .ProjectTo<TagDto>(Mapper.ConfigurationProvider).ToArrayAsync(cancellationToken);
        }
    }

    #endregion
}