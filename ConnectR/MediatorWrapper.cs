using System;
using System.Threading;
using System.Threading.Tasks;

namespace MediatR.ConnectR
{
    public abstract class MediatorWrapper<TMessage> : IMediatorWrapper
    {
        public Type MessageType => typeof(TMessage);

        public Task<object> Invoke(
            object message,
            CancellationToken cancellationToken
        )
            => Invoke((TMessage)message, cancellationToken);

        public abstract Task<object> Invoke(
            TMessage request,
            CancellationToken cancellationToken
        );
    }
}