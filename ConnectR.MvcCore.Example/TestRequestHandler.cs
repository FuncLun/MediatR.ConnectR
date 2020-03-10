using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace ConnectR.MvcCore.Example
{
    public class TestRequestHandler : IRequestHandler<TestRequest>
    {
        public Task<Unit> Handle(
            TestRequest request,
            CancellationToken cancellationToken
        )
        {
            return Unit.Task;
        }
    }
}
