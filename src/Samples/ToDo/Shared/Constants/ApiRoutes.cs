namespace Samples.ToDo.Shared;

public static class ApiRoutes
{
    #region Constants

    /// <summary>
    ///     HttpMethod.POST
    /// </summary>
    public const string SignUp = "Auth/SignUp";

    /// <summary>
    ///     HttpMethod.POST
    /// </summary>
    public const string SignIn = "Auth/SignIn";

    /// <summary>
    ///     HttpMethod.POST
    /// </summary>
    public const string RefreshToken = "Auth/RefreshToken";

    /// <summary>
    ///     HttpMethod.GET
    /// </summary>
    public const string ReadProjects = "Projects/Read";

    /// <summary>
    ///     HttpMethod.POST
    /// </summary>
    public const string CreateOrUpdateProject = "Projects/CreateOrUpdate";

    /// <summary>
    ///     HttpMethod.GET
    /// </summary>
    public const string ReadStatuses = "Statuses/Read";

    /// <summary>
    ///     HttpMethod.POST
    /// </summary>
    public const string CreateOrUpdateStatus = "Statuses/CreateOrUpdate";

    /// <summary>
    ///     HttpMethod.GET
    /// </summary>
    public const string ReadTags = "Tags/Read";

    /// <summary>
    ///     HttpMethod.POST
    /// </summary>
    public const string CreateTag = "Tags/Create";

    /// <summary>
    ///     HttpMethod.GET
    /// </summary>
    public const string ReadTasks = "Tasks/Read";

    /// <summary>
    ///     HttpMethod.POST
    /// </summary>
    public const string CreateOrUpdateTask = "Tasks/CreateOrUpdate";

    /// <summary>
    ///     HttpMethod.PUT
    /// </summary>
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