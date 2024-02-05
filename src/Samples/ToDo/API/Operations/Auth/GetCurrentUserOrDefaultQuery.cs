namespace Samples.ToDo.API;

#region << Using >>

using CRUD.CQRS;
using JetBrains.Annotations;

#endregion

public class GetCurrentUserOrDefaultQuery : QueryBase<UserDto>
{
    #region Nested Classes

    [UsedImplicitly]
    class Handler : QueryHandlerBase<GetCurrentUserOrDefaultQuery, UserDto>
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

        protected override Task<UserDto> Execute(GetCurrentUserOrDefaultQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(this.httpAccessor.HttpContext?.User.ToUserDto());
        }
    }

    #endregion
}