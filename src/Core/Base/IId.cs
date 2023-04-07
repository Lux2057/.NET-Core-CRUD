namespace CRUD.Core
{
    public interface IId<TId>
    {
        #region Properties

        public TId Id { get; set; }

        #endregion
    }
}