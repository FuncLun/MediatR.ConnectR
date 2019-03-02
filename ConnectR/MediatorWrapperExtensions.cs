using System;
using System.Collections.Generic;
using System.Linq;

namespace MediatR.ConnectR
{
    public static class MediatorWrapperExtensions
    {
        public static IEnumerable<Type> WhereIsMediatorWrapper(
            this IEnumerable<Type> types
        ) => types
            .Where(t => t.IsClosedTypeOf(typeof(MediatorWrapper<>)));

        public static IEnumerable<Type> MakeMediatorWrappers(
            this IEnumerable<RequestHandlerTypeInfo> typeInfos
        )
            => typeInfos.SelectMany(t => t.MakeMediatorWrappers());

        public static IEnumerable<Type> MakeMediatorWrappers(
            this RequestHandlerTypeInfo typeInfo
        )
            => typeInfo.RequestTypeInfo
                .Select(rti =>
                    typeof(MediatorRequestWrapper<,>)
                        .MakeGenericType(
                            rti.RequestType,
                            rti.ResponseType)
                );

        public static IEnumerable<Type> MakeMediatorWrappers(
            this IEnumerable<NotificationHandlerTypeInfo> typeInfos
        )
            => typeInfos.SelectMany(t => t.MakeMediatorWrappers());

        public static IEnumerable<Type> MakeMediatorWrappers(
            this NotificationHandlerTypeInfo typeInfo
        )
            => typeInfo.NotificationTypeInfo
                .Select(nti =>
                    typeof(MediatorNotificationWrapper<>)
                        .MakeGenericType(nti.NotificationType)
                );
    }
}
