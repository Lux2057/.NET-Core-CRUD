namespace CRUD.Example
{
    #region << Using >>

    using CRUD.CQRS;
    using CRUD.MVC;
    using Microsoft.AspNetCore.Mvc;

    #endregion

    [Route("[controller]/[action]")]

    public class ExampleReadController : EntityReadControllerBase<ExampleEntity, int, ExampleDto>
    {
        #region Constructors

        public ExampleReadController(IReadWriteDispatcher dispatcher) : base(dispatcher) { }

        #endregion
    }
}