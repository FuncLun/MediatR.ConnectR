using System;
using System.Threading;
using System.Threading.Tasks;

namespace MediatR.ConnectR
{
    public class MediatorWrapperForRequest<TRequest, TResponse> : MediatorWrapper<TRequest>
        where TRequest : IRequest<TResponse>
    {
        public MediatorWrapperForRequest(
            IMediator mediator
        )
        {
            Mediator = mediator;
        }

        private IMediator Mediator { get; }

        public override Task<object> Invoke(
            TRequest request, 
            CancellationToken cancellationToken
            )
        {
            var res = new TaskCompletionSource<object>();

            return Mediator.Send(request, cancellationToken)
                .ContinueWith(t =>
                    {
                        if (t.IsFaulted)
                        {
                            var innerExceptions = t.Exception?.InnerExceptions;

                            if (innerExceptions is null)
                                res.TrySetException(
                                    t.Exception
                                    ?? new Exception("Task Faulted with null Exception")
                                );
                            else
                                res.TrySetException(innerExceptions);
                        }
                        else if (t.IsCanceled)
                            res.TrySetCanceled();
                        else
                            res.TrySetResult(t.Result);

                        return res.Task;
                    },
                    cancellationToken
                )
                .Unwrap();
        }
    }
}