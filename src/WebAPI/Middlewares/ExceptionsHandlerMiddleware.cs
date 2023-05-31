namespace CRUD.WebAPI
{
    #region << Using >>

    using System.Net;
    using CRUD.Core;
    using CRUD.CQRS;
    using CRUD.Extensions;
    using JetBrains.Annotations;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;
    using JsonSerializer = System.Text.Json.JsonSerializer;

    #endregion

    public class ExceptionsHandlerMiddleware
    {
        #region Constants

        private const string applicationJson = "application/json";

        #endregion

        #region Properties

        private readonly RequestDelegate _next;

        #endregion

        #region Constructors

        public ExceptionsHandlerMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        #endregion

        [UsedImplicitly]
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await this._next(context);
            }
            catch (Exception pipelineException)
            {
                var dispatcher = context.RequestServices.GetService<IDispatcher>()!;

                try
                {
                    await dispatcher.PushAsync(new AddLogCommand
                                               {
                                                       LogLevel = LogLevel.Error,
                                                       Message = pipelineException.Message,
                                                       Exception = pipelineException
                                               });
                }
                catch (Exception addLogException)
                {
                    var relativeFileLogPath = context.RequestServices.GetService<IOptions<PathOptions>>()?.Value.RelativeFileLogPath ?? "Logs";
                    var logsFolder = Path.Combine(PathHelper.GetApplicationRootOrDefault(), relativeFileLogPath);

                    if (!Directory.Exists(logsFolder))
                        Directory.CreateDirectory(logsFolder);

                    var logPath = Path.Combine(logsFolder, $"Log_{DateTime.UtcNow.ToShortDateString()}.txt");

                    var pipelineExceptionJson = pipelineException.ToJsonString(new JsonSerializerSettings
                                                                               {
                                                                                       ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                                                                               });

                    var messages = new[]
                                   {
                                           $"Can't save log to the DB: {Environment.NewLine}{addLogException.ToJsonString()}",
                                           $"{Environment.NewLine}",
                                           $"LogLevel: {LogLevel.Error}{Environment.NewLine}"
                                         + $"Message: {pipelineException.Message}{Environment.NewLine}"
                                         + $"Exception: {pipelineExceptionJson}"
                                   }.ToJoinedString(Environment.NewLine);

                    await File.AppendAllTextAsync(logPath, messages);
                }

                var response = context.Response;
                response.ContentType = applicationJson;

                response.StatusCode = pipelineException switch
                {
                        _ => (int)HttpStatusCode.InternalServerError
                };

                var result = JsonSerializer.Serialize(new { message = pipelineException.Message });
                await response.WriteAsync(result);
            }
        }
    }
}