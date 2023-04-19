namespace CRUD.Core
{
    #region << Using >>

    using MediatR;
    using Microsoft.Extensions.DependencyInjection;

    #endregion

    public static class ServicesExt
    {
        public static void AddEntityCRUD<TEntity, TId, TDto>(this IServiceCollection services)
                where TEntity : class, IId<TId>, new()
                where TDto : class, IId<TId>, new()
        {
            services.AddEntityRead<TEntity, TId, TDto>();
            services.AddTransient(typeof(INotificationHandler<CreateOrUpdateEntitiesCommand<TEntity, TId, TDto>>), typeof(CreateOrUpdateEntitiesCommand<TEntity, TId, TDto>.Handler));
            services.AddTransient(typeof(INotificationHandler<DeleteEntitiesCommand<TEntity, TId>>), typeof(DeleteEntitiesCommand<TEntity, TId>.Handler));
        }

        public static void AddEntityRead<TEntity, TId, TDto>(this IServiceCollection services)
                where TEntity : class, IId<TId>, new()
                where TDto : class, new()
        {
            services.AddTransient(typeof(IRequestHandler<ReadEntitiesQuery<TEntity, TId, TDto>, TDto[]>), typeof(ReadEntitiesQuery<TEntity, TId, TDto>.Handler));
        }
    }
}