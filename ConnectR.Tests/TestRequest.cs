using System.Threading;
using System.Threading.Tasks;

namespace MediatR.ConnectR
{
    public class TestRequest : IRequest<TestResponse>
    {
        public string Data { get; set; }
    }

    public class TestResponse
    {
        public string Result { get; set; }
    }


    public class TestHandler : IRequestHandler<TestRequest, TestResponse>
    {
        public Task<TestResponse> Handle(TestRequest request, CancellationToken cancellationToken)
            => Task.FromResult(new TestResponse()
            {
                Result = request.Data,
            });
    }
}
