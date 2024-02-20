namespace Samples.ToDo.UI;

#region << Using >>

using Extensions;
using Fluxor;
using JetBrains.Annotations;

#endregion

[FeatureState]
public class ValidationState
{
    #region Properties

    private readonly ValidationFailureResult commonFailure;

    readonly Dictionary<string, ValidationFailureResult> failsDict = new();

    #endregion

    #region Constructors

    [UsedImplicitly]
    ValidationState() { }

    public ValidationState(string key, ValidationFailureResult failure)
    {
        if (key.IsNullOrWhitespace())
        {
            this.commonFailure = failure;
        }
        else
        {
            if (this.failsDict.ContainsKey(key))
                this.failsDict[key] = failure;
            else
                this.failsDict.Add(key, failure);
        }
    }

    #endregion

    public string[] ValidationErrors(string key, string propertyName)
    {
        string[] getErrors(ValidationFailureResult failure)
        {
            return failure == null ?
                           Array.Empty<string>() :
                           failure.Errors?
                                  .Where(r => r.PropertyName == propertyName)
                                  .Select(r => r.Message)
                                  .ToArray() ?? Array.Empty<string>();
        }

        if (key.IsNullOrWhitespace())
            return getErrors(this.commonFailure);

        return !this.failsDict.ContainsKey(key) ? Array.Empty<string>() : getErrors(this.failsDict[key]);
    }
}