using Arch.EntityFrameworkCore.UnitOfWork;


namespace SimpleProxy.UnitOfWork
{
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// The Unit of Work Factory for creating Unit of Work instances with different DbContext Types
    /// </summary>
    public interface IUnitOfWorkFactory
    {
        /// <summary>
        /// Provides an instance of the <see cref="IUnitOfWork"/> with the Injected <see cref="DbContext"/>
        /// </summary>
        IUnitOfWork Create<TDbContextType>() where TDbContextType : DbContext;
    }
}