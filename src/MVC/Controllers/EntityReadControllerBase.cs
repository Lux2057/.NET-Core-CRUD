namespace CRUD.MVC
{
    #region << Using >>

    using CRUD.Core;
    using CRUD.CQRS;
    using Microsoft.AspNetCore.Mvc;

    #endregion

    public abstract class EntityReadControllerBase<TEntity, TId, TDto> : DispatcherControllerBase
            where TEntity : EntityBase<TId>, new()
            where TDto : class, new()
    {
        #region Constructors

        protected EntityReadControllerBase(IReadWriteDispatcher dispatcher) : base(dispatcher) { }

        #endregion

        [HttpGet]
        public virtual async Task<IActionResult> Read(TId[] ids, int? page, int? pageSize, CancellationToken cancellationToken = default)
        {
            var entities = await this.Dispatcher.QueryAsync(new ReadEntitiesQuery<TEntity, TId, TDto>(ids)
                                                            {
                                                                    Page = page,
                                                                    PageSize = pageSize
                                                            }, cancellationToken);

            return Ok(entities);
        }
    }
}