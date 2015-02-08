using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ToolKit.Data
{
    /// <summary>
    /// An class that implements the common functions of a repository.
    /// </summary>
    /// <typeparam name="T">The entity type this repository supports.</typeparam>
    /// <typeparam name="TId">The type of the id.</typeparam>
    public class Repository<T, TId> : IRepository<T, TId>
        where T : class, IEntityWithTypedId<TId>
        where TId : IEquatable<TId>, IComparable<TId>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Repository&lt;T, TId&gt;"/> class.
        /// </summary>
        /// <param name="unitOfWorkContext">The unit of work context.</param>
        public Repository(IUnitOfWork unitOfWorkContext)
        {
            Context = unitOfWorkContext;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository&lt;T, TId&gt;"/> class.
        /// </summary>
        protected Repository()
        {
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="Repository&lt;T, TId&gt;" /> class.
        /// </summary>
        ~Repository()
        {
            Dispose(false);
        }

        /// <summary>
        /// Gets or sets the Unit of Work Context.
        /// </summary>
        public IUnitOfWork Context { get; set; }

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

            return FindById(entity.Id) != null;
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
            foreach (var entity in entities)
            {
                Context.Delete(entity);
            }
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
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing">
        /// <c>true</c> to release both managed and unmanaged resources;
        /// <c>false</c> to release only unmanaged resources.
        /// </param>
        public void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Context != null)
                {
                    Context.Dispose();
                    Context = null;
                }
            }
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
            foreach (var entity in entities)
            {
                Context.Save(entity);
            }
        }
    }
}
