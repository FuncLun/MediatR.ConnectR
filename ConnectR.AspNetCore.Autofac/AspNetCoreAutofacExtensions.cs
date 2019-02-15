using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Builder;
using Autofac.Core;
using Autofac.Features.Scanning;

namespace MediatR.ConnectR.AspNetCore.Autofac
{
    public static class AspNetCoreAutofacExtensions
    {
        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle>
            RegisterMediatorWrappers(
                this ContainerBuilder builder,
                object assemblyContainingObject
            )
            => builder.RegisterMediatorWrappers(assemblyContainingObject.GetType().Assembly);

        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle>
            RegisterMediatorWrappers(
                this ContainerBuilder builder,
                Type assemblyContainingType
            )
            => builder.RegisterMediatorWrappers(assemblyContainingType.Assembly);

        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle>
            RegisterMediatorWrappers<TAssemblyContainingType>(
                this ContainerBuilder builder
            )
            => builder.RegisterMediatorWrappers(typeof(TAssemblyContainingType).Assembly);

        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle>
            RegisterMediatorWrappers(
                this ContainerBuilder builder,
                Assembly assemblies
            )
            => builder.RegisterMediatorWrappers(new[] {assemblies});

        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle>
            RegisterMediatorWrappers(
                this ContainerBuilder builder,
                params Assembly[] assemblies
            )
            => builder.RegisterMediatorWrappers(assemblies.AsEnumerable());

        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle>
            RegisterMediatorWrappers(
                this ContainerBuilder builder,
                IEnumerable<Assembly> assemblies
            )
            => builder.RegisterTypes(
                    assemblies
                        .SelectMany(asm => asm.ScanForMediatorMessageTypes())
                        .MakeMediatorMessageDelegates()
                        .ToArray()
                )
                .AsSelf();

        public static void RegisterMediatorRegistry<TRegistry>(
            this ContainerBuilder builder
        )
            where TRegistry : IMediatorRegistry, new()
            => builder.Register(c =>
                {
                    var registry = new TRegistry();
                    registry.LoadTypes(
                        c.ScanForMediatorWrappers()
                    );
                    return registry;
                })
                .AsImplementedInterfaces()
                .SingleInstance();

        public static List<Type> ScanForMediatorWrappers(
            this IComponentContext componentContext
        )
        {
            return componentContext
                .ComponentRegistry
                .Registrations
                .SelectMany(r => r.Services)
                .OfType<TypedService>()
                .Select(ts => ts.ServiceType)
                .Where(t => t.IsGenericType)
                .Where(t => t.IsClosedTypeOf(typeof(MediatorWrapperForRequest<,>))
                            || t.IsClosedTypeOf(typeof(MediatorWrapperForNotification<>)))
                .ToList();
        }

        public static TRegistry LoadTypesFromContext<TRegistry>(
            this TRegistry registry,
            IComponentContext context
        )
            where TRegistry : IMediatorRegistry
        {
            registry.LoadTypes(
                context.ScanForMediatorWrappers()
            );

            return registry;
        }
    }
}
