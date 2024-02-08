namespace Samples.ToDo.Shared;

public static class ApiRoutes
{
    #region Constants

    public const string ReadToDoLists = "ToDoLists/Read";

    public const string CreateOrUpdateToDoList = "ToDoLists/CreatOrUpdate";

    public const string DeleteToDoList = "ToDoLists/Delete";

    public const string ReadToDoListItems = "ToDoListItems/Read";

    public const string CreateOrUpdateToDoListItem = "ToDoListItems/CreatOrUpdate";

    public const string DeleteToDoListItem = "ToDoListItems/Delete";

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

    public const string SetTaskStatus = "Tasks/SetStatus";

    #endregion

    #region Nested Classes

    public static class Params
    {
        #region Constants

        /// <summary>
        ///     Type: int[]
        /// </summary>
        public const string ids = "ids";

        /// <summary>
        ///     Type: int
        /// </summary>
        public const string id = "id";

        /// <summary>
        ///     Type: int?
        /// </summary>
        public const string page = "page";

        /// <summary>
        ///     Type: int?
        /// </summary>
        public const string pageSize = "pageSize";

        /// <summary>
        ///     Type: int
        /// </summary>
        public const string toDoListId = "toDoListId";

        /// <summary>
        ///     Type: string
        /// </summary>
        public const string SearchTerm = "searchTerm";

        /// <summary>
        ///     Type: int[]
        /// </summary>
        public const string TagsIds = "tagsIds";

        /// <summary>
        ///     Type: int
        /// </summary>
        public const string ProjectId = "projectId";

        #endregion
    }

    #endregion
}