namespace SimpleProxy.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Method)]
    public class MethodInterceptorAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the order of aspect execution if several aspects are applied to the same method.
        /// </summary>
        internal int Order { get; set; }

        public MethodInterceptorAttribute(int order = 0)
        {
            this.Order = order;
        }
    }
}
