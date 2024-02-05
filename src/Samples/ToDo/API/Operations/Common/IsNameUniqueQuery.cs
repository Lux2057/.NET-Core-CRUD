namespace Samples.ToDo.API;

#region << Using >>

using CRUD.CQRS;
using CRUD.DAL.Abstractions;
using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;

#endregion

public class IsNameUniqueQuery<TEntity> : QueryBase<bool>
        where TEntity : EntityBase,
        NameProp.Interface,
        UserIdProp.Interface,
        new()
{
    #region Properties

    public int? Id { get; }

    public int UserId { get; }

    public string Name { get; }

    #endregion

    #region Constructors

    public IsNameUniqueQuery(int? id, int userId, string name)
    {
        Id = id;
        UserId = userId;
        Name = name;
    }

    #endregion

    #region Nested Classes

    [UsedImplicitly]
    class Handler : QueryHandlerBase<IsNameUniqueQuery<TEntity>, bool>
    {
        #region Constructors

        public Handler(IServiceProvider serviceProvider) : base(serviceProvider) { }

        #endregion

        protected override async Task<bool> Execute(IsNameUniqueQuery<TEntity> request, CancellationToken cancellationToken)
        {
            return !await Repository.Read(new IsDeletedProp.FindBy.EqualTo<TEntity>(false) &&
                                          new NameProp.FindBy.EqualTo<TEntity>(request.Name, true) &&
                                          new UserIdProp.FindBy.EqualTo<TEntity>(request.UserId) &&
                                          !new FindEntityByIntId<TEntity>(request.Id.GetValueOrDefault(0)))
                                    .AnyAsync(cancellationToken);
        }
    }

    #endregion

    public static void Register(IServiceCollection services)
    {
        services.AddTransient(typeof(IRequestHandler<IsNameUniqueQuery<TEntity>, bool>), typeof(Handler));
    }
}