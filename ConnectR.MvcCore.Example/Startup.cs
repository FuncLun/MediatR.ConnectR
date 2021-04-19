using System.Threading.Tasks;
using ConnectR.MvcCore.Example.Controllers;
using MediatR.ConnectR;
using MediatR.ConnectR.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing.Patterns;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ConnectR.MvcCore.Example
{
    public class Startup
    {
        public Startup(
            IConfiguration configuration
        )
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(
            IServiceCollection services
        )
        {
            services.AddControllers();

            services.AddMediatR();
            services.AddMediatRClasses(typeof(WeatherForecastController).Assembly);
            services.AddConnectR();

            services.AddMediatRRequests(GetType().Assembly, typeof(ExceptionDetail).Assembly);
            services.AddMediatRNotifications(GetType().Assembly, typeof(ExceptionDetail).Assembly);
        }


        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env
        )
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseConnectR();

            app.UseRouting();

            app.Map(
                "ConnectR/MvcCore/Example/TestRequest",
                context => context.UseMiddleware<ConnectorMiddleware>());

            app.UseEndpoints(endpoints =>
            {
                endpoints.Map((RoutePattern)null, Handle);
                //endpoints.MapHealthChecks("/health");
            });

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", context => context.Response.WriteAsync("Hello world"));
                endpoints.MapControllers();
            });
        }

        public Task Handle(
            HttpContext context
        )
        {
            return Task.CompletedTask;
        }
    }
}
