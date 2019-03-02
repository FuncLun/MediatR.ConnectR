using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using MediatR.ConnectR.Autofac;
using Xunit;
using Xunit.Sdk;
// ReSharper disable UnusedVariable

namespace MediatR.ConnectR
{
    public class PipelineTests
    {
        [Fact]
        public async Task Test1_Pipeline_Is_Not_Invoked()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule<MediatorModule>();
            builder.RegisterAssemblyMediatorHandlers<RegistrationTests>();

            using (var scope = builder.Build())
            {
                var mediator = scope.Resolve<IMediator>();
                var req = new Test1Request();

                var expected = await mediator.Send(req);

                var tryGet = Test1Pipeline
                    .RequestHistory
                    .TryGetValue(req, out var actual);

                Assert.False(tryGet);
                Assert.NotNull(expected);
                Assert.Null(actual);
                Assert.NotSame(expected, actual);
            }
        }


        [Fact]
        public async Task Test1_Pipeline_Is_Invoked()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule<MediatorModule>();
            builder.RegisterAssemblyMediatorHandlers<RegistrationTests>();
            builder.RegisterType<Test1Pipeline>()
                .AsImplementedInterfaces();


            using (var scope = builder.Build())
            {
                var mediator = scope.Resolve<IMediator>();
                var req = new Test1Request();

                var expected = await mediator.Send(req);

                var tryGet = Test1Pipeline
                    .RequestHistory
                    .TryGetValue(req, out var actual);

                Assert.True(tryGet);
                Assert.NotNull(expected);
                Assert.NotNull(actual);
                Assert.Same(expected, actual);
            }
        }


        [Fact]
        public async Task TestGeneric_Pipeline_Is_Not_Invoked()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule<MediatorModule>();
            builder.RegisterAssemblyMediatorHandlers<RegistrationTests>();

            using (var scope = builder.Build())
            {
                var mediator = scope.Resolve<IMediator>();
                var req = new Test1Request();

                var expected = await mediator.Send(req);

                var tryGet = TestGenericPipeline<Test1Request, Test1Response>
                    .RequestHistory
                    .TryGetValue(req, out var actual);

                Assert.False(tryGet);
                Assert.NotNull(expected);
                Assert.Null(actual);
                Assert.NotSame(expected, actual);
            }
        }

        [Fact]
        public async Task TestGeneric_Pipeline_Is_Invoked()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule<MediatorModule>();
            builder.RegisterAssemblyMediatorHandlers<RegistrationTests>();
            builder.RegisterType<TestGenericPipeline<Test1Request, Test1Response>>()
                .AsImplementedInterfaces();


            using (var scope = builder.Build())
            {
                var mediator = scope.Resolve<IMediator>();
                var req = new Test1Request();

                var expected = await mediator.Send(req);

                var tryGet = TestGenericPipeline<Test1Request, Test1Response>
                    .RequestHistory
                    .TryGetValue(req, out var actual);

                Assert.True(tryGet);
                Assert.NotNull(expected);
                Assert.NotNull(actual);
                Assert.Same(expected, actual);
            }
        }

        [Fact]
        public async Task TestGeneric_Pipeline_SpecificRegistration_Is_Not_Invoked()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule<MediatorModule>();
            builder.RegisterAssemblyMediatorHandlers<RegistrationTests>();
            builder.RegisterAssemblyMediatorPipelines<Test1Request>();

            builder.RegisterTypes(
                    new[] { typeof(Test2Request) }
                        .MakePipelineTypes(typeof(TestGenericPipeline<,>))
                        .ToArray()
                )
                .AsImplementedInterfaces();

            using (var scope = builder.Build())
            {
                var mediator = scope.Resolve<IMediator>();
                var req = new Test1Request();

                var expected = await mediator.Send(req);

                var tryGet = TestGenericPipeline<Test1Request, Test1Response>
                    .RequestHistory
                    .TryGetValue(req, out var actual);

                Assert.False(tryGet);
                Assert.NotNull(expected);
                Assert.Null(actual);
                Assert.NotSame(expected, actual);
            }
        }

        [Fact]
        public async Task TestGeneric_Pipeline_SpecificRegistration_Is_Invoked()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule<MediatorModule>();
            builder.RegisterAssemblyMediatorHandlers<RegistrationTests>();
            builder.RegisterAssemblyMediatorPipelines<Test1Request>();

            builder.RegisterTypes(
                    new[] { typeof(Test1Request) }
                        .MakePipelineTypes(typeof(TestGenericPipeline<,>))
                        .ToArray()
                )
                .AsImplementedInterfaces();

            using (var scope = builder.Build())
            {
                var mediator = scope.Resolve<IMediator>();
                var req = new Test1Request();

                var expected = await mediator.Send(req);

                var tryGet = TestGenericPipeline<Test1Request, Test1Response>
                    .RequestHistory
                    .TryGetValue(req, out var actual);

                Assert.True(tryGet);
                Assert.NotNull(expected);
                Assert.NotNull(actual);
                Assert.Same(expected, actual);
            }
        }

        [Fact]
        public async Task TestGeneric_Pipeline_ScannedRegistration_IsInvoked()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule<MediatorModule>();
            builder.RegisterAssemblyMediatorHandlers<RegistrationTests>();
            builder.RegisterAssemblyMediatorPipelines<Test1Request>();

            builder.RegisterPipelineForTypes(
                typeof(TestGenericPipeline<,>),
                typeof(Test1Request),
                typeof(Test2Request)
            );

            using (var scope = builder.Build())
            {
                var mediator = scope.Resolve<IMediator>();

                var req1 = new Test1Request();
                var expected1 = await mediator.Send(req1);
                var tryGet1 = TestGenericPipeline<Test1Request, Test1Response>
                    .RequestHistory
                    .TryGetValue(req1, out var actual1);

                Assert.True(tryGet1);
                Assert.NotNull(expected1);
                Assert.NotNull(actual1);
                Assert.Same(expected1, actual1);

                var req2 = new Test2Request();
                var expected2 = await mediator.Send(req2);
                var tryGet2 = TestGenericPipeline<Test2Request, Test2Response>
                    .RequestHistory
                    .TryGetValue(req2, out var actual2);

                Assert.True(tryGet2);
                Assert.NotNull(expected2);
                Assert.NotNull(actual2);
                Assert.Same(expected2, actual2);
            }
        }
    }
}
