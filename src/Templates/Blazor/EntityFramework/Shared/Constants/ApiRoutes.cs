namespace Templates.Blazor.EF.Shared;

public static class ApiRoutes
{
    #region Constants

    public const string ReadToDoLists = "ToDoLists/Read";

    public const string CreateOrUpdateToDoList = "ToDoLists/CreatOrUpdate";

    public const string DeleteToDoList = "ToDoLists/Delete";

    public const string ReadToDoListItems = "ToDoListItems/Read";

    public const string CreateOrUpdateToDoListItem = "ToDoListItems/CreatOrUpdate";

    public const string DeleteToDoListItem = "ToDoListItems/Delete";

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

        #endregion
    }

    #endregion
}