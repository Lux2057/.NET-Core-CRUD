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

    readonly Dictionary<string, ValidationFailureResult> failsDict = new();

    #endregion

    #region Constructors

    [UsedImplicitly]
    ValidationState() { }

    public ValidationState(string key, ValidationFailureResult failure)
    {
        if (key.IsNullOrWhitespace())
            return;

        if (this.failsDict.ContainsKey(key))
            this.failsDict[key] = failure;
        else
            this.failsDict.Add(key, failure);
    }

    #endregion

    public string SummaryMessage(string key)
    {
        if (key.IsNullOrWhitespace() || !this.failsDict.ContainsKey(key))
            return string.Empty;

        return this.failsDict[key]?.SummaryMessage;
    }

    public string[] ValidationErrors(string key)
    {
        if (key.IsNullOrWhitespace() || !this.failsDict.ContainsKey(key))
            return Array.Empty<string>();

        return this.failsDict[key]?.Errors?
                   .Select(r => r.Message)
                   .ToArray() ?? Array.Empty<string>();
    }

    public string[] ValidationErrors(string key, string propertyName)
    {
        if (key.IsNullOrWhitespace() || !this.failsDict.ContainsKey(key))
            return Array.Empty<string>();

        return this.failsDict[key]?.Errors?.Where(r => r.PropertyName == propertyName)
                   .Select(r => r.Message)
                   .ToArray() ?? Array.Empty<string>();
    }
}