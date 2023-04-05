namespace CRUD.DAL
{
    public interface IId<T>
    {
        #region Properties

        public T Id { get; set; }

        #endregion
    }
}