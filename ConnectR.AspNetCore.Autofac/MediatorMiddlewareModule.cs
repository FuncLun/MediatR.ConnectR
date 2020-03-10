using System.Linq;
using Autofac;
using Autofac.Core;

namespace MediatR.ConnectR.AspNetCore.Autofac
{
    public class MediatorMiddlewareModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ConnectorMiddleware>();

            //builder.RegisterType<MediatorRegistry>()
            //    .AsImplementedInterfaces()
            //    .SingleInstance();

            //builder.Register(c =>
            //        new MediatorRegistry(
            //            c.ComponentRegistry
            //                .Registrations
            //                .SelectMany(r => r.Services)
            //                .OfType<TypedService>()
            //                .Select(ts => ts.ServiceType)
            //                .WhereIsMediatorWrapper()
            //                .ToList()
            //        )
            //    )
            //    .AsImplementedInterfaces()
            //    .SingleInstance();
        }
    }
}
