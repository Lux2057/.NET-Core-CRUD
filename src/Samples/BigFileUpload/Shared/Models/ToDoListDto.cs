namespace Samples.UploadBigFile.Shared;

#region << Using >>

using CRUD.DAL.Abstractions;

#endregion

public class ToDoListDto : IId<int>
{
    #region Properties

    public int Id { get; set; }

    public string Name { get; set; }

    public DateTime CrDt { get; set; }

    #endregion
}