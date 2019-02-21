using System;
using System.Linq;
using Autofac;
using Autofac.Builder;
using Autofac.Features.Scanning;

namespace MediatR.ConnectR.Autofac
{
    public static class MediatorClientHandlerExtensions
    {
        public static IRegistrationBuilder
            <
                object,
                ScanningActivatorData,
                DynamicRegistrationStyle
            >
            RegisterClientRequestHandlers<TAssemblyFromType>(
                this ContainerBuilder builder,
                Type openGenericType
            )
            => builder.RegisterTypes(
                    typeof(TAssemblyFromType).Assembly
                        .WhereIsRequest()
                        .Select(v =>
                            openGenericType
                                .MakeGenericType(
                                    v.RequestType,
                                    v.ResponseType
                                )
                        )
                        .ToArray()
                )
                .AsImplementedInterfaces();

        public static IRegistrationBuilder
            <
                object,
                ScanningActivatorData,
                DynamicRegistrationStyle
            >
            RegisterClientNotificationHandlers<TAssemblyFromType>(
                this ContainerBuilder builder,
                Type openGenericType
            )
            => builder.RegisterTypes(
                    typeof(TAssemblyFromType).Assembly
                        .WhereIsNotification()
                        .Select(v =>
                            openGenericType
                                .MakeGenericType(
                                    v.NotificationType,
                                    v.ResponseType
                                )
                        )
                        .ToArray()
                )
                .AsImplementedInterfaces();
    }
}
