using System;
using System.Collections.Generic;
using System.Linq;

namespace MediatR.ConnectR
{
    public struct NotificationTypeInfo
    {
        public static IEnumerable<NotificationTypeInfo> FromNotificationType(Type notificationType)
        {
            return notificationType
                .GetInterfaces()
                .Where(i => i.IsGenericType)
                .Where(i => i.IsClosedTypeOf(typeof(IRequest<>)))
                .Select(i => new NotificationTypeInfo()
                {
                    NotificationType = notificationType,
                    NotificationInterface = i,
                });
        }


        public Type NotificationType { get; private set; }
        public Type NotificationInterface { get; private set; }
    }
}