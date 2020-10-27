namespace SimpleProxy.Extensions
{
    using System;
    using Castle.DynamicProxy;
    using Configuration;
    using Internal;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// Extension Methods for the <see cref="IServiceCollection"/>
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Enables Proxy Generation for Services registered in <see cref="IServiceCollection"/>
        /// </summary>
        /// <param name="services">Services Collection</param>
        /// <param name="options">Proxy Configuration Options</param>
        /// <returns><see cref="IServiceCollection"/></returns>
        public static IServiceCollection EnableSimpleProxy(this IServiceCollection services, Action<SimpleProxyConfiguration> options)
        {
            // Check Inputs for Null
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            // Store the Proxy Configuration
            services.Configure(options);

            // Proxy Generator needs to be registered as a Singleton for performance reasons
            services.AddSingleton<IProxyGenerator, ProxyGenerator>();

            // Return the IServiceCollection for chaining configuration
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
            var serviceProvider = services.BuildServiceProvider();
            var proxyConfiguration = services.GetProxyConfiguration();
            var proxyGenerator = serviceProvider.GetService<IProxyGenerator>();
            var proxyInstance = ActivatorUtilities.CreateInstance<TService>(serviceProvider);

            // Wrap the service with a Proxy instance and add it with Transient Scope
            services.AddTransient(typeof(TInterface),
                p => new ProxyFactory<TInterface>(serviceProvider, proxyGenerator, proxyConfiguration)
                .CreateProxy(ActivatorUtilities.CreateInstance<TService>(serviceProvider)));

            // Return the IServiceCollection for chaining configuration
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
            var serviceProvider = services.BuildServiceProvider();
            var proxyConfiguration = services.GetProxyConfiguration();
            var proxyGenerator = serviceProvider.GetService<IProxyGenerator>();
            var proxyInstance = ActivatorUtilities.CreateInstance<TService>(serviceProvider);

            // Wrap the service with a Proxy instance and add it with Scoped Scope
            services.AddScoped(typeof(TInterface), p => new ProxyFactory<TInterface>(serviceProvider, proxyGenerator, proxyConfiguration).CreateProxy(proxyInstance));

            // Return the IServiceCollection for chaining configuration
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
            var serviceProvider = services.BuildServiceProvider();
            var proxyConfiguration = services.GetProxyConfiguration();
            var proxyGenerator = serviceProvider.GetService<IProxyGenerator>();
            var proxyInstance = ActivatorUtilities.CreateInstance<TService>(serviceProvider);

            // Wrap the service with a Proxy instance and add it with Singleton Scope
            services.AddSingleton(typeof(TInterface), p => new ProxyFactory<TInterface>(serviceProvider, proxyGenerator, proxyConfiguration).CreateProxy(proxyInstance));

            // Return the IServiceCollection for chaining configuration
            return services;
        }

        /// <summary>
        /// Gets the ProxyConfiguration to pass to the Proxy Factory Method
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        /// <returns>Proxy Configuration</returns>
        private static SimpleProxyConfiguration GetProxyConfiguration(this IServiceCollection services)
        {
            return services.BuildServiceProvider().GetRequiredService<IOptions<SimpleProxyConfiguration>>().Value;
        }
    }
}
