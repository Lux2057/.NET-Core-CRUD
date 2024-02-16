namespace Samples.ToDo.UI;

public static class UiRoutes
{
    #region Constants

    public const string ToDoLists = "";

    public const string ToDoList = "toDoList/{Id:int}";

    public const string About = "about";

    public const string DragulaTestPage = "dragula";

    public const string Auth = "auth";

    #endregion

    public static string ToDoListRoute(int id)
    {
        return $"toDoList/{id}";
    }
}