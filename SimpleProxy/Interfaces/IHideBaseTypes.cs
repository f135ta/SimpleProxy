namespace SimpleProxy.Interfaces
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// Daniel Cazzulino's hack to hide methods defined on <see cref="object"/> for IntelliSense on Fluent Interfaces
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IHideBaseTypes
    {
        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        Type GetType();

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        int GetHashCode();

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        string ToString();

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        bool Equals(object other);
    }
}
