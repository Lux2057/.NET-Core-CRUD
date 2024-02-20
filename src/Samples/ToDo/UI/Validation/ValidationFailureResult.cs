namespace Samples.ToDo.UI;

public record ValidationFailureResult(string Message, ValidationError[] Errors);