using System;

namespace ToolKit.Data
{
    /// <summary>
    /// This interface serves as the base interface for entities.
    /// </summary>
    /// <typeparam name="TId">The type of the id.</typeparam>
    public interface IEntityWithTypedId<TId>
        where TId : IEquatable<TId>, IComparable<TId>
    {
        /// <summary>
        /// Gets or sets the internal ID of this entity as persisted by the storage sub-system.
        /// </summary>
        TId Id { get; set; }

        /// <summary>
        /// Determines whether this instance is transient. Transient objects are not
        /// associated with an item already in storage. More specifically, if the Id
        /// is the default value, the entity is transient.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this entity is transient; otherwise, <c>false</c>.
        /// </returns>
        bool IsTransient();
    }
}