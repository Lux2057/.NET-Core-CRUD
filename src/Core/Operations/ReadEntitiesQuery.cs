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
    using LinqSpecs;
    using Microsoft.EntityFrameworkCore;

    #endregion

    /// <summary>
    ///     Returns a page collection of Entities by specified id collection.
    ///     If id collection is null or empty, returns all Entities from data storage.
    /// </summary>
    public class ReadEntitiesQuery<TEntity, TId, TDto> : IQuery<TDto[]>
            where TEntity : class, IId<TId>, new()
            where TDto : class, new()
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

        internal class Handler : QueryHandlerBase<ReadEntitiesQuery<TEntity, TId, TDto>, TDto[]>
        {
            #region Constructors

            public Handler(IServiceProvider serviceProvider) : base(serviceProvider) { }

            #endregion

            protected override async Task<TDto[]> Execute(ReadEntitiesQuery<TEntity, TId, TDto> request, CancellationToken cancellationToken)
            {
                Specification<TEntity> specification = new EntitiesByIdsSpec<TEntity, TId>(request.Ids);
                if (request.Specification != null)
                    specification = specification && request.Specification;

                var entitiesQueryable = request.DisablePaging ?
                                                Repository<TEntity>().Get(specification) :
                                                await Repository<TEntity>().GetPageAsync(specification, request.Page, request.PageSize, cancellationToken);

                var entities = await entitiesQueryable.ToArrayAsync(cancellationToken);

                return this.Mapper.Map<TDto[]>(entities).ToArray();
            }
        }

        #endregion
    }
}