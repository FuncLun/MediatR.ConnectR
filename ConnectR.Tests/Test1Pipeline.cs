using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace MediatR.ConnectR
{
    public class Test1Pipeline : IPipelineBehavior<Test1Request, Test1Response>
    {
        public static ConcurrentDictionary<Test1Request, Test1Response> RequestHistory
        { get; } = new ConcurrentDictionary<Test1Request, Test1Response>();

        /// <inheritdoc />
        public async Task<Test1Response> Handle(Test1Request request, CancellationToken cancellationToken, RequestHandlerDelegate<Test1Response> next)
        {
            var response = await next.Invoke();
            RequestHistory.TryAdd(request, response);
            return response;
        }
    }
}
