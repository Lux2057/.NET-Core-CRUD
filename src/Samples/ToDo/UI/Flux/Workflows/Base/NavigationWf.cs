namespace Samples.ToDo.UI;

#region << Using >>

using Fluxor;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Components;

#endregion

public class NavigationWf
{
    #region Properties

    readonly NavigationManager navigationManager;

    #endregion

    #region Constructors

    public NavigationWf(NavigationManager navigationManager)
    {
        this.navigationManager = navigationManager;
    }

    #endregion

    #region Nested Classes

    public record NavigateTo(string Route);

    #endregion

    [EffectMethod]
    [UsedImplicitly]
    public Task HandleNavigateToAuth(NavigateTo action, IDispatcher dispatcher)
    {
        this.navigationManager.NavigateTo(action.Route, true);

        return Task.CompletedTask;
    }
}