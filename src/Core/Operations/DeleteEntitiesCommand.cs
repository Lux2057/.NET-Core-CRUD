namespace CRUD.Core
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using CRUD.CQRS;
    using CRUD.Extensions;
    using Microsoft.EntityFrameworkCore;

    #endregion

    /// <summary>
    ///     Deletes Entities from data storage by specified id collection.
    /// </summary>
    public class DeleteEntitiesCommand<TEntity, TId> : CommandBase where TEntity : class, IId<TId>, new()
    {
        #region Properties

        public TId[] Ids { get; }

        public new bool Result { get; set; }

        #endregion

        #region Constructors

        public DeleteEntitiesCommand(IEnumerable<TId> ids)
        {
            var arrayOrEmpty = ids.ToArrayOrEmpty();

            if (!arrayOrEmpty.Any())
                throw new ArgumentNullException("Ids can't be null or empty!");

            Ids = arrayOrEmpty;
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
                var entities = await Repository<TEntity>().Get(new FindEntitiesByIds<TEntity, TId>(command.Ids)).ToArrayAsync(cancellationToken);

                await Repository<TEntity>().DeleteAsync(entities, cancellationToken);

                command.Result = true;
            }
        }

        #endregion
    }
}