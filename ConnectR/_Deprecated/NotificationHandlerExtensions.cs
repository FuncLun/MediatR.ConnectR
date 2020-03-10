using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MediatR.ConnectR
{
    public static class NotificationHandlerExtensions
    {
        public static IEnumerable<NotificationHandlerTypeInfo>
            SelectNotificationHandlerTypeInfos(
                this Assembly assembly
            )
            => assembly
                .GetTypes()
                .SelectNotificationHandlerTypeInfos();

        public static IEnumerable<NotificationHandlerTypeInfo>
            SelectNotificationHandlerTypeInfos(
                this IEnumerable<Assembly> assemblies
            )
            => assemblies
                .SelectMany(asm => asm
                    .GetTypes()
                    .SelectMany(NotificationHandlerTypeInfo.FromHandlerType)
                );

        public static IEnumerable<NotificationHandlerTypeInfo>
            SelectNotificationHandlerTypeInfos(
                this IEnumerable<Type> notificationHandlerType
            )
            => notificationHandlerType.SelectMany(NotificationHandlerTypeInfo.FromHandlerType);
    }
}
