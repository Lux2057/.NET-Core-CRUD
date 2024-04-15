namespace Samples.ToDo.UI;

#region << Using >>

using Fluxor;

#endregion

public abstract class ApiBase
{
    #region Properties

    protected readonly IDispatcher dispatcher;

    protected readonly HttpClient Http;

    protected readonly IState<LanguageState> localizationState;

    #endregion

    #region Constructors

    public ApiBase(HttpClient http,
                   IDispatcher dispatcher,
                   IState<LanguageState> localizationState)
    {
        this.Http = http;
        this.dispatcher = dispatcher;
        this.localizationState = localizationState;
    }

    #endregion
}