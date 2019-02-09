namespace SimpleProxy.Configuration
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Configuration class for Proxy Generation
    /// </summary>
    public class ProxyConfiguration
    {
        internal Dictionary<Type, Type> ConfiguredInterceptors { get; set; }
        internal bool UsingPyramidOrdering = true;
        internal bool IgnoreInvalidInterceptors = true;

        /// <summary>
        /// Adds an Interceptor to the Dependency Injection Container and assigns it to an attribute
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <typeparam name="TInterceptor"></typeparam>
        /// <returns></returns>
        public ProxyConfiguration AddInterceptor<TAttribute, TInterceptor>()
        {
            if (this.ConfiguredInterceptors == null)
            {
                this.ConfiguredInterceptors = new Dictionary<Type, Type>();
            }

            this.ConfiguredInterceptors.Add(typeof(TAttribute), typeof(TInterceptor));
            return this;
        }

        /// <summary>
        /// Prevents Exceptions being thrown when Interceptors are not configured correctly
        /// </summary>
        /// <returns><see cref="ProxyConfiguration"/></returns>
        public ProxyConfiguration IgnoreInvalidInterceptorConfigurations()
        {
            this.IgnoreInvalidInterceptors = true;
            return this;
        }

        /// <summary>
        /// Enables Pyramid Ordering for Method Interceptors
        /// </summary>
        /// <returns><see cref="ProxyConfiguration"/></returns>
        public ProxyConfiguration EnablePyramidOrdering()
        {
            this.UsingPyramidOrdering = true;
            return this;
        }
    }
}