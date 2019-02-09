namespace SimpleProxy.Core
{
    using System.Collections.Generic;
    using System.Reflection;
    using Attributes;
    using Interfaces;

    /// <inheritdoc />
    public class InterceptionContext : IInterceptionContext
    {
        /// <inheritdoc />
        public Dictionary<int, MethodInterceptor> InterceptorsForCurrentMethod { get; set; }

        /// <inheritdoc />
        public ParameterInfo[] MethodInputParameters { get; set; }

        /// <inheritdoc />
        public MethodInfo CurrentMethod { get; set; }

        /// <inheritdoc />
        public IEnumerable<MethodInterceptorAttribute> MethodInterceptorAttributes { get; set; }
    }
}
