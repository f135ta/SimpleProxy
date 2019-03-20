namespace SimpleProxy.Strategies
{
    using System.Collections.Generic;
    using Interfaces;

    /// <summary>
    /// Sequential Ordering Strategy for Interceptors
    /// </summary>
    public sealed class SequentialOrderStrategy : IOrderingStrategy
    {
        /// <inheritdoc />
        public IEnumerable<InvocationContext> OrderBeforeInterception(IEnumerable<InvocationContext> interceptors)
        {
            return interceptors;
        }

        /// <inheritdoc />
        public IEnumerable<InvocationContext> OrderAfterInterception(IEnumerable<InvocationContext> interceptors)
        {
            return interceptors;
        }
    }
}
