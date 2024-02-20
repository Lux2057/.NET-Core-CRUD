namespace Samples.ToDo.UI;

#region << Using >>

using Microsoft.JSInterop;

#endregion

public static class JsExt
{
    public static async Task CloseModal(this IJSRuntime js, string modalId)
    {
        await js.InvokeVoidAsync("closeModal", modalId);
    }
}