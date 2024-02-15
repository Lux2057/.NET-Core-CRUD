namespace CRUD.Logging.Common;

#region << Using >>

using System.Net;
using CRUD.CQRS;
using Extensions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

#endregion

public class ExceptionsHandlerMiddleware<TLogCommand> where TLogCommand : CommandBase, IAddLogCommand, new()
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
    public async Task Invoke(HttpContext context, IDispatcher dispatcher)
    {
        try
        {
            await this._next(context);
        }
        catch (Exception pipelineException)
        {
            try
            {
                await dispatcher.PushAsync(new TLogCommand
                                           {
                                                   LogLevel = LogLevel.Error,
                                                   Exception = pipelineException,
                                                   Message = pipelineException.Message
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
            response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var result = JsonSerializer.Serialize(new { message = "Internal server error!" });
            await response.WriteAsync(result);
        }
    }
}