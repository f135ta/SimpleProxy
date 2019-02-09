namespace SampleApp.Interceptors
{
    using System;
    using SimpleProxy.Core;
    using SimpleProxy.Interfaces;

    public class ConsoleLogInterceptor : MethodInterceptor
    {
        public override void BeforeInvoke(IInterceptionContext interceptionContext)
        {
            Console.WriteLine($"Method executing: {interceptionContext.CurrentMethod.Name}");
        }

        public override void AfterInvoke(IInterceptionContext interceptionContext, object methodResult)
        {
            Console.WriteLine($"Method executed: {interceptionContext.CurrentMethod.Name}");
        }
    }
}
