namespace WebAPI_Template_EF;

#region << Using >>

using CRUD.DAL.Abstractions;

#endregion

public abstract class ApiEntityBase : IId<int>, IDt
{
    #region Properties

    public int Id { get; set; }

    public DateTime CrDt { get; set; }

    public DateTime? UpDt { get; set; }

    #endregion
}