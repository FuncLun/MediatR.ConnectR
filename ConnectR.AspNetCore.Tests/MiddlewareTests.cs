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
            builder.RegisterAssemblyHandlers(this);
            builder.RegisterMediatorWrappers(this);
            builder.RegisterMediatorRegistry<MediatorRegistry>();

            builder.Register<RequestDelegate>(ctx => _ => throw new Exception("Should not execute RequestDelegate"));

            var someData = "Some Data";
            var bytes = Encoding.UTF8.GetBytes($"{{\"Data\":\"{someData}\"}}");

            using (var scope = builder.Build())
            using (var requestStream = new MemoryStream(bytes))
            using (var responseStream = new MemoryStream())
            {
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

                var expected = $"{{\"Result\":\"{someData}\"}}";
                var actual = Encoding.UTF8.GetString(responseStream.ToArray());

                Assert.Equal(expected, actual);
            }
        }
    }
}
