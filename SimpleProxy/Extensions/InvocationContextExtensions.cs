namespace SimpleProxy.Extensions
{
    using System;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Extension Methods for <see cref="InvocationContext"/>
    /// </summary>
    public static class InvocationContextExtensions
    {
        /// <summary>
        /// Gets the Executing Method Info
        /// </summary>
        /// <param name="context">Invocation Context</param>
        /// <returns>Returns the position of the type in the method parameters. Returns -1 if not found</returns>
        public static MethodInfo GetExecutingMethodInfo(this InvocationContext context)
        {
            return context.Invocation.Method;
        }

        /// <summary>
        /// Gets the Executing Method Name
        /// </summary>
        /// <param name="context">Invocation Context</param>
        /// <returns>The Name of the executing method</returns>
        public static string GetExecutingMethodName(this InvocationContext context)
        {
            return context.Invocation.Method.Name;
        }

        /// <summary>
        /// Method to try and identify the position of the specified type in the methods parameter list
        /// </summary>
        /// <param name="context">Invocation Context</param>
        /// <param name="typeToFind">Type To Find</param>
        /// <returns>Returns the position of the type in the method parameters. Returns -1 if not found</returns>
        public static int GetParameterPosition(this InvocationContext context, Type typeToFind)
        {
            var method = context.Invocation.Method;
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
        public static int GetParameterPosition<TTypeToFind>(this InvocationContext context)
        {
            return context.GetParameterPosition(typeof(TTypeToFind));
        }

        /// <summary>
        /// Gets the specified attribute from the executing method
        /// </summary>
        public static TAttribute GetAttributeFromMethod<TAttribute>(this InvocationContext context) where TAttribute : Attribute
        {
            return context.Invocation.MethodInvocationTarget.GetCustomAttributes().OfType<TAttribute>().FirstOrDefault();
        }
    }
}
