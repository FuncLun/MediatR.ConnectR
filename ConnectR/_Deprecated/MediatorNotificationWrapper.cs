using System.Threading;
using System.Threading.Tasks;

namespace MediatR.ConnectR
{
    public class MediatorNotificationWrapper<TNotification> : MediatorWrapper<TNotification>
        where TNotification : INotification
    {
        public MediatorNotificationWrapper(
            IMediator mediator
        )
        {
            Mediator = mediator;
        }

        private IMediator Mediator { get; }

        public override async Task<object> Invoke(
            TNotification notification,
            CancellationToken cancellationToken
        )
        {
            await Mediator.Publish(notification, cancellationToken);
            return Unit.Task;
        }
    }
}