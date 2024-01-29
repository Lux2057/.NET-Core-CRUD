namespace CRUD.Logging.EntityFramework;

#region << Using >>

using CRUD.DAL.Abstractions;

#endregion

public class LogDto : IId<int>
{
    #region Properties

    public int Id { get; set; }

    public DateTime CrDt { get; set; }

    public string LogLevel { get; set; }

    public string Message { get; set; }

    public string Exception { get; set; }

    #endregion
}