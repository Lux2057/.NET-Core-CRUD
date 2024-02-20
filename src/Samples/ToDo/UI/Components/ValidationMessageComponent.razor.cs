namespace Samples.ToDo.UI.Components;

#region << Using >>

using Microsoft.AspNetCore.Components;

#endregion

public partial class ValidationMessageComponent : ComponentBase<ValidationState>
{
    #region Properties

    [Parameter]
    public string Key { get; set; }

    [Parameter, EditorRequired]
    public string Name { get; set; }

    public string[] Messages => State.ValidationErrors(Key, Name);

    #endregion
}