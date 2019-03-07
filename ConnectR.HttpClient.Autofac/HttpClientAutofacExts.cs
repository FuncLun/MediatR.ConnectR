using System;
using System.Linq;
using Autofac;
using Autofac.Builder;
using Autofac.Features.Scanning;
using MediatR.ConnectR.Autofac;
using Http = System.Net.Http;

namespace MediatR.ConnectR.HttpClient.Autofac
{
    public static class HttpClientAutofacExts
    {
        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle>
            RegisterHttpClientRequestHandlers<TAssemblyFromType>(
                this ContainerBuilder builder,
                Action<IHttpClientHandler> handlerSetup
            )
            => builder.RegisterHttpClientRequestHandlers<TAssemblyFromType>(
                (handler, context) =>
                {
                    handlerSetup(handler);
                });

        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle>
            RegisterHttpClientRequestHandlers<TAssemblyFromType, TResolvedService>(
                this ContainerBuilder builder,
                Action<IHttpClientHandler, TResolvedService> handlerSetup
            )
            => builder.RegisterHttpClientRequestHandlers<TAssemblyFromType>(
                (handler, context) =>
                {
                    var resolvedService = context.Resolve<TResolvedService>();
                    handlerSetup(handler, resolvedService);
                });

        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle>
            RegisterHttpClientRequestHandlers<TAssemblyFromType>(
                this ContainerBuilder builder,
                Action<IHttpClientHandler, IComponentContext> handlerSetup = null
            )
        {
            var registration = builder.RegisterTypes(
                    typeof(TAssemblyFromType).Assembly
                        .WhereIsRequest()
                        .Select(v =>
                            typeof(HttpClientHandler<,>)
                                .MakeGenericType(
                                    v.RequestType,
                                    v.ResponseType
                                )
                        )
                        .ToArray()
                )
                .AsImplementedInterfaces()
                .OnActivating(eventArgs =>
                {
                    if (!(eventArgs.Instance is IHttpClientHandler handler))
                        return;

                    handlerSetup?.Invoke(handler, eventArgs.Context);
                });

            return registration;
        }

        public static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle>
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
