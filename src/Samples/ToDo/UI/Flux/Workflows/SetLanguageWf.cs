namespace Samples.ToDo.UI;

#region << Using >>

using Fluxor;
using JetBrains.Annotations;
using Microsoft.JSInterop;

#endregion

public class SetLanguageWf
{
    #region Properties

    private readonly IJSRuntime js;

    #endregion

    #region Constructors

    public SetLanguageWf(IJSRuntime js)
    {
        this.js = js;
    }

    #endregion

    #region Nested Classes

    public record Init(string Language,
                       Action Callback = default);

    public record Update(string Language,
                         Action Callback);

    #endregion

    [ReducerMethod,
     UsedImplicitly]
    public static LanguageState OnInit(LanguageState state, Init action)
    {
        return new LanguageState(isUpdating: true,
                                 language: state.Language);
    }

    [EffectMethod,
     UsedImplicitly]
    public async Task HandleInit(Init action, IDispatcher dispatcher)
    {
        await this.js.SetLocalStorageAsync(LocalStorage.Key.Language, action.Language);

        dispatcher.Dispatch(new Update(action.Language, action.Callback));
    }

    [ReducerMethod,
     UsedImplicitly]
    public static LanguageState OnUpdate(LanguageState state, Update action)
    {
        return new LanguageState(isUpdating: false,
                                 language: action.Language);
    }

    [EffectMethod,
     UsedImplicitly]
    public Task HandleUpdate(Update action, IDispatcher dispatcher)
    {
        action.Callback?.Invoke();

        return Task.CompletedTask;
    }
}