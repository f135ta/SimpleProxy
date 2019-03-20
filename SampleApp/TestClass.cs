namespace SampleApp
{
    using System;
    using SimpleProxy.Caching;
    using SimpleProxy.Diagnostics;
    using SimpleProxy.Logging;

    /// <summary>
    /// Sample Class used to demonstrate the SimpleProxy Interception
    /// </summary>
    public class TestClass : ITestClass
    {
        /// <summary>
        /// Test Method
        /// </summary>
        [ConsoleLog]
        [Diagnostics]
        [Cache]
        public DateTime TestMethod()
        {
            var dateTime = DateTime.Now;
            Console.WriteLine($"Inside Method! {dateTime}");
            return dateTime;
        }
    }

    public interface ITestClass
    {
        DateTime TestMethod();
    }
}
