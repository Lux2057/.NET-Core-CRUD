namespace Samples.ToDo.API;

#region << Using >>

using CRUD.CQRS;
using CRUD.DAL.Abstractions;
using FluentValidation;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Samples.ToDo.Shared;

#endregion

public class CreateOrUpdateStatusCommand : CommandBase
{
    #region Properties

    public int? Id { get; }

    public int UserId { get; }

    public string Name { get; }

    #endregion

    #region Constructors

    public CreateOrUpdateStatusCommand(int? id, int userId, string name)
    {
        Id = id;
        UserId = userId;
        Name = name.Trim();
    }

    #endregion

    #region Nested Classes

    [UsedImplicitly]
    public class Validator : AbstractValidator<CreateOrUpdateStatusCommand>
    {
        #region Constructors

        public Validator(IDispatcher dispatcher)
        {
            RuleFor(r => r.UserId).NotEmpty()
                                  .MustAsync((id, _) => dispatcher.QueryAsync(new DoesUserExistQuery(id)))
                                  .WithMessage(ValidationMessagesConst.Invalid_user_id);

            RuleFor(r => r.Name).NotEmpty()
                                .MustAsync((command, _, _) => dispatcher.QueryAsync(new IsNameUniqueQuery<StatusEntity>(command.Id, command.UserId, command.Name)))
                                .WithMessage(ValidationMessagesConst.Name_is_not_unique);
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
                status = new StatusEntity { User = user };

            status.Name = command.Name;

            if (isNew)
                await Repository.CreateAsync(status, cancellationToken);
            else
                await Repository.UpdateAsync(status, cancellationToken);
        }
    }

    #endregion
}