using System.Linq;
using Autofac;
using Autofac.Builder;
using Autofac.Core;

namespace MediatR.ConnectR.Autofac
{
    public static class AutofacExts
    {
        public static IRegistrationBuilder<TLimit, TReflectionActivatorData, TStyle>
            WithParameters<TLimit, TReflectionActivatorData, TStyle>(
                this IRegistrationBuilder<TLimit, TReflectionActivatorData, TStyle> registration,
                params Parameter[] parameters
            )
            where TReflectionActivatorData : ReflectionActivatorData
            => registration.WithParameters(parameters.AsEnumerable());
    }
}
