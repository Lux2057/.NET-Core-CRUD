namespace WebAPI.API;

#region << Using >>

using CRUD.DAL.Abstractions;

#endregion

public class SampleDto : IId<int>
{
    #region Properties

    public int Id { get; set; }

    public string Text { get; set; }

    #endregion
}