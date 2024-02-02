namespace Samples.ToDo.API;

#region << Using >>

using CRUD.CQRS;
using CRUD.DAL.Abstractions;
using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;

#endregion

public class DoesEntityExistQuery<TEntity> : QueryBase<bool> where TEntity : EntityBase, new()
{
    #region Properties

    public int Id { get; }

    #endregion

    #region Constructors

    public DoesEntityExistQuery(int id)
    {
        Id = id;
    }

    #endregion

    #region Nested Classes

    [UsedImplicitly]
    class Handler : QueryHandlerBase<DoesEntityExistQuery<TEntity>, bool>
    {
        #region Constructors

        public Handler(IServiceProvider serviceProvider) : base(serviceProvider) { }

        #endregion

        protected override async Task<bool> Execute(DoesEntityExistQuery<TEntity> request, CancellationToken cancellationToken)
        {
            return await Repository.Read(new FindEntityByIntId<TEntity>(request.Id)).AnyAsync(cancellationToken);
        }
    }

    #endregion

    public static void Register(IServiceCollection services)
    {
        services.AddTransient(typeof(IRequestHandler<DoesEntityExistQuery<TEntity>, bool>), typeof(Handler));
    }
}