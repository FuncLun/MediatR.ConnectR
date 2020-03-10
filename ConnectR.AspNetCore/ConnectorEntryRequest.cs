using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MediatR.ConnectR
{
    public class ConnectorEntryRequest<TRequest, TResponse> : IConnectorEntry
        where TRequest : IRequest<TResponse>
    {
        public ConnectorEntryRequest()
        {
            Keys = new[]
                {
                    typeof(TRequest).FullName,
                }
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Distinct()
                .Select(s => "/" + s.Replace(".", "/"))
                .ToList();
            MessageType = typeof(TRequest);
        }

        public IReadOnlyList<string> Keys { get; }
        public Type MessageType { get; }

        public async Task<object> Invoke(
            IMediator mediator,
            object message,
            CancellationToken cancellationToken
        )
            => await mediator.Send((TRequest)message, cancellationToken);
    }
}