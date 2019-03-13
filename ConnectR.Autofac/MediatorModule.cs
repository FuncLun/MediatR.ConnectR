using Autofac;
using MediatR.Pipeline;

namespace MediatR.ConnectR.Autofac
{
    /// <summary>
    /// Registers Mediator as IMediator. Registration should occur once for any application using
    /// MediatR. Nothing specific to Messy is registered.
    /// </summary>
    public class MediatorModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(IMediator).Assembly)
                .AsImplementedInterfaces();

            builder.RegisterGeneric(typeof(RequestPostProcessorBehavior<,>))
                .As(typeof(IPipelineBehavior<,>));

            builder.RegisterGeneric(typeof(RequestPreProcessorBehavior<,>))
                .As(typeof(IPipelineBehavior<,>));

            //builder.RegisterTypes(new Type[]{})
            //    .AsClosedTypesOf()

            builder.Register<ServiceFactory>(c =>
            {
                //Resolving IComponentContext due to concurrency issues
                //https://autofaccn.readthedocs.io/en/latest/advanced/concurrency.html#service-resolution
                var threadSpecificContext = c.Resolve<IComponentContext>();
                return t => threadSpecificContext.Resolve(t);
            });
        }
    }
}
