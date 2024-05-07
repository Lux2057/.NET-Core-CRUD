namespace Samples.ToDo.API;

#region << Using >>

using CRUD.CQRS;
using CRUD.Logging.EntityFramework;
using CRUD.WebAPI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

#endregion

[Authorize,
 Route("[controller]/[action]")]
public class LogsController : EntityReadControllerBase<LogEntity, int, LogDto>
{
    #region Constructors

    public LogsController(IDispatcher dispatcher) : base(dispatcher) { }

    #endregion
}