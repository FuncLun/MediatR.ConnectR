using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Builder;
using Autofac.Features.Scanning;

namespace MediatR.ConnectR.AspNetCore.Autofac
{
    public static class RequestWrapperExtensions
    {
        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle>
            RegisterMediatorRequestWrappers(
                this ContainerBuilder builder,
                object assemblyContainingObject
            )
            => builder.RegisterMediatorRequestWrappers(new[] { assemblyContainingObject.GetType().Assembly }.AsEnumerable());

        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle>
            RegisterMediatorRequestWrappers(
                this ContainerBuilder builder,
                Type assemblyContainingType
            )
            => builder.RegisterMediatorRequestWrappers(new[] { assemblyContainingType.Assembly }.AsEnumerable());

        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle>
            RegisterMediatorRequestWrappers<TAssemblyContainingType>(
                this ContainerBuilder builder
            )
            => builder.RegisterMediatorRequestWrappers(new[] { typeof(TAssemblyContainingType).Assembly }.AsEnumerable());

        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle>
            RegisterMediatorRequestWrappers(
                this ContainerBuilder builder,
                Assembly assembly
            )
            => builder.RegisterMediatorRequestWrappers(new[] { assembly }.AsEnumerable());

        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle>
            RegisterMediatorRequestWrappers(
                this ContainerBuilder builder,
                params Assembly[] assemblies
            )
            => builder.RegisterMediatorRequestWrappers(assemblies.AsEnumerable());

        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle>
            RegisterMediatorRequestWrappers(
                this ContainerBuilder builder,
                IEnumerable<Assembly> assemblies
            )
            => builder.RegisterTypes(
                    assemblies
                        .SelectMany(asm => asm.ScanForMediatorRequestTypes())
                        .MakeMediatorMessageWrappers()
                        .ToArray()
                )
                .AsSelf();
    }
}
