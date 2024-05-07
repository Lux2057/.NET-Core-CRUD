namespace Samples.ToDo.UI;

#region << Using >>

using Fluxor;
using JetBrains.Annotations;
using Samples.ToDo.Shared;

#endregion

public class SetTaskStatusWf
{
    #region Properties

    private readonly TasksAPI api;

    #endregion

    #region Constructors

    public SetTaskStatusWf(HttpClient http,
                           IDispatcher dispatcher)
    {
        this.api = new TasksAPI(http, dispatcher);
    }

    #endregion

    #region Nested Classes

    public record Init : ValidatingAction<SetTaskStatusRequest>, IAuthRequiredAction
    {
        #region Properties

        public Action SuccessCallback { get; }

        public Action FailCallback { get; }

        public string AccessToken { get; set; }

        #endregion

        #region Constructors

        public Init(SetTaskStatusRequest request,
                    Action successCallback = default,
                    Action failCallback = default,
                    string validationKey = default)
                : base(request, validationKey)
        {
            SuccessCallback = successCallback;
            FailCallback = failCallback;
        }

        #endregion
    }

    #endregion

    [EffectMethod,
     UsedImplicitly]
    public async Task HandleInit(Init action, IDispatcher dispatcher)
    {
        var success = await this.api.SetStatusAsync(request: action.Request,
                                                    accessToken: action.AccessToken,
                                                    validationKey: action.ValidationKey);

        if (success)
            action.SuccessCallback?.Invoke();
        else
            action.FailCallback?.Invoke();
    }
}