namespace Samples.ToDo.UI.Shared.Components;

#region << Using >>

using System.Linq.Expressions;
using Extensions;
using Microsoft.AspNetCore.Components;

#endregion

public partial class ValidationMessageComponent<TRequest> : ComponentBase<ValidationState>
{
    #region Properties

    [Parameter]
    public string Key { get; set; }

    [Parameter, EditorRequired]
    public Expression<Func<TRequest, object>> Name { get; set; }

    private string validationKey => Key.IsNullOrWhitespace() ? typeof(TRequest).Name : Key;

    private string validationName => Name.GetPropertyInfo()?.Name;

    public string[] Messages => State.ValidationErrors(validationKey, validationName);

    #endregion
}