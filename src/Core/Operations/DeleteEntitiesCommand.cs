namespace CRUD.Core
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using CRUD.CQRS;
    using CRUD.DAL;
    using CRUD.Extensions;

    #endregion

    public class DeleteEntitiesCommand<TEntity> : CommandBase where TEntity : EntityBase, new()
    {
        #region Properties

        public object[] Ids { get; }

        public new bool Result { get; set; }

        #endregion

        #region Constructors

        public DeleteEntitiesCommand(IEnumerable<object> ids)
        {
            Ids = ids.ToArrayOrEmpty();
        }

        #endregion

        #region Nested Classes

        public class Handler : CommandHandlerBase<DeleteEntitiesCommand<TEntity>>
        {
            #region Constructors

            public Handler(IServiceProvider serviceProvider) : base(serviceProvider) { }

            #endregion

            protected override async Task Execute(DeleteEntitiesCommand<TEntity> command, CancellationToken cancellationToken)
            {
                await Repository<TEntity>().DeleteAsync(command.Ids, cancellationToken);

                command.Result = true;
            }
        }

        #endregion
    }
}