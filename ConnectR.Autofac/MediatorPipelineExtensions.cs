using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;

namespace MediatR.ConnectR.Autofac
{
    public static class MediatorPipelineExtensions
    {
        public static ContainerBuilder
            RegisterAssemblyMediatorPipelines(
                this ContainerBuilder builder,
                object assemblyContainingObject
            )
            => builder.RegisterAssemblyMediatorPipelines(new[] { assemblyContainingObject.GetType().Assembly });

        public static ContainerBuilder
            RegisterAssemblyMediatorPipelines(
                this ContainerBuilder builder,
                Type assemblyContainingType
            )
            => builder.RegisterAssemblyMediatorPipelines(new[] { assemblyContainingType.Assembly });

        public static ContainerBuilder
            RegisterAssemblyMediatorPipelines<TAssemblyContainingType>(
                this ContainerBuilder builder
            )
            => builder.RegisterAssemblyMediatorPipelines(new[] { typeof(TAssemblyContainingType).Assembly });


        public static ContainerBuilder RegisterAssemblyMediatorPipelines(
            this ContainerBuilder builder,
            IEnumerable<Assembly> assemblies
        )
            => builder.RegisterAssemblyMediatorPipelines(assemblies.ToArray());

        /// <summary>
        /// Registers concrete handlers that implement IRequestHandler&lt;,&gt; or INotificationHandler&lt;&gt;
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        public static ContainerBuilder RegisterAssemblyMediatorPipelines(
            this ContainerBuilder builder,
            params Assembly[] assemblies
        )
        {
            builder
                .RegisterAssemblyTypes(assemblies)
                .AsClosedTypesOf(typeof(IPipelineBehavior<,>))
                .AsImplementedInterfaces();

            return builder;
        }


        public static ContainerBuilder RegisterPipelineForTypes(
            this ContainerBuilder builder,
            Type pipelineType,
            params Type[] requestTypes
        )
        {
            builder.RegisterTypes(
                    requestTypes.MakePipelineTypes(pipelineType)
                        .ToArray()
                )
                .AsImplementedInterfaces();

            return builder;
        }

        public static IEnumerable<Type> MakePipelineTypes(
            this IEnumerable<Type> requestTypes,
            Type openPipelineType
        )
            => requestTypes
                .SelectRequestTypes()
                .Select(t => t.MakePipelineTypes(openPipelineType));

        //public static IEnumerable<Type> MakePipelineTypes(
        //    this RequestTypeInfo info,
        //    Type openPipelineType
        //)
        //    => info
        //        .Select(rti => rti.MakePipelineTypes(openPipelineType));

        public static Type MakePipelineTypes(
            this RequestTypeInfo info,
            Type openPipelineType
        )
            => openPipelineType
                .MakeGenericType(
                    info.RequestType,
                    info.ResponseType
                );
    }
}
