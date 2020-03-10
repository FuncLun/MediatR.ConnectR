using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MediatR.ConnectR
{
    public class ConnectorEntryNotification<TNotification> : IConnectorEntry
        where TNotification : INotification
    {
        public ConnectorEntryNotification()
        {
            Keys = new[]
                {
                    typeof(TNotification).FullName
                }
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => "/" + s.Replace(".", "/"))
                .ToList();
            MessageType = typeof(TNotification);
        }

        public IReadOnlyList<string> Keys { get; }
        public Type MessageType { get; }

        public async Task<object> Invoke(
            IMediator mediator,
            object message,
            CancellationToken cancellationToken
        )
        {
            await mediator.Publish((TNotification)message, cancellationToken);
            return Unit.Value;
        }
    }
}