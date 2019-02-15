using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MediatR.ConnectR
{
    public static class MediatorExtensions
    {
        public static IEnumerable<(Type MessageType, Type ResponseType)> ScanForMediatorMessageTypes(
            this Assembly assembly
        )
            => assembly
                .GetExportedTypes()
                .ScanForMediatorMessageTypes();

        public static IEnumerable<(Type MessageType, Type ResponseType)> ScanForMediatorMessageTypes(
            this IEnumerable<Assembly> assemblies
        )
            => assemblies
                .SelectMany(asm => asm
                    .GetExportedTypes()
                    .ScanForMediatorMessageTypes()
                );

        public static IEnumerable<(Type MessageType, Type ResponseType)> ScanForMediatorMessageTypes(
            this IEnumerable<Type> messageType
        )
            => messageType
                .SelectMany(t => t
                    .GetInterfaces()
                    .Where(i => i.IsGenericType)
                    .Where(i => i.IsClosedTypeOf(typeof(IRequest<>)) || i == typeof(INotification))
                    .Select(i => (MessageType: t, Interface: i)))
                .Select(t =>
                (
                    t.MessageType,
                    ResponseType: t.Interface
                                      .GenericTypeArguments
                                      .FirstOrDefault()
                                  ?? typeof(Unit)
                ));
    }
}
