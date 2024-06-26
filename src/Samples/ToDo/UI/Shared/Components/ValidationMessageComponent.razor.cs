﻿namespace Samples.ToDo.UI.Shared.Components;

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

    private string key => Key.IsNullOrWhitespace() ? typeof(TRequest).Name : Key;

    private string name
    {
        get
        {
            var body = Name.Body as MemberExpression ?? ((UnaryExpression)Name.Body).Operand as MemberExpression;

            return body?.Member.Name;
        }
    }

    private string[] messages => State.ValidationErrors(key, name);

    #endregion
}