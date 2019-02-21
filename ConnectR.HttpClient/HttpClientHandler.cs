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
            Uri baseUri = null,
            Uri relativePath = null,
            JsonSerializerSettings jsonSerializerSettings = null
        )
        {
            HttpClient = httpClient;
            BaseUri = baseUri;
            RelativePath = relativePath;
            JsonSerializerSettings = jsonSerializerSettings;
        }

        internal Http.HttpClient HttpClient { get; }

        private Uri BaseUri { get; }

        private Uri RelativePath { get; }

        private JsonSerializerSettings JsonSerializerSettings { get; }

        private static Uri DefaultRelativePath { get; }
            = new Uri(
                typeof(TRequest).MessageRelativePath() ?? "",
                UriKind.Relative
            );

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
        {
            var requestString = JsonConvert.SerializeObject(request, JsonSerializerSettings);

            var uri = (BaseUri == null)
                ? RelativePath ?? DefaultRelativePath
                : new Uri(BaseUri, RelativePath ?? DefaultRelativePath);

            using (var requestContent = new StringContent(requestString, Encoding.UTF8, "applicaiton/json"))
            using (var response = await HttpClient.PostAsync(uri, requestContent, cancellationToken))
            {
                var responseString = await response.Content.ReadAsStringAsync();
                var responseObject = JsonConvert.DeserializeObject<TResponse>(responseString, JsonSerializerSettings);

                return responseObject;
            }
        }
    }
}
