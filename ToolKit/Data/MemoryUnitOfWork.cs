using System.Collections;
using System.Linq;

namespace ToolKit.Data
{
    /// <summary>
    /// A facade for interacting with an ORM. This particular unit of work stores the entities in
    /// memory making it a good choice to use with Unit Test.
    /// </summary>
    public class MemoryUnitOfWork : DisposableObject, IUnitOfWork
    {
        private readonly ArrayList _list = new ArrayList();

        /// <summary>
        /// Attaches the specified detached entity to the persistence context.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        public void Attach<T>(T entity)
            where T : class
        {
            Save<T>(entity);
        }

        /// <summary>
        /// Deletes the specified entity from the persistence context.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        public void Delete<T>(T entity)
            where T : class
        {
            if (_list.Contains(entity))
            {
                _list.Remove(entity);
            }
        }

        /// <summary>
        /// Gets this instance from the persistence context.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <returns>
        /// An instance of the entity that can be used by the Repository implementation to further
        /// query the results.
        /// </returns>
        public IQueryable<T> Get<T>()
            where T : class => _list.Cast<T>().AsQueryable();

        /// <summary>
        /// Mark this unit of work to be rollback.
        /// </summary>
        public void Rollback()
        {
            // Rollbacks not supported with Memory Unit Of Work
        }

        /// <summary>
        /// Saves the specified entity to the persistence context.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        public void Save<T>(T entity)
            where T : class
        {
            if (_list.Contains(entity))
            {
                _list.Remove(entity);
            }

            _list.Add(entity);
        }

        /// <summary>
        /// Disposes the resources used by the inherited class.
        /// </summary>
        /// <param name="disposing">
        /// <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release
        /// only unmanaged resources.
        /// </param>
        protected override void DisposeResources(bool disposing)
        {
            _list.Clear();
        }
    }
}
