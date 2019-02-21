using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MediatR.ConnectR
{
    public static class MediatorRequestExtensions
    {
        public static IEnumerable<(Type RequestType, Type ResponseType)> ScanForMediatorRequestTypes(
            this Assembly assembly
        )
            => assembly
                .GetExportedTypes()
                .ScanForMediatorRequestTypes();

        public static IEnumerable<(Type RequestType, Type ResponseType)> ScanForMediatorRequestTypes(
            this IEnumerable<Assembly> assemblies
        )
            => assemblies
                .SelectMany(asm => asm
                    .GetExportedTypes()
                    .ScanForMediatorRequestTypes()
                );

        public static IEnumerable<(Type RequestType, Type ResponseType)> ScanForMediatorRequestTypes(
            this IEnumerable<Type> messageType
        )
            => messageType
                .SelectMany(t => t
                    .GetInterfaces()
                    .Where(i => i.IsGenericType)
                    .Where(i => i.IsClosedTypeOf(typeof(IRequest<>)))
                    .Select(i => (RequestType: t, Interface: i))
                )
                .Select(t =>
                (
                    t.RequestType,
                    ResponseType: t.Interface
                                      .GenericTypeArguments
                                      .FirstOrDefault()
                                  ?? typeof(Unit)
                ));
    }
}
