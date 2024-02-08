﻿namespace Samples.ToDo.API;

#region << Using >>

using CRUD.CQRS;
using FluentValidation;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Samples.ToDo.Shared;
using Samples.ToDo.Shared.Resources;

#endregion

public class SignUpCommand : CommandBase
{
    #region Properties

    public string UserName { get; }

    public string Password { get; }

    public new AuthResultDto AuthResultDto { get; set; }

    #endregion

    #region Constructors

    public SignUpCommand(string userName, string password)
    {
        UserName = userName;
        Password = password;
    }

    #endregion

    #region Nested Classes

    [UsedImplicitly]
    public class Validator : AbstractValidator<SignUpCommand>
    {
        #region Constructors

        public Validator(IDispatcher dispatcher)
        {
            RuleFor(r => r.UserName).NotEmpty().MinimumLength(6).MaximumLength(30)
                                    .MustAsync((userName, _) => dispatcher.QueryAsync(new IsUserNameUniqueQuery(userName)))
                                    .WithMessage(Localization.User_name_is_not_unique);

            RuleFor(r => r.Password).NotEmpty().MinimumLength(6).MaximumLength(30);
        }

        #endregion
    }

    [UsedImplicitly]
    class Handler : CommandHandlerBase<SignUpCommand>
    {
        #region Constructors

        public Handler(IServiceProvider serviceProvider) : base(serviceProvider) { }

        #endregion

        protected override async Task Execute(SignUpCommand command, CancellationToken cancellationToken)
        {
            var userEntity = new UserEntity { UserName = command.UserName };
            userEntity.PasswordHash = new PasswordHasher<UserEntity>().HashPassword(userEntity, command.Password);

            await Repository.CreateAsync(userEntity);

            var signInCommand = new SignInCommand(command.UserName, command.Password);
            await Dispatcher.PushAsync(signInCommand);

            command.AuthResultDto = signInCommand.AuthResultDto;
        }
    }

    #endregion
}