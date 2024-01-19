namespace Samples.UploadBigFile.API;

#region << Using >>

using CRUD.Core;
using CRUD.CQRS;
using CRUD.DAL.Abstractions;
using CRUD.WebAPI;
using Microsoft.AspNetCore.Mvc;
using Samples.UploadBigFile.Shared;

#endregion

[Route("[controller]/[action]")]
public class ToDoListsController : DispatcherControllerBase
{
    #region Constructors

    public ToDoListsController(IDispatcher dispatcher) : base(dispatcher) { }

    #endregion

    [Route("~/" + ApiRoutes.ReadToDoLists)]
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResponseDto<ToDoListDto>), 200)]
    public async Task<IActionResult> Read([FromQuery(Name = ApiRoutes.Params.page)] int? page,
                                          [FromQuery(Name = ApiRoutes.Params.pageSize)]
                                          int? pageSize,
                                          CancellationToken cancellationToken = new())
    {
        var response = await Dispatcher.QueryAsync(new ReadEntitiesQuery<ToDoListEntity, int, ToDoListDto>
                                                   {
                                                           PageSize = pageSize,
                                                           Page = page,
                                                           OrderSpecifications = new[]
                                                                                 {
                                                                                         new OrderById<ToDoListEntity, int>(false)
                                                                                 }
                                                   }, cancellationToken);

        return Ok(response);
    }

    [Route("~/" + ApiRoutes.CreateOrUpdateToDoList)]
    [HttpPost]
    [ProducesResponseType(200)]
    public async Task<IActionResult> CreateOrUpdate([FromBody] ToDoListDto dto,
                                                    CancellationToken cancellationToken = new())
    {
        var command = new CreateOrUpdateToDoListCommand { Dto = dto };
        await Dispatcher.PushAsync(command, cancellationToken);

        return Ok(command.Result);
    }

    [Route("~/" + ApiRoutes.DeleteToDoList)]
    [HttpDelete]
    [ProducesResponseType(200)]
    public async Task<IActionResult> Delete([FromQuery(Name = ApiRoutes.Params.id)] int id,
                                            CancellationToken cancellationToken = new())
    {
        var command = new DeleteToDoListCommand { Id = id };
        await Dispatcher.PushAsync(command, cancellationToken);

        return Ok(command.Result);
    }
}