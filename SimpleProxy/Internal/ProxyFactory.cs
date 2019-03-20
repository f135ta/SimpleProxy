namespace SimpleProxy.Internal
{
    using System;
    using Castle.DynamicProxy;
    using SimpleProxy.Configuration;

    /// <summary>
    /// Proxy Factory that generates Proxy Classes
    /// </summary>
    /// <typeparam name="T">Type of Object to Proxy</typeparam>
    internal class ProxyFactory<T>
    {
        /// <summary>
        /// Gets or sets the Proxy Configuration
        /// </summary>
        private readonly SimpleProxyConfiguration simpleProxyConfiguration;

        /// <summary>
        /// Gets the CastleCore Proxy Generator
        /// </summary>
        private readonly IProxyGenerator proxyGenerator;

        /// <summary>
        /// Gets the Service Provider
        /// </summary>
        private readonly IServiceProvider serviceProvider;

        /// <summary>
        /// Initialises a new instance of the MasterProxy that wraps an object
        /// </summary>
        /// <param name="serviceProvider">Services Collection</param>
        /// <param name="proxyGenerator">Proxy Generator Instance</param>
        /// <param name="config">Proxy Configuration</param>
        public ProxyFactory(IServiceProvider serviceProvider, IProxyGenerator proxyGenerator, SimpleProxyConfiguration config)
        {
            this.simpleProxyConfiguration = config;
            this.proxyGenerator = proxyGenerator;
            this.serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Creates a Proxy Object of the object passed in
        /// </summary>
        /// <param name="originalObject">The Object to be Proxied</param>
        /// <returns>The Proxied Object</returns>
        public T CreateProxy(T originalObject)
        {
            // Proxy the Original Object
            var masterInterceptor = new CoreInterceptor(this.serviceProvider, this.simpleProxyConfiguration);
            var proxy = this.proxyGenerator.CreateInterfaceProxyWithTarget(typeof(T), originalObject, masterInterceptor);

            // Make sure the Proxy was created correctly
            if (proxy == null)
            {
                throw new ArgumentNullException(nameof(proxy));
            }

            // Return the Proxied Object
            return (T)proxy;
        }
    }
}