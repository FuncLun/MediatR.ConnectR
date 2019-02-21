using System.Threading;
using System.Threading.Tasks;
using Example.AspNetCore.Requests;
using MediatR;

namespace Example.AspNetCore.Handlers
{
    public class Test1Handler : IRequestHandler<Test1Query, Test1Response>
    {
        /// <inheritdoc />
        public Task<Test1Response> Handle(Test1Query request, CancellationToken cancellationToken)
            => Task.FromResult(new Test1Response()
            {
                DataOut = request.DataIn,
            });
    }
}
