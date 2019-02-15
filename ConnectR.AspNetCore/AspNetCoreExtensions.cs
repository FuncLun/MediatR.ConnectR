using Microsoft.AspNetCore.Builder;

namespace MediatR.ConnectR.AspNetCore
{
    public static class AspNetCoreExtensions
    {
        public static IApplicationBuilder UseMediatorMiddleware(
            this IApplicationBuilder applicationBuilder
        )
            => applicationBuilder.UseMiddleware<MediatorMiddleware>();
    }
}
