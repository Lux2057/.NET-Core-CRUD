namespace CRUD.DAL.Abstractions;

public interface IId<out TId>
{
    #region Properties

    public TId Id { get; }

    #endregion
}