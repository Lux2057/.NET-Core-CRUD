namespace Samples.ToDo.API;

public class ProjectDto
{
    #region Properties

    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public TagDto[] Tags { get; set; }

    #endregion
}