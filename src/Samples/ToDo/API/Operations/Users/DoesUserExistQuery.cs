namespace Samples.ToDo.API;

#region << Using >>

using CRUD.CQRS;
using CRUD.DAL.Abstractions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

#endregion

public class DoesUserExistQuery : QueryBase<bool>
{
    #region Properties

    public int Id { get; }

    #endregion

    #region Constructors

    public DoesUserExistQuery(int id)
    {
        Id = id;
    }

    #endregion

    #region Nested Classes

    [UsedImplicitly]
    class Handler : QueryHandlerBase<DoesUserExistQuery, bool>
    {
        #region Constructors

        public Handler(IServiceProvider serviceProvider) : base(serviceProvider) { }

        #endregion

        protected override async Task<bool> Execute(DoesUserExistQuery request, CancellationToken cancellationToken)
        {
            return await Repository.Read(new FindEntityByIntId<UserEntity>(request.Id)).AnyAsync(cancellationToken);
        }
    }

    #endregion
}