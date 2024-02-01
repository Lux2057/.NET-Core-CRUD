namespace Samples.ToDo.API;

#region << Using >>

using FluentValidation;
using JetBrains.Annotations;

#endregion

public class ProjectDto
{
    #region Properties

    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public TagDto[] Tags { get; set; }

    #endregion

    #region Nested Classes

    [UsedImplicitly]
    public class Validator : AbstractValidator<ProjectDto>
    {
        #region Constructors

        public Validator()
        {
            RuleFor(r => r.Id).NotEmpty();
            RuleFor(r => r.Name).NotEmpty();
            RuleFor(r => r.Description).NotEmpty();
        }

        #endregion
    }

    #endregion
}