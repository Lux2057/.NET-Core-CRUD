namespace Samples.ToDo.API;

#region << Using >>

using CRUD.CQRS;
using JetBrains.Annotations;

#endregion

public class GetCurrentUserIdOrDefaultQuery : QueryBase<int?>
{
    #region Nested Classes

    [UsedImplicitly]
    class Handler : QueryHandlerBase<GetCurrentUserIdOrDefaultQuery, int?>
    {
        #region Properties

        private readonly IHttpContextAccessor httpAccessor;

        #endregion

        #region Constructors

        public Handler(IServiceProvider serviceProvider, IHttpContextAccessor httpAccessor) : base(serviceProvider)
        {
            this.httpAccessor = httpAccessor;
        }

        #endregion

        protected override Task<int?> Execute(GetCurrentUserIdOrDefaultQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(this.httpAccessor.HttpContext?.User.ToUserDto().Id);
        }
    }

    #endregion
}