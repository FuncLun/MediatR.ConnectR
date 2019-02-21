using MediatR;

namespace Example.AspNetCore.Requests
{
    public class Test2Command : IRequest
    {
        public string DataIn { get; set; }
        public TestDto TestDto { get; set; }

    }
}
