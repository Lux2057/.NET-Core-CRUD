﻿namespace Samples.ToDo.API.Projects;

#region << Using >>

using CRUD.CQRS;
using CRUD.DAL.Abstractions;
using FluentValidation;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

#endregion

public class CreateOrUpdateProjectCommand : CommandBase
{
    #region Properties

    public int? Id { get; }

    public int UserId { get; }

    public string Name { get; }

    public string Description { get; }

    #endregion

    #region Constructors

    public CreateOrUpdateProjectCommand(int? id, int userId, string name, string description)
    {
        Id = id;
        UserId = userId;
        Name = name.Trim();
        Description = description.Trim();
    }

    #endregion

    #region Nested Classes

    [UsedImplicitly]
    public class Validator : AbstractValidator<CreateOrUpdateProjectCommand>
    {
        #region Constructors

        public Validator(IDispatcher dispatcher)
        {
            RuleFor(r => r.UserId).NotEmpty()
                                  .MustAsync((id, _) => dispatcher.QueryAsync(new DoesUserExistQuery(id)))
                                  .WithMessage(ValidationMessagesConst.Invalid_user_id);

            RuleFor(r => r.Name).NotEmpty()
                                .MustAsync((command, _, _) => dispatcher.QueryAsync(new IsNameUniqueQuery<ProjectEntity>(command.Id, command.UserId, command.Name)))
                                .WithMessage(ValidationMessagesConst.Name_is_not_unique);

            RuleFor(r => r.Description).NotEmpty();
        }

        #endregion
    }

    [UsedImplicitly]
    class Handler : CommandHandlerBase<CreateOrUpdateProjectCommand>
    {
        #region Constructors

        public Handler(IServiceProvider serviceProvider) : base(serviceProvider) { }

        #endregion

        protected override async Task Execute(CreateOrUpdateProjectCommand command, CancellationToken cancellationToken)
        {
            var user = await Repository.Read(new FindEntityByIntId<UserEntity>(command.UserId)).SingleAsync(cancellationToken);

            var project = command.Id == null ?
                                  null :
                                  await Repository.Read(new FindEntityByIntId<ProjectEntity>(command.Id.Value)).SingleOrDefaultAsync(cancellationToken);

            var isNew = project == null;
            if (isNew)
                project = new ProjectEntity { User = user };

            project.Name = command.Name;
            project.Description = command.Description;
            project.UpDt = DateTime.UtcNow;

            if (isNew)
                await Repository.CreateAsync(project, cancellationToken);
            else
                await Repository.UpdateAsync(project, cancellationToken);
        }
    }

    #endregion
}