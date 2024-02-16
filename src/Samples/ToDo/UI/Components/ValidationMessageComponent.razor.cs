namespace Samples.ToDo.UI.Components;

#region << Using >>

using Microsoft.AspNetCore.Components;

#endregion

public partial class ValidationMessageComponent : ComponentBase<ValidationState>
{
    #region Properties

    [Parameter, EditorRequired]
    public string Name { get; set; }

    public string[] Messages
    {
        get
        {
            var messages = State.IsFailure ?
                                   State.ValidationFailure
                                        .Errors?
                                        .Where(r => r.PropertyName == Name)
                                        .Select(r => r.Message).ToArray() ?? Array.Empty<string>() :
                                   Array.Empty<string>();

            return messages;
        }
    }

    #endregion
}