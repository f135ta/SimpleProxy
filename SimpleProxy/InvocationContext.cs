namespace SimpleProxy
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Attributes;
    using Castle.DynamicProxy;
    using Interfaces;

    /// <summary>
    /// Metadata Class that describes the Current Invocation
    /// </summary>
    public class InvocationContext : IHideBaseTypes
    {
        #region Internal Properties

        /// <summary>
        /// Gets or sets the Invocation Context
        /// </summary>
        internal IInvocation Invocation { get; set; }

        /// <summary>
        /// Gets or sets the Attribute that triggered the interceptor
        /// </summary>
        internal MethodInterceptionAttribute Attribute { get; set; }

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

        #endregion

        /// <summary>
        /// Inititalises a new instance of the <see cref="InvocationContext"/>
        /// </summary>
        public InvocationContext()
        {
            this.TempData = new ConcurrentDictionary<string, object>();
        }

        /// <summary>
        /// Gets the attribute that initiated the interception
        /// </summary>
        /// <returns><see cref="MethodInterceptionAttribute"/></returns>
        public MethodInterceptionAttribute GetOwningAttribute()
        {
            return this.Attribute;
        }

        /// <summary>
        /// Gets the type that owns the executing method
        /// </summary>
        /// <returns><see cref="Type"/></returns>
        public Type GetOwningType()
        {
            return this.Invocation.Method.DeclaringType;
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
        /// Gets the position of the specified type in the methods parameter list
        /// </summary>
        /// <typeparam name="T">Type of value to get</typeparam>
        /// <param name="parameterPosition">Parameter Position</param>
        /// <returns>Returns the value of the Parameter at the given position as {T}</returns>
        public T GetParameterValue<T>(int parameterPosition)
        {
            return (T)this.Invocation.GetArgumentValue(parameterPosition);
        }

        /// <summary>
        /// Gets the position of the specified type in the methods parameter list
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
            return this.TempData.TryGetValue(name, out var value) ? value : default;
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
        /// Overrides the return value for the method being called. Usually called with [BypassInvocation] to shortcut interception
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

        /// <summary>
        /// Gets the Executing Method Info
        /// </summary>
        /// <returns>Returns the position of the type in the method parameters. Returns -1 if not found</returns>
        public MethodInfo GetExecutingMethodInfo()
        {
            return this.Invocation.Method;
        }

        /// <summary>
        /// Gets the Executing Method Name
        /// </summary>
        /// <returns>The Name of the executing method</returns>
        public string GetExecutingMethodName()
        {
            return this.Invocation.Method.Name;
        }

        /// <summary>
        /// Method to try and identify the position of the specified type in the methods parameter list
        /// </summary>
        /// <param name="typeToFind">Type To Find</param>
        /// <returns>Returns the position of the type in the method parameters. Returns -1 if not found</returns>
        public int GetParameterPosition(Type typeToFind)
        {
            var method = this.Invocation.Method;
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            for (var i = method.GetParameters().Length - 1; i >= 0; i--)
            {
                var paramType = method.GetParameters()[i].ParameterType;
                if (paramType != typeToFind)
                {
                    continue;
                }

                return i;
            }

            return -1;
        }

        /// <summary>
        /// Gets the position of the specified type in the methods parameter list
        /// </summary>
        /// <typeparam name="TTypeToFind">Type to find</typeparam>
        /// <returns>Returns the position of the type in the method parameters. Returns -1 if not found</returns>
        public int GetParameterPosition<TTypeToFind>()
        {
            return this.GetParameterPosition(typeof(TTypeToFind));
        }

        /// <summary>
        /// Gets the specified attribute from the executing method
        /// </summary>
        public TAttribute GetAttributeFromMethod<TAttribute>() where TAttribute : Attribute
        {
            return this.Invocation.MethodInvocationTarget.GetCustomAttributes().OfType<TAttribute>().FirstOrDefault();
        }
    }
}
