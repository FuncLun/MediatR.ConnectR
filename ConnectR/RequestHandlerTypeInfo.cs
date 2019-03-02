using System;
using System.Collections.Generic;
using System.Linq;

namespace MediatR.ConnectR
{
    public struct RequestHandlerTypeInfo
    {
        public static IEnumerable<RequestHandlerTypeInfo> FromHandlerType(Type requestHandlerType)
            => requestHandlerType
                .GetInterfaces()
                .Where(iHandler => iHandler.IsGenericType)
                .Where(iHandler => iHandler.IsClosedTypeOf(typeof(IRequestHandler<,>)))
                .Select(iHandler => new RequestHandlerTypeInfo()
                {
                    HandlerType = requestHandlerType,
                    HandlerInterface = iHandler,
                    RequestTypeInfo = ConnectR.RequestTypeInfo
                        .FromRequestType(iHandler.GetGenericArguments().First())
                        .ToList(),
                });

        public Type HandlerType { get; private set; }
        public Type HandlerInterface { get; private set; }
        public IReadOnlyList<RequestTypeInfo> RequestTypeInfo { get; private set; }
    }
}
