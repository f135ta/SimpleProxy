namespace SimpleProxy.Logging
{
    using Attributes;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Method Interceptor Example Attribute
    /// </summary>
    public class LogAttribute : MethodInterceptionAttribute
    {
        /// <summary>
        /// The Logging Level
        /// </summary>
        internal LogLevel LoggingLevel { get; set; }

        /// <summary>
        /// Initialises a new instance of the <see cref="LogAttribute"/>
        /// </summary>
        /// <param name="loggingLevel">Logging Level</param>
        public LogAttribute(LogLevel loggingLevel)
        {
            this.LoggingLevel = loggingLevel;
        }
    }
}
