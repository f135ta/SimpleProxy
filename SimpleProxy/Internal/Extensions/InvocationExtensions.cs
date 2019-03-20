namespace SimpleProxy.Internal.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Attributes;
    using Castle.DynamicProxy;
    using Exceptions;
    using Microsoft.Extensions.DependencyInjection;
    using SimpleProxy.Configuration;
    using SimpleProxy.Interfaces;

    /// <summary>
    /// Extension methods for <see cref="IInvocation"/>
    /// </summary>
    internal static class InvocationExtensions
    {
        /// <summary>
        /// Gets a collection of all the interceptors associated with the current executing method
        /// </summary>
        /// <param name="invocation">Current Invocation</param>
        /// <param name="serviceProvider">Service Provider Instance</param>
        /// <param name="simpleProxyConfiguration">Proxy Configuration</param>
        /// <returns><see cref="Dictionary{TKey,TValue}"/> of all configured interceptors for this method</returns>
        internal static List<InvocationContext> GetInterceptorMetadataForMethod(IInvocation invocation, IServiceProvider serviceProvider, SimpleProxyConfiguration simpleProxyConfiguration)
        {
            // Create the Interceptor List to store the configured interceptors
            var interceptorList = new List<InvocationContext>();

            // Get the Attributes applied to the method being invoked
            var methodAttributes = invocation
                .MethodInvocationTarget
                .GetCustomAttributes()
                .Where(p => p.GetType().IsSubclassOf(typeof(MethodInterceptionAttribute)))
                .Cast<MethodInterceptionAttribute>();

            var index = 0;
            foreach (var methodAttribute in methodAttributes)
            {
                // Get the Interceptor that is bound to the attribute
                var interceptorType = simpleProxyConfiguration.ConfiguredInterceptors.FirstOrDefault(p => p.AttributeType == methodAttribute.GetType())?.InterceptorType;
                if (interceptorType == null)
                {
                    if (simpleProxyConfiguration.IgnoreInvalidInterceptors)
                    {
                        continue;
                    }

                    throw new InvalidInterceptorException($"The Interceptor Attribute '{methodAttribute}' is applied to the method, but there is no configured interceptor to handle it");
                }

                // Use the Service Provider to create the Interceptor instance so you can inject dependencies into the constructor
                var instance = (IMethodInterceptor)ActivatorUtilities.CreateInstance(serviceProvider, interceptorType);

                // New InvocationContext Instance
                var context = new InvocationContext
                {
                    Attribute = methodAttribute, 
                    Interceptor = instance,
                    Invocation = invocation,
                    Order = index,
                    ServiceProvider = serviceProvider
                };

                interceptorList.Add(context);
                index += 1;
            }

            // Return the list of configured interceptors
            return interceptorList;
        }
    }
}
