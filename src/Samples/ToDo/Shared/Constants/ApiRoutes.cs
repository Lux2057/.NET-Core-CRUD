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
    ///     HttpMethod.DELETE
    /// </summary>
    public const string DeleteProject = "Projects/Delete";

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

    /// <summary>
    ///     HttpMethod.DELETE
    /// </summary>
    public const string DeleteTask = "Tasks/Delete";

    #endregion

    #region Nested Classes

    public static class Params
    {
        #region Constants

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
        public const string ProjectId = "projectId";

        #endregion
    }

    #endregion
}