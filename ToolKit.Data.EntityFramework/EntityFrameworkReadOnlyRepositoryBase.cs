using System;

namespace ToolKit.Data.EntityFramework
{
    /// <summary>
    /// An abstract class that implements the common functions of a read-only repository for use with EntityFramework.
    /// </summary>
    /// <typeparam name="T">The entity type this repository supports.</typeparam>
    /// <typeparam name="TId">The type of the id.</typeparam>
    public abstract class EntityFrameworkReadOnlyRepositoryBase<T, TId> 
        : ReadOnlyRepositoryBase<T, TId>, IEntityFrameworkReadOnlyRepository<T, TId>
        where T : class, IEntityWithTypedId<TId>
        where TId : IEquatable<TId>, IComparable<TId>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFrameworkReadOnlyRepositoryBase&lt;T, TId&gt;"/> class.
        /// </summary>
        /// <param name="unitOfWorkContext">The unit of work context.</param>
        protected EntityFrameworkReadOnlyRepositoryBase(IEntityFrameworkUnitOfWork unitOfWorkContext)
            : base(unitOfWorkContext)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFrameworkReadOnlyRepositoryBase&lt;T, TId&gt;"/> class.
        /// </summary>
        protected EntityFrameworkReadOnlyRepositoryBase()
        {
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="EntityFrameworkReadOnlyRepositoryBase&lt;T, TId&gt;"/> class.
        /// </summary>
        ~EntityFrameworkReadOnlyRepositoryBase()
        {
            Dispose(false);
        }

        /// <summary>
        /// Gets or sets the Unit of Work Context.
        /// </summary>
        protected new IEntityFrameworkUnitOfWork Context
        {
            get
            {
                return (IEntityFrameworkUnitOfWork)base.Context;
            }

            set
            {
                base.Context = value;
            }
        }

        /// <summary>
        /// Determines whether the specified entity exists within the repository.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>
        ///   <c>true</c> if the specified entity exists within the repository; otherwise, <c>false</c>.
        /// </returns>
        public new bool Contains(T entity)
        {
            if (entity.IsTransient())
            {
                return false;
            }

            return FindById(entity.Id) != null;
        }
    }
}
