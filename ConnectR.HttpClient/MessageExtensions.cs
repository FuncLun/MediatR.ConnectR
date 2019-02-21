using System;

namespace MediatR.ConnectR.HttpClient
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
