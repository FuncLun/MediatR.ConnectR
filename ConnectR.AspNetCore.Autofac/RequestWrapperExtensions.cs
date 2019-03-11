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
                object objectInAssemblyContainingHandler
            )
            => builder.RegisterMediatorRequestWrappers(
                new[]
                    {
                        objectInAssemblyContainingHandler.GetType().Assembly
                    }
                    .AsEnumerable()
            );

        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle>
            RegisterMediatorRequestWrappers(
                this ContainerBuilder builder,
                Type typeInAssemblyContainingHandler
            )
            => builder.RegisterMediatorRequestWrappers(
                new[]
                    {
                        typeInAssemblyContainingHandler.Assembly
                    }
                    .AsEnumerable()
            );

        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle>
            RegisterMediatorRequestWrappers<TTypeInAssemblyContainingHandler>(
                this ContainerBuilder builder
            )
            => builder.RegisterMediatorRequestWrappers(
                new[]
                    {
                        typeof(TTypeInAssemblyContainingHandler).Assembly
                    }
                    .AsEnumerable()
            );

        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle>
            RegisterMediatorRequestWrappers(
                this ContainerBuilder builder,
                Assembly assemblyContainingHandlers
            )
            => builder.RegisterMediatorRequestWrappers(
                new[]
                    {
                        assemblyContainingHandlers
                    }
                    .AsEnumerable()
            );

        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle>
            RegisterMediatorRequestWrappers(
                this ContainerBuilder builder,
                params Assembly[] assembliesContainingHandlers
            )
            => builder.RegisterMediatorRequestWrappers(
                assembliesContainingHandlers.AsEnumerable()
            );

        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle>
            RegisterMediatorRequestWrappers(
                this ContainerBuilder builder,
                IEnumerable<Assembly> assembliesContainingHandlers
            )
            => builder.RegisterTypes(
                    assembliesContainingHandlers
                        .SelectRequestHandlerTypes()
                        .MakeMediatorWrappers()
                        .ToArray()
                )
                .AsSelf();
    }
}
