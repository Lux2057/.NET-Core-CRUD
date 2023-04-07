namespace CRUD.Core
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using CRUD.CQRS;
    using CRUD.DAL;
    using Microsoft.EntityFrameworkCore;

    #endregion

    public class ReadEntitiesQuery<TEntity, TId, TDto> : IQuery<TDto[]>
            where TEntity : class, IId<TId>, new()
            where TDto : class, new()
    {
        #region Constants

        public const int defaultPageSize = 50;

        public const int defaultPage = 1;

        #endregion

        #region Properties

        public TId[] Ids { get; }

        public int? Page { get; init; }

        public int? PageSize { get; init; }

        #endregion

        #region Constructors

        public ReadEntitiesQuery(IEnumerable<TId> ids)
        {
            Ids = ids.ToArrayOrEmpty();
        }

        #endregion

        #region Nested Classes

        public class Handler : QueryHandlerBase<ReadEntitiesQuery<TEntity, TId, TDto>, TDto[]>
        {
            #region Constructors

            public Handler(IServiceProvider serviceProvider) : base(serviceProvider) { }

            #endregion

            protected override async Task<TDto[]> Execute(ReadEntitiesQuery<TEntity, TId, TDto> request, CancellationToken cancellationToken)
            {
                var entitiesCount = await Repository<TEntity>().Get(new EntitiesByIdsSpec<TEntity, TId>(request.Ids)).CountAsync(cancellationToken);
                var pageSize = new[] { defaultPage, request.PageSize.GetValueOrDefault(defaultPageSize) }.Max();
                var maxPage = new[] { 1, entitiesCount / pageSize }.Max();
                var currentPage = new[] { new[] { defaultPage, request.Page.GetValueOrDefault(defaultPage) }.Max(), maxPage }.Min() - 1;

                var entities = await Repository<TEntity>().Get(new EntitiesByIdsSpec<TEntity, TId>(request.Ids))
                                                          .Skip(currentPage * pageSize).Take(pageSize)
                                                          .ToArrayAsync(cancellationToken);

                return this.Mapper.Map<TDto[]>(entities).ToArray();
            }
        }

        #endregion
    }
}