namespace CRUD.CQRS.Linq2Db;

#region << Using >>

using CRUD.DAL.Linq2Db;
using Microsoft.Extensions.DependencyInjection;

#endregion

public abstract class QueryHandlerBase<TRequest, TResponse> : CRUD.CQRS.QueryHandlerBase<TRequest, TResponse> where TRequest : QueryBase<TResponse>
{
    #region Properties

    protected new ILinq2DbReadRepository Repository { get; }

    #endregion

    #region Constructors

    protected QueryHandlerBase(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        Repository = serviceProvider.GetService<ILinq2DbReadRepository>();
    }

    #endregion
}