namespace SimpleProxy.Diagnostics
{
    using System;
    using System.Diagnostics;
    using Interfaces;
    using Microsoft.Extensions.Logging;
    using SimpleProxy;

    /// <summary>
    /// Interceptor for Diagnostics Logging
    /// </summary>
    public class DiagnosticsInterceptor : IMethodInterceptor
    {
        private readonly Stopwatch diagnosticStopwatch;

        /// <summary>
        /// The Logger
        /// </summary>
        private ILogger logger;

        /// <summary>
        /// Initialises a new instance of the <see cref="DiagnosticsInterceptor"/>
        /// </summary>
        public DiagnosticsInterceptor(ILoggerFactory loggerFactory)
        {
            this.diagnosticStopwatch = new Stopwatch();
            this.logger = loggerFactory.CreateLogger<DiagnosticsInterceptor>();
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
            this.logger.LogInformation($"Method executed in: {this.diagnosticStopwatch.ElapsedMilliseconds}ms");
        }
    }
}
