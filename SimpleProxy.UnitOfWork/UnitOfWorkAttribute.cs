namespace SimpleProxy.UnitOfWork
{
    using System;
    using Attributes;

    /// <summary>
    /// The Unit of Work Attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class UnitOfWorkAttribute : MethodInterceptionAttribute
    {
        /// <summary>
        /// Gets the DbContext Type
        /// </summary>
        public Type DbContextType;

        /// <summary>
        /// Gets or sets a value to override the ChangeTracking setting
        /// </summary>
        public bool ChangeTrackingEnabled;

        /// <summary>
        /// Gets or sets a value which automatically calls DbContext.SaveChanges() at the end of the method
        /// </summary>
        public bool SaveChanges;

        /// <summary>
        /// Initialises a new instance of the <see cref="UnitOfWorkAttribute"/>
        /// </summary>
        public UnitOfWorkAttribute(Type dbContextType, bool enableChangeTracking = false, bool saveChanges = false)
        {
            this.DbContextType = dbContextType;
            this.ChangeTrackingEnabled = enableChangeTracking;
            this.SaveChanges = saveChanges;
        }
    }
}
