using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ToolKit.Data
{
    /// <summary>
    /// Interface for a Repository supporting the specified entity type
    /// </summary>
    /// <typeparam name="T">The entity type this repository supports.</typeparam>
    /// <typeparam name="TId">The type of the id.</typeparam>
    public interface IRepository<T, TId> : IReadOnlyRepository<T, TId>, IDisposable
        where T : IEntityWithTypedId<TId>
        where TId : IEquatable<TId>, IComparable<TId>
    {
        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Delete(T entity);

        /// <summary>
        /// Deletes the list of entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        void Delete(IEnumerable<T> entities);

        /// <summary>
        /// Saves the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Save(T entity);

        /// <summary>
        /// Saves the list of entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        void Save(IEnumerable<T> entities);
    }
}
