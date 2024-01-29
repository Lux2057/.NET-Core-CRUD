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

    [UsedImplicitly]
    public class Validator : AbstractValidator<TaskDto>
    {
        #region Constructors

        public Validator()
        {
            RuleFor(r => r.Id).NotEmpty();
            RuleFor(r => r.Name).NotEmpty();
            RuleFor(r => r.Description).NotEmpty();
            RuleFor(r => r.StatusId).NotEmpty();
        }

        #endregion
    }
}