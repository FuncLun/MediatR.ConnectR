using System;
using System.Threading;
using System.Threading.Tasks;

namespace MediatR.ConnectR
{
    public interface IMediatorWrapper
    {
        Type MessageType { get; }

        Task<object> Invoke(
            object message,
            CancellationToken cancellationToken
        );
    }
}