using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace ToolKit.Data
{
    /// <summary>
    /// Value Objects are objects that don't have any real identity and are mainly used to describe
    /// aspects of an entity.
    /// </summary>
    [SuppressMessage(
        "Major Code Smell",
        "S4035:Classes implementing \"IEquatable<T>\" should be sealed",
        Justification = "Derived classes must implement GetPropertyValue so this class knows how to compare.")]
    public abstract class ValueObject : IEquatable<ValueObject>
    {
        /// <summary>
        /// Indicates whether the left object is not equal to the right object.
        /// </summary>
        /// <param name="left">The left hand value object.</param>
        /// <param name="right">The right hand value object.</param>
        /// <returns>
        /// <c>true</c> if the left object is not equal to the right object; otherwise, <c>false</c>.
        /// </returns>
        public static bool operator !=(ValueObject left, ValueObject right) => NotEqualOperator(left, right);

        /// <summary>
        /// Indicates whether the left object is not equal to the right object.
        /// </summary>
        /// <param name="left">The left hand value object.</param>
        /// <param name="right">The right hand value object.</param>
        /// <returns>
        /// <c>true</c> if the left object is not equal to the right object; otherwise, <c>false</c>.
        /// </returns>
        public static bool operator ==(ValueObject left, ValueObject right) => EqualOperator(left, right);

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>
        /// <c>true</c> if the specified object is equal to the current object; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj) => Equals(obj as ValueObject);

        /// <summary>
        /// Determines whether the specified value object is equal to the this value object.
        /// </summary>
        /// <param name="other">The value object to compare with the current value object.</param>
        /// <returns>
        /// <c>true</c> if the this value object is equal to the current value object; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(ValueObject other)
        {
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (other == null || other.GetType() != GetType())
            {
                return false;
            }

            return GetPropertyValues().SequenceEqual(other.GetPropertyValues());
        }

        /// <summary>
        /// Get a copy of this value object.
        /// </summary>
        /// <returns>a copy of the value object.</returns>
        public ValueObject GetCopy() => MemberwiseClone() as ValueObject;

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return GetPropertyValues()
                .Select(x => x?.GetHashCode() ?? 0)
                .Aggregate((x, y) => x ^ y);
        }

        /// <summary>
        /// Indicates whether the left object is equal to the right object.
        /// </summary>
        /// <param name="left">The left hand value object.</param>
        /// <param name="right">The right hand value object.</param>
        /// <returns>
        /// <c>true</c> if the left object is equal to the right object; otherwise, <c>false</c>.
        /// </returns>
        protected static bool EqualOperator(ValueObject left, ValueObject right)
        {
            if (left is null ^ right is null)
            {
                return false;
            }

            return left is null || left.Equals(right);
        }

        /// <summary>
        /// Indicates whether the left object is not equal to the right object.
        /// </summary>
        /// <param name="left">The left hand value object.</param>
        /// <param name="right">The right hand value object.</param>
        /// <returns>
        /// <c>true</c> if the left object is not equal to the right object; otherwise, <c>false</c>.
        /// </returns>
        protected static bool NotEqualOperator(ValueObject left, ValueObject right) => !EqualOperator(left, right);

        /// <summary>
        /// Implemented in derived classes to provide a list of properties to compare for equality.
        /// </summary>
        /// <returns>A list of yielded properties to compare.</returns>
        /// <example>
        /// <code>
        /// protected override IEnumerable&lt;object&gt; GetPropertyValues()
        /// {
        ///     yield return Property1;
        ///     yield return Property2;
        /// }
        /// </code>
        /// </example>
        protected abstract IEnumerable<object> GetPropertyValues();
    }
}
