namespace CRUD.MVC;

public record ValidationFailureResult(string Message, ValidationError[] Errors);