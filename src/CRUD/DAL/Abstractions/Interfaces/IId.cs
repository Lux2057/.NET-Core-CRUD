namespace CRUD.DAL.Abstractions
{
    public interface IId<TId>
    {
        #region Properties

        public TId Id { get; set; }

        #endregion
    }
}