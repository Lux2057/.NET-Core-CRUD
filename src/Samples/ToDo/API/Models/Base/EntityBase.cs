namespace Samples.ToDo.API;

#region << Using >>

using CRUD.DAL.Abstractions;
using Samples.ToDo.Shared;

#endregion

public abstract class EntityBase : IId<int>, ICrDt, IsDeletedProp.Interface
{
    #region Properties

    public int Id { get; set; }

    public DateTime CrDt { get; set; }

    public bool IsDeleted { get; set; }

    #endregion

    #region Constructors

    protected EntityBase()
    {
        CrDt = DateTime.UtcNow;
    }

    #endregion
}