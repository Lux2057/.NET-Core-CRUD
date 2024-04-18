namespace Samples.ToDo.API;

#region << Using >>

using CRUD.CQRS;
using CRUD.DAL.Abstractions;
using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;

#endregion

public class DoesEntityBelongToUserQuery<TEntity> : QueryBase<bool> where TEntity : EntityBase, UserIdProp.Interface, new()
{
    #region Properties

    public int? Id { get; }

    public int UserId { get; }

    #endregion

    #region Constructors

    public DoesEntityBelongToUserQuery(int? id, int userId)
    {
        Id = id;
        UserId = userId;
    }

    #endregion

    #region Nested Classes

    [UsedImplicitly]
    class Handler : QueryHandlerBase<DoesEntityBelongToUserQuery<TEntity>, bool>
    {
        #region Constructors

        public Handler(IServiceProvider serviceProvider) : base(serviceProvider) { }

        #endregion

        protected override async Task<bool> Execute(DoesEntityBelongToUserQuery<TEntity> request, CancellationToken cancellationToken)
        {
            return request.Id == null ||
                   request.Id == 0 ||
                   await Repository.Read(new IsDeletedProp.FindBy.EqualTo<TEntity>(false) &&
                                         new FindEntityByIntId<TEntity>(request.Id.GetValueOrDefault()) &&
                                         new UserIdProp.FindBy.EqualTo<TEntity>(request.UserId))
                                   .AnyAsync(cancellationToken);
        }
    }

    #endregion

    public static void Register(IServiceCollection services)
    {
        services.AddTransient(typeof(IRequestHandler<DoesEntityBelongToUserQuery<TEntity>, bool>), typeof(Handler));
    }
}