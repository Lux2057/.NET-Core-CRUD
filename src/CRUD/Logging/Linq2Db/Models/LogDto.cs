namespace CRUD.Logging.Linq2Db;

#region << Using >>

using CRUD.DAL.Abstractions;

#endregion

public class LogDto : IId<string>
{
    #region Properties

    public string Id { get; set; }

    public DateTime CrDt { get; set; }

    public string LogLevel { get; set; }

    public string Message { get; set; }

    public string Exception { get; set; }

    #endregion
}