namespace SimpleProxy.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Attributes;
    using Interfaces;
    using Internal.Configuration;
    using Internal.Interfaces;
    using Strategies;

    /// <summary>
    /// Configuration Class for Proxy Generation
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class SimpleProxyConfiguration
    {
        /// <summary>
        /// Gets or sets a collection of all configured interceptors
        /// </summary>
        internal List<IInterceptorMapping> ConfiguredInterceptors { get; set; } = new List<IInterceptorMapping>();

        /// <summary>
        /// Gets or sets the Ordering Strategy for Interceptors
        /// </summary>
        internal IOrderingStrategy OrderingStrategy { get; set; } = new PyramidOrderStrategy();

        /// <summary>
        /// Gets or sets a value which determines whether invalid interceptors are ignored (rather than throw exceptions)
        /// </summary>
        internal bool IgnoreInvalidInterceptors = true;

        /// <inheritdoc />
        public SimpleProxyConfiguration AddInterceptor<TAttribute, TInterceptor>() where TAttribute : MethodInterceptionAttribute where TInterceptor : IMethodInterceptor
        {
            // Adds an Interceptor Mapping for matching up attributes to interceptors
            this.ConfiguredInterceptors.Add(new InterceptorMapping<TAttribute, TInterceptor>());

            // Return the SimpleProxyConfiguration for chaining configuration
            return this;
        }

        /// <inheritdoc />
        public SimpleProxyConfiguration IgnoreInvalidInterceptorConfigurations()
        {
            // Invalid Interceptor Configurations are ignored and wont throw exceptions
            this.IgnoreInvalidInterceptors = true;

            // Return the SimpleProxyConfiguration for chaining configuration
            return this;
        }

        /// <inheritdoc />
        public SimpleProxyConfiguration WithOrderingStrategy<TStrategy>() where TStrategy : IOrderingStrategy
        {
            // Creates a new instance of the Ordering Strategy and assigns it the configuration
            this.OrderingStrategy = Activator.CreateInstance<TStrategy>();

            // Return the SimpleProxyConfiguration for chaining configuration
            return this;
        }
    }
}