using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
// ReSharper disable UnusedVariable

namespace MediatR.ConnectR
{
    public sealed class MicrosoftDependencyInjectionTests : IDisposable
    {
        private IServiceCollection _services;

        private IServiceCollection Services
            => _services
                ??= new ServiceCollection()
                    .AddMediatR()
                    .AddMediatRClasses(GetType().Assembly);

        private ServiceProvider _provider;

        private IServiceProvider Provider
            => _provider
                ??= Services.BuildServiceProvider();

        private IMediator _mediator;

        private IMediator Mediator
            => _mediator
                ??= Provider.GetService<IMediator>();


        [Fact]
        public async Task Test1_Pipeline_Is_Invoked()
        {
            Services.AddTransient<IPipelineBehavior<Test1Request, Test1Response>, Test1Pipeline>();

            var req = new Test1Request();

            var expected = await Mediator.Send(req);

            var tryGet = Test1Pipeline
                .RequestHistory
                .TryGetValue(req, out var actual);

            Assert.True(tryGet);
            Assert.NotNull(expected);
            Assert.NotNull(actual);
            Assert.Same(expected, actual);
        }

        [Fact]
        public async Task Test1_Pipeline_Is_Not_Invoked()
        {
            var req = new Test1Request();

            var expected = await Mediator.Send(req);

            var tryGet = Test1Pipeline
                .RequestHistory
                .TryGetValue(req, out var actual);

            Assert.False(tryGet);
            Assert.NotNull(expected);
            Assert.Null(actual);
            Assert.NotSame(expected, actual);
        }

        [Fact]
        public async Task TestGeneric_Pipeline_Is_Invoked()
        {
            Services
                .AddTransient<IPipelineBehavior<Test1Request, Test1Response>,
                    TestGenericPipeline<Test1Request, Test1Response>>();

            var req = new Test1Request();

            var expected = await Mediator.Send(req);

            var tryGet = TestGenericPipeline<Test1Request, Test1Response>
                .RequestHistory
                .TryGetValue(req, out var actual);

            Assert.True(tryGet);
            Assert.NotNull(expected);
            Assert.NotNull(actual);
            Assert.Same(expected, actual);
        }

        [Fact]
        public async Task TestGeneric_Pipeline_Is_Not_Invoked()
        {
            var req = new Test1Request();

            var expected = await Mediator.Send(req);

            var tryGet = TestGenericPipeline<Test1Request, Test1Response>
                .RequestHistory
                .TryGetValue(req, out var actual);

            Assert.False(tryGet);
            Assert.NotNull(expected);
            Assert.Null(actual);
            Assert.NotSame(expected, actual);
        }

        public void Dispose()
        {
            _provider?.Dispose();
        }
    }
}
