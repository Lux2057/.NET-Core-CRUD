namespace Templates.Blazor.EF.UI;

public static class UiRoutes
{
    #region Constants

    public const string ToDoLists = "";

    public const string ToDoList = "toDoList/{Id:int}";

    public const string About = "about";

    #endregion

    public static string ToDoListRoute(int id)
    {
        return $"toDoList/{id}";
    }
}