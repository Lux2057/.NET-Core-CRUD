namespace Templates.Blazor.EF.Shared;

#region << Using >>

using CRUD.DAL.Abstractions;

#endregion

public class ToDoListDto : IId<int>
{
    #region Properties

    public int Id { get; set; }

    public string Name { get; set; }

    #endregion
}