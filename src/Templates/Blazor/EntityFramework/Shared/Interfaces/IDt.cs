namespace Templates.Blazor.EF.Shared;

#region << Using >>

using System;

#endregion

public interface IDt
{
    #region Properties

    public DateTime CrDt { get; set; }

    public DateTime? UpDt { get; set; }

    #endregion
}