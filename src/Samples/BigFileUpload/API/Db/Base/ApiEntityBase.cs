namespace Samples.UploadBigFile.API;

#region << Using >>

using CRUD.DAL.Abstractions;
using Samples.UploadBigFile.Shared;

#endregion

public abstract class ApiEntityBase : IId<int>, IDt
{
    #region Properties

    public int Id { get; set; }

    public DateTime CrDt { get; set; }

    public DateTime? UpDt { get; set; }

    #endregion
}