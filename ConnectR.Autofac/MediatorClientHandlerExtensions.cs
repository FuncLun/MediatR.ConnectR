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
            RegisterClientHandlers<TAssemblyFromType>(
                this ContainerBuilder builder,
                Type openGenericType
            )
            => builder.RegisterTypes(
                    typeof(TAssemblyFromType).Assembly
                        .ScanForMediatorMessageTypes()
                        .Select(v =>
                            openGenericType
                                .MakeGenericType(
                                    v.MessageType,
                                    v.ResponseType
                                )
                        )
                        .ToArray()
                )
                .AsImplementedInterfaces();
    }
}
