namespace SampleApp
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using SimpleProxy.Caching;
    using SimpleProxy.Diagnostics;
    using SimpleProxy.Extensions;
    using SimpleProxy.Logging;
    using SimpleProxy.Strategies;

    public class Program
    {
        public static void Main(string[] args)
        {
            // Configure the Service Provider
            var services = new ServiceCollection();

            // Required
            services.AddOptions();
            services.AddMemoryCache();

            services.EnableSimpleProxy(p => p
                .AddInterceptor<ConsoleLogAttribute, ConsoleLogInterceptor>()
                .AddInterceptor<DiagnosticsAttribute, DiagnosticsInterceptor>()
                .AddInterceptor<CacheAttribute, CacheInterceptor>()
                .WithOrderingStrategy<PyramidOrderStrategy>());

            services.AddTransientWithProxy<ITestClass, TestClass>();

            // Get a Proxied Class and call a method
            var serviceProvider = services.BuildServiceProvider();
            var testProxy = serviceProvider.GetService<ITestClass>();
            testProxy.TestMethod();

            Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}
