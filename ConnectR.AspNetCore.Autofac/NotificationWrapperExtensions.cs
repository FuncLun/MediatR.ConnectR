using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Builder;
using Autofac.Features.Scanning;

namespace MediatR.ConnectR.AspNetCore.Autofac
{
    public static class NotificationWrapperExtensions
    {
        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle>
            RegisterMediatorNotificationWrappers(
                this ContainerBuilder builder,
                object assemblyContainingObject
            )
            => builder.RegisterMediatorNotificationWrappers(new[] { assemblyContainingObject.GetType().Assembly }.AsEnumerable());

        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle>
            RegisterMediatorNotificationWrappers(
                this ContainerBuilder builder,
                Type assemblyContainingType
            )
            => builder.RegisterMediatorNotificationWrappers(new[] { assemblyContainingType.Assembly }.AsEnumerable());

        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle>
            RegisterMediatorNotificationWrappers<TAssemblyContainingType>(
                this ContainerBuilder builder
            )
            => builder.RegisterMediatorNotificationWrappers(new[] { typeof(TAssemblyContainingType).Assembly }.AsEnumerable());

        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle>
            RegisterMediatorNotificationWrappers(
                this ContainerBuilder builder,
                Assembly assembly
            )
            => builder.RegisterMediatorNotificationWrappers(new[] { assembly }.AsEnumerable());

        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle>
            RegisterMediatorNotificationWrappers(
                this ContainerBuilder builder,
                params Assembly[] assemblies
            )
            => builder.RegisterMediatorNotificationWrappers(assemblies.AsEnumerable());

        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle>
            RegisterMediatorNotificationWrappers(
                this ContainerBuilder builder,
                IEnumerable<Assembly> assemblies
            )
            => builder.RegisterTypes(
                    assemblies
                        .SelectNotificationHandlerTypes()
                        .MakeMediatorWrappers()
                        .ToArray()
                )
                .AsSelf();
    }
}
