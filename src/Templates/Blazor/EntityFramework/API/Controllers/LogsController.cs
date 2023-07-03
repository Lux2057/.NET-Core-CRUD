namespace Templates.Blazor.EF.API;

#region << Using >>

using CRUD.CQRS;
using CRUD.Logging.Common;
using CRUD.WebAPI;
using Microsoft.AspNetCore.Mvc;

#endregion

[Route("[controller]/[action]")]
public class LogsController : EntityReadControllerBase<LogEntity, int, LogDto>
{
    #region Constructors

    public LogsController(IDispatcher dispatcher) : base(dispatcher) { }

    #endregion
}