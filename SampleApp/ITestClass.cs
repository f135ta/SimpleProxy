namespace SampleApp
{
    using System;
    using System.Threading.Tasks;

    public interface ITestClass
    {
        string Instance { get; set; }
        DateTime TestMethod();
        DateTime TestMethodWithExpirationPolicy();
        Task<DateTime> TestMethodAsync();
    }

    public interface ICommon
    {
        string Instance { get; set; }
    }

    public interface IDBMock
    {
        string Instance { get; set; }
    }
}
