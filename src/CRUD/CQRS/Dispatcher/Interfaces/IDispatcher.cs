namespace CRUD.CQRS;

#region << Using >>

using System.Threading;
using System.Threading.Tasks;

#endregion

/// <summary>
///     A dispatcher interface to perform read- and write-based operations.
/// </summary>
public interface IDispatcher : IReadDispatcher
{
    public Task PushAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default) where TCommand : CommandBase;
}