namespace CRUD.CQRS;

#region << Using >>

using System;
using System.Threading;
using System.Threading.Tasks;
using System.Data;

#endregion

/// <summary>
///     A dispatcher interface to perform read-based operations.
/// </summary>
public interface IReadDispatcher : IDisposable
{
    public Task<TResponse> QueryAsync<TResponse>(QueryBase<TResponse> queryBase,
                                                 CancellationToken cancellationToken = default,
                                                 IsolationLevel? isolationLevel = null);
}