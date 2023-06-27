namespace WebAPI.API;

#region << Using >>

using CRUD.CQRS;
using CRUD.WebAPI;
using Microsoft.AspNetCore.Mvc;
using WebAPI.API.Entities;

#endregion

[Route("[controller]/[action]")]
public class SampleController : EntityCRUDControllerBase<SampleEntity, int, SampleDto>
{
    #region Constructors

    public SampleController(IDispatcher dispatcher) : base(dispatcher) { }

    #endregion
}