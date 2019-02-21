using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;

namespace MediatR.ConnectR.Autofac
{
    public static class MediatorHandlerExtensions
    {
        public static ContainerBuilder
            RegisterAssemblyMediatorHandlers(
                this ContainerBuilder builder,
                object assemblyContainingObject
            )
            => builder.RegisterAssemblyMediatorHandlers(new[] { assemblyContainingObject.GetType().Assembly });

        public static ContainerBuilder
            RegisterAssemblyMediatorHandlers(
                this ContainerBuilder builder,
                Type assemblyContainingType
            )
            => builder.RegisterAssemblyMediatorHandlers(new[] { assemblyContainingType.Assembly });

        public static ContainerBuilder
            RegisterAssemblyMediatorHandlers<TAssemblyContainingType>(
                this ContainerBuilder builder
            )
            => builder.RegisterAssemblyMediatorHandlers(new[] { typeof(TAssemblyContainingType).Assembly });


        public static ContainerBuilder RegisterAssemblyMediatorHandlers(
            this ContainerBuilder builder,
            IEnumerable<Assembly> assemblies
        )
            => builder.RegisterAssemblyMediatorHandlers(assemblies.ToArray());

        /// <summary>
        /// Registers concrete handlers that implement IRequestHandler&lt;,&gt; or INotificationHandler&lt;&gt;
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        public static ContainerBuilder RegisterAssemblyMediatorHandlers(
            this ContainerBuilder builder,
            params Assembly[] assemblies
        )
        {
            builder
                .RegisterAssemblyTypes(assemblies)
                .AsClosedTypesOf(typeof(IRequestHandler<,>))
                .AsImplementedInterfaces();

            builder
                .RegisterAssemblyTypes(assemblies)
                .AsClosedTypesOf(typeof(INotificationHandler<>))
                .AsImplementedInterfaces();

            return builder;
        }
    }
}
