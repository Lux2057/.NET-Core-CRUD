namespace Samples.ToDo.UI;

#region << Using >>

using Extensions;

#endregion

public abstract record ValidatingAction<TRequest>
{
    #region Properties

    public TRequest Request { get; }

    public string ValidationKey { get; }

    #endregion

    #region Constructors

    protected ValidatingAction(TRequest request, string validationKey = default)
    {
        Request = request;
        ValidationKey = validationKey.IsNullOrWhitespace() ? typeof(TRequest).Name : validationKey;
    }

    #endregion
}