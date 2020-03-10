using System;

namespace MediatR.ConnectR
{
    public static class MessageExtensions
    {
        public static string MessageRelativePath(this Type messageType)
            => messageType
                .FullName
                ?.Replace('.', '/')
                .Replace('+', '.');
    }
}
