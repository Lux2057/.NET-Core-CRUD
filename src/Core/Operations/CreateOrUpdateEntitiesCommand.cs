namespace CRUD.Core
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using CRUD.CQRS;
    using CRUD.DAL;
    using CRUD.Extensions;

    #endregion

    public class CreateOrUpdateEntitiesCommand<TEntity, TDto> : CommandBase
            where TEntity : EntityBase, new()
            where TDto : DtoBase, new()
    {
        #region Properties

        public TDto[] Dtos { get; }

        public new object[] Result { get; set; }

        #endregion

        #region Constructors

        public CreateOrUpdateEntitiesCommand(IEnumerable<TDto> dtos)
        {
            Dtos = dtos.ToArrayOrEmpty();
        }

        #endregion

        #region Nested Classes

        public class Handler : CommandHandlerBase<CreateOrUpdateEntitiesCommand<TEntity, TDto>>
        {
            #region Constructors

            public Handler(IServiceProvider serviceProvider) : base(serviceProvider) { }

            #endregion

            protected override async Task Execute(CreateOrUpdateEntitiesCommand<TEntity, TDto> command, CancellationToken cancellationToken)
            {
                if (!command.Dtos.Any())
                    return;

                var entities = this.Mapper.Map<TEntity[]>(command.Dtos);

                var dt = DateTime.UtcNow;
                Parallel.ForEach(entities, entity => entity.CrDt = dt);

                await Repository<TEntity>().AddOrUpdateAsync(entities, cancellationToken);

                command.Result = entities.GetIds();
            }
        }

        #endregion
    }
}