using System;
using System.Collections.Generic;
using System.Linq;
using Example.AspNetCore.Requests;
using MediatR.ConnectR;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Example.AspNetCore
{
    public static class SwaggerExts
    {
        public static void Add(this IDictionary<string, PathItem> paths, Type messageType)
        {
            var pathKey = messageType.MessageRelativePath();

            if (string.IsNullOrWhiteSpace(pathKey))
                return;

            paths.Add(
                $"/{pathKey}",
                FakePathItem(messageType)
            );
        }

        public static PathItem FakePathItem(Type messageType)
        {
            var lastChar = ' ';
            var tag = messageType.Name
                .Reverse()
                .TakeWhile(chr =>
                {
                    var stop = char.IsUpper(lastChar);
                    lastChar = chr;
                    return !stop;
                })
                .Reverse()
                .ConcatToString();

            var operation = new Operation()
            {
                Tags = new[] { messageType.Name },
                OperationId = "Post",
                Consumes = new[]
                {
                    "application/json-patch+json",
                    "application/json",
                    "text/json",
                    "application/*+json",
                }.ToList(),
                Parameters = new List<IParameter>()
                {
                    new BodyParameter()
                    {
                        Name = messageType.FullName,
                        Schema = new Schema()
                        {
                            Ref = "#/definitions/Test2Command",
                        },
                    }
                },
                Produces = new[] { "application/json" },
                Responses = new Dictionary<string, Response>()
                {
                    {
                        "200",
                        new Response()
                        {
                            Description = "Success",
                        }
                    },
                },
            };


            var pathItem = new PathItem();
            switch (tag)
            {
                case "Query":
                    pathItem.Get = operation;
                    break;

                //case "Command":
                default:
                    pathItem.Post = operation;
                    break;
            }

            return pathItem;
        }

        internal static string ConcatToString(this IEnumerable<char> chars)
            => string.Concat(chars);
    }



    public class ConnectorFakes : IDocumentFilter
    {
        /// <inheritdoc />
        public void Apply(SwaggerDocument swaggerDoc, DocumentFilterContext context)
        {
            swaggerDoc.Paths.Add(typeof(Test2Command));
        }
    }
}
