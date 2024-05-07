namespace Samples.ToDo.UI;

#region << Using >>

using Fluxor;

#endregion

public abstract class ApiBase
{
    #region Properties

    protected readonly IDispatcher dispatcher;

    protected readonly HttpClient Http;

    #endregion

    #region Constructors

    public ApiBase(HttpClient http,
                   IDispatcher dispatcher)
    {
        this.Http = http;
        this.dispatcher = dispatcher;
    }

    #endregion
}