using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using MediatR.ConnectR.AspNetCore.Autofac;
using MediatR.ConnectR.Autofac;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Moq;
using Newtonsoft.Json.Linq;
using Xunit;

namespace MediatR.ConnectR.AspNetCore
{
    public class HttpClientTests
    {
        [Fact]
        public async Task Middleware_Writes_To_OutputStream()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule<MediatorModule>();
            builder.RegisterModule<MediatorMiddlewareModule>();
            builder.RegisterAssemblyMediatorHandlers(this);
            builder.RegisterMediatorRequestWrappers(this);

            builder.Register<RequestDelegate>(ctx => _ => throw new Exception("Should not execute RequestDelegate"));

            var someData = "Some Data";
            var bytes = Encoding.UTF8.GetBytes($"{{\"Data\":\"{someData}\"}}");

            using var scope = builder.Build();
            using var requestStream = new MemoryStream(bytes);
            using var responseStream = new MemoryStream();
            var middleware = scope.Resolve<MediatorMiddleware>();
            var serviceFactory = scope.Resolve<ServiceFactory>();

            var request = new Mock<HttpRequest>();
            request.Setup(r => r.Path)
                .Returns("/MediatR/ConnectR/AspNetCore/TestRequest");
            request.Setup(r => r.Method)
                .Returns("POST");
            request.Setup(r => r.Body)
                .Returns(requestStream);
            request.Setup(r => r.Query)
                .Returns(new QueryCollection());

            var response = new Mock<HttpResponse>();
            response.SetupProperty(r => r.StatusCode);
            response.Setup(r => r.Body)
                .Returns(responseStream);

            var context = new Mock<HttpContext>();
            context.Setup(c => c.Request)
                .Returns(request.Object);
            context.Setup(c => c.Response)
                .Returns(response.Object);

            await middleware.Invoke(context.Object, serviceFactory);

            var expected = $"{{\"result\":\"{someData}\"}}";
            var actual = Encoding.UTF8.GetString(responseStream.ToArray());

            Assert.Equal(expected, actual, StringComparer.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task DeserializeBody_Json_Is_CamelCase()
        {
            var someData = "Some Data";
            var bytes = Encoding.UTF8.GetBytes($"{{\"Data\":\"{someData}\"}}");

            using var stream = new MemoryStream(bytes);
            var middlewareMock = new Mock<MediatorMiddleware>(MockBehavior.Strict, null, null, null);
            var httpContext = new DefaultHttpContext();
            var httpRequest = httpContext.Request;
            httpRequest.Method = "POST";
            httpRequest.QueryString = new QueryString($"?Data2={someData}");
            httpRequest.Body = stream;

            var requestType = typeof(TestRequest);

            middlewareMock.Setup(m => m.DeserializeRequest(httpRequest, requestType))
                .CallBase();
            middlewareMock.Setup(m => m.DeserializeBody(httpRequest))
                .CallBase();
            middlewareMock.Setup(m => m.DeserializeQueryString(httpRequest, It.IsAny<JObject>()))
                .CallBase();

            var requestObject = await middlewareMock.Object.DeserializeRequest(httpRequest, requestType);

            Assert.IsType<TestRequest>(requestObject);

            var request = (TestRequest)requestObject;

            Assert.Equal(someData, request.Data);
            Assert.Equal(someData, request.Data2);
        }
    }
}
