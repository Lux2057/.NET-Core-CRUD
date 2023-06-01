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

    /// <summary>
    ///     Creates new Entities by specified Dtos if data storage doesn't contain entries by specified ids,
    ///     otherwise updates existing Entities. Entity and Dto classes must have Automapper.Profiles for two way mapping.
    /// </summary>
    public class CreateOrUpdateEntitiesCommand<TEntity, TId, TDto> : CommandBase
            where TEntity : class, IId<TId>, new()
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

        internal class Handler : CommandHandlerBase<CreateOrUpdateEntitiesCommand<TEntity, TId, TDto>>
        {
            #region Constructors

            public Handler(IServiceProvider serviceProvider) : base(serviceProvider) { }

            #endregion

            protected override async Task Execute(CreateOrUpdateEntitiesCommand<TEntity, TId, TDto> command, CancellationToken cancellationToken)
            {
                if (!command.Dtos.Any())
                    return;

                var entities = Mapper.Map<TEntity[]>(command.Dtos);

                var existingEntitiesIds = Repository.Get(new FindEntitiesByIds<TEntity, TId>(entities.GetIds<TEntity, TId>())).Select(r => r.Id).ToArray();

                await Repository.AddAsync(entities.Where(r => !existingEntitiesIds.Contains(r.Id)), cancellationToken);
                await Repository.UpdateAsync(entities.Where(r => existingEntitiesIds.Contains(r.Id)), cancellationToken);

                command.Result = entities.GetIds<TEntity, TId>();
            }
        }

        #endregion
    }
}