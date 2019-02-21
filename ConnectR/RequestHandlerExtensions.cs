using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MediatR.ConnectR
{
    public static class RequestHandlerExtensions
    {
        public static IEnumerable<(Type HandlerType, Type HandlerInterface)>
            SelectRequestHandlerTypes(
                this Assembly assembly
            )
            => assembly
                .GetTypes()
                .SelectRequestHandlerTypes();

        public static IEnumerable<(Type HandlerType, Type HandlerInterface)>
            SelectRequestHandlerTypes(
                this IEnumerable<Assembly> assemblies
            )
            => assemblies
                .SelectMany(asm => asm
                    .GetTypes()
                    .SelectRequestHandlerTypes()
                );

        public static IEnumerable<(Type HandlerType, Type HandlerInterface)>
            SelectRequestHandlerTypes(
                this IEnumerable<Type> types
            )
            => types
                .SelectMany(t => t
                    .GetInterfaces()
                    .Where(i => i.IsGenericType)
                    .Where(i => i.IsClosedTypeOf(typeof(IRequestHandler<,>)))
                    .Select(i => (HandlerType: t, HandlerInterface: i))
                );
        //.Select(t =>
        //{
        //    var requestResponseTypes = t.HandlerInterface
        //        .GenericTypeArguments;

        //    return (
        //        t.HandlerType,
        //        t.HandlerInterface,
        //        RequestType: requestResponseTypes[0],
        //        ResponseType: requestResponseTypes[1]
        //    );
        //});
    }
}
