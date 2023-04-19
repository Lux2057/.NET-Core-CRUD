namespace CRUD.MVC
{
    #region << Using >>

    using System.Threading;
    using System.Threading.Tasks;
    using CRUD.Core;
    using CRUD.CQRS;
    using Microsoft.AspNetCore.Mvc;

    #endregion

    public abstract class EntityCRUDControllerBase<TEntity, TId, TDto> : DispatcherControllerBase
            where TEntity : class, IId<TId>, new()
            where TDto : class, IId<TId>, new()
    {
        #region Constructors

        protected EntityCRUDControllerBase(IReadWriteDispatcher dispatcher) : base(dispatcher) { }

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

        [HttpPost]
        public virtual async Task<IActionResult> CreateOrUpdate([FromBody] TDto[] dtos, CancellationToken cancellationToken = default)
        {
            var createOrUpdateEntitiesCommand = new CreateOrUpdateEntitiesCommand<TEntity, TId, TDto>(dtos);
            await this.Dispatcher.PushAsync(createOrUpdateEntitiesCommand, cancellationToken);

            return Ok(createOrUpdateEntitiesCommand.Result);
        }

        [HttpDelete]
        public virtual async Task<IActionResult> Delete([FromBody] TId[] ids, CancellationToken cancellationToken = default)
        {
            var deleteEntitiesCommand = new DeleteEntitiesCommand<TEntity, TId>(ids);
            await this.Dispatcher.PushAsync(deleteEntitiesCommand, cancellationToken);

            return Ok(deleteEntitiesCommand.Result);
        }
    }
}