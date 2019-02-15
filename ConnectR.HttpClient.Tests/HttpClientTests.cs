using System;
using Autofac;
using MediatR.ConnectR.Autofac;
using MediatR.ConnectR.HttpClient.Autofac;
using Xunit;
using Http = System.Net.Http;

namespace MediatR.ConnectR.HttpClient
{
    public class HttpClientTests
    {


        [Fact]
        public void Mediator_Resolves_HttpClient()
        {
            var expectedBaseAddress = new Uri("http://localhost/");
            var builder = new ContainerBuilder();

            builder.RegisterModule<MediatorModule>();
            //builder.RegisterModule<MediatorMiddlewareModule>();

            builder.Register(c => new System.Net.Http.HttpClient()
            {
                BaseAddress = new Uri("http://NotUsed1"),
            });

            builder.RegisterHttpClientHandlers<TestRequest>(expectedBaseAddress.ToString());

            builder.Register(c => new Http.HttpClient()
            {
                BaseAddress = new Uri("http://NotUsed2"),
            });

            builder.RegisterType(GetType()).WithParameters();

            using (var scope = builder.Build())
            {
                var handler = scope.Resolve<IRequestHandler<TestRequest, TestResponse>>();
                var typedHandler = handler as HttpClientHandler<TestRequest, TestResponse>;

                Assert.NotNull(typedHandler);
                Assert.Equal(expectedBaseAddress, typedHandler.HttpClient.BaseAddress);

                var actual = handler
                    .GetType()
                    .GetGenericTypeDefinition();
                var expected = typeof(HttpClientHandler<,>);
                Assert.Equal(expected, actual);

            }
        }
    }
}
