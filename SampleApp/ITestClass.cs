namespace SampleApp
{
    using System;
    using System.Threading.Tasks;

    public interface ITestClass
    {
        DateTime TestMethod();
        DateTime TestMethodWithExpirationPolicy();
        Task<DateTime> TestMethodAsync();
    }
}
