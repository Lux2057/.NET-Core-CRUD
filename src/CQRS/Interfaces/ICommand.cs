namespace CRUD.CQRS
{
    #region << Using >>

    using MediatR;

    #endregion

    public interface ICommand : IMessage, INotification { }
}