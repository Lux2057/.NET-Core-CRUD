namespace Templates.Blazor.EF.API;

#region << Using >>

using CRUD.Core;
using CRUD.CQRS;
using CRUD.WebAPI;
using Microsoft.AspNetCore.Mvc;
using Templates.Blazor.EF.Shared;

#endregion

[Route("[controller]/[action]")]
public class ToDoListsController : EntityCRUDControllerBase<ToDoListEntity, int, ToDoListDto>
{
    #region Constructors

    public ToDoListsController(IDispatcher dispatcher) : base(dispatcher) { }

    #endregion

    [Route("~/" + ApiRoutes.ReadToDoLists)]
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResponseDto<ToDoListDto>), 200)]
    public override Task<IActionResult> Read([FromQuery(Name = ApiRoutes.Params.ids)] int[] ids,
                                             [FromQuery(Name = ApiRoutes.Params.page)]
                                             int? page,
                                             [FromQuery(Name = ApiRoutes.Params.pageSize)]
                                             int? pageSize,
                                             CancellationToken cancellationToken = new())
    {
        return base.Read(ids, page, pageSize, cancellationToken);
    }

    [Route("~/" + ApiRoutes.CreateOrUpdateToDoLists)]
    [HttpPost]
    [ProducesResponseType(200)]
    public override Task<IActionResult> CreateOrUpdate([FromQuery(Name = ApiRoutes.Params.dtos)] ToDoListDto[] dtos,
                                                       CancellationToken cancellationToken = new())
    {
        return base.CreateOrUpdate(dtos, cancellationToken);
    }

    [Route("~/" + ApiRoutes.DeleteToDoLists)]
    [HttpDelete]
    [ProducesResponseType(200)]
    public override Task<IActionResult> Delete([FromQuery(Name = ApiRoutes.Params.ids)] int[] ids,
                                               CancellationToken cancellationToken = new())
    {
        return base.Delete(ids, cancellationToken);
    }
}