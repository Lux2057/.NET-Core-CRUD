namespace Samples.ToDo.API;

using FluentValidation;
using JetBrains.Annotations;

public class StatusDto
{
    #region Properties

    public string Id { get; set; }

    public string Name { get; set; }

    #endregion

    [UsedImplicitly]
    public class Validator : AbstractValidator<StatusDto>
    {
        #region Constructors

        public Validator()
        {
            RuleFor(r => r.Id).NotEmpty();
            RuleFor(r => r.Name).NotEmpty();
        }

        #endregion
    }
}