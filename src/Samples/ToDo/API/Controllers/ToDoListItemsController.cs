namespace Samples.ToDo.API;

#region << Using >>

using CRUD.Core;
using CRUD.CQRS;
using CRUD.DAL.Abstractions;
using CRUD.WebAPI;
using Microsoft.AspNetCore.Mvc;
using Samples.ToDo.Shared;

#endregion

[Route("[controller]/[action]")]
public class ToDoListItemsController : DispatcherControllerBase
{
    #region Constructors

    public ToDoListItemsController(IDispatcher dispatcher) : base(dispatcher) { }

    #endregion

    [HttpGet,
     Route("~/" + ApiRoutes.ReadToDoListItems),
     ProducesResponseType(typeof(PaginatedResponseDto<ToDoListItemDto>), 200)]
    public async Task<IActionResult> Read([FromQuery(Name = ApiRoutes.Params.toDoListId)] int toDoListId,
                                          [FromQuery(Name = ApiRoutes.Params.page)]
                                          int? page,
                                          [FromQuery(Name = ApiRoutes.Params.pageSize)]
                                          int? pageSize,
                                          CancellationToken cancellationToken = new())
    {
        var response = await Dispatcher.QueryAsync(new ReadEntitiesQuery<ToDoListItemEntity, int, ToDoListItemDto>
                                                   {
                                                           PageSize = pageSize,
                                                           Page = page,
                                                           OrderSpecifications = new[]
                                                                                 {
                                                                                         new OrderById<ToDoListItemEntity, int>(false)
                                                                                 },
                                                           Specification = new ToDoListIdProp.FindBy.EqualTo<ToDoListItemEntity>(toDoListId)
                                                   }, cancellationToken);

        return Ok(response);
    }

    [HttpPost,
     Route("~/" + ApiRoutes.CreateOrUpdateToDoListItem),
     ProducesResponseType(200)]
    public async Task<IActionResult> CreateOrUpdate([FromBody] ToDoListItemDto dto,
                                                    CancellationToken cancellationToken = new())
    {
        var command = new CreateOrUpdateToDoListItemCommand { Dto = dto };
        await Dispatcher.PushAsync(command, cancellationToken);

        return Ok(command.Result);
    }

    [HttpDelete,
     Route("~/" + ApiRoutes.DeleteToDoListItem),
     ProducesResponseType(200)]
    public async Task<IActionResult> Delete([FromQuery(Name = ApiRoutes.Params.id)] int id,
                                            CancellationToken cancellationToken = new())
    {
        var command = new DeleteEntitiesCommand<ToDoListItemEntity, int>(new[] { id });
        await Dispatcher.PushAsync(command, cancellationToken);

        return Ok(command.Result);
    }
}