namespace Samples.ToDo.API;

#region << Using >>

using CRUD.CQRS;
using CRUD.DAL.Abstractions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

#endregion

public class GetUserNameQuery : QueryBase<string>
{
    #region Properties

    public int Id { get; }

    #endregion

    #region Constructors

    public GetUserNameQuery(int id)
    {
        Id = id;
    }

    #endregion

    #region Nested Classes

    [UsedImplicitly]
    class Handler : QueryHandlerBase<GetUserNameQuery, string>
    {
        #region Constructors

        public Handler(IServiceProvider serviceProvider) : base(serviceProvider) { }

        #endregion

        protected override async Task<string> Execute(GetUserNameQuery request, CancellationToken cancellationToken)
        {
            var user = await Repository.Read(new FindEntityByIntId<UserEntity>(request.Id) &&
                                             new IsDeletedProp.FindBy.EqualTo<UserEntity>(false))
                                       .SingleOrDefaultAsync(cancellationToken);

            return user?.UserName ?? string.Empty;
        }
    }

    #endregion
}