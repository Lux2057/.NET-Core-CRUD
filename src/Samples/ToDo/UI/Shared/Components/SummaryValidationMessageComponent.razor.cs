namespace Samples.ToDo.UI.Shared.Components;

#region << Using >>

using Extensions;
using Microsoft.AspNetCore.Components;

#endregion

public partial class SummaryValidationMessageComponent<TRequest> : ComponentBase<ValidationState>
{
    #region Properties

    [Parameter]
    public string Key { get; set; }

    private string key => Key.IsNullOrWhitespace() ? typeof(TRequest).Name : Key;

    private string message => State?.SummaryMessage(key);

    #endregion
}