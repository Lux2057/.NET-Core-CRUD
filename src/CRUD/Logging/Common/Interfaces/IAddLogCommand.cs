namespace CRUD.Logging.Common;

#region << Using >>

using Microsoft.Extensions.Logging;

#endregion

public interface IAddLogCommand
{
    #region Properties

    public LogLevel LogLevel { get; init; }

    public string Message { get; init; }

    public Exception Exception { get; init; }

    #endregion
}