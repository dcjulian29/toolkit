using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolKit.Data.EntityFramework
{
    /// <summary>
    /// An abstract class that implements the common functions of a read-only repository for use with EntityFramework.
    /// </summary>
    /// <typeparam name="T">The entity type this repository supports.</typeparam>
    /// <typeparam name="TId">The type of the id.</typeparam>
    public abstract class EntityFrameworkRepositoryBase<T, TId> 
        : EntityFrameworkReadOnlyRepositoryBase<T, TId>, IEntityFrameworkRepository<T, TId>
        where T : class, IEntityWithTypedId<TId>
        where TId : IEquatable<TId>, IComparable<TId>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFrameworkRepositoryBase&lt;T, TId&gt;"/> class.
        /// </summary>
        /// <param name="unitOfWorkContext">The unit of work context.</param>
        protected EntityFrameworkRepositoryBase(IEntityFrameworkUnitOfWork unitOfWorkContext)
            : base(unitOfWorkContext)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFrameworkRepositoryBase&lt;T, TId&gt;"/> class.
        /// </summary>
        protected EntityFrameworkRepositoryBase()
        {
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="EntityFrameworkRepositoryBase&lt;T, TId&gt;"/> class.
        /// </summary>
        ~EntityFrameworkRepositoryBase()
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
