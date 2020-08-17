

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
            var proceedWithInterception = InterceptBeforeProceed(invocation, out var invocationMetadataCollection, out var orderingStrategy);

            if (!proceedWithInterception) {
                invocation.Proceed();
                return;
            }

            // Execute the Real Method
            if (!invocationMetadataCollection.Any(p => p.InvocationIsBypassed)) {
                invocation.Proceed();
            }

            InterceptAfterProceed(invocationMetadataCollection, orderingStrategy);
        }

        public void InterceptAsynchronous(IInvocation invocation)
        {
            invocation.ReturnValue = InternalInterceptAsynchronousAsync(invocation);
        }

        private async Task InternalInterceptAsynchronousAsync(IInvocation invocation)
        {
            var proceedWithInterception = InterceptBeforeProceed(invocation, out var invocationMetadataCollection, out var orderingStrategy);

            if (!proceedWithInterception) {
                invocation.Proceed();
                var task = (Task)invocation.ReturnValue;
                await task;
                return;
            }

            // Execute the Real Method
            if (!invocationMetadataCollection.Any(p => p.InvocationIsBypassed)) {
                invocation.Proceed();
                var task = (Task)invocation.ReturnValue;
                await task;
            }

            InterceptAfterProceed(invocationMetadataCollection, orderingStrategy);
        }

        public void InterceptAsynchronous<TResult>(IInvocation invocation)
        {
            invocation.ReturnValue = InternalInterceptAsynchronousAsync<TResult>(invocation);
        }

        private async Task<TResult> InternalInterceptAsynchronousAsync<TResult>(IInvocation invocation)
        {
            var proceedWithInterception = InterceptBeforeProceed(invocation, out var invocationMetadataCollection, out var orderingStrategy);

            TResult result;

            if (!proceedWithInterception) {
                invocation.Proceed();
                var task = (Task<TResult>)invocation.ReturnValue;
                result = await task;
                return result;
            }

            // Execute the Real Method
            if (!invocationMetadataCollection.Any(p => p.InvocationIsBypassed)) {
                invocation.Proceed();
                var task = (Task<TResult>)invocation.ReturnValue;
                result = await task;
            }
            else {
                result = default;
            }

            InterceptAfterProceed(invocationMetadataCollection, orderingStrategy);

            return result;
        }

        private bool InterceptBeforeProceed(IInvocation invocation, out List<InvocationContext> invocationMetadataCollection, out IOrderingStrategy orderingStrategy)
        {
            // Map the configured interceptors to this type based on its attributes
            invocationMetadataCollection = InvocationExtensions.GetInterceptorMetadataForMethod(invocation, this.serviceProvider, this.proxyConfiguration);

            // If there are no configured interceptors, leave now
            if (invocationMetadataCollection == null || !invocationMetadataCollection.Any()) {
                orderingStrategy = null;
                return false;
            }

            // Get the Ordering Strategy for Interceptors
            orderingStrategy = this.proxyConfiguration.OrderingStrategy;

            // Process the BEFORE Interceptions
            foreach (var invocationContext in orderingStrategy.OrderBeforeInterception(invocationMetadataCollection)) {
                invocationContext.Interceptor.BeforeInvoke(invocationContext);
            }

            return true;
        }

        private static void InterceptAfterProceed(List<InvocationContext> invocationMetadataCollection, IOrderingStrategy orderingStrategy)
        {
            // Process the AFTER Interceptions
            foreach (var invocationContext in orderingStrategy.OrderAfterInterception(invocationMetadataCollection)) {
                invocationContext.Interceptor.AfterInvoke(invocationContext, invocationContext.GetMethodReturnValue());
            }
        }

    }

}
