namespace SimpleProxy.UnitOfWork
{
    using System.Linq;
    using Extensions;
    using Interfaces;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Unit of Work Interceptor
    /// </summary>
    public class UnitOfWorkInterceptor : IMethodInterceptor
    {
        /// <summary>
        /// Unit of Work Factory
        /// </summary>
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        /// <summary>
        /// Unit of Work Attribute
        /// </summary>
        private UnitOfWorkAttribute unitOfWorkAttribute;

        /// <summary>
        /// Unit of Work
        /// </summary>
        private IUnitOfWork unitOfWork;

        /// <summary>
        /// Initialises a new instance of the <see cref="UnitOfWorkInterceptor"/>
        /// </summary>
        /// <param name="unitOfWorkFactory">Unit of Work Factory</param>
        public UnitOfWorkInterceptor(IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        /// <inheritdoc />
        public void BeforeInvoke(InvocationContext invocationContext)
        {
            this.unitOfWorkAttribute = invocationContext.GetAttributeFromMethod<UnitOfWorkAttribute>();

            var dbContextType = this.unitOfWorkAttribute.DbContextType;
            var uowPropertyPosition = invocationContext.GetParameterPosition<IUnitOfWork>();
            var uow = invocationContext.GetParameterValue<IUnitOfWork>(uowPropertyPosition);

            // If the Unit of Work is already set - skip the interception
            if (uow != null)
            {
                return;
            }

            // Get the Generic Type Definition
            var methodInfo = this.unitOfWorkFactory.GetType().GetMethods().FirstOrDefault(p => p.IsGenericMethod && p.Name == nameof(this.unitOfWorkFactory.Create));

            // Build a method with the DB Context Type
            var method = methodInfo?.MakeGenericMethod(dbContextType);

            // Create the new UnitOfWork
            this.unitOfWork = (IUnitOfWork)method?.Invoke(this.unitOfWorkFactory, null);

            // Replace the UOW on the method with the new one
            invocationContext.SetParameterValue(uowPropertyPosition, this.unitOfWork);
        }

        /// <inheritdoc />
        public void AfterInvoke(InvocationContext invocationContext, object methodResult)
        {
            // Save Changes
            if (this.unitOfWorkAttribute.SaveChanges)
            {
                this.unitOfWork.SaveChanges();
            }
        }
    }
}