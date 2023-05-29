namespace CRUD.DAL
{
    public interface IId<TId>
    {
        #region Properties

        public TId Id { get; set; }

        #endregion
    }
}