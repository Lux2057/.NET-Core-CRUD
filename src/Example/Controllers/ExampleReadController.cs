namespace CRUD.Example
{
    #region << Using >>

    using CRUD.CQRS;

    #endregion

    public class ExampleReadController : EntityReadControllerBase<ExampleEntity, ExampleDto>
    {
        #region Constructors

        public ExampleReadController(IReadWriteDispatcher dispatcher) : base(dispatcher) { }

        #endregion
    }
}