using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Blazor;
using Http = System.Net.Http;

namespace MediatR.ConnectR.BlazorHttpClient
{
    public class HttpClientHandler<TRequest, TResponse>
        : IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        public HttpClientHandler(
            Http.HttpClient httpClient,
            Uri uriBase = null
        )
        {
            HttpClient = httpClient;
            UriBase = uriBase;
        }

        public Http.HttpClient HttpClient { get; }

        public Uri UriBase { get; }

        public static Uri RelativePath { get; }
            = new Uri(typeof(TRequest).FullName
                          ?.Replace('.', '/')
                          .Replace('+', '.')
                      ?? "",
                UriKind.Relative
            );

        public Uri OverridePath { get; set; }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
        {

            var uri = (UriBase == null)
                ? OverridePath ?? RelativePath
                : new Uri(UriBase, OverridePath ?? RelativePath);

            if (typeof(TResponse) != typeof(Unit))
                return await HttpClient.SendJsonAsync<TResponse>(
                    HttpMethod.Post,
                    uri.ToString(),
                    request);

            await HttpClient.SendJsonAsync(
                HttpMethod.Post,
                uri.ToString(),
                request);

            return default;
        }
    }
}