using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace ToolKit.Data
{
    /// <summary>
    /// Provides a base class for entities which will be persisted to the database. Include an Id
    /// property along with a consistent manner for comparing entities.
    /// </summary>
    /// <typeparam name="TId">The type of the id.</typeparam>
    [Serializable]
    [DataContract]
    [DebuggerDisplay("Id = {Id}")]
    public abstract class EntityWithTypedId<TId> : IEntityWithTypedId<TId>
      where TId : IEquatable<TId>, IComparable<TId>
    {
        /// <summary>
        /// Gets or sets the internal ID of this entity as persisted by the storage sub-system. Id
        /// may be of type string, integer, long, double, custom type, etc.
        /// </summary>
        [DataMember]
        public virtual TId Id
        {
            get;
            protected internal set;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="a">The first object of this type to compare.</param>
        /// <param name="b">The second object of this type to compare.</param>
        /// <returns><c>true</c> if the two objects are equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(EntityWithTypedId<TId> a, EntityWithTypedId<TId> b) => !(a == b);

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="a">The first object of this type to compare.</param>
        /// <param name="b">The second object of this type to compare.</param>
        /// <returns><c>true</c> if the two objects are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(EntityWithTypedId<TId> a, EntityWithTypedId<TId> b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if ((a is null) || (b is null))
            {
                return false;
            }

            return a.Equals(b);
        }

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// A 32-bit signed integer that indicates the relative order of the objects being compared.
        /// The return value has the following meanings:
        /// <list type="table">
        /// <listheader>
        /// <term>Value</term>
        /// <description>Meaning</description>
        /// </listheader>
        /// <item>
        /// <term>Less than zero</term>
        /// <description>This object is less than the <paramref name="other" /> parameter.</description>
        /// </item>
        /// <item>
        /// <term>Zero</term>
        /// <description>This object is equal to <paramref name="other" />.</description>
        /// </item>
        /// <item>
        /// <term>Greater than zero</term>
        /// <description>This object is greater than <paramref name="other" />.</description>
        /// </item>
        /// </list>
        /// </returns>
        public virtual int CompareTo(EntityWithTypedId<TId> other)
        {
            // If other is not a valid object reference, this instance is greater.
            if (other == null)
            {
                return 1;
            }

            if (IsTransient() && other.IsTransient())
            {
                return 0;
            }

            if (IsTransient())
            {
                return -1;
            }

            if (other.IsTransient())
            {
                return 1;
            }

            return Id.CompareTo(other.Id);
        }

        /// <summary>
        /// Determines whether the specified <see cref="object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="object" /> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj) => Equals(obj as EntityWithTypedId<TId>);

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// <c>true</c> if the current object is equal to the <paramref name="other" /> parameter;
        /// otherwise, <c>false</c>.
        /// </returns>
        public virtual bool Equals(EntityWithTypedId<TId> other)
        {
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (other == null || (GetType() != other.GetType()))
            {
                return false;
            }

            return Id.Equals(other.Id);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data
        /// structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            if (IsTransient())
            {
                return 0;
            }
            else
            {
                return Id.GetHashCode();
            }
        }

        /// <summary>
        /// Determines whether this instance is transient. Transient objects are not associated with
        /// an item already in storage. More specifically, if the Id is the default value, the
        /// entity is transient.
        /// </summary>
        /// <returns><c>true</c> if this entity is transient; otherwise, <c>false</c>.</returns>
        public virtual bool IsTransient() => Id == null || Id.Equals(default);
    }
}
