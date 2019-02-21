using MediatR;

namespace Example.AspNetCore.Requests
{
    public class Test1Query : IRequest<Test1Response>
    {
        public string DataIn { get; set; }
        public TestDto TestDto { get; set; }
    }

    public class Test1Response
    {
        public string DataOut { get; set; }
    }
}
