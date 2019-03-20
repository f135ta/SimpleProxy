namespace SimpleProxy.Attributes
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Interfaces;

    /// <summary>
    /// Method Interception Attribute (inherit from this to create your interception attributes)
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    [ExcludeFromCodeCoverage]
    public abstract class MethodInterceptionAttribute : Attribute, IInterceptorAttribute
    {
    }
}
