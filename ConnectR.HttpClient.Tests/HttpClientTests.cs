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
        private readonly Uri _expectedBaseAddress = new Uri("http://localhost/");
        private readonly Uri _notUsedBaseAddress = new Uri("http://NotUsed2");

        public HttpClientTests()
        {
            Builder = new ContainerBuilder();

            Builder.RegisterModule<MediatorModule>();

            Builder.RegisterInstance(new TestConfig()
                {
                    BaseUri = _expectedBaseAddress,
                })
                .AsImplementedInterfaces();

            Builder.Register(c => new Http.HttpClient()
                {
                    BaseAddress = _notUsedBaseAddress,
                })
                .SingleInstance();

            Builder.RegisterType(GetType()).WithParameters();
        }

        private ContainerBuilder Builder { get; }

        [Fact]
        public void Mediator_NoContextExt_Resolves_HttpClient()
        {
            Builder.RegisterHttpClientRequestHandlers<TestRequest>(
                (
                    handler
                ) => handler.BaseUri = _expectedBaseAddress
            );

            using var scope = Builder.Build();
            var httpClient = scope.Resolve<Http.HttpClient>();
            Assert.Equal(_notUsedBaseAddress, httpClient.BaseAddress);

            var handler = scope.Resolve<IRequestHandler<TestRequest, TestResponse>>();
            var typedHandler = handler as HttpClientHandler<TestRequest, TestResponse>;

            Assert.NotNull(typedHandler);
            Assert.Equal(_expectedBaseAddress, typedHandler.BaseUri);

            var actual = handler
                .GetType()
                .GetGenericTypeDefinition();

            var expected = typeof(HttpClientHandler<,>);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Mediator_ConfigExt_Resolves_HttpClient()
        {
            Builder.RegisterHttpClientRequestHandlers<TestRequest, ITestConfig>(
                (
                    h,
                    c
                ) => h.BaseUri = c.BaseUri
            );

            using var scope = Builder.Build();
            var httpClient = scope.Resolve<Http.HttpClient>();
            Assert.Equal(_notUsedBaseAddress, httpClient.BaseAddress);

            var handler = scope.Resolve<IRequestHandler<TestRequest, TestResponse>>();
            var typedHandler = handler as HttpClientHandler<TestRequest, TestResponse>;

            Assert.NotNull(typedHandler);
            Assert.Equal(_expectedBaseAddress, typedHandler.BaseUri);

            var actual = handler
                .GetType()
                .GetGenericTypeDefinition();

            var expected = typeof(HttpClientHandler<,>);
            Assert.Equal(expected, actual);

        }
    }
}
