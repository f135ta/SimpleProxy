using System.Threading.Tasks;


namespace SampleApp
{
    using System;

    public interface ITestClass
    {
        DateTime TestMethod();

        Task<DateTime> TestMethodAsync();

    }
}
