namespace SimpleProxy.Internal
{
    using System;
    using System.Linq;
    using Castle.DynamicProxy;
    using Extensions;
    using SimpleProxy.Configuration;

    /// <summary>
    /// The Master Interceptor Class wraps a proxied object and handles all of its interceptions
    /// </summary>
    internal class CoreInterceptor : IInterceptor
    {
        /// <summary>
        /// Gets the <see cref="SimpleProxyConfiguration"/>
        /// </summary>
        private readonly SimpleProxyConfiguration simpleProxyConfiguration;

        /// <summary>
        /// Gets the <see cref="IServiceProvider"/>
        /// </summary>
        private readonly IServiceProvider serviceProvider;

        /// <summary>
        /// Initialises a new instance of the <see cref="CoreInterceptor"/> class
        /// </summary>
        /// <param name="serviceProvider">Service Provider</param>
        /// <param name="simpleProxyConfiguration">Proxy Configuration</param>
        public CoreInterceptor(IServiceProvider serviceProvider, SimpleProxyConfiguration simpleProxyConfiguration)
        {
            this.serviceProvider = serviceProvider;
            this.simpleProxyConfiguration = simpleProxyConfiguration;
        }

        /// <inheritdoc />
        public void Intercept(IInvocation invocation)
        {
            // Map the configured interceptors to this type based on its attributes
            var invocationMetadataCollection = InvocationExtensions.GetInterceptorMetadataForMethod(invocation, this.serviceProvider, this.simpleProxyConfiguration);

            // If there are no configured interceptors, leave now
            if (invocationMetadataCollection == null || !invocationMetadataCollection.Any())
            {
                invocation.Proceed();
                return;
            }

            // Get the Ordering Strategy for Interceptors
            var orderingStrategy = this.simpleProxyConfiguration.OrderingStrategy;

            // Process the BEFORE Interceptions
            foreach (var invocationContext in orderingStrategy.OrderBeforeInterception(invocationMetadataCollection))
            {
                invocationContext.Interceptor.BeforeInvoke(invocationContext);
            }

            // Execute the Real Method
            if (!invocationMetadataCollection.Any(p => p.InvocationIsBypassed))
            {
                invocation.Proceed();
            }

            // Process the AFTER Interceptions
            foreach (var invocationContext in orderingStrategy.OrderAfterInterception(invocationMetadataCollection))
            {
                invocationContext.Interceptor.AfterInvoke(invocationContext, invocationContext.GetMethodReturnValue());
            }
        }
    }
}