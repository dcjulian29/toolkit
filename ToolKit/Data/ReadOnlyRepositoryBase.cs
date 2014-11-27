using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ToolKit.Data
{
    /// <summary>
    /// An abstract class that implements the common functions of a read-only repository.
    /// </summary>
    /// <typeparam name="T">The entity type this repository supports.</typeparam>
    /// <typeparam name="TId">The type of the id.</typeparam>
    public abstract class ReadOnlyRepositoryBase<T, TId> : IReadOnlyRepository<T, TId>
        where T : class, IEntityWithTypedId<TId>
        where TId : IEquatable<TId>, IComparable<TId>
    {
        private IUnitOfWork _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyRepositoryBase&lt;T, TId&gt;"/> class.
        /// </summary>
        /// <param name="unitOfWorkContext">The unit of work context.</param>
        protected ReadOnlyRepositoryBase(IUnitOfWork unitOfWorkContext)
        {
            _context = unitOfWorkContext;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyRepositoryBase&lt;T, TId&gt;"/> class.
        /// </summary>
        protected ReadOnlyRepositoryBase()
        {
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="ReadOnlyRepositoryBase&lt;T, TId&gt;" /> class.
        /// </summary>
        ~ReadOnlyRepositoryBase()
        {
            Dispose(false);
        }

        /// <summary>
        /// Gets or sets the Unit of Work Context.
        /// </summary>
        protected IUnitOfWork Context
        {
            get
            {
                return _context;
            }

            set
            {
                _context = value;
            }
        }

        /// <summary>
        /// Determines whether the specified entity exists within the repository.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>
        ///   <c>true</c> if the specified entity exists within the repository; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(T entity)
        {
            if (entity.IsTransient())
            {
                return false;
            }

            return Context.Get<T>().FirstOrDefault(e => e == entity) != null;
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Finds all of the entities.
        /// </summary>
        /// <returns>All of the entities.</returns>
        public IEnumerable<T> FindAll()
        {
            return Context.Get<T>();
        }

        /// <summary>
        /// Finds all entities based on a predicate.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>all entities where the predicate is <c>true</c>.</returns>
        public IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            return Context.Get<T>().Where(predicate).ToList();
        }

        /// <summary>
        /// Finds an entity identified by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>An entity identified by id.</returns>
        public T FindById(TId id)
        {
            T entity;

            try
            {
                var entities = from e in Context.Get<T>()
                               where e.Id.Equals(id)
                               select e;

                entity = entities.FirstOrDefault();
            }
            catch (NotSupportedException)
            {
                // ID is a struct type so we need to cast it to an object type
                // in order to pass it down to the underlying context.
                var entities = from e in Context.Get<T>()
                               where (object)e.Id == (object)id
                               select e;

                entity = entities.FirstOrDefault();
            }

            return entity;
        }

        /// <summary>
        /// Finds the first entity based on a predicate.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>
        /// If the entity is found, The first entity where the predicate is <c>true</c> is returned;
        /// otherwise, a default entity is returned.
        /// </returns>
        public T FindFirst(Expression<Func<T, bool>> predicate)
        {
            return Context.Get<T>().Where(predicate).FirstOrDefault();
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing">
        /// <c>true</c> to release both managed and unmanaged resources;
        /// <c>false</c> to release only unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_context != null)
                {
                    _context.Dispose();
                    _context = null;
                }
            }
        }
    }
}
