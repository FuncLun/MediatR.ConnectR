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
            this IEnumerable<(Type HandlerType, Type HandlerInterface)> types
        )
            => types.SelectMany(t => t.MakeMediatorWrappers());

        public static IEnumerable<Type> MakeMediatorWrappers(
            this (Type HandlerType, Type HandlerInterface) types
        )
        {
            if (types.HandlerInterface.IsClosedTypeOf(typeof(IRequestHandler<,>)))
                yield return
                    typeof(MediatorRequestWrapper<,>)
                        .MakeGenericType(
                            types.HandlerInterface.GenericTypeArguments[0],
                            types.HandlerInterface.GenericTypeArguments[1]
                            );

            if (types.HandlerInterface.IsClosedTypeOf(typeof(INotificationHandler<>)))
                yield return
                    typeof(MediatorNotificationWrapper<>)
                        .MakeGenericType(
                            types.HandlerInterface.GenericTypeArguments[0],
                            typeof(Unit)
                            );
        }
    }
}
