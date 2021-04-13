

namespace SimpleProxy.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Castle.DynamicProxy;
    using Extensions;
    using SimpleProxy.Configuration;
    using SimpleProxy.Interfaces;

    /// <summary>
    /// The Master Interceptor Class wraps a proxied object and handles all of its interceptions
    /// </summary>
    internal class CoreInterceptor : IAsyncInterceptor
    {
        /// <summary>
        /// Gets the <see cref="proxyConfiguration"/>
        /// </summary>
        private readonly SimpleProxyConfiguration proxyConfiguration;

        /// <summary>
        /// Gets the <see cref="IServiceProvider"/>
        /// </summary>
        private readonly IServiceProvider serviceProvider;

        /// <summary>
        /// Initialises a new instance of the <see cref="CoreInterceptor"/> class
        /// </summary>
        /// <param name="serviceProvider">Service Provider</param>
        /// <param name="proxyConfiguration">Proxy Configuration</param>
        public CoreInterceptor(IServiceProvider serviceProvider, SimpleProxyConfiguration proxyConfiguration)
        {
            this.serviceProvider = serviceProvider;
            this.proxyConfiguration = proxyConfiguration;
        }

        public void InterceptSynchronous(IInvocation invocation)
        {
            var proceedWithInterception = InterceptBeforeProceed(invocation).Result;

            if (!proceedWithInterception.DontProceed) {
                invocation.Proceed();
                return;
            }

            // Execute the Real Method
            if (!proceedWithInterception.InvocationContexts.Any(p => p.InvocationIsBypassed)) {
                invocation.Proceed();
            }

            InterceptAfterProceed(proceedWithInterception.InvocationContexts, proceedWithInterception.OrderingStrategy);
        }

        public void InterceptAsynchronous(IInvocation invocation)
        {
            invocation.ReturnValue = InternalInterceptAsynchronousAsync(invocation);
        }

        private async Task InternalInterceptAsynchronousAsync(IInvocation invocation)
        {
            var proceedWithInterception = await InterceptBeforeProceed(invocation);

            if (!proceedWithInterception.DontProceed) {
                invocation.Proceed();
                var task = (Task)invocation.ReturnValue;
                await task;
                return;
            }

            // Execute the Real Method
            if (!proceedWithInterception.InvocationContexts.Any(p => p.InvocationIsBypassed)) {
                invocation.Proceed();
                var task = (Task)invocation.ReturnValue;
                await task;
            }

            InterceptAfterProceed(proceedWithInterception.InvocationContexts, proceedWithInterception.OrderingStrategy);
        }

        public void InterceptAsynchronous<TResult>(IInvocation invocation)
        {
            invocation.ReturnValue = InternalInterceptAsynchronousAsync<TResult>(invocation);
        }

        private async Task<TResult> InternalInterceptAsynchronousAsync<TResult>(IInvocation invocation)
        {
            var proceedWithInterception = await InterceptBeforeProceed(invocation);

            TResult result;

            if (!proceedWithInterception.DontProceed) {
                invocation.Proceed();
                var task = (Task<TResult>)invocation.ReturnValue;
                result = await task;
                return result;
            }

            // Execute the Real Method
            if (!proceedWithInterception.InvocationContexts.Any(p => p.InvocationIsBypassed)) {
                invocation.Proceed();
                var task = (Task<TResult>)invocation.ReturnValue;
                result = await task;
            }
            else {
                result = default;
            }

            InterceptAfterProceed(proceedWithInterception.InvocationContexts, proceedWithInterception.OrderingStrategy);

            return result;
        }

        private async Task<InterceptorData> InterceptBeforeProceed(IInvocation invocation)
        {
            InterceptorData data = new InterceptorData();

            // Map the configured interceptors to this type based on its attributes
            var invocationMetadataCollection = InvocationExtensions.GetInterceptorMetadataForMethod(invocation, this.serviceProvider, this.proxyConfiguration);
            

            // If there are no configured interceptors, leave now
            if (invocationMetadataCollection == null || !invocationMetadataCollection.Any()) {
                data.OrderingStrategy = null;
                data.DontProceed = false;
            }

            // Get the Ordering Strategy for Interceptors
            var orderingStrategy = this.proxyConfiguration.OrderingStrategy;
            data.OrderingStrategy = orderingStrategy;

            data.InvocationContexts = new List<InvocationContext>();

            // Process the BEFORE Interceptions
            foreach (var invocationContext in orderingStrategy.OrderBeforeInterception(invocationMetadataCollection))
            {
                
                var dontProceed = await invocationContext.Interceptor.BeforeInvoke(invocationContext);
                if (dontProceed)
                {
                    data.DontProceed = true;
                    invocationContext.BypassInvocation();
                    data.InvocationContexts.Add(invocationContext);
                    return data;
                }

                // in case it is valid one, add it to our list
                data.InvocationContexts.Add(invocationContext);
            }

            
            data.DontProceed = true;
            return data;
        }

        private static void InterceptAfterProceed(List<InvocationContext> invocationMetadataCollection, IOrderingStrategy orderingStrategy)
        {
            // Process the AFTER Interceptions
            foreach (var invocationContext in orderingStrategy.OrderAfterInterception(invocationMetadataCollection)) {
                invocationContext.Interceptor.AfterInvoke(invocationContext, invocationContext.GetMethodReturnValue());
            }
        }

    }

    internal class InterceptorData
    {
        public List<InvocationContext> InvocationContexts { get; set; }
        public IOrderingStrategy OrderingStrategy { get; set; }

        public bool DontProceed { get; set; }
    }

}
