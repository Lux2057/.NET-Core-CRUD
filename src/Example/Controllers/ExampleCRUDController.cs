namespace CRUD.Example
{
    #region << Using >>

    using CRUD.CQRS;

    #endregion

    public class ExampleCRUDController : EntityCRUDControllerBase<ExampleEntity, ExampleDto>
    {
        #region Constructors

        public ExampleCRUDController(IReadWriteDispatcher dispatcher) : base(dispatcher) { }

        #endregion
    }
}