using System.Threading;
using System.Threading.Tasks;

namespace MediatR.ConnectR.AspNetCore
{
    public class TestNotification : INotification
    {
        public string Data { get; set; }
    }

    public class TestNotificationHandler : INotificationHandler<TestNotification>
    {
        public static string LastData { get; set; }

        public async Task Handle(TestNotification notification, CancellationToken cancellationToken)
        {
            await Task.Yield();
            LastData = notification.Data;
        }
    }
}
