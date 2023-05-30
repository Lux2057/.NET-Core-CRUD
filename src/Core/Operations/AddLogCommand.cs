namespace CRUD.Core
{
    #region << Using >>

    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using CRUD.CQRS;
    using JetBrains.Annotations;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;

    #endregion

    public class AddLogCommand : CommandBase
    {
        #region Properties

        public LogLevel LogLevel { get; init; }

        public string Message { get; init; }

        public Exception Exception { get; init; }

        #endregion

        #region Nested Classes

        [UsedImplicitly]
        class Handler : CommandHandlerBase<AddLogCommand>
        {
            #region Constructors

            public Handler(IServiceProvider serviceProvider) : base(serviceProvider) { }

            #endregion

            protected override async Task Execute(AddLogCommand command, CancellationToken cancellationToken)
            {
                await Repository<LogEntity>().AddAsync(new LogEntity
                                                       {
                                                               CrDt = DateTime.UtcNow,
                                                               LogLevel = command.LogLevel,
                                                               Message = command.Message,
                                                               Exception = JsonConvert.SerializeObject(command.Exception,
                                                                                                       new JsonSerializerSettings
                                                                                                       {
                                                                                                               ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                                                                                                       })
                                                       }, cancellationToken);
            }
        }

        #endregion
    }
}