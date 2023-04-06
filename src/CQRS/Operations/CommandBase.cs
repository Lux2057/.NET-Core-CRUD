namespace CRUD.CQRS
{
    #region << Using >>

    using MediatR;

    #endregion

    public abstract class CommandBase : MessageBase, INotification { }
}