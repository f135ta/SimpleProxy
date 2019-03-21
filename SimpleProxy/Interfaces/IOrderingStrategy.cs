namespace SimpleProxy.Interfaces
{
    using System.Collections.Generic;

    /// <summary>
    /// Interface to define an ordering strategy
    /// </summary>
    public interface IOrderingStrategy
    {
        /// <summary>
        /// Defines the order of interceptors before the method is executed
        /// </summary>
        /// <param name="interceptors">Interceptor Collection</param>
        /// <returns>Ordered Collection of Interceptors</returns>
        IEnumerable<InvocationContext> OrderBeforeInterception(IEnumerable<InvocationContext> interceptors);

        /// <summary>
        /// Defines the order of interceptors after the method is executed
        /// </summary>
        /// <param name="interceptors">Interceptor Collection</param>
        /// <returns>Ordered Collection of Interceptors</returns>
        IEnumerable<InvocationContext> OrderAfterInterception(IEnumerable<InvocationContext> interceptors);
    }
}