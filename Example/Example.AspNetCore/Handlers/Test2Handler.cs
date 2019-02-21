using System.Threading;
using System.Threading.Tasks;
using Example.AspNetCore.Requests;
using MediatR;

namespace Example.AspNetCore.Handlers
{
    public class Test2Handler : IRequestHandler<Test2Command>
    {
        public string LastDataIn { get; set; }

        /// <inheritdoc />
        public Task<Unit> Handle(Test2Command request, CancellationToken cancellationToken)
        {
            LastDataIn = request.DataIn;
            return Unit.Task;
        }
    }
}
