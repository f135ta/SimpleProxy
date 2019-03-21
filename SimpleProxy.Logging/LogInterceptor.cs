namespace SimpleProxy.Logging
{
    using Extensions;
    using Interfaces;
    using Microsoft.Extensions.Logging;
    using SimpleProxy;

    /// <summary>
    /// Interceptor for Logging
    /// </summary>
    public class LogInterceptor : IMethodInterceptor
    {
        /// <summary>
        /// The Logger
        /// </summary>
        private readonly ILoggerFactory loggerFactory;

        /// <summary>
        /// The Logging Attribute
        /// </summary>
        private LogAttribute loggingAttribute;

        /// <summary>
        /// The Logger
        /// </summary>
        private ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogInterceptor"/> class. 
        /// </summary>
        /// <param name="loggerFactory">Logger Factory</param>
        public LogInterceptor(ILoggerFactory loggerFactory)
        {
            this.loggerFactory = loggerFactory;
        }

        /// <inheritdoc />
        public void BeforeInvoke(InvocationContext invocationContext)
        {
            // Create a logger based on the class type that owns the executing method
            this.logger = this.loggerFactory.CreateLogger(invocationContext.GetOwningType());

            // Get the Logging Attribute
            this.loggingAttribute = invocationContext.GetAttributeFromMethod<LogAttribute>();

            // Get the Logging Level
            var loggingLevel = this.loggingAttribute.LoggingLevel;

            // Log the method being executed
            this.logger.Log(loggingLevel, $"{invocationContext.GetOwningType()}: Method executing: {invocationContext.GetExecutingMethodName()}");
        }

        /// <inheritdoc />
        public void AfterInvoke(InvocationContext invocationContext, object methodResult)
        {
            this.logger.Log(this.loggingAttribute.LoggingLevel, $"{invocationContext.GetOwningType()}: Method executed: {invocationContext.GetExecutingMethodName()}");
        }
    }
}
