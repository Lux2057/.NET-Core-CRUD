namespace CRUD.WebAPI
{
    #region << Using >>

    using CRUD.Core;
    using CRUD.CQRS;
    using CRUD.DAL;
    using Microsoft.AspNetCore.Mvc;

    #endregion

    public abstract class EntityReadControllerBase<TEntity, TId, TDto> : DispatcherControllerBase
            where TEntity : class, IId<TId>, new()
            where TDto : class, new()
    {
        #region Constructors

        protected EntityReadControllerBase(IDispatcher dispatcher) : base(dispatcher) { }

        #endregion

        [HttpGet]
        public virtual async Task<IActionResult> Read(TId[] ids, int? page, int? pageSize, CancellationToken cancellationToken = default)
        {
            var entities = await QueryAsync(new ReadEntitiesQueryBase<TEntity, TId, TDto>(ids)
                                                            {
                                                                    Page = page,
                                                                    PageSize = pageSize
                                                            }, cancellationToken);

            return Ok(entities);
        }
    }
}