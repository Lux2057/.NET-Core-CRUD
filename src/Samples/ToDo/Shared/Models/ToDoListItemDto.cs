namespace Samples.ToDo.Shared;

#region << Using >>

using CRUD.DAL.Abstractions;

#endregion

public class ToDoListItemDto : IId<int>
{
    #region Properties

    public int Id { get; set; }

    public string Description { get; set; }

    public ItemStatus Status { get; set; }

    public int ToDoListId { get; set; }

    #endregion
}