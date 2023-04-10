namespace CRUD.MVC;

#region << Using >>

using System.Net;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

#endregion

public class ValidationErrorsHandlerMiddleware
{
    #region Properties

    private readonly RequestDelegate _next;

    #endregion

    #region Constructors

    public ValidationErrorsHandlerMiddleware(RequestDelegate next)
    {
        this._next = next;
    }

    #endregion

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await this._next(context);
        }
        catch (ValidationException validationException)
        {
            var response = context.Response;
            response.ContentType = Constants.ApplicationJsonContentType;
            response.StatusCode = (int)HttpStatusCode.BadRequest;

            var result = JsonConvert.SerializeObject(new ValidationFailureResult(validationException.Message,
                                                                                 validationException.Errors
                                                                                                    .Select(r => new ValidationError(r.ErrorMessage, r.PropertyName)).ToArray()));

            await response.WriteAsync(result);
        }
    }
}