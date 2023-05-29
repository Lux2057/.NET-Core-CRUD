namespace CRUD.WebAPI;

public record ValidationFailureResult(string Message, ValidationError[] Errors);