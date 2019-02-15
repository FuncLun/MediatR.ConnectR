using Autofac;
using Autofac.Core;

namespace MediatR.ConnectR.AspNetCore.Autofac
{
    public class MediatorMiddlewareModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MediatorMiddleware>();
        }
    }
}
