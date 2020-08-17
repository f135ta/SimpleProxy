namespace SimpleProxy.Caching
{
    using Attributes;
    using System;

    /// <summary>
    /// Invocation Attribute that applies caching
    /// </summary>
    public class CacheAttribute : MethodInterceptionAttribute
    {
        /// <summary>
        /// Set the milliseconds to expire the registry
        /// </summary>
        public int MillisecondsToExpire { get; set; }
    }
}
