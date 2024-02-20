namespace Samples.ToDo.UI;

public class ValidationException : Exception
{
    #region Properties

    public ValidationFailureResult ValidationFailure { get; }

    #endregion

    #region Constructors

    public ValidationException(ValidationFailureResult validationFailure)
    {
        ValidationFailure = validationFailure;
    }

    #endregion
}