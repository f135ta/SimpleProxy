namespace SimpleProxy.Extensions
{
    using System;
    using Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// Extension Methods for the <see cref="IServiceCollection"/>
    /// </summary>
    public static class ServicesCollectionExtensions
    {
        /// <summary>
        /// Enables Proxy Generation for Services registered in <see cref="IServiceCollection"/>
        /// </summary>
        /// <param name="services">Services Collection</param>
        /// <param name="options">Proxy Configuration Options</param>
        /// <returns><see cref="IServiceCollection"/></returns>
        public static IServiceCollection EnableSimpleProxy(this IServiceCollection services, Action<ProxyConfiguration> options)
        {
            services.AddOptions();
            services.Configure(options);
            return services;
        }

        /// <summary>
        /// Adds a Transient Service to the <see cref="ServiceCollection"/> that is wrapped in a Proxy
        /// </summary>
        /// <typeparam name="TInterface">Interface Type</typeparam>
        /// <typeparam name="TService">Implementation Type</typeparam>
        /// <param name="services">Services Collection</param>
        /// <returns><see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddTransientWithProxy<TInterface, TService>(this IServiceCollection services) where TService : TInterface
        {
            var proxyConfiguration = GetProxyConfiguration(services);
            var proxyInstance = ActivatorUtilities.CreateInstance<TService>(services.BuildServiceProvider());

            services.AddTransient(typeof(TInterface), p => new Proxy<TInterface>().CreateProxy(proxyInstance, proxyConfiguration));
            return services;
        }

        /// <summary>
        /// Adds a Scoped Service to the <see cref="ServiceCollection"/> that is wrapped in a Proxy
        /// </summary>
        /// <typeparam name="TInterface">Interface Type</typeparam>
        /// <typeparam name="TService">Implementation Type</typeparam>
        /// <param name="services">Services Collection</param>
        /// <returns><see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddScopedWithProxy<TInterface, TService>(this IServiceCollection services) where TService : TInterface
        {
            var proxyConfiguration = GetProxyConfiguration(services);
            var proxyInstance = ActivatorUtilities.CreateInstance<TService>(services.BuildServiceProvider());

            services.AddScoped(typeof(TInterface), p => new Proxy<TInterface>().CreateProxy(proxyInstance, proxyConfiguration));
            return services;
        }

        /// <summary>
        /// Adds a Singleton Service to the <see cref="ServiceCollection"/> that is wrapped in a Proxy
        /// </summary>
        /// <typeparam name="TInterface">Interface Type</typeparam>
        /// <typeparam name="TService">Implementation Type</typeparam>
        /// <param name="services">Services Collection</param>
        /// <returns><see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddSingletonWithProxy<TInterface, TService>(this IServiceCollection services) where TService : TInterface
        {
            var proxyConfiguration = GetProxyConfiguration(services);
            var proxyInstance = ActivatorUtilities.CreateInstance<TService>(services.BuildServiceProvider());

            services.AddSingleton(typeof(TInterface), p => new Proxy<TInterface>().CreateProxy(proxyInstance, proxyConfiguration));
            return services;
        }

        /// <summary>
        /// Gets the ProxyConfiguration to pass to the Proxy Factory Method
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        /// <returns>Proxy Configuration</returns>
        private static ProxyConfiguration GetProxyConfiguration(IServiceCollection services)
        {
            var config = services.BuildServiceProvider().GetRequiredService<IOptions<ProxyConfiguration>>();
            return config.Value;
        }
    }
}
