namespace Samples.ToDo.API;

#region << Using >>

using CRUD.CQRS;
using Extensions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

#endregion

public class IsUserLoginUniqueQuery : QueryBase<bool>
{
    #region Properties

    public string Login { get; }

    #endregion

    #region Constructors

    public IsUserLoginUniqueQuery(string login)
    {
        Login = login;
    }

    #endregion

    #region Nested Classes

    [UsedImplicitly]
    class Handler : QueryHandlerBase<IsUserLoginUniqueQuery, bool>
    {
        #region Constructors

        public Handler(IServiceProvider serviceProvider) : base(serviceProvider) { }

        #endregion

        protected override async Task<bool> Execute(IsUserLoginUniqueQuery request, CancellationToken cancellationToken)
        {
            if (request.Login.IsNullOrWhitespace())
                return false;

            return !await Repository.Read(new UserEntity.FindBy.LoginEqualTo(request.Login, false)).AnyAsync(cancellationToken);
        }
    }

    #endregion
}