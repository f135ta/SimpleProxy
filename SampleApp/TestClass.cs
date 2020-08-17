namespace SampleApp
{
    using System;
    using Microsoft.Extensions.Logging;
    using SimpleProxy.Caching;
    using SimpleProxy.Diagnostics;
    using SimpleProxy.Logging;

    /// <summary>
    /// Sample Class used to demonstrate the SimpleProxy Interception
    /// </summary>
    public class TestClass : ITestClass
    {
        /// <summary>
        /// The Logger
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Initialises a new instance of the <see cref="TestClass"/>
        /// </summary>
        /// <param name="loggerFactory">Logger Factory</param>
        public TestClass(ILoggerFactory loggerFactory)
        {
            this.logger = loggerFactory.CreateLogger<TestClass>();
        }

        /// <summary>
        /// Test Method
        /// </summary>
        [Log(LogLevel.Debug)]
        [Diagnostics]
        [Cache]
        public DateTime TestMethod()
        {
            var dateTime = DateTime.Now;

            this.logger.LogInformation($"====> The Real Method is Executed Here! <====");

            return dateTime;
        }

        /// <summary>
        /// Test Method With Expire Policy
        /// </summary>
        [Log(LogLevel.Debug)]
        [Diagnostics]
        [Cache(MillisecondsToExpire = 20000)]
        public DateTime TestMethodWithExpirationPolicy()
        {
            var dateTime = DateTime.Now;

            this.logger.LogInformation($"====> The Real Method With Expiration is Executed Here! <====");

            return dateTime;
        }
    }
}
