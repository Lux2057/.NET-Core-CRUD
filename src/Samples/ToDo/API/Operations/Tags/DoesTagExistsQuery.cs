namespace Samples.ToDo.API;

#region << Using >>

using CRUD.CQRS;
using CRUD.DAL.Abstractions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

#endregion

public class DoesTagExistsQuery : QueryBase<bool>
{
    #region Properties

    public int Id { get; }

    #endregion

    #region Constructors

    public DoesTagExistsQuery(int id)
    {
        Id = id;
    }

    #endregion

    #region Nested Classes

    [UsedImplicitly]
    class Handler : QueryHandlerBase<DoesTagExistsQuery, bool>
    {
        #region Constructors

        public Handler(IServiceProvider serviceProvider) : base(serviceProvider) { }

        #endregion

        protected override async Task<bool> Execute(DoesTagExistsQuery request, CancellationToken cancellationToken)
        {
            return await Repository.Read(new FindEntityByIntId<TagEntity>(request.Id)).AnyAsync(cancellationToken);
        }
    }

    #endregion
}