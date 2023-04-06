namespace CRUD.Core
{
    #region << Using >>

    using CRUD.DAL;
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;

    #endregion

    public static class ServicesExt
    {
        public static void AddEntityCRUD<TEntity, TDto>(this IServiceCollection services)
                where TEntity : EntityBase, new()
                where TDto : DtoBase, new()
        {
            services.AddEntityRead<TEntity, TDto>();
            services.AddTransient(typeof(INotificationHandler<CreateOrUpdateEntitiesCommand<TEntity, TDto>>), typeof(CreateOrUpdateEntitiesCommand<TEntity, TDto>.Handler));
            services.AddTransient(typeof(INotificationHandler<DeleteEntitiesCommand<TEntity>>), typeof(DeleteEntitiesCommand<TEntity>.Handler));
        }

        public static void AddEntityRead<TEntity, TDto>(this IServiceCollection services)
                where TEntity : EntityBase, new()
                where TDto : DtoBase, new()
        {
            services.AddTransient(typeof(IRequestHandler<ReadEntitiesQuery<TEntity, TDto>, TDto[]>), typeof(ReadEntitiesQuery<TEntity, TDto>.Handler));
        }
    }
}