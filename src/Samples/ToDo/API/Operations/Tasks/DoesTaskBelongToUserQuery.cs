namespace Samples.ToDo.API;

#region << Using >>

using CRUD.CQRS;
using CRUD.DAL.Abstractions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

#endregion

public class DoesTaskBelongToUserQuery : QueryBase<bool>
{
    #region Properties

    public int Id { get; }

    public int UserId { get; }

    #endregion

    #region Constructors

    public DoesTaskBelongToUserQuery(int id, int userId)
    {
        Id = id;
        UserId = userId;
    }

    #endregion

    #region Nested Classes

    [UsedImplicitly]
    class Handler : QueryHandlerBase<DoesTaskBelongToUserQuery, bool>
    {
        #region Constructors

        public Handler(IServiceProvider serviceProvider) : base(serviceProvider) { }

        #endregion

        protected override async Task<bool> Execute(DoesTaskBelongToUserQuery request, CancellationToken cancellationToken)
        {
            return await Repository.Read(new FindEntityByIntId<TaskEntity>(request.Id) &&
                                         new UserIdProp.FindBy.EqualTo<TaskEntity>(request.UserId)).AnyAsync(cancellationToken);
        }
    }

    #endregion
}