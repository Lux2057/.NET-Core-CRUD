namespace Samples.ToDo.API.Projects;

#region << Using >>

using CRUD.CQRS;
using CRUD.DAL.Abstractions;
using Extensions;
using FluentValidation;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Samples.ToDo.API.Resources;

#endregion

public class CreateOrUpdateProjectCommand : CommandBase
{
    #region Properties

    public int? Id { get; }

    public int UserId { get; }

    public string Name { get; }

    public string Description { get; }

    public int[] TagsIds { get; }

    public new int Result { get; set; }

    #endregion

    #region Constructors

    public CreateOrUpdateProjectCommand(int? id,
                                        int userId,
                                        string name,
                                        string description,
                                        IEnumerable<int> tagsIds)
    {
        Id = id;
        UserId = userId;
        Name = name.Trim();
        Description = description.Trim();
        TagsIds = tagsIds.ToDistinctArrayOrEmpty();
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

            RuleFor(r => r.UserId).NotEmpty()
                                  .MustAsync((id, _) => dispatcher.QueryAsync(new DoesEntityExistQuery<UserEntity>(id)))
                                  .WithMessage(Localization.User_id_is_invalid);

            RuleFor(r => r.Name).NotEmpty()
                                .MustAsync((command, _, _) => dispatcher.QueryAsync(new IsNameUniqueQuery<ProjectEntity>(command.Id, command.UserId, command.Name)))
                                .WithMessage(Localization.Name_is_not_unique);

            RuleFor(r => r.Description).NotEmpty();

            RuleFor(r => r.TagsIds).NotEmpty().Must(ids => ids.Length <= 5).WithMessage(Localization.Tags_count_cant_be_more_than_5);
            RuleForEach(r => r.TagsIds).MustAsync((id, _) => dispatcher.QueryAsync(new DoesEntityExistQuery<TagEntity>(id)))
                                       .WithMessage((_, id) => $"{Localization.Tag_id_is_invalid}: {id}");
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
            {
                await Repository.CreateAsync(project, cancellationToken);

                await Repository.CreateAsync(command.TagsIds.Select(tagId => new ProjectToTagEntity
                                                                             {
                                                                                     UserId = command.UserId,
                                                                                     TagId = tagId,
                                                                                     ProjectId = project.Id
                                                                             }), cancellationToken);
            }
            else
            {
                await Repository.UpdateAsync(project, cancellationToken);

                var existingTagsMaps = await Repository.Read(new UserIdProp.FindBy.EqualTo<ProjectToTagEntity>(command.UserId) &&
                                                             new ProjectIdProp.FindBy.EqualTo<ProjectToTagEntity>(project.Id))
                                                       .Select(r => new
                                                                    {
                                                                            r.Id,
                                                                            r.TagId
                                                                    }).ToArrayAsync(cancellationToken);

                var existingTagsIds = existingTagsMaps.Select(r => r.TagId).ToArray();
                var tagsIdsToCreate = command.TagsIds.Where(tagId => !existingTagsIds.Contains(tagId)).ToArray();

                await Repository.CreateAsync(tagsIdsToCreate.Select(tagId => new ProjectToTagEntity
                                                                             {
                                                                                     UserId = command.UserId,
                                                                                     ProjectId = project.Id,
                                                                                     TagId = tagId
                                                                             }), cancellationToken);

                var tagsMapsToDelete = existingTagsMaps.Where(r => !command.TagsIds.Contains(r.TagId)).ToArray();
                var tagsToDelete = await Repository.Read(new FindEntitiesByIds<ProjectToTagEntity, int>(tagsMapsToDelete.Select(r => r.Id))).ToArrayAsync(cancellationToken);

                await Repository.DeleteAsync(tagsToDelete, cancellationToken);
            }

            command.Result = project.Id;
        }
    }

    #endregion
}