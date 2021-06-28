namespace SampleApp {
    using System;
    using System.Threading.Tasks;

    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    using SimpleProxy.Caching;
    using SimpleProxy.Diagnostics;
    using SimpleProxy.Extensions;
    using SimpleProxy.Logging;
    using SimpleProxy.Strategies;

    public class Program
    {
        public static void Main(string[] args)
        {
            for (int i = 0; i <= 300; i++)
            {
                AddScopedTest();
                Console.WriteLine($"Run completed for index {i}");
            }
            Console.WriteLine("All completed.");
            Console.ReadLine();
        }

        private static async Task AddScopedTest()
        {
            // Configure the Service Provider
            var services = new ServiceCollection();

            // Required
            services.AddOptions();
            services.AddMemoryCache();
            services.AddLogging(p => p.AddConsole(x => x.IncludeScopes = true).SetMinimumLevel(LogLevel.Trace));
            services.AddScoped<ICommon, Common>();
            services.AddScoped<IDBMock, DBMOck>();

            services.EnableSimpleProxy(p => p
                .AddInterceptor<LogAttribute, LogInterceptor>()
                .AddInterceptor<DiagnosticsAttribute, DiagnosticsInterceptor>()
                .AddInterceptor<CacheAttribute, CacheInterceptor>()
                .WithOrderingStrategy<PyramidOrderStrategy>());

            services.AddScopedWithProxy<ITestClass, TestClass>();

            // Get a Proxied Class and call a method
            var serviceProvider = services.BuildServiceProvider();

            var testProxy = serviceProvider.GetService<ITestClass>();
            System.Diagnostics.Debug.WriteLine($"Test Class Instance {testProxy.Instance}");
            for (int j = 0; j < 100; j++)
            {
                var testProxy1 = serviceProvider.GetService<ITestClass>();
                System.Diagnostics.Debug.WriteLine($"Test Class Instance {testProxy1.Instance}");

                testProxy.TestMethod();

                testProxy.TestMethodWithExpirationPolicy(); // set the cache
            }
            Console.WriteLine("====> All Test Methods Complete.  Press a key. <====");

        }
    }
}
