namespace CRUD.WebAPI;

#region << Using >>

using CRUD.CQRS;
using Microsoft.AspNetCore.Mvc;

#endregion

public abstract class DispatcherControllerBase : ControllerBase
{
    #region Properties

    protected IDispatcher Dispatcher { get; }

    #endregion

    #region Constructors

    protected DispatcherControllerBase(IDispatcher dispatcher)
    {
        Dispatcher = dispatcher;
    }

    #endregion
}