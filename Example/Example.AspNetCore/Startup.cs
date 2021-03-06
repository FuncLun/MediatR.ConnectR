﻿using Autofac;
using Example.AspNetCore.Handlers;
using Example.AspNetCore.Requests;
using MediatR.ConnectR;
using MediatR.ConnectR.AspNetCore;
using MediatR.ConnectR.AspNetCore.Autofac;
using MediatR.ConnectR.Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace Example.AspNetCore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new Info
                    {
                        Title = "Example.AspNetCore",
                        Version = "v1",
                        Description = "Examples for MediatR.ConnectR",
                    });
                    c.DocumentFilter<ConnectorFakes>();
                });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule<MediatorModule>();
            builder.RegisterModule<MediatorMiddlewareModule>();

            builder.RegisterAssemblyMediatorHandlers<Test1Handler>();

            builder.RegisterMediatorRequestWrappers<Test1Query>();
            //builder.RegisterMediatorRegistry<MediatorRegistry>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Example.AspNetCore");
                //c.SwaggerEndpoint("/swagger.json", "My API V1");

            });
            app.UseHttpsRedirection();
            app.UseMediatorMiddleware();
            app.UseMvc();
        }
    }
}
