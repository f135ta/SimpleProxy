namespace SampleApp
{
    using Attributes;
    using Interfaces;

    /// <summary>
    /// Sample Class used to demonstrate the SimpleProxy Interception
    /// </summary>
    public class TestClass : ITestClass
    {
        /// <summary>
        /// Test Method with the <see cref="ConsoleLogAttribute"/> applied
        /// </summary>
        [ConsoleLog]
        public void TestMethod()
        {

        }
    }
}
