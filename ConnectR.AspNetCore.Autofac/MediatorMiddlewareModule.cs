using System.Linq;
using Autofac;
using Autofac.Core;

namespace MediatR.ConnectR.AspNetCore.Autofac
{
    public class MediatorMiddlewareModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MediatorMiddleware>();

            builder.RegisterType<MediatorRegistry>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.Register(c =>
                    new MediatorRegistry(
                        c.ComponentRegistry
                            .Registrations
                            .SelectMany(r => r.Services)
                            .OfType<TypedService>()
                            .Select(ts => ts.ServiceType)
                            .Where(t => t.IsClosedTypeOf(typeof(MediatorWrapper<>)))
                            .ToList()
                    )
                )
                .AsImplementedInterfaces()
                .SingleInstance();
        }
    }
}
