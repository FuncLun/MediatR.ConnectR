using System;
using Autofac;
using Autofac.Builder;
using Autofac.Features.Scanning;
using MediatR.ConnectR.Autofac;
using Http = System.Net.Http;

namespace MediatR.ConnectR.HttpClient.Autofac
{
    public static class HttpClientAutofacExts
    {
        public static IRegistrationBuilder
            <
                object,
                ScanningActivatorData,
                DynamicRegistrationStyle
            >
            RegisterHttpClientRequestHandlers<TAssemblyFromType>(
                this ContainerBuilder builder,
                Uri baseAddress
            )
            => builder.RegisterClientRequestHandlers<TAssemblyFromType>(typeof(HttpClientHandler<,>))
                .WithParameter(
                    TypedParameter.From(
                        new Http.HttpClient()
                        {
                            BaseAddress = baseAddress
                        })
                );

        public static IRegistrationBuilder
            <
                object,
                ScanningActivatorData,
                DynamicRegistrationStyle
            >
            RegisterHttpClientNotificationHandlers<TAssemblyFromType>(
                this ContainerBuilder builder,
                Uri baseAddress
            )
            => builder.RegisterClientRequestHandlers<TAssemblyFromType>(typeof(HttpClientHandler<,>))
                .WithParameter(
                    TypedParameter.From(
                        new Http.HttpClient()
                        {
                            BaseAddress = baseAddress
                        })
                );

    }
}
