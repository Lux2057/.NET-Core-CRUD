﻿namespace Samples.ToDo.UI.Pages;

#region << Using >>

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

#endregion

[Route(UiRoutes.DragulaTestPage), Authorize]
public partial class DragulaTestPage : PageBase<DragulaTestState>
{
    #region Constants

    private const string todoId = "todo";

    private const string inprogressId = "inprogress";

    private const string completedId = "completed";

    #endregion

    #region Properties

    private DotNetObjectReference<DragulaTestPage> objRef;

    #endregion

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        this.objRef = DotNetObjectReference.Create(this);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
            await JS.InvokeVoidAsync("initDragula", this.objRef, new[] { todoId, inprogressId, completedId }, nameof(DropCallback));

        await base.OnAfterRenderAsync(firstRender);
    }

    [JSInvokable]
    public void DropCallback(string uid, string source, string target)
    {
        Dispatcher.Dispatch(new SetDragulaMessageWf.InitAction($"{uid} has been dropped from {source} to {target}"));
    }

    protected override void Dispose(bool disposing)
    {
        this.objRef?.Dispose();

        base.Dispose(disposing);
    }
}