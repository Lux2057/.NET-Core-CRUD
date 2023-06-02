namespace EfTests.Core;

#region << Using >>

using CRUD.CQRS;
using CRUD.Extensions;
using CRUD.Logging.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

#endregion

public class AddLogTests : DispatcherTest
{
    #region Constructors

    public AddLogTests(TestDbContext context, IDispatcher dispatcher)
            : base(context, dispatcher) { }

    #endregion

    [Fact]
    public async Task Should_add_log()
    {
        const LogLevel logLevel = LogLevel.Information;
        const string message = "TEST";
        var exception = new Exception(message);

        await this.dispatcher.PushAsync(new AddLogCommand
                                        {
                                                Exception = exception,
                                                LogLevel = logLevel,
                                                Message = message
                                        });

        var logsInDb = await this.context.Set<LogEntity>().ToArrayAsync();

        Assert.Single(logsInDb);
        Assert.Equal(message, logsInDb.Single().Message);
        Assert.Equal(exception.ToJsonString(), logsInDb.Single().Exception);
        Assert.Equal(logLevel, logsInDb.Single().LogLevel);
    }
}