using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace ConnectR.MvcCore.Example
{
    public class TestNotification : INotification { }

    public class TestNotificationHandler : INotificationHandler<TestNotification>
    {
        public Task Handle(
            TestNotification notification,
            CancellationToken cancellationToken
        )
        {
            return Task.CompletedTask;
        }
    }
}