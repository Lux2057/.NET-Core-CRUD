namespace CRUD.CQRS
{
    #region << Using >>

    using MediatR;

    #endregion

    public abstract class CommandBase : INotification
    {
        #region Properties

        public virtual object Result { get; set; }

        #endregion
    }
}