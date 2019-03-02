using System.Threading;
using System.Threading.Tasks;

namespace MediatR.ConnectR
{
    public class Test1Request : IRequest<Test1Response>
    {
        public string Data { get; set; }
    }

    public class Test1Response
    {
        public string Result { get; set; }
    }


    public class Test1Handler : IRequestHandler<Test1Request, Test1Response>
    {
        public Task<Test1Response> Handle(Test1Request request, CancellationToken cancellationToken)
            => Task.FromResult(new Test1Response()
            {
                Result = request.Data,
            });
    }
}
