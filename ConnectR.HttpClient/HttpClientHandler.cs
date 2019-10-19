using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Http = System.Net.Http;

namespace MediatR.ConnectR.HttpClient
{
    public interface IHttpClientHandler
    {
        Uri BaseUri { get; set; }

        JsonSerializerSettings JsonSerializerSettings { get; set; }

        Uri RelativePath { get; set; }

    }

    public class HttpClientHandler<TRequest, TResponse>
        : IHttpClientHandler, IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private static Uri DefaultRelativePath { get; }
            = new Uri(
                typeof(TRequest).MessageRelativePath() ?? "",
                UriKind.Relative
            );

        public HttpClientHandler(
            Http.HttpClient httpClient,
            Uri baseUri = null,
            JsonSerializerSettings jsonSerializerSettings = null,
            Uri relativePath = null
        )
        {
            HttpClient = httpClient;
            BaseUri = baseUri;
            JsonSerializerSettings = jsonSerializerSettings;
            RelativePath = relativePath;
        }

        private Http.HttpClient HttpClient { get; }

        public Uri BaseUri { get; set; }

        public JsonSerializerSettings JsonSerializerSettings { get; set; }

        public Uri RelativePath { get; set; }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
        {
            var requestString = JsonConvert.SerializeObject(request, JsonSerializerSettings);

            var uri = (BaseUri == null)
                ? RelativePath ?? DefaultRelativePath
                : new Uri(BaseUri, RelativePath ?? DefaultRelativePath);

            using var requestContent = new StringContent(requestString, Encoding.UTF8, "applicaiton/json");
            using var response = await HttpClient.PostAsync(uri, requestContent, cancellationToken);
            var responseString = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<TResponse>(responseString, JsonSerializerSettings);

            //TODO: Throw on non success (200/300) response codes

            return responseObject;
        }
    }
}
