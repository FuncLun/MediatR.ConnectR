using System;
using System.Collections.Generic;
using System.Linq;

namespace MediatR.ConnectR
{
    public struct RequestTypeInfo
    {
        public static IEnumerable<RequestTypeInfo> FromRequestType(Type requestType)
        {
            return requestType
                .GetInterfaces()
                .Where(i => i.IsGenericType)
                .Where(i => i.IsClosedTypeOf(typeof(IRequest<>)))
                .Select(i => new RequestTypeInfo()
                {
                    RequestType = requestType,
                    ResponseType = i.GetGenericArguments().First(),
                    RequestInterface = i,
                });
        }


        public Type RequestType { get; private set; }
        public Type ResponseType { get; private set; }
        public Type RequestInterface { get; private set; }
    }
}