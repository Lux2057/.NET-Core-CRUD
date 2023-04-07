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
    using CRUD.Extensions;
    using Microsoft.EntityFrameworkCore;

    #endregion

    public class ReadEntitiesQuery<TEntity, TDto> : IQuery<TDto[]>
            where TEntity : EntityBase, new()
            where TDto : DtoBase, new()
    {
        #region Constants

        public const int defaultPageSize = 50;

        public const int defaultPage = 1;

        #endregion

        #region Properties

        public int[] Ids { get; }

        public int? Page { get; init; }

        public int? PageSize { get; init; }

        #endregion

        #region Constructors

        public ReadEntitiesQuery(IEnumerable<int> ids)
        {
            Ids = ids.ToArrayOrEmpty();
        }

        #endregion

        #region Nested Classes

        public class Handler : QueryHandlerBase<ReadEntitiesQuery<TEntity, TDto>, TDto[]>
        {
            #region Constructors

            public Handler(IServiceProvider serviceProvider) : base(serviceProvider) { }

            #endregion

            protected override async Task<TDto[]> Execute(ReadEntitiesQuery<TEntity, TDto> request, CancellationToken cancellationToken)
            {
                var entitiesCount = await Repository<TEntity>().Get(new EntitiesByIdsSpec<TEntity>(request.Ids)).CountAsync(cancellationToken);
                var pageSize = new[] { defaultPage, request.PageSize.GetValueOrDefault(defaultPageSize) }.Max();
                var maxPage = new[] { 1, entitiesCount / pageSize }.Max();
                var currentPage = new[] { new[] { defaultPage, request.Page.GetValueOrDefault(defaultPage) }.Max(), maxPage }.Min() - 1;

                var entities = await Repository<TEntity>().Get(new EntitiesByIdsSpec<TEntity>(request.Ids))
                                                          .Skip(currentPage * pageSize).Take(pageSize)
                                                          .ToArrayAsync(cancellationToken);

                return this.Mapper.Map<TDto[]>(entities).ToArray();
            }
        }

        #endregion
    }
}