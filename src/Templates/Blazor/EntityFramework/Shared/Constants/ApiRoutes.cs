namespace Templates.Blazor.EF.Shared;

public static class ApiRoutes
{
    #region Constants

    public const string ReadToDoLists = "ToDoLists/Read";

    public const string CreateOrUpdateToDoLists = "ToDoLists/CreatOrUpdate";

    public const string DeleteToDoLists = "ToDoLists/Delete";

    public const string ReadToDoListItems = "ToDoListItems/Read";

    public const string CreateOrUpdateToDoListItems = "ToDoListItems/CreatOrUpdate";

    public const string DeleteToDoListItems = "ToDoListItems/Delete";

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
        ///     Type: int?
        /// </summary>
        public const string page = "page";

        /// <summary>
        ///     Type: int?
        /// </summary>
        public const string pageSize = "pageSize";

        /// <summary>
        ///     Type: workflow specific Dto model
        /// </summary>
        public const string dtos = "dtos";

        #endregion
    }

    #endregion
}