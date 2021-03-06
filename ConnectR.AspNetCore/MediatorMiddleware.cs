﻿using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace MediatR.ConnectR.AspNetCore
{
    public class MediatorMiddleware
    {
        public MediatorMiddleware(
            RequestDelegate next,
            IMediatorRegistry mediatorRegistry,
            MediatorMiddlewareOptions mediatorMiddlewareOptions = null
            )
        {
            Next = next;
            MediatorRegistry = mediatorRegistry;
            MediatorMiddlewareOptions = mediatorMiddlewareOptions;
        }

        private RequestDelegate Next { get; }
        private IMediatorRegistry MediatorRegistry { get; }
        private MediatorMiddlewareOptions MediatorMiddlewareOptions { get; }

        private JsonSerializerSettings _jsonSerializerSettings;

        public JsonSerializerSettings JsonSerializerSettings
            => MediatorMiddlewareOptions?.JsonSerializerSettings
               ?? _jsonSerializerSettings
               ?? (_jsonSerializerSettings = new JsonSerializerSettings()
               {
                   Formatting = Formatting.None,
                   NullValueHandling = NullValueHandling.Ignore,
                   ContractResolver = new DefaultContractResolver()
                   {
                       NamingStrategy = new CamelCaseNamingStrategy(),
                   }
               });

        public async Task Invoke(HttpContext context, ServiceFactory serviceFactory)
        {
            var mediatorWrapperType = MediatorRegistry.FindWrapperType(context.Request.Path);
            if (mediatorWrapperType is null)
            {
                await Next.Invoke(context);
                return;
            }

            var mediatorWrapper = TryFindWrapper(context, serviceFactory);
            if (mediatorWrapper is null)
            {
                await Next.Invoke(context);
                return;
            }

            var requestObject = await DeserializeRequest(context.Request, mediatorWrapper.MessageType);
            if (requestObject is null)
            {
                await Next.Invoke(context);
                return;
            }

            var responseObject = await TryInvoke(context, requestObject, mediatorWrapper);
            await SerializeResponse(context.Response, responseObject);
        }

        public virtual IMediatorWrapper TryFindWrapper(HttpContext context, ServiceFactory serviceFactory)
        {
            try
            {
                var mediatorWrapperType = MediatorRegistry.FindWrapperType(context.Request.Path);

                if (!(mediatorWrapperType is null))
                    return serviceFactory(mediatorWrapperType) as IMediatorWrapper;
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
                    {
                        if (sr.EndOfStream)
                            return new JObject();

                        using (var jr = new JsonTextReader(sr))
                            return await JObject.LoadAsync(jr);
                    }

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
            IMediatorWrapper mediatorWrapper
            )
        {
            try
            {
                return await mediatorWrapper.Invoke(requestObject, context.RequestAborted);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;

                if (MediatorMiddlewareOptions?.BreakOnException == true && Debugger.IsAttached)
                    Debugger.Break();

                return (MediatorMiddlewareOptions?.ReturnExceptionMessage == true)
                    ? new ExceptionDetail(ex, MediatorMiddlewareOptions.ReturnExceptionDetails)
                    : default;
            }
        }

        public virtual async Task SerializeResponse(HttpResponse httpResponse, object responseObject)
        {
            httpResponse.ContentType = "Content-Type: application/json; charset=utf-8";

            using (var sw = new StreamWriter(httpResponse.Body))
                await sw.WriteAsync(JsonConvert.SerializeObject(responseObject, JsonSerializerSettings));
        }
    }
}
