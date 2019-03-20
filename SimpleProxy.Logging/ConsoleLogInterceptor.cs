namespace SimpleProxy.Logging
{
    using System;
    using Extensions;
    using Interfaces;
    using SimpleProxy;

    /// <summary>
    /// Interceptor for Console Logging
    /// </summary>
    public class ConsoleLogInterceptor : IMethodInterceptor
    {
        /// <inheritdoc />
        public void BeforeInvoke(InvocationContext invocationContext)
        {
            Console.WriteLine($"ConsoleLog: Method executing: {invocationContext.GetExecutingMethodName()}");
        }

        /// <inheritdoc />
        public void AfterInvoke(InvocationContext invocationContext, object methodResult)
        {
            Console.WriteLine($"ConsoleLog: Method executed: {invocationContext.GetExecutingMethodName()}");
        }
    }
}
