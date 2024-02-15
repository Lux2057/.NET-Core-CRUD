namespace CRUD.CQRS;

#region << Using >>

using FluentValidation.Results;
using MediatR;

#endregion

public abstract class QueryBase<TResponse> : IRequest<TResponse>
{
    #region Properties

    public ValidationResult ValidationResult { get; set; }

    #endregion
}