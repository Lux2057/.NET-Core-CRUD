namespace Samples.ToDo.UI;

public static class UiRoutes
{
    #region Constants

    public const string Projects = "";

    public const string Tasks = "tasks/{ProjectId:int}";

    public const string About = "about";

    public const string DragulaTestPage = "dragula";

    public const string Auth = "auth";

    #endregion

    public static string TasksRoute(int projectId)
    {
        return $"tasks/{projectId}";
    }
}