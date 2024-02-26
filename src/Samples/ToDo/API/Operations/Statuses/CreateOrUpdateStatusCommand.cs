namespace Samples.ToDo.API;

#region << Using >>

using CRUD.CQRS;
using CRUD.DAL.Abstractions;
using FluentValidation;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Samples.ToDo.API.Resources;
using Samples.ToDo.Shared;

#endregion

public class CreateOrUpdateStatusCommand : CommandBase, ICreateOrUpdateStatusRequest
{
    #region Properties

    public int? Id { get; }

    public int UserId { get; }

    public string Name { get; }

    public new bool Result { get; set; }

    #endregion

    #region Constructors

    public CreateOrUpdateStatusCommand(int? id,
                                       int userId,
                                       string name)
    {
        Id = id;
        UserId = userId;
        Name = name?.Trim() ?? string.Empty;
    }

    #endregion

    #region Nested Classes

    [UsedImplicitly]
    public class Validator : AbstractValidator<CreateOrUpdateStatusCommand>
    {
        #region Constructors

        public Validator(IDispatcher dispatcher)
        {
            When(r => r.Id != null,
                 () =>
                 {
                     RuleFor(r => r.Id).MustAsync((id, _) => dispatcher.QueryAsync(new DoesEntityExistQuery<StatusEntity>(id.Value)))
                                       .WithMessage(Localization.Status_id_is_invalid);
                 });

            RuleFor(r => r.UserId).NotEmpty().WithMessage(Localization.User_id_cant_be_empty)
                                  .MustAsync((id, _) => dispatcher.QueryAsync(new DoesEntityExistQuery<UserEntity>(id)))
                                  .WithMessage(Localization.User_id_is_invalid);

            RuleFor(r => r.Name).NotEmpty().WithMessage(Localization.Name_cant_be_empty)
                                .MustAsync((command, _, _) => dispatcher.QueryAsync(new IsNameUniqueQuery<StatusEntity>(command.Id, command.UserId, command.Name)))
                                .WithMessage(Localization.Name_is_not_unique);
        }

        #endregion
    }

    [UsedImplicitly]
    class Handler : CommandHandlerBase<CreateOrUpdateStatusCommand>
    {
        #region Constructors

        public Handler(IServiceProvider serviceProvider) : base(serviceProvider) { }

        #endregion

        protected override async Task Execute(CreateOrUpdateStatusCommand command, CancellationToken cancellationToken)
        {
            var user = await Repository.Read(new FindEntityByIntId<UserEntity>(command.UserId)).SingleAsync(cancellationToken);

            var status = command.Id == null ?
                                 null :
                                 await Repository.Read(new FindEntityByIntId<StatusEntity>(command.Id.Value)).SingleOrDefaultAsync(cancellationToken);

            var isNew = status == null;
            if (isNew)
                status = new StatusEntity { UserId = user.Id };

            status.Name = command.Name;

            if (isNew)
                await Repository.CreateAsync(status, cancellationToken);
            else
                await Repository.UpdateAsync(status, cancellationToken);

            command.Result = true;
        }
    }

    #endregion
}