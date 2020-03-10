using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace MediatR.ConnectR.AspNetCore
{
    public static class AspNetCoreExtensions
    {
        public static IServiceCollection AddConnectR(
            this IServiceCollection services
        )
        {
            services.TryAddSingleton<IConnectorProvider, ConnectorProvider>();
            services.TryAddSingleton<IConnectorSerializer, ConnectorNewtonsoftSerializer>();

            return services;
        }

        public static IApplicationBuilder UseConnectR(
            this IApplicationBuilder applicationBuilder
        )
            => applicationBuilder.UseMiddleware<ConnectorMiddleware>();


        public static ConnectorCollection ToConnectorCollection(
            this IEnumerable<IConnectorEntry> entries
        )
            => new ConnectorCollection(entries);

        public static IServiceCollection AddMediatRNotifications(
            this IServiceCollection services,
            params Assembly[] assemblies
        )
            => services.AddMediatRNotifications((IEnumerable<Assembly>)assemblies);
        public static IServiceCollection AddMediatRNotifications(
            this IServiceCollection services,
            IEnumerable<Assembly> assemblies
        )
        {
            var connectorCollection = assemblies
                    .SelectNotificationTypes()
                    .Select(t => typeof(ConnectorEntryNotification<>)
                            .MakeGenericType(t)
                    )
                    .Select(t => (IConnectorEntry)Activator.CreateInstance(t))
                    .ToConnectorCollection();

            services.AddSingleton<IConnectorCollection>(connectorCollection);

            return services;
        }

        
        public static IServiceCollection AddMediatRRequests(
            this IServiceCollection services,
            params Assembly[] assemblies
        )
            => services.AddMediatRRequests((IEnumerable<Assembly>)assemblies);

        public static IServiceCollection AddMediatRRequests(
            this IServiceCollection services,
            IEnumerable<Assembly> assemblies
        )
        {
            var connectorCollection = assemblies
                    .SelectRequestTypes()
                    .Select(a => typeof(ConnectorEntryRequest<,>)
                            .MakeGenericType(a.RequestType, a.ResponseType)
                    )
                    .Select(t => (IConnectorEntry)Activator.CreateInstance(t))
                    .ToConnectorCollection();

            services.AddSingleton<IConnectorCollection>(connectorCollection);

            return services;
        }

        public static IServiceCollection AddMediatRRequests(
            this IServiceCollection services,
            params Type[] types
        )
            => services.AddMediatRRequests((IEnumerable<Type>)types);

        public static IServiceCollection AddMediatRRequests(
            this IServiceCollection services,
            IEnumerable<Type> types
        )
        {

            return services;
        }

    }
}
