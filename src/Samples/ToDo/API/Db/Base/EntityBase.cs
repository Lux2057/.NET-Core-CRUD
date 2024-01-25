namespace Samples.ToDo.API;

#region << Using >>

using CRUD.DAL.Abstractions;
using Samples.ToDo.Shared;

#endregion

public abstract class EntityBase : IId<int>, IDt
{
    #region Properties

    public int Id { get; set; }

    public DateTime CrDt { get; set; }

    public DateTime? UpDt { get; set; }

    #endregion
}