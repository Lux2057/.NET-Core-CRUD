namespace Examples.WebAPI
{
    #region << Using >>

    using CRUD.CQRS;
    using CRUD.WebAPI;
    using Microsoft.AspNetCore.Mvc;

    #endregion

    [Route("[controller]/[action]")]
    public class ExampleCRUDController : EntityCRUDControllerBase<ExampleEntity, int, ExampleDto>
    {
        #region Constructors

        public ExampleCRUDController(IReadWriteDispatcher dispatcher) : base(dispatcher) { }

        #endregion
    }
}