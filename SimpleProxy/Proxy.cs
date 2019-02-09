namespace SimpleProxy
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;
    using Attributes;
    using Configuration;
    using Core;
    using Exceptions;
    using Interfaces;

    /// <summary>
    /// Proxy class which wraps all proxied classes
    /// </summary>
    /// <typeparam name="T">Type of Object to Proxy</typeparam>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class Proxy<T> : DispatchProxy
    {
        /// <summary>
        /// Gets the Interception Context
        /// </summary>
        // ReSharper disable once StyleCop.SA1306
        private IInterceptionContext InterceptionContext;

        /// <summary>
        /// Gets the Original Object
        /// </summary>
        // ReSharper disable once StyleCop.SA1306
        private T OriginalObject;

        /// <summary>
        /// Gets or sets the Proxy Configuration
        /// </summary>
        // ReSharper disable once StyleCop.SA1306
        private ProxyConfiguration ProxyConfiguration;

        /// <summary>
        /// Creates a Proxy Object of the object passed in
        /// </summary>
        /// <param name="originalObject">The Object to be Proxied</param>
        /// <param name="config">Proxy Configuration</param>
        /// <returns>The Proxied Object</returns>
        public T CreateProxy(T originalObject, ProxyConfiguration config)
        {
            // Proxy the Original Object
            object proxy = Create<T, Proxy<T>>();

            // ReSharper disable once JoinNullCheckWithUsage
            if (proxy == null)
            {
                throw new ArgumentNullException(nameof(proxy));
            }

            // Set the configuration for the Proxy Object
            ((Proxy<T>)proxy).ProxyConfiguration = config;
            ((Proxy<T>)proxy).OriginalObject = originalObject;
            ((Proxy<T>)proxy).InterceptionContext = new InterceptionContext();

            // Return the Proxied Object
            return (T)proxy;
        }

        /// <summary>
        /// The Main [Proxy] Invoke Method
        /// </summary>
        /// <param name="targetMethod">Target Method</param>
        /// <param name="methodParameters">Method Parameters</param>
        /// <returns>OriginalObject Value</returns>
        protected override object Invoke(MethodInfo targetMethod, object[] methodParameters)
        {
            // Takes the Target Method and extracts information into the Proxy object for debugging 
            var context = this.InitialiseContext(targetMethod);
            this.InterceptionContext = context;

            // Process the Interceptors
            try
            {
                // Process the [Before] for all interceptors
                this.ProcessBeforeInterceptors(context);

                // Execute the Original Method
                var result = targetMethod.Invoke(this.OriginalObject, methodParameters);

                // Capture the Input Parameters and assign them to the Proxy so they can be read during debug
                this.InterceptionContext.MethodInputParameters = targetMethod.GetParameters();

                // Process the [After] for all interceptors
                this.ProcessAfterInterceptors(context, result);

                // Return Result
                return result;
            }
            catch (Exception ex) when (ex is TargetInvocationException)
            {
                throw ex.InnerException ?? ex;
            }
        }

        /// <summary>
        /// Initialises the Interception Context for the Proxy
        /// </summary>
        /// <param name="targetMethod">Executing Method</param>
        /// <returns><see cref="IInterceptionContext"/></returns>
        private IInterceptionContext InitialiseContext(MethodInfo targetMethod)
        {
            return new InterceptionContext
            {
                CurrentMethod = targetMethod,
                InterceptorsForCurrentMethod = this.GetInterceptorsForMethod(targetMethod),
                MethodInterceptorAttributes = this.GetMethodInterceptorAttributes(targetMethod)
            };
        }

        /// <summary>
        /// Executes all the configured interceptors [BeforeInvoke] method
        /// </summary>
        private void ProcessBeforeInterceptors(IInterceptionContext context)
        {
            var interceptors = this.InterceptionContext.InterceptorsForCurrentMethod.Values;
            foreach (var methodInterceptor in interceptors)
            {
                methodInterceptor.BeforeInvoke(context);
            }
        }

        /// <summary>
        /// Executes all the configured interceptors [AfterInvoke] method
        /// </summary>
        /// <param name="context">Interception Context</param>
        /// <param name="methodResult">Method Result Value</param>
        private void ProcessAfterInterceptors(IInterceptionContext context, object methodResult)
        {
            // Flip the interceptor order if Pyramid Ordering is enabled and
            // Process the [After] for all interceptors
            if (this.ProxyConfiguration.UsingPyramidOrdering)
            {
                foreach (var methodInterceptor in this.InterceptionContext.InterceptorsForCurrentMethod.OrderByDescending(p => p.Key))
                {
                    methodInterceptor.Value.AfterInvoke(context, methodResult);
                }
            }
            else
            {
                foreach (var methodInterceptor in this.InterceptionContext.InterceptorsForCurrentMethod)
                {
                    methodInterceptor.Value.AfterInvoke(context, methodResult);
                }
            }
        }

        /// <summary>
        /// Gets a collection of all the interceptors associated with the current executing method
        /// </summary>
        /// <param name="targetMethod">The Executing Method</param>
        /// <returns><see cref="Dictionary{TKey,TValue}"/> of all configured interceptors for this method</returns>
        private Dictionary<int, MethodInterceptor> GetInterceptorsForMethod(MethodInfo targetMethod)
        {
            var interceptorList = new Dictionary<int, MethodInterceptor>();
            var methodAttributes = this.GetMethodInterceptorAttributes(targetMethod);

            var index = 0;
            foreach (var interceptor in methodAttributes)
            {
                var interceptorType = this.ProxyConfiguration.ConfiguredInterceptors.FirstOrDefault(p => p.Key == interceptor.GetType()).Value;
                if (interceptorType == null)
                {
                    if (this.ProxyConfiguration.IgnoreInvalidInterceptors)
                    {
                        continue;
                    }

                    throw new InvalidInterceptorException($"The Interceptor Attribute '{interceptor}' is applied to the method, but there is no configured interceptor to handle it");
                }

                var instance = (MethodInterceptor)Activator.CreateInstance(interceptorType);
                interceptorList.Add(index, instance);
                index += 1;
            }

            return interceptorList;
        }

        /// <summary>
        /// Gets the <see cref="MethodInterceptorAttribute"/>'s applied to the method
        /// </summary>
        /// <param name="targetMethod">Target Method</param>
        /// <returns><see cref="IEnumerable{T}"/></returns>
        private IEnumerable<MethodInterceptorAttribute> GetMethodInterceptorAttributes(MemberInfo targetMethod)
        {
            var attributes = this.OriginalObject
                .GetType()
                .GetMethod(targetMethod.Name)
                .GetCustomAttributes()
                .Where(p => p.GetType().IsSubclassOf(typeof(MethodInterceptorAttribute)));

            return attributes.Cast<MethodInterceptorAttribute>().ToList();
        }
    }
}