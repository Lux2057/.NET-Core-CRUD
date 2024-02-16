﻿namespace Samples.ToDo.UI;

#region << Using >>

using Extensions;
using Fluxor;
using JetBrains.Annotations;

#endregion

[FeatureState]
public class ValidationState
{
    #region Properties

    public ValidationFailureResult ValidationFailure { get; }

    public bool IsFailure => ValidationFailure != null &&
                             (ValidationFailure.Errors?.Any() == true ||
                              !ValidationFailure.Message.IsNullOrWhitespace());

    #endregion

    #region Constructors

    [UsedImplicitly]
    ValidationState()
    {
        ValidationFailure = null;
    }

    public ValidationState(ValidationFailureResult validationFailure)
    {
        ValidationFailure = validationFailure;
    }

    #endregion
}