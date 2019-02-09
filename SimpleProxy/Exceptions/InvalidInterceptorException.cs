namespace SimpleProxy.Exceptions
{
    using System;

    /// <summary>
    /// Exception thrown when an Interceptor Attribute is applied but the Interceptor hasnt been configured
    /// </summary>
    public class InvalidInterceptorException : Exception
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="InvalidInterceptorException"/> class
        /// </summary>
        /// <param name="errorMessage">Error Message Text</param>
        public InvalidInterceptorException(string errorMessage) : base(errorMessage)
        {
        }
    }
}
