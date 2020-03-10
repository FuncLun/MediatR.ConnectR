using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

#nullable enable

namespace MediatR.ConnectR.AspNetCore
{
    public class ConnectorMiddleware
    {
        public ConnectorMiddleware(
            RequestDelegate next,
            IConnectorProvider connectorProvider,
            IConnectorSerializer connectorSerializer,
            ConnectorMiddlewareOptions? connectorMiddlewareOptions = null
        )
        {
            Next = next;
            ConnectorProvider = connectorProvider;
            ConnectorSerializer = connectorSerializer;
            ConnectorMiddlewareOptions = connectorMiddlewareOptions
                                        ?? new ConnectorMiddlewareOptions();
        }

        private RequestDelegate Next { get; }

        private IConnectorProvider ConnectorProvider { get; }
        private IConnectorSerializer ConnectorSerializer { get; }

        private ConnectorMiddlewareOptions ConnectorMiddlewareOptions { get; }

        public async Task Invoke(
            HttpContext context,
            ServiceFactory serviceFactory
        )
        {
            var cancellationToken = context.RequestAborted;

            var entry = ConnectorProvider.TryGetEntry(context.Request.Path);
            if (entry is null)
            {
                await Next.Invoke(context);
                return;
            }

            object? response;
            var httpRequest = context.Request;

            try
            {
                var requestObject = await ConnectorSerializer.DeserializeRequest(
                    httpRequest.Body,
                    httpRequest.Query,
                    entry.MessageType,
                    cancellationToken
                );

                var mediator = (IMediator)serviceFactory(typeof(IMediator));

                response = await entry.Invoke(mediator, requestObject, cancellationToken);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;

                if (ConnectorMiddlewareOptions.BreakOnException && Debugger.IsAttached)
                    Debugger.Break();

                response = (ConnectorMiddlewareOptions.ReturnExceptionMessage)
                    ? new ExceptionDetail(ex, ConnectorMiddlewareOptions.ReturnExceptionDetails)
                    : default;
            }

            var httpResponse = context.Response;
            var (responseString, contentType) = ConnectorSerializer.SerializeResponse(response);

            context.Response.ContentType = $"Content-Type: {contentType}";

#if NETSTANDARD2_1
            var sw = new StreamWriter(httpResponse.Body);
            await using var _ = sw.ConfigureAwait(false);
#else
            using var sw = new StreamWriter(httpResponse.Body);
#endif
            await sw.WriteAsync(responseString);
        }
    }
}
