namespace SimpleProxy.Internal.Interfaces
{
    using System;

    /// <summary>
    /// Interceptor Mapping Interface
    /// </summary>
    internal interface IInterceptorMapping 
    {
        /// <summary>
        /// Attribute Type
        /// </summary>
        Type AttributeType { get; }

        /// <summary>
        /// Interceptor Type
        /// </summary>
        Type InterceptorType { get; }
    }
}