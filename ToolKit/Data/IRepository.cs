using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ToolKit.Data
{
    /// <summary>
    /// Interface for a Repository supporting the specified entity type.
    /// </summary>
    /// <typeparam name="T">The entity type this repository supports.</typeparam>
    /// <typeparam name="TId">The type of the id.</typeparam>
    public interface IRepository<T, in TId> : IDisposable
        where T : IEntityWithTypedId<TId>
        where TId : IEquatable<TId>, IComparable<TId>
    {
        /// <summary>
        /// Gets or sets the Unit of Work.
        /// </summary>
        IUnitOfWork Context { get; set; }

        /// <summary>
        /// Determines whether the specified entity exists within the repository.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>
        ///   <c>true</c> if the specified entity exists within the repository; otherwise, <c>false</c>.
        /// </returns>
        bool Contains(T entity);

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
        /// Finds all of the entities.
        /// </summary>
        /// <returns>All of the entities.</returns>
        IEnumerable<T> FindAll();

        /// <summary>
        /// Finds all entities based on a predicate.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>all entities where the predicate is <c>true</c>.</returns>
        IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Finds an entity identified by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>An entity identified by id.</returns>
        T FindById(TId id);

        /// <summary>
        /// Finds the first entity based on a predicate.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>
        /// If the entity is found, The first entity where the predicate is <c>true</c> is returned;
        /// otherwise, a default entity is returned.
        /// </returns>
        T FindFirst(Expression<Func<T, bool>> predicate);

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
