namespace Samples.ToDo.API;

#region << Using >>

using FluentValidation;
using JetBrains.Annotations;

#endregion

public class TagDto
{
    #region Properties

    public int Id { get; set; }

    public string Name { get; set; }

    #endregion

    #region Nested Classes

    [UsedImplicitly]
    public class Validator : AbstractValidator<TagDto>
    {
        #region Constructors

        public Validator()
        {
            RuleFor(r => r.Id).NotEmpty();
            RuleFor(r => r.Name).NotEmpty();
        }

        #endregion
    }

    #endregion
}