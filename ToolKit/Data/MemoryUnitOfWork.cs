using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ToolKit.Data
{
    /// <summary>
    /// A facade for interacting with an ORM. This particular unit of work stores the entities
    /// in memory making it a good choice to use with Unit Test.
    /// </summary>
    public class MemoryUnitOfWork : IUnitOfWork
    {
        private ArrayList _list = new ArrayList();

        /// <summary>
        /// Attaches the specified detached entity to the persistence context.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        public void Attach<T>(T entity) where T : class
        {
            Save<T>(entity);
        }

        /// <summary>
        /// Deletes the specified entity from the persistence context.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        public void Delete<T>(T entity) where T : class
        {
            if (_list.Contains(entity))
            {
                _list.Remove(entity);
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _list.Clear();
        }

        /// <summary>
        /// Gets this instance from the persistence context.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <returns>
        /// An IQueryable instance of the entity that can be used by the Repository implementation.
        /// </returns>
        public IQueryable<T> Get<T>() where T : class
        {
            return _list.Cast<T>().ToList().AsQueryable<T>();
        }

        /// <summary>
        /// Mark this unit of work to be rollback.
        /// </summary>
        public void Rollback()
        {
        }

        /// <summary>
        /// Saves the specified entity to the persistence context.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        public void Save<T>(T entity) where T : class
        {
            if (_list.Contains(entity))
            {
                _list.Remove(entity);
            }

            _list.Add(entity);
        }
    }
}