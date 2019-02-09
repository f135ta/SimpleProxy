namespace SimpleProxy.Core
{
    using Interfaces;

    /// <summary>
    /// Abstract class for configuring Method Interceptors
    /// </summary>
    public abstract class MethodInterceptor
    {
        /// <summary>
        /// Executes before the target method is executed
        /// </summary>
        /// <param name="invocation">Interception Context to retrieve interception details</param>
        public abstract void BeforeInvoke(IInterceptionContext invocation);

        /// <summary>
        /// Executes after the target method is executed
        /// </summary>
        /// <param name="invocation">Interception Context to retrieve interception details</param>
        /// <param name="methodResult">The return result of the executed method</param>
        public abstract void AfterInvoke(IInterceptionContext invocation, object methodResult);
    }
}
