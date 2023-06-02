namespace CRUD.CQRS
{
    #region << Using >>

    using MediatR;

    #endregion

    public abstract class QueryBase<TResponse> : IRequest<TResponse> { }
}