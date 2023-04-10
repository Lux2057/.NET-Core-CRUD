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
    using Microsoft.EntityFrameworkCore;

    #endregion

    public class CreateOrUpdateEntitiesCommand<TEntity, TId, TDto> : CommandBase
            where TEntity : EntityBase<TId>, new()
            where TDto : class, IId<TId>, new()
    {
        #region Properties

        public TDto[] Dtos { get; }

        public new TId[] Result { get; set; }

        #endregion

        #region Constructors

        public CreateOrUpdateEntitiesCommand(IEnumerable<TDto> dtos)
        {
            Dtos = dtos.ToArrayOrEmpty();
        }

        #endregion

        #region Nested Classes

        public class Handler : CommandHandlerBase<CreateOrUpdateEntitiesCommand<TEntity, TId, TDto>>
        {
            #region Constructors

            public Handler(IServiceProvider serviceProvider) : base(serviceProvider) { }

            #endregion

            protected override async Task Execute(CreateOrUpdateEntitiesCommand<TEntity, TId, TDto> command, CancellationToken cancellationToken)
            {
                if (!command.Dtos.Any())
                    return;

                var entities = this.Mapper.Map<TEntity[]>(command.Dtos);

                var dt = DateTime.UtcNow;
                Parallel.ForEach(entities, entity => entity.CrDt = dt);

                var existingEntitiesIds = await Repository<TEntity>().Get(new EntitiesByIdsSpec<TEntity, TId>(entities.GetIds<TEntity, TId>())).Select(r => r.Id).ToArrayAsync(cancellationToken);

                await Repository<TEntity>().AddAsync(entities.Where(r => !existingEntitiesIds.Contains(r.Id)), cancellationToken);
                await Repository<TEntity>().UpdateAsync(entities.Where(r => existingEntitiesIds.Contains(r.Id)), cancellationToken);

                command.Result = entities.GetIds<TEntity, TId>();
            }
        }

        #endregion
    }
}