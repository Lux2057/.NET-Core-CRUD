namespace Linq2DbTests.Logging;

#region << Using >>

using CRUD.CQRS;
using CRUD.DAL.Linq2Db;
using CRUD.Logging.Linq2Db;
using Extensions;
using Microsoft.Extensions.Logging;

#endregion

public class AddLogTests : DispatcherTest
{
    #region Constructors

    public AddLogTests(IDispatcher dispatcher, TestDataConnection connection) : base(dispatcher, connection) { }

    #endregion

    [Fact]
    public async Task Should_add_log()
    {
        Connection.TryCreateTable<LogEntity>("Logs", true);

        const LogLevel logLevel = LogLevel.Information;
        const string message = "TEST";
        var exception = new Exception(message);

        await Dispatcher.PushAsync(new AddLogCommand
                                   {
                                           Exception = exception,
                                           LogLevel = logLevel,
                                           Message = message
                                   });

        var logsInDb = await Dispatcher.QueryAsync(new GetLogsQuery());

        Assert.Single(logsInDb);
        Assert.Equal(message, logsInDb.Single().Message);
        Assert.Equal(exception.ToJsonString(), logsInDb.Single().Exception);
        Assert.Equal(logLevel, logsInDb.Single().LogLevel);
    }
}