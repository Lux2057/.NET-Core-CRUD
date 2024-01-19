namespace Samples.UploadBigFile.API;

#region << Using >>

using CRUD.CQRS;
using CRUD.DAL.Abstractions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

#endregion

public class DeleteToDoListCommand : CommandBase
{
    #region Properties

    public int Id { get; init; }

    public new bool Result { get; set; }

    #endregion

    #region Nested Classes

    [UsedImplicitly]
    private class Handler : CommandHandlerBase<DeleteToDoListCommand>
    {
        #region Constructors

        public Handler(IServiceProvider serviceProvider) : base(serviceProvider) { }

        #endregion

        protected override async Task Execute(DeleteToDoListCommand command, CancellationToken cancellationToken)
        {
            var toDoList = await Repository.Read(new FindEntityByIntId<ToDoListEntity>(command.Id)).SingleOrDefaultAsync();

            if (toDoList == null)
            {
                command.Result = false;
                return;
            }

            var toDoListItems = await Repository.Read(new ToDoListIdProp.FindByValue<ToDoListItemEntity>(command.Id)).ToArrayAsync();

            await Repository.DeleteAsync(toDoListItems, cancellationToken);

            await Repository.DeleteAsync(toDoList, cancellationToken);

            command.Result = true;
        }
    }

    #endregion
}