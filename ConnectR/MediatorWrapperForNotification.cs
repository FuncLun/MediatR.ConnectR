using System.Threading;
using System.Threading.Tasks;

namespace MediatR.ConnectR
{
    public class MediatorWrapperForNotification<TNotification> : MediatorWrapper<TNotification>
        where TNotification : INotification
    {
        public MediatorWrapperForNotification(
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