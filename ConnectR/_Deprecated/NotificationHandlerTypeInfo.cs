using System;
using System.Collections.Generic;
using System.Linq;

namespace MediatR.ConnectR
{
    public struct NotificationHandlerTypeInfo
    {
        public static IEnumerable<NotificationHandlerTypeInfo> FromHandlerType(Type notificationHandlerType)
            => notificationHandlerType
                .GetInterfaces()
                .Where(iHandler => iHandler.IsGenericType)
                .Where(iHandler => iHandler.IsClosedTypeOf(typeof(INotificationHandler<>)))
                .Select(iHandler => new NotificationHandlerTypeInfo()   
                {
                    HandlerType = notificationHandlerType,
                    HandlerInterface = iHandler,
                    NotificationTypeInfo = ConnectR.NotificationTypeInfo
                        .FromNotificationType(iHandler.GetGenericArguments().First())
                        .ToList(),
                })
                .ToList();

        public Type HandlerType { get; private set; }
        public Type HandlerInterface { get; private set; }
        public IReadOnlyList<NotificationTypeInfo> NotificationTypeInfo { get; private set; }
    }
}
