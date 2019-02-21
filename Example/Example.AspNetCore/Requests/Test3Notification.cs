using MediatR;

namespace Example.AspNetCore.Requests
{
    public class Test3Notification : INotification
    {
        public string DataIn { get; set; }
        public TestDto TestDto { get; set; }

    }
}
