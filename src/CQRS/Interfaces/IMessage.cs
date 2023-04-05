namespace CRUD.CQRS
{
    public interface IMessage
    {
        #region Properties

        public object Result { get; set; }

        #endregion
    }
}