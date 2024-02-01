namespace Samples.ToDo.API;

using FluentValidation;
using JetBrains.Annotations;

public class TaskDto
{
    #region Properties

    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public DateTime? DueDate { get; set; }

    public int StatusId { get; set; }

    #endregion
}