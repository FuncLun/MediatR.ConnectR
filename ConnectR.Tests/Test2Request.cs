using System.Threading;
using System.Threading.Tasks;

namespace MediatR.ConnectR
{
    public class Test2Request : IRequest<Test2Response>
    {
        public string Data { get; set; }
    }

    public class Test2Response
    {
        public string Result { get; set; }
    }


    public class Test2Handler : IRequestHandler<Test2Request, Test2Response>
    {
        public Task<Test2Response> Handle(Test2Request request, CancellationToken cancellationToken)
            => Task.FromResult(new Test2Response()
            {
                Result = request.Data,
            });
    }
}
