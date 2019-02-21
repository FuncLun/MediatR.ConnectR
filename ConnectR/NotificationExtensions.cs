using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MediatR.ConnectR
{
    public static class NotificationExtensions
    {
        public static IEnumerable<(Type NotificationType, Type ResponseType)> 
            WhereIsNotification(
            this Assembly assembly
        )
            => assembly
                .GetExportedTypes()
                .WhereIsNotification();

        public static IEnumerable<(Type NotificationType, Type ResponseType)> 
            WhereIsNotification(
            this IEnumerable<Assembly> assemblies
        )
            => assemblies
                .SelectMany(asm => asm
                    .GetExportedTypes()
                    .WhereIsNotification()
                );

        public static IEnumerable<(Type NotificationType, Type ResponseType)> 
            WhereIsNotification(
            this IEnumerable<Type> notificationType
        )
            => notificationType
                .SelectMany(t => t
                    .GetInterfaces()
                    .Where(i => i == typeof(INotification))
                    .Select(i => (NotificationType: t, Interface: i))
                )
                .Select(t =>
                (
                    t.NotificationType,
                    ResponseType: typeof(Unit)
                ));
    }
}
