namespace CRUD.CQRS;

#region << Using >>

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

#endregion

/// <summary>
///     Issue #75: https://github.com/Lux2057/.NET-Core-CRUD/issues/75#issue-1695287763
/// </summary>
public class ReMediator : Mediator
{
    #region Constructors

    public ReMediator(IServiceProvider serviceFactory)
            : base(serviceFactory) { }

    #endregion

    protected override async Task PublishCore(IEnumerable<NotificationHandlerExecutor> handlerExecutors,
                                              INotification notification,
                                              CancellationToken cancellationToken)
    {
        HashSet<Type> doneMethods = new();
        foreach (var handler in handlerExecutors)
        {
            var handlerType = handler.HandlerInstance.GetType().DeclaringType;
            if (!doneMethods.Add(handlerType))
                continue;

            await handler.HandlerCallback(notification, cancellationToken).ConfigureAwait(false);
        }
    }
}