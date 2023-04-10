namespace CRUD.MVC
{
    #region << Using >>

    using CRUD.CQRS;
    using Microsoft.AspNetCore.Mvc;

    #endregion

    public abstract class DispatcherControllerBase : ControllerBase
    {
        #region Properties

        protected readonly IReadWriteDispatcher Dispatcher;

        #endregion

        #region Constructors

        protected DispatcherControllerBase(IReadWriteDispatcher dispatcher)
        {
            this.Dispatcher = dispatcher;
        }

        #endregion
    }
}