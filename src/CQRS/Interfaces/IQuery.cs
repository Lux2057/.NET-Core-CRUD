namespace CRUD.CQRS
{
    #region << Using >>

    using MediatR;

    #endregion

    public interface IQuery<TResponse> : IMessage, IRequest<TResponse> { }
}