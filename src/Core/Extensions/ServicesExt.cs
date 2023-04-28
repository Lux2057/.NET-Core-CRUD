namespace CRUD.Core
{
    #region << Using >>

    using MediatR;
    using Microsoft.Extensions.DependencyInjection;

    #endregion

    public static class ServicesExt
    {
        /// <summary>
        ///     Add dependencies for an Entity CRUD operations
        /// </summary>
        public static void AddEntityCRUD<TEntity, TId, TDto>(this IServiceCollection services)
                where TEntity : class, IId<TId>, new()
                where TDto : class, IId<TId>, new()
        {
            services.AddEntityRead<TEntity, TId, TDto>();
            services.AddTransient(typeof(INotificationHandler<CreateOrUpdateEntitiesCommand<TEntity, TId, TDto>>), typeof(CreateOrUpdateEntitiesCommand<TEntity, TId, TDto>.Handler));
            services.AddTransient(typeof(INotificationHandler<DeleteEntitiesCommand<TEntity, TId>>), typeof(DeleteEntitiesCommand<TEntity, TId>.Handler));
        }

        /// <summary>
        ///     Add dependency for an Entity Read operation
        /// </summary>
        public static void AddEntityRead<TEntity, TId, TDto>(this IServiceCollection services)
                where TEntity : class, IId<TId>, new()
                where TDto : class, new()
        {
            services.AddTransient(typeof(IRequestHandler<ReadEntitiesQuery<TEntity, TId, TDto>, PaginatedResponseDto<TDto>>), typeof(ReadEntitiesQuery<TEntity, TId, TDto>.Handler));
        }
    }
}