namespace CRUD.Core
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using CRUD.CQRS;
    using CRUD.Extensions;
    using Microsoft.EntityFrameworkCore;

    #endregion

    public class ReadEntitiesQuery<TEntity, TId, TDto> : IQuery<TDto[]>
            where TEntity : class, IId<TId>, new()
            where TDto : class, new()
    {
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
                var paginatedAsync = await Repository<TEntity>().GetPageAsync(new EntitiesByIdsSpec<TEntity, TId>(request.Ids), request.Page, request.PageSize, cancellationToken);
                var entities = await paginatedAsync.ToArrayAsync(cancellationToken);

                return this.Mapper.Map<TDto[]>(entities).ToArray();
            }
        }

        #endregion
    }
}