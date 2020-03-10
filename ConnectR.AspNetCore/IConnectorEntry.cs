using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MediatR.ConnectR
{
    public interface IConnectorEntry
    {
        IReadOnlyList<string> Keys { get; }
        Type MessageType { get; }

        Task<object> Invoke(
            IMediator mediator,
            object message,
            CancellationToken cancellationToken
        );
    }
}