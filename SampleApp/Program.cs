namespace SampleApp
{
    using System;
    using Attributes;
    using Interceptors;
    using Interfaces;
    using Microsoft.Extensions.DependencyInjection;
    using SimpleProxy.Extensions;

    public class Program
    {
        public static void Main(string[] args)
        {
            // Configure the Service Provider
            var serviceProvider = ConfigureServices();

            // Get a Proxied Class and call a method
            var testProxy = serviceProvider.GetService<ITestClass>();
            testProxy.TestMethod();

            Console.WriteLine();
            Console.WriteLine("Done");
            Console.ReadLine();
        }

        /// <summary>
        /// Configures a Service Collection for SimpleProxy
        /// </summary>
        /// <returns></returns>
        private static ServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            // Required
            services.AddOptions();

            services.EnableSimpleProxy(p => p
                .IgnoreInvalidInterceptorConfigurations()
                .AddInterceptor<ConsoleLogAttribute, ConsoleLogInterceptor>());

            services.AddTransientWithProxy<ITestClass, TestClass>();

            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider;
        }
    }
}
