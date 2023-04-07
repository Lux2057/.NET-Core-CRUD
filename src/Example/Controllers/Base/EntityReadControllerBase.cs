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

    public abstract class EntityReadControllerBase<TEntity, TDto> : DispatcherControllerBase
            where TEntity : EntityBase, new()
            where TDto : DtoBase, new()
    {
        #region Constructors

        protected EntityReadControllerBase(IReadWriteDispatcher dispatcher) : base(dispatcher) { }

        #endregion

        [HttpGet]
        public virtual async Task<IActionResult> Read(int[] ids, int? page, int? pageSize, CancellationToken cancellationToken = default)
        {
            var entities = await this.Dispatcher.QueryAsync(new ReadEntitiesQuery<TEntity, TDto>(ids)
                                                            {
                                                                    Page = page,
                                                                    PageSize = pageSize
                                                            }, cancellationToken);

            return Ok(entities);
        }
    }
}