namespace CRUD.CQRS
{
    public abstract class MessageBase
    {
        #region Properties

        public virtual object Result { get; set; }

        #endregion
    }
}