using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MediatR.ConnectR
{
    public static class RequestHandlerExtensions
    {
        public static IEnumerable<RequestHandlerTypeInfo>
            SelectRequestHandlerTypes(
                this Assembly assembly
            )
            => assembly
                .GetTypes()
                .SelectRequestHandlerTypes();

        public static IEnumerable<RequestHandlerTypeInfo>
            SelectRequestHandlerTypes(
                this IEnumerable<Assembly> assemblies
            )
            => assemblies
                .SelectMany(asm => asm
                    .GetTypes()
                    .SelectMany(RequestHandlerTypeInfo.FromHandlerType)
                );

        public static IEnumerable<RequestHandlerTypeInfo>
            SelectRequestHandlerTypes(
                this IEnumerable<Type> requestHandlerTypes
            )
            => requestHandlerTypes.SelectMany(RequestHandlerTypeInfo.FromHandlerType);


        public static IEnumerable<RequestTypeInfo>
            SelectRequestTypes(
                this IEnumerable<Type> requestTypes
            )
            => requestTypes.SelectMany(RequestTypeInfo.FromRequestType);
    }
}
