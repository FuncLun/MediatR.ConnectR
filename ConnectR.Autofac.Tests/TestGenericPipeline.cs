using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace MediatR.ConnectR
{
    public class TestGenericPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        public static ConcurrentDictionary<TRequest, TResponse> RequestHistory
        { get; } = new ConcurrentDictionary<TRequest, TResponse>();

        /// <inheritdoc />
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var response = await next.Invoke();
            RequestHistory.TryAdd(request, response);
            return response;
        }
    }
}
