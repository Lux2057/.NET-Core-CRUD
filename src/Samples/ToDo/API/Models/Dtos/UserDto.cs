namespace Samples.ToDo.API;

#region << Using >>

using FluentValidation;
using JetBrains.Annotations;

#endregion

public class UserDto
{
    #region Properties

    public int Id { get; set; }

    public string Login { get; set; }

    #endregion

    #region Nested Classes

    [UsedImplicitly]
    public class Validator : AbstractValidator<UserDto>
    {
        #region Constructors

        public Validator()
        {
            RuleFor(r => r.Id).NotEmpty();
            RuleFor(r => r.Login).NotEmpty();
        }

        #endregion
    }

    #endregion
}