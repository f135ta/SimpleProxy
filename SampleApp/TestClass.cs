namespace SampleApp
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using SimpleProxy.Caching;
    using SimpleProxy.Diagnostics;
    using SimpleProxy.Logging;

    /// <summary>
    /// Sample Class used to demonstrate the SimpleProxy Interception
    /// </summary>
    public class TestClass : ITestClass
    {
        private static int count = 0;

        /// <summary>
        /// The Logger
        /// </summary>
        private readonly ILogger logger;

        private readonly ICommon common;

        /// <summary>
        /// Initialises a new instance of the <see cref="TestClass"/>
        /// </summary>
        /// <param name="loggerFactory">Logger Factory</param>
        public TestClass(ILoggerFactory loggerFactory, ICommon common)
        {
            this.logger = loggerFactory.CreateLogger<TestClass>();
            this.common = common;
            this.Instance = $"Test Class Instnce: {++count}. -- Common:  {common.Instance} ";
        }

        public string Instance { get; set; }

        /// <summary>
        /// Test Method
        /// </summary>
        [Log(LogLevel.Debug)]
        [Diagnostics]
        [Cache]
        public DateTime TestMethod()
        {
            var dateTime = DateTime.Now;

            this.logger.LogInformation("====> The Real Method is Executed Here! <====");

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

        [Log(LogLevel.Debug)]
        [Diagnostics]
        [Cache]
        public Task<DateTime> TestMethodAsync()
        {
            var dateTime = DateTime.Now;

            this.logger.LogInformation("====> The Real Async Method is Executed Here! <====");

            return Task.FromResult(dateTime);
        }
    }

    public class Common : ICommon
    {
        private static int count = 0;
        public IDBMock mock;
        public Common(IDBMock dBMock)
        {
            this.mock = dBMock;

            Instance = $"Common Instance Id {++count} -- DBMOck Id:  {mock.Instance }";
        }
        public string Instance { get; set; }
    }

    public class DBMOck : IDBMock
    {
        private static int count = 0;

        public DBMOck()
        {
            Instance = $"DB MOCK ID : {++count}";

        }

        public string Instance { get; set; }
    }

}
