namespace Samples.ToDo.API;

#region << Using >>

using CRUD.CQRS;
using CRUD.DAL.Abstractions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

#endregion

public class GetLoginQuery : QueryBase<string>
{
    #region Properties

    public int Id { get; }

    #endregion

    #region Constructors

    public GetLoginQuery(int id)
    {
        Id = id;
    }

    #endregion

    #region Nested Classes

    [UsedImplicitly]
    class Handler : QueryHandlerBase<GetLoginQuery, string>
    {
        #region Constructors

        public Handler(IServiceProvider serviceProvider) : base(serviceProvider) { }

        #endregion

        protected override async Task<string> Execute(GetLoginQuery request, CancellationToken cancellationToken)
        {
            var user = await Repository.Read(new FindEntityByIntId<UserEntity>(request.Id) &&
                                             new IsDeletedProp.FindBy.EqualTo<UserEntity>(false))
                                       .SingleOrDefaultAsync(cancellationToken);

            return user?.Login ?? string.Empty;
        }
    }

    #endregion
}