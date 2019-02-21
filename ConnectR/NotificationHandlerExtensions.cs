using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MediatR.ConnectR
{
    public static class NotificationHandlerExtensions
    {
        public static IEnumerable<(Type NotificationType, Type ResponseType)>
            WhereIsNotificationHandler(
            this Assembly assembly
        )
            => assembly
                .GetTypes()
                .WhereIsNotificationHandler();

        public static IEnumerable<(Type NotificationType, Type ResponseType)>
            WhereIsNotificationHandler(
            this IEnumerable<Assembly> assemblies
        )
            => assemblies
                .SelectMany(asm => asm
                    .GetTypes()
                    .WhereIsNotificationHandler()
                );

        public static IEnumerable<(Type NotificationType, Type ResponseType)>
            WhereIsNotificationHandler(
            this IEnumerable<Type> type
        )
            => type
                .SelectMany(t => t
                    .GetInterfaces()
                    .Where(i => i.IsGenericType)
                    .Where(i => i.IsClosedTypeOf(typeof(INotificationHandler<>)))
                    .Select(i => (NotificationType: t, Interface: i))
                )
                .Select(t =>
                (
                    t.NotificationType,
                    ResponseType: typeof(Unit)
                ));
    }
}
