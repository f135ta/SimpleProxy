namespace SimpleProxy
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using Castle.DynamicProxy;
    using Interfaces;

    /// <summary>
    /// Metadata Class that describes the Current Invocation
    /// </summary>
    public class InvocationContext : IHideBaseTypes
    {
        /// <summary>
        /// Gets or sets the Invocation Context
        /// </summary>
        internal IInvocation Invocation { get; set; }

        /// <summary>
        /// Gets or sets the Attribute that triggered the interceptor
        /// </summary>
        internal IInterceptorAttribute Attribute { get; set; }

        /// <summary>
        /// Gets or sets the Interceptor that is triggered for this method
        /// </summary>
        internal IMethodInterceptor Interceptor { get; set; }

        /// <summary>
        /// Stores data which can be passed between interceptors
        /// </summary>
        internal ConcurrentDictionary<string, object> TempData { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="IServiceProvider"/>
        /// </summary>
        internal IServiceProvider ServiceProvider { get; set; }

        /// <summary>
        /// Gets or sets the Order Priority of this invocation
        /// </summary>
        internal int Order { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates whether invocation of the underlying method is bypassed
        /// </summary>
        internal bool InvocationIsBypassed { get; set; }

        /// <summary>
        /// Inititalises a new instance of the <see cref="InvocationContext"/>
        /// </summary>
        public InvocationContext()
        {
            this.TempData = new ConcurrentDictionary<string, object>();
        }

        /// <summary>
        /// Gets the Service Provider for resolving dependencies
        /// </summary>
        /// <returns><see cref="IServiceProvider"/></returns>
        public IServiceProvider GetServiceProvider()
        {
            return this.ServiceProvider;
        }

        /// <summary>
        /// Method to try and identify the position of the specified type in the methods parameter list
        /// </summary>
        /// <typeparam name="T">Type of value to get</typeparam>
        /// <param name="parameterPosition">Parameter Position</param>
        /// <returns>Returns the value of the Parameter at the given position as {T}</returns>
        public T GetParameterValue<T>(int parameterPosition)
        {
            return (T)this.Invocation.GetArgumentValue(parameterPosition);
        }

        /// <summary>
        /// Method to try and identify the position of the specified type in the methods parameter list
        /// </summary>
        /// <param name="parameterPosition">Parameter Position</param>
        /// <returns>Returns the value of the Parameter at the given position</returns>
        public object GetParameterValue(int parameterPosition)
        {
            return this.Invocation.GetArgumentValue(parameterPosition);
        }

        /// <summary>
        /// Sets the value of the parameter at the specified location
        /// </summary>
        /// <param name="parameterPosition">Parameter Position</param>
        /// <param name="newValue">New Value</param>
        public void SetParameterValue(int parameterPosition, object newValue)
        {
            this.Invocation.SetArgumentValue(parameterPosition, newValue);
        }
        
        /// <summary>
        /// Adds temporary data to the context
        /// </summary>
        /// <param name="name">Name to identify the data</param>
        /// <param name="value">Data Value</param>
        public void SetTemporaryData(string name, object value)
        {
            this.TempData.TryAdd(name, value);
        }

        /// <summary>
        /// Gets temporary data from the context
        /// </summary>
        /// <param name="name">Name to identify the data</param>
        /// <returns></returns>
        public object GetTemporaryData(string name)
        {
            return this.TempData.GetValueOrDefault(name);
        }

        /// <summary>
        /// Gets the return value from the method that was called
        /// </summary>
        /// <returns>Method Return Value</returns>
        public object GetMethodReturnValue()
        {
            return this.Invocation.ReturnValue;
        }

        /// <summary>
        /// Overrides the return value for the method being called
        /// </summary>
        /// <returns>Method Return Value</returns>
        public void OverrideMethodReturnValue(object returnValue)
        {
            this.Invocation.ReturnValue = returnValue;
        }

        /// <summary>
        /// Sets the Invocation of the underlying method to be bypassed
        /// </summary>
        public void BypassInvocation()
        {
            this.InvocationIsBypassed = true;
        }
    }
}
