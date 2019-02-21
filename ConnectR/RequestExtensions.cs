using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MediatR.ConnectR
{
    public static class RequestExtensions
    {
        public static IEnumerable<(Type RequestType, Type ResponseType)> 
            WhereIsRequest(
            this Assembly assembly
        )
            => assembly
                .GetExportedTypes()
                .WhereIsRequest();

        public static IEnumerable<(Type RequestType, Type ResponseType)> 
            WhereIsRequest(
            this IEnumerable<Assembly> assemblies
        )
            => assemblies
                .SelectMany(asm => asm
                    .GetExportedTypes()
                    .WhereIsRequest()
                );

        public static IEnumerable<(Type RequestType, Type ResponseType)> 
            WhereIsRequest(
            this IEnumerable<Type> type
        )
            => type
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
