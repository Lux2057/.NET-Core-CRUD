namespace CRUD.CQRS
{
    public abstract class QueryBase<T> : IQuery<T>
    {
        #region Properties

        public object Result { get; set; }

        #endregion
    }
}