namespace SimpleProxy.Interfaces
{
    using System.Collections.Generic;

    public interface IOrderingStrategy
    {
        IEnumerable<InvocationContext> OrderBeforeInterception(IEnumerable<InvocationContext> interceptors);
        IEnumerable<InvocationContext> OrderAfterInterception(IEnumerable<InvocationContext> interceptors);
    }
}