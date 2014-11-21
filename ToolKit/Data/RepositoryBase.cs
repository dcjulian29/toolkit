using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ToolKit.Data
{
    /// <summary>
    /// An abstract class that implements the common functions of a repository.
    /// </summary>
    /// <typeparam name="T">The entity type this repository supports.</typeparam>
    /// <typeparam name="TId">The type of the id.</typeparam>
    public abstract class RepositoryBase<T, TId> : ReadOnlyRepositoryBase<T, TId>, IRepository<T, TId>
        where T : class, IEntityWithTypedId<TId>
        where TId : IEquatable<TId>, IComparable<TId>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryBase&lt;T, TId&gt;"/> class.
        /// </summary>
        /// <param name="unitOfWorkContext">The unit of work context.</param>
        protected RepositoryBase(IUnitOfWork unitOfWorkContext)
        {
            Context = unitOfWorkContext;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryBase&lt;T, TId&gt;"/> class.
        /// </summary>
        protected RepositoryBase()
        {
        }

        /// <summary>
        /// Finalizes an instance of the RepositoryBase class.
        /// </summary>
        ~RepositoryBase()
        {
            Dispose(false);
        }

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void Delete(T entity)
        {
            Context.Delete(entity);
        }

        /// <summary>
        /// Deletes the list of entities.
        /// </summary>
        /// <param name="entities">The list of entities.</param>
        public void Delete(IEnumerable<T> entities)
        {
            entities.Each(entity => Context.Delete(entity));
        }

        /// <summary>
        /// Saves the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void Save(T entity)
        {
            Context.Save(entity);
        }

        /// <summary>
        /// Saves the list of entities.
        /// </summary>
        /// <param name="entities">The list of entities.</param>
        public void Save(IEnumerable<T> entities)
        {
            entities.Each(entity => Context.Save(entity));
        }
    }
}
