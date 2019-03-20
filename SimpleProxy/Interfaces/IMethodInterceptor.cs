namespace SimpleProxy.Interfaces
{
    /// <summary>
    /// Defines a mapping between an interceptor and the attribute the interceptor responds to.
    /// </summary>
    public interface IMethodInterceptor
    {
        /// <summary>
        /// Method that is invoked instead of the method to which the interceptor has been applied
        /// </summary>
        void BeforeInvoke(InvocationContext invocationContext);

        /// <summary>
        /// Method that is invoked instead of the method to which the interceptor has been applied
        /// </summary>
        void AfterInvoke(InvocationContext invocationContext, object methodResult);
    }
}