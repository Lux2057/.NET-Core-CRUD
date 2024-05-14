namespace CRUD.CQRS.Linq2Db;

#region << Using >>

using CRUD.DAL.Linq2Db;
using Microsoft.Extensions.DependencyInjection;

#endregion

public abstract class CommandHandlerBase<TNotification> : CRUD.CQRS.CommandHandlerBase<TNotification>
        where TNotification : CommandBase
{
    #region Properties

    protected new ILinq2DbRepository Repository { get; }

    #endregion

    #region Constructors

    protected CommandHandlerBase(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        Repository = serviceProvider.GetService<ILinq2DbRepository>();
    }

    #endregion
}