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
            RegisterHttpClientHandlers<TAssemblyFromType>(
                this ContainerBuilder builder,
                string baseAddress,
                Type openGenericType = null
            )
            => builder.RegisterHttpClientHandlers<TAssemblyFromType>(
                new Uri(baseAddress),
                openGenericType
            );

        public static IRegistrationBuilder
            <
                object,
                ScanningActivatorData,
                DynamicRegistrationStyle
            >
            RegisterHttpClientHandlers<TAssemblyFromType>(
                this ContainerBuilder builder,
                Uri baseAddress,
                Type openGenericType = null
            )
            => builder.RegisterClientRequestHandlers<TAssemblyFromType>(openGenericType ?? typeof(HttpClientHandler<,>))
                .WithParameter(
                    TypedParameter.From(
                        new Http.HttpClient()
                        {
                            BaseAddress = baseAddress
                        })
                );

    }
}
