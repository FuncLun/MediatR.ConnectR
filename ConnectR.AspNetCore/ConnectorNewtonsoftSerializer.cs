using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

#nullable enable

namespace MediatR.ConnectR.AspNetCore
{
    public interface IConnectorSerializer
    {
        Task<object> DeserializeRequest(
            Stream body,
            IQueryCollection query,
            Type requestType,
            CancellationToken cancellationToken
        );

        (string ResponseString, string ContentType) SerializeResponse(
            object? responseObject
        );
    }

    public class ConnectorNewtonsoftSerializer : IConnectorSerializer
    {
        public ConnectorNewtonsoftSerializer()
            : this(null)
        { }

        public ConnectorNewtonsoftSerializer(
            JsonSerializerSettings? jsonSerializerSettings
        )
        {
            JsonSerializerSettings = jsonSerializerSettings
                                     ?? new JsonSerializerSettings()
                                     {
                                         Formatting = Formatting.None,
                                         NullValueHandling = NullValueHandling.Ignore,
                                         ContractResolver = new DefaultContractResolver()
                                         {
                                             NamingStrategy = new CamelCaseNamingStrategy(),
                                         }
                                     };
        }

        public JsonSerializerSettings JsonSerializerSettings { get; }

        public async Task<object> DeserializeRequest(
            Stream body,
            IQueryCollection query,
            Type requestType,
            CancellationToken cancellationToken
        )
        {
            var jObject = await DeserializeBody(body, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            DeserializeQueryString(query, jObject);

            cancellationToken.ThrowIfCancellationRequested();
            return jObject.ToObject(requestType);
        }

        public virtual async Task<JObject> DeserializeBody(
            Stream body,
            CancellationToken cancellationToken
        )
        {
            using var sr = new StreamReader(body);

            var bodyString = await sr.ReadToEndAsync();
            if (string.IsNullOrWhiteSpace(bodyString))
                bodyString = "{}";

            return JObject.Parse(bodyString);
        }

        public virtual void DeserializeQueryString(
            IQueryCollection? queryCollection,
            JObject jObject
        )
        {
            if (queryCollection?.Any() != true)
                return;

            queryCollection
                .Select(
                    kvp =>
                    (
                        ParameterName: kvp.Key,
                        Value: JToken.FromObject(
                            (kvp.Value.Count < 2)
                                ? (object)kvp.Value.ToString()
                                : kvp.Value.Where(v => !string.IsNullOrEmpty(v))
                        )
                    )
                )
                .ToList()
                .ForEach(a => jObject.Add(a.ParameterName, a.Value));
        }

        public (string ResponseString, string ContentType) SerializeResponse(
            object? responseObject
        )
            => (
                //TODO: null check: responseObject
                JsonConvert.SerializeObject(responseObject, JsonSerializerSettings),
                "application/json; charset=utf-8"
            );
    }
}