namespace CRUD.Example
{
    #region << Using >>

    using System.Threading;
    using System.Threading.Tasks;
    using CRUD.Core;
    using CRUD.CQRS;
    using CRUD.DAL;
    using Microsoft.AspNetCore.Mvc;

    #endregion

    public abstract class EntityCRUDControllerBase<TEntity, TDto> : DispatcherControllerBase
            where TEntity : EntityBase, new()
            where TDto : DtoBase, new()
    {
        #region Constructors

        protected EntityCRUDControllerBase(IReadWriteDispatcher dispatcher) : base(dispatcher) { }

        #endregion

        [HttpGet]
        public virtual async Task<IActionResult> Read(object[] ids, int? page, int? pageSize, CancellationToken cancellationToken = default)
        {
            var entities = await this.Dispatcher.QueryAsync<ReadEntitiesQuery<TEntity, TDto>, TDto[]>(new ReadEntitiesQuery<TEntity, TDto>(ids)
                                                                                                      {
                                                                                                              Page = page,
                                                                                                              PageSize = pageSize
                                                                                                      }, cancellationToken);

            return Ok(entities);
        }

        [HttpPost]
        public virtual async Task<IActionResult> CreateOrUpdate([FromBody] TDto[] dtos, CancellationToken cancellationToken = default)
        {
            var createOrUpdateEntitiesCommand = new CreateOrUpdateEntitiesCommand<TEntity, TDto>(dtos);
            await this.Dispatcher.PushAsync(createOrUpdateEntitiesCommand, cancellationToken);

            return Ok(createOrUpdateEntitiesCommand.Result);
        }

        [HttpDelete]
        public virtual async Task<IActionResult> Delete([FromBody] object[] ids, CancellationToken cancellationToken = default)
        {
            var deleteEntitiesCommand = new DeleteEntitiesCommand<TEntity>(ids);
            await this.Dispatcher.PushAsync(deleteEntitiesCommand, cancellationToken);

            return Ok(deleteEntitiesCommand.Result);
        }
    }
}