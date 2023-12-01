namespace NhTests.Logging;

#region << Using >>

using CRUD.CQRS;
using CRUD.Logging.NHibernate;
using Extensions;
using Microsoft.Extensions.Logging;

#endregion

public class AddLogTests : DispatcherTest
{
    #region Constructors

    public AddLogTests(IDispatcher dispatcher) : base(dispatcher) { }

    #endregion

    [Fact]
    public async Task Should_add_log()
    {
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