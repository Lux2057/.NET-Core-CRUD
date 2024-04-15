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

    public record NavigateTo(string Route, bool ForceLoad);

    public record Refresh(bool ForceLoad);

    #endregion

    [EffectMethod,
     UsedImplicitly]
    public Task HandleNavigateToAuth(NavigateTo action, IDispatcher _)
    {
        this.navigationManager.NavigateTo(action.Route, action.ForceLoad);

        return Task.CompletedTask;
    }

    [EffectMethod,
     UsedImplicitly]
    public Task HandleNavigateToAuth(Refresh action, IDispatcher _)
    {
        this.navigationManager.Refresh(action.ForceLoad);

        return Task.CompletedTask;
    }
}