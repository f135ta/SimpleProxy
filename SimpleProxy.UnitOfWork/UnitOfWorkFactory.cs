namespace SimpleProxy.UnitOfWork
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    /// <inheritdoc />
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        /// <summary>
        /// The Services Builder Instance
        /// </summary>
        private readonly IServiceProvider services;

        /// <summary>
        /// Initialises a new instance of the <see cref="UnitOfWorkFactory"/>
        /// </summary>
        /// <param name="services">Services Collection</param>
        public UnitOfWorkFactory(IServiceProvider services)
        {
            this.services = services;
        }

        /// <inheritdoc />
        public IUnitOfWork Create<TDbContextType>() where TDbContextType : DbContext
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase("Test Database")
                .Options;

            var dbContext = ActivatorUtilities.CreateInstance<TDbContextType>(this.services, options);
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            return new UnitOfWork<TDbContextType>(dbContext);
        }
    }
}