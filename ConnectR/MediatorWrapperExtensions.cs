using System;
using System.Collections.Generic;
using System.Linq;

namespace MediatR.ConnectR
{
    public static class MediatorWrapperExtensions
    {
        public static IEnumerable<Type> MakeMediatorMessageDelegates(
            this IEnumerable<(Type MessageType, Type ResponseType)> types
        )
            => types.SelectMany(t => t.MakeMediatorMessageDelegates())
                .ToList();


        public static IEnumerable<Type> MakeMediatorMessageDelegates(
            this (Type MessageType, Type ResponseType) types
        )
        {
            var interfaces = types.MessageType.GetInterfaces();
            if (interfaces.Any(i => i.IsClosedTypeOf(typeof(IRequest<>))))
                yield return
                    typeof(MediatorWrapperForRequest<,>)
                        .MakeGenericType(types.MessageType, types.ResponseType);

            if (interfaces.Any(i => i == typeof(INotification)))
                yield return
                    typeof(MediatorWrapperForNotification<>)
                        .MakeGenericType(types.MessageType);
        }
    }
}
