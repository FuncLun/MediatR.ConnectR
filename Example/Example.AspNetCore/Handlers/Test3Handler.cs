using System.Threading;
using System.Threading.Tasks;
using Example.AspNetCore.Requests;
using MediatR;

namespace Example.AspNetCore.Handlers
{
    public class Test3Handler : INotificationHandler<Test3Notification>
    {
        public static string LastDataIn { get; set; }
        /// <inheritdoc />
        public Task Handle(Test3Notification notification, CancellationToken cancellationToken)
        {
            LastDataIn = notification.DataIn;
            return Task.CompletedTask;
        }
    }
}
