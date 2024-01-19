namespace Samples.UploadBigFile.API;

#region << Using >>

using CRUD.CQRS;
using CRUD.DAL.Abstractions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Samples.UploadBigFile.Shared;

#endregion

public class CreateOrUpdateToDoListItemCommand : CommandBase
{
    #region Properties

    public ToDoListItemDto Dto { get; init; }

    public new int? Result { get; set; }

    #endregion

    #region Nested Classes

    [UsedImplicitly]
    private class Handler : CommandHandlerBase<CreateOrUpdateToDoListItemCommand>
    {
        #region Constructors

        public Handler(IServiceProvider serviceProvider) : base(serviceProvider) { }

        #endregion

        protected override async Task Execute(CreateOrUpdateToDoListItemCommand command, CancellationToken cancellationToken)
        {
            var item = await Repository.Read(new FindEntityByIntId<ToDoListItemEntity>(command.Dto.Id)).SingleOrDefaultAsync(cancellationToken);

            var isNew = item == null;

            if (isNew)
            {
                var toDoListExists = await Repository.Read(new FindEntityByIntId<ToDoListEntity>(command.Dto.ToDoListId)).AnyAsync(cancellationToken);
                if (!toDoListExists)
                {
                    command.Result = null;
                    return;
                }

                item = new ToDoListItemEntity { ToDoListId = command.Dto.ToDoListId };
            }

            item.Description = command.Dto.Description.Trim();
            item.Status = command.Dto.Status;

            if (isNew)
                await Repository.CreateAsync(item, cancellationToken);
            else
                await Repository.UpdateAsync(item, cancellationToken);

            command.Result = item.Id;
        }
    }

    #endregion
}