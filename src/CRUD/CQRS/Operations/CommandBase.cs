namespace CRUD.CQRS;

#region << Using >>

using FluentValidation.Results;
using MediatR;

#endregion

public abstract class CommandBase : INotification
{
    #region Properties

    public virtual object Result { get; set; }

    public ValidationResult ValidationResult { get; set; }

    #endregion
}