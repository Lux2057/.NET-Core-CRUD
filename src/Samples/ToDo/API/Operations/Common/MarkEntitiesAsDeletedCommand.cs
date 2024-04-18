namespace Samples.ToDo.API;

#region << Using >>

using CRUD.CQRS;
using CRUD.DAL.Abstractions;
using Extensions;
using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;

#endregion

public class MarkEntitiesAsDeletedCommand<TEntity> : CommandBase where TEntity : EntityBase, new()
{
    #region Properties

    public int[] Ids { get; }

    public new bool Result { get; set; }

    #endregion

    #region Constructors

    public MarkEntitiesAsDeletedCommand(IEnumerable<int> ids)
    {
        Ids = ids.ToDistinctArrayOrEmpty();
    }

    #endregion

    #region Nested Classes

    [UsedImplicitly]
    class Handler : CommandHandlerBase<MarkEntitiesAsDeletedCommand<TEntity>>
    {
        #region Constructors

        public Handler(IServiceProvider serviceProvider) : base(serviceProvider) { }

        #endregion

        protected override async Task Execute(MarkEntitiesAsDeletedCommand<TEntity> command, CancellationToken cancellationToken)
        {
            if (command.Ids.Length == 0)
                return;

            var entities = await Repository.Read(new FindEntitiesByIds<TEntity, int>(command.Ids)).ToArrayAsync(cancellationToken);

            if (entities.Length == 0)
                return;

            Parallel.ForEach(entities, entity => entity.IsDeleted = true);

            await Repository.UpdateAsync(entities, cancellationToken);

            command.Result = true;
        }
    }

    #endregion

    public static void Register(IServiceCollection services)
    {
        services.AddTransient(typeof(INotificationHandler<MarkEntitiesAsDeletedCommand<TEntity>>), typeof(Handler));
    }
}