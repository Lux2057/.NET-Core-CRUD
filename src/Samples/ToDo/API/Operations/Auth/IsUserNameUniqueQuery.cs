namespace Samples.ToDo.API;

#region << Using >>

using CRUD.CQRS;
using Extensions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

#endregion

public class IsUserNameUniqueQuery : QueryBase<bool>
{
    #region Properties

    public string UserName { get; }

    #endregion

    #region Constructors

    public IsUserNameUniqueQuery(string userName)
    {
        UserName = userName;
    }

    #endregion

    #region Nested Classes

    [UsedImplicitly]
    class Handler : QueryHandlerBase<IsUserNameUniqueQuery, bool>
    {
        #region Constructors

        public Handler(IServiceProvider serviceProvider) : base(serviceProvider) { }

        #endregion

        protected override async Task<bool> Execute(IsUserNameUniqueQuery request, CancellationToken cancellationToken)
        {
            if (request.UserName.IsNullOrWhitespace())
                return false;

            return !await Repository.Read(new IsDeletedProp.FindBy.EqualTo<UserEntity>(false) &&
                                          new UserEntity.FindBy.UserNameEqualTo(request.UserName, false)).AnyAsync(cancellationToken);
        }
    }

    #endregion
}