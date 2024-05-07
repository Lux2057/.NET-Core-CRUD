namespace Samples.ToDo.UI;

public record ValidationFailureResult(string SummaryMessage, ValidationError[] Errors);