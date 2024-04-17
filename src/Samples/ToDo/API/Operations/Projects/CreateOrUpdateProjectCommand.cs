namespace Samples.ToDo.API.Projects;

#region << Using >>

using CRUD.CQRS;
using CRUD.DAL.Abstractions;
using Extensions;
using FluentValidation;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Samples.ToDo.API.Resources;
using Samples.ToDo.Shared;

#endregion

public class CreateOrUpdateProjectCommand : CommandBase, ICreateOrUpdateProjectRequest
{
    #region Properties

    public int? Id { get; }

    public int UserId { get; }

    public string Name { get; }

    public string Description { get; }

    public new bool Result { get; set; }

    #endregion

    #region Constructors

    public CreateOrUpdateProjectCommand(int? id,
                                        int userId,
                                        string name,
                                        string description)
    {
        Id = id;
        UserId = userId;
        Name = name?.Trim() ?? string.Empty;
        Description = description?.Trim() ?? string.Empty;
    }

    #endregion

    #region Nested Classes

    [UsedImplicitly]
    public class Validator : AbstractValidator<CreateOrUpdateProjectCommand>
    {
        #region Constructors

        public Validator(IDispatcher dispatcher)
        {
            When(r => r.Id != null,
                 () =>
                 {
                     RuleFor(r => r.Id).MustAsync((id, _) => dispatcher.QueryAsync(new DoesEntityExistQuery<ProjectEntity>(id.Value)))
                                       .WithMessage(Localization.Project_id_is_invalid);
                 });

            RuleFor(r => r.UserId).NotEmpty().WithMessage(Localization.User_id_cant_be_empty)
                                  .MustAsync((id, _) => dispatcher.QueryAsync(new DoesEntityExistQuery<UserEntity>(id)))
                                  .WithMessage(Localization.User_id_is_invalid);

            RuleFor(r => r.Name).NotEmpty().WithMessage(Localization.Name_cant_be_empty)
                                .MustAsync((command, _, _) => dispatcher.QueryAsync(new IsNameUniqueQuery<ProjectEntity>(command.Id, command.UserId, command.Name)))
                                .WithMessage(Localization.Name_is_not_unique);

            RuleFor(r => r.Description).NotEmpty().WithMessage(Localization.Description_cant_be_empty);
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
            var project = command.Id == null ?
                                  null :
                                  await Repository.Read(new FindEntityByIntId<ProjectEntity>(command.Id.Value)).SingleOrDefaultAsync(cancellationToken);

            var isNew = project == null;
            if (isNew)
                project = new ProjectEntity { UserId = command.UserId };

            project.Name = command.Name;
            project.Description = command.Description;
            project.UpDt = DateTime.UtcNow;

            if (isNew)
                await Repository.CreateAsync(project, cancellationToken);
            else
                await Repository.UpdateAsync(project, cancellationToken);

            command.Result = true;
        }
    }

    #endregion
}