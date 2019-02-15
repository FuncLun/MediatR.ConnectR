using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Builder;
using Autofac.Features.Scanning;

namespace MediatR.ConnectR.Autofac
{
    public static class MediatorHandlerExtensions
    {
        public static ContainerBuilder
            RegisterAssemblyHandlers(
                this ContainerBuilder builder,
                object assemblyContainingObject
            )
            => builder.RegisterHandlers(assemblyContainingObject.GetType().Assembly);

        public static ContainerBuilder
            RegisterAssemblyHandlers(
                this ContainerBuilder builder,
                Type assemblyContainingType
            )
            => builder.RegisterHandlers(assemblyContainingType.Assembly);

        public static ContainerBuilder
            RegisterAssemblyHandlers<TAssemblyContainingType>(
                this ContainerBuilder builder
            )
            => builder.RegisterHandlers(typeof(TAssemblyContainingType).Assembly);


        /// <summary>
        /// Registers concrete handlers that implement IRequestHandler&lt;,&gt; or INotificationHandler&lt;&gt;
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        public static ContainerBuilder RegisterHandlers(
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


        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle>
            RegisterAssemblyObjectHandlers<TAssemblyContainingType>(
                this ContainerBuilder builder
            )
            => builder.RegisterMiddlewareHandlers(typeof(TAssemblyContainingType).Assembly);


        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle>
            RegisterMiddlewareHandlers(
                this ContainerBuilder builder,
                params Assembly[] assemblies
            )
            => builder.RegisterTypes(
                    assemblies.ScanForMediatorMessageTypes()
                        .Select(v => v.MessageType)
                        .ToArray()
                    );


        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle>
            RegisterNewGeneric(
                this ContainerBuilder builder,
                Type openHandlerType,
                IEnumerable<(Type RequestType, Type IRequestType)> requestTypes
            )
            => builder.RegisterTypes(
                requestTypes
                    .Select(tuple =>
                        openHandlerType.MakeGenericType(
                            tuple.RequestType,
                            tuple.IRequestType.GetGenericArguments()[0]
                        )
                    )
                    .ToArray()
            );
    }
}
