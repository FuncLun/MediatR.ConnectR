using System.Threading;
using System.Threading.Tasks;
using Autofac;
using MediatR.ConnectR.Autofac;
using Xunit;

// ReSharper disable ConvertToConstant.Local

namespace MediatR.ConnectR
{
    public class RegistrationTests
    {
        [Fact]
        public async Task Autofac_Resolves_Handler()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule<MediatorModule>();
            builder.RegisterAssemblyHandlers<RegistrationTests>();

            using (var scope = builder.Build())
            {
                var data = "Some Data";
                var req = new TestRequest() { Data = data };

                var handler = scope.Resolve<IRequestHandler<TestRequest, TestResponse>>();

                var resp = await handler.Handle(req, CancellationToken.None);

                Assert.Equal(data, resp.Result);
            }
        }
    }
}
