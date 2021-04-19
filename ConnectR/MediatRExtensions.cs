using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MediatR.Registration;
using Microsoft.Extensions.DependencyInjection;

namespace MediatR.ConnectR
{
    public static class MediatRExtensions
    {
        public static IServiceCollection AddMediatRClasses(
            this IServiceCollection services,
            params Assembly[] assemblies
        )
        {
            ServiceRegistrar.AddMediatRClasses(services, assemblies);
            return services;
        }

        public static IServiceCollection AddMediatRClasses<TAssemblyFromType>(
            this IServiceCollection services
        )
        {
            ServiceRegistrar.AddMediatRClasses(services, new[] { typeof(TAssemblyFromType).Assembly });
            return services;
        }



        public static IServiceCollection AddMediatR(
            this IServiceCollection services
        )
            => services.AddMediatR((Action<MediatRServiceConfiguration>)null);

        public static IServiceCollection AddMediatR(
            this IServiceCollection services,
            Action<MediatRServiceConfiguration> configuration
        )
        {
            var serviceConfiguration = new MediatRServiceConfiguration();

            configuration?.Invoke(serviceConfiguration);
            ServiceRegistrar.AddRequiredServices(services, serviceConfiguration);
            
            return services;
        }



        public static IEnumerable<(Type RequestType, Type ResponseType)> SelectRequestTypes(
                this IEnumerable<Assembly> assemblies
            )
            => assemblies
                .SelectMany(asm => asm.DefinedTypes)
                .SelectRequestTypes();

        public static IEnumerable<(Type RequestType, Type ResponseType)>
            SelectRequestTypes(
                this IEnumerable<Type> types
            )
            => types
                .Select(t =>
                    (
                        Type: t,
                        TypeInfo: t.GetTypeInfo()
                    )
                )
                .Where(a => !a.TypeInfo.IsGenericTypeDefinition
                            || !a.TypeInfo.ContainsGenericParameters
                )
                .Select(a =>
                    (
                        a.Type,
                        a.TypeInfo,
                        Interface: a.TypeInfo
                            .GetInterfaces()
                            .FirstOrDefault(i => i.IsGenericType
                                                 && i.GetGenericTypeDefinition() == typeof(IRequest<>)
                            )
                    )
                )
                .Where(a => !(a.Interface is null))
                .Select(a =>
                    (
                        RequestType: a.Type,
                        ResponseType: a.Interface.GetGenericArguments().Single()
                    )
                );

        public static IEnumerable<Type> SelectNotificationTypes(
            this IEnumerable<Assembly> assemblies
        )
            => assemblies
                .SelectMany(asm => asm.DefinedTypes)
                .SelectNotificationTypes();

        public static IEnumerable<Type> SelectNotificationTypes(
            this IEnumerable<Type> types
        )
            => types
                .Select(t =>
                    (
                        Type: t,
                        TypeInfo: t.GetTypeInfo()
                    )
                )
                .Where(a => !a.TypeInfo.IsGenericTypeDefinition
                            || !a.TypeInfo.ContainsGenericParameters
                )
                .Where(a => a.TypeInfo
                    .GetInterfaces()
                    .Any(i => i == typeof(INotification))
                )
                .Select(a => a.Type);
    }
}
