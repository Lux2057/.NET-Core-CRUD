namespace Samples.ToDo.Shared;

public abstract class ApiRoutesConst
{
    #region Constants

    public const string SignUp = "Auth/SignUp";

    public const string SignIn = "Auth/SignIn";

    public const string RefreshToken = "Auth/RefreshToken";

    public const string GetProjects = "Projects/Get";

    public const string CreateProject = "Projects/Create";

    public const string UpdateProject = "Projects/Update";

    public const string GetStatuses = "Statuses/Get";

    public const string CreateStatus = "Statuses/Create";

    public const string UpdateStatus = "Statuses/Update";

    public const string GetTags = "Tags/Get";

    public const string CreateTag = "Tags/Create";

    public const string GetTasks = "Tasks/Get";

    public const string CreateTask = "Tasks/Create";

    public const string UpdateTask = "Tasks/Update";

    #endregion

    #region Nested Classes

    public abstract class Params
    {
        #region Constants

        public const string SearchTerm = "searchTerm";

        public const string TagsIds = "tagsIds";

        #endregion
    }

    #endregion
}