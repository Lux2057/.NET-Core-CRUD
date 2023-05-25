namespace CRUD.Core
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper.QueryableExtensions;
    using CRUD.CQRS;
    using CRUD.Extensions;
    using LinqSpecs;
    using Microsoft.EntityFrameworkCore;

    #endregion

    /// <summary>
    ///     Returns a page collection of Entities by specified id collection.
    ///     If id collection is null or empty, returns all Entities from data storage.
    /// </summary>
    public class ReadEntitiesQuery<TEntity, TId, TResponseDto> : IQuery<PaginatedResponseDto<TResponseDto>>
            where TEntity : class, IId<TId>, new()
            where TResponseDto : class, new()
    {
        #region Properties

        public TId[] Ids { get; }

        public int? Page { get; init; }

        public int? PageSize { get; init; }

        public bool DisablePaging { get; init; }

        public Specification<TEntity> Specification { get; init; }

        #endregion

        #region Constructors

        public ReadEntitiesQuery(IEnumerable<TId> ids = null)
        {
            Ids = ids.ToArrayOrEmpty();
        }

        #endregion

        #region Nested Classes

        internal class Handler : QueryHandlerBase<ReadEntitiesQuery<TEntity, TId, TResponseDto>, PaginatedResponseDto<TResponseDto>>
        {
            #region Constructors

            public Handler(IServiceProvider serviceProvider) : base(serviceProvider) { }

            #endregion

            protected override async Task<PaginatedResponseDto<TResponseDto>> Execute(ReadEntitiesQuery<TEntity, TId, TResponseDto> request, CancellationToken cancellationToken)
            {
                Specification<TEntity> specification = new FindEntitiesByIds<TEntity, TId>(request.Ids);
                if (request.Specification != null)
                    specification = specification && request.Specification;

                var queryable = Repository<TEntity>().Get(specification).AsNoTracking().ProjectTo<TResponseDto>(this.Mapper.ConfigurationProvider);

                PagingInfoDto pagingInfo = null;
                if (!request.DisablePaging)
                {
                    var totalCount = await queryable.CountAsync(cancellationToken);
                    pagingInfo = await this.Dispatcher.QueryAsync(new GetPagingInfoQuery(request.Page, request.PageSize, totalCount), cancellationToken);

                    queryable = queryable.Skip(pagingInfo.PageSize * (pagingInfo.CurrentPage - 1)).Take(pagingInfo.PageSize);
                }

                var items = await queryable.ToArrayAsync(cancellationToken);

                return new PaginatedResponseDto<TResponseDto>
                       {
                               Items = items,
                               PagingInfo = pagingInfo ?? new PagingInfoDto
                                                          {
                                                                  PageSize = items.Length,
                                                                  CurrentPage = 1,
                                                                  TotalPages = 1,
                                                                  TotalItemsCount = items.Length
                                                          }
                       };
            }
        }

        #endregion
    }
}