namespace Samples.ToDo.API;

#region << Using >>

using System.Security.Cryptography;
using CRUD.CQRS;
using JetBrains.Annotations;

#endregion

public class GetRefreshTokenQuery : QueryBase<string>
{
    #region Nested Classes

    [UsedImplicitly]
    class Handler : QueryHandlerBase<GetRefreshTokenQuery, string>
    {
        #region Constructors

        public Handler(IServiceProvider serviceProvider) : base(serviceProvider) { }

        #endregion

        protected override Task<string> Execute(GetRefreshTokenQuery request, CancellationToken cancellationToken)
        {
            var randomNumber = new byte[32];
            using var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(randomNumber);

            return Task.FromResult(Convert.ToBase64String(randomNumber));
        }
    }

    #endregion
}