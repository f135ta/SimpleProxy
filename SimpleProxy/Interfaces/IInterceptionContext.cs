namespace SimpleProxy.Interfaces
{
    using System.Collections.Generic;
    using System.Reflection;
    using Attributes;
    using Core;

    /// <summary>
    /// The Interception Context class stores information about the Proxy Object and its current state
    /// </summary>
    public interface IInterceptionContext
    {
        /// <summary>
        /// Gets the Configured Interceptors for the Current Executing Method
        /// </summary>
        Dictionary<int, MethodInterceptor> InterceptorsForCurrentMethod { get; set; }

        /// <summary>
        /// Gets the Input Parameters supplied to the Executing Method
        /// </summary>
        ParameterInfo[] MethodInputParameters { get; set; }

        /// <summary>
        /// Gets the Current Executing Method
        /// </summary>
        MethodInfo CurrentMethod { get; set; }

        /// <summary>
        /// Gets or sets the Interceptor Attributes for the Current Method
        /// </summary>
        IEnumerable<MethodInterceptorAttribute> MethodInterceptorAttributes { get; set; }
    }
}
