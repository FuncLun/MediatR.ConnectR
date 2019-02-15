using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MediatR.ConnectR.AspNetCore
{
    public class MediatorMiddleware
    {
        public MediatorMiddleware(
            RequestDelegate next,
            IMediatorRegistry mediatorRegistry
            )
        {
            Next = next;
            MediatorRegistry = mediatorRegistry;
        }

        private RequestDelegate Next { get; }
        private IMediatorRegistry MediatorRegistry { get; }

        public async Task Invoke(HttpContext context, ServiceFactory serviceFactory)
        {
            var mediatorDelegateType = MediatorRegistry.FindDelegateType(context.Request.Path);
            if (mediatorDelegateType is null)
            {
                await Next.Invoke(context);
                return;
            }

            var mediatorDelegate = TryFindDelegate(context, serviceFactory);
            if (mediatorDelegate is null)
            {
                await Next.Invoke(context);
                return;
            }

            var requestObject = await DeserializeRequest(context.Request, mediatorDelegate.MessageType);
            if (requestObject is null)
            {
                await Next.Invoke(context);
                return;
            }

            var responseObject = await TryInvoke(context, requestObject, mediatorDelegate);
            await SerializeResponse(context.Response, responseObject);
        }

        public virtual IMediatorWrapper TryFindDelegate(HttpContext context, ServiceFactory serviceFactory)
        {
            try
            {
                var mediatorDelegateType = MediatorRegistry.FindDelegateType(context.Request.Path);

                if (!(mediatorDelegateType is null))
                    return serviceFactory(mediatorDelegateType) as IMediatorWrapper;
            }
            catch (Exception ex)
            {
                //TODO: Log Exception
                Debug.WriteLine(ex.Message);
            }

            return default;
        }

        public virtual async Task<object> DeserializeRequest(
            HttpRequest request,
            Type requestType
        )
        {
            var jObject = await DeserializeBody(request);

            DeserializeQueryString(request, jObject);

            return jObject.ToObject(requestType);
        }

        public virtual async Task<JObject> DeserializeBody(HttpRequest httpRequest)
        {
            switch (httpRequest.Method.ToUpperInvariant())
            {
                case "POST":
                    using (var sr = new StreamReader(httpRequest.Body))
                    using (var jr = new JsonTextReader(sr))
                        return await JObject.LoadAsync(jr);

                case "GET":
                    return new JObject();

                default:
                    throw new InvalidOperationException("Only POST and GET are supported at this time");
            }
        }

        public virtual void DeserializeQueryString(HttpRequest httpRequest, JObject jObject)
        {
            if (httpRequest.Query?.Any() != true)
                return;

            foreach (
                var (parameterName, value) in httpRequest
                    .Query
                    .Select(
                        kvp =>
                        (
                            ParameterName: kvp.Key,
                            Value: JToken.FromObject(
                                (kvp.Value.Count < 2)
                                    ? (object)kvp.Value.ToString()
                                    : kvp.Value.Where(v => !string.IsNullOrEmpty(v)))
                        )
                    )
            )
            {
                jObject.Add(parameterName, value);
            }
        }

        public virtual async Task<object> TryInvoke(
            HttpContext context,
            object requestObject,
            IMediatorWrapper mediatorDelegate
            )
        {
            try
            {
                return await mediatorDelegate.Invoke(requestObject, context.RequestAborted);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;

                Debug.WriteLine(ex.Message);
                Debugger.Break();
            }

            return default;
        }

        public virtual Task SerializeResponse(HttpResponse httpResponse, object responseObject)
        {
            var js = new JsonSerializer();

            //if (resp != null && !(resp is Unit))
            using (var sw = new StreamWriter(httpResponse.Body))
            using (var jw = new JsonTextWriter(sw))
                js.Serialize(jw, responseObject);

            httpResponse.ContentType = "Content-Type: application/json; charset=utf-8";
            httpResponse.StatusCode = 200;

            return Task.CompletedTask;
        }
    }
}
