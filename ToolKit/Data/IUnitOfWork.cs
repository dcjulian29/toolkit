using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace ToolKit.Data
{
    /// <summary>
    /// A facade for interacting with an ORM. Most ORMs implement a Unit of Work at some base level.
    /// Classes that implement this interface will implement a consistent unit of work when dealing
    /// with the persistence layer.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Attaches the specified detached entity to the persistence context.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        void Attach<T>(T entity)
            where T : class;

        /// <summary>
        /// Deletes the specified entity from the persistence context.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        void Delete<T>(T entity)
            where T : class;

        /// <summary>
        /// Gets this instance from the persistence context.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <returns>
        /// An instance of the entity that can be used by the Repository implementation to further query the results.
        /// </returns>
        [SuppressMessage(
            "Naming",
            "CA1716:Identifiers should not match keywords",
            Justification = "Really? Get off your high horse")]
        IQueryable<T> Get<T>()
            where T : class;

        /// <summary>
        /// Mark this unit of work to be rollback.
        /// </summary>
        void Rollback();

        /// <summary>
        /// Saves the specified entity to the persistence context.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        void Save<T>(T entity)
            where T : class;
    }
}
