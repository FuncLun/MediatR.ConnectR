using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Http = System.Net.Http;

namespace MediatR.ConnectR.HttpClient
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
            var requestString = JsonConvert.SerializeObject(request);

            var uri = (UriBase == null)
                ? OverridePath ?? RelativePath
                : new Uri(UriBase, OverridePath ?? RelativePath);

            using (var requestContent = new StringContent(requestString, Encoding.UTF8, "applicaiton/json"))
            using (var response = await HttpClient.PostAsync(uri, requestContent, cancellationToken))
            {
                var responseString = await response.Content.ReadAsStringAsync();
                var responseObject = JsonConvert.DeserializeObject<TResponse>(responseString);

                return responseObject;
            }
        }
    }
}
