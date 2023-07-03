namespace Templates.Blazor.NH.API;

#region << Using >>

using CRUD.CQRS;
using CRUD.DAL.Abstractions;
using JetBrains.Annotations;
using NHibernate.Linq;
using Templates.Blazor.NH.Shared;

#endregion

public class CreateOrUpdateToDoListCommand : CommandBase
{
    #region Properties

    public ToDoListDto Dto { get; init; }

    public new int Result { get; set; }

    #endregion

    #region Nested Classes

    [UsedImplicitly]
    class Handler : CommandHandlerBase<CreateOrUpdateToDoListCommand>
    {
        #region Constructors

        public Handler(IServiceProvider serviceProvider) : base(serviceProvider) { }

        #endregion

        protected override async Task Execute(CreateOrUpdateToDoListCommand command, CancellationToken cancellationToken)
        {
            var toDoList = await Repository.Read(new FindEntityByIntId<ToDoListEntity>(command.Dto.Id)).SingleOrDefaultAsync(cancellationToken);

            var isNew = false;
            if (toDoList == null)
            {
                isNew = true;
                toDoList = new ToDoListEntity { CrDt = DateTime.UtcNow };
            }

            toDoList.Name = command.Dto.Name;

            if (isNew)
                await Repository.CreateAsync(toDoList, cancellationToken);
            else
                await Repository.UpdateAsync(toDoList, cancellationToken);

            command.Result = toDoList.Id;
        }
    }

    #endregion
}