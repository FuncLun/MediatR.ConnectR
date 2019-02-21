using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MediatR.ConnectR
{
    public static class MediatorNotificationExtensions
    {
        public static IEnumerable<(Type NotificationType, Type ResponseType)> ScanForMediatorNotificationTypes(
            this Assembly assembly
        )
            => assembly
                .GetExportedTypes()
                .ScanForMediatorNotificationTypes();

        public static IEnumerable<(Type NotificationType, Type ResponseType)> ScanForMediatorNotificationTypes(
            this IEnumerable<Assembly> assemblies
        )
            => assemblies
                .SelectMany(asm => asm
                    .GetExportedTypes()
                    .ScanForMediatorNotificationTypes()
                );

        public static IEnumerable<(Type NotificationType, Type ResponseType)> ScanForMediatorNotificationTypes(
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
