namespace SimpleProxy.Diagnostics
{
    using System;
    using System.Diagnostics;
    using Interfaces;
    using SimpleProxy;

    /// <summary>
    /// Interceptor for Diagnostics Logging
    /// </summary>
    public class DiagnosticsInterceptor : IMethodInterceptor
    {
        private readonly Stopwatch diagnosticStopwatch;

        /// <summary>
        /// Initialises a new instance of the <see cref="DiagnosticsInterceptor"/>
        /// </summary>
        public DiagnosticsInterceptor()
        {
            this.diagnosticStopwatch = new Stopwatch();
        }

        /// <inheritdoc />
        public void BeforeInvoke(InvocationContext invocationContext)
        {
            this.diagnosticStopwatch.Start();
        }

        /// <inheritdoc />
        public void AfterInvoke(InvocationContext invocationContext, object methodResult)
        {
            this.diagnosticStopwatch.Stop();
            Console.WriteLine($"Diagnostics Log: Method executed in: {this.diagnosticStopwatch.ElapsedMilliseconds}ms");
        }
    }
}
