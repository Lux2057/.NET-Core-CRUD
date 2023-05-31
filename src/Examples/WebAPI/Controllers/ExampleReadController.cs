namespace Examples.WebAPI
{
    #region << Using >>

    using CRUD.CQRS;
    using CRUD.WebAPI;
    using Microsoft.AspNetCore.Mvc;

    #endregion

    [Route("[controller]/[action]")]
    public class ExampleReadController : EntityReadControllerBase<ExampleEntity, int, ExampleDto>
    {
        #region Constructors

        public ExampleReadController(IDispatcher dispatcher) : base(dispatcher) { }

        #endregion
    }
}