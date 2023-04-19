namespace CRUD.Core
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using CRUD.CQRS;
    using CRUD.Extensions;
    using Microsoft.EntityFrameworkCore;

    #endregion

    public class DeleteEntitiesCommand<TEntity, TId> : CommandBase where TEntity : EntityBase<TId>, new()
    {
        #region Properties

        public TId[] Ids { get; }

        public new bool Result { get; set; }

        #endregion

        #region Constructors

        public DeleteEntitiesCommand(IEnumerable<TId> ids)
        {
            Ids = ids.ToArrayOrEmpty();
        }

        #endregion

        #region Nested Classes

        internal class Handler : CommandHandlerBase<DeleteEntitiesCommand<TEntity, TId>>
        {
            #region Constructors

            public Handler(IServiceProvider serviceProvider) : base(serviceProvider) { }

            #endregion

            protected override async Task Execute(DeleteEntitiesCommand<TEntity, TId> command, CancellationToken cancellationToken)
            {
                var entities = await Repository<TEntity>().Get(new EntitiesByIdsSpec<TEntity, TId>(command.Ids)).ToArrayAsync(cancellationToken);

                await Repository<TEntity>().DeleteAsync(entities, cancellationToken);

                command.Result = true;
            }
        }

        #endregion
    }
}