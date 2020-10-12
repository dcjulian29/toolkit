using System;
using System.Data;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using NHibernate;
using NHibernate.Engine;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;
using ToolKit.Validation;

namespace ToolKit.Data.NHibernate.UserTypes
{
    /// <summary>
    /// Stores the Boolean value as a byte in the database.
    /// </summary>
    public class BoolAsByte : IUserType
    {
        /// <inheritdoc/>
        /// <summary>
        /// Gets a value indicating whether this instance is mutable.
        /// </summary>
        /// <value><c>true</c> if this instance is mutable; otherwise, <c>false</c>.</value>
        public bool IsMutable => false;

        /// <inheritdoc/>
        /// <summary>
        /// Gets the type returned by <c>NullSafeGet</c>.
        /// </summary>
        /// <value>The type returned by <c>NullSafeGet</c>.</value>
        public Type ReturnedType => typeof(bool);

        /// <inheritdoc/>
        /// <summary>
        /// Gets the SQL types for the columns mapped by this type. In this case just a SQL Type will
        /// be returned: <seealso cref="DbType.Byte"/>.
        /// </summary>
        [SuppressMessage(
            "Performance",
            "CA1819:Properties should not return arrays",
            Justification = "Constrained by the Interface")]
        public SqlType[] SqlTypes => new[] { SqlTypeFactory.Byte };

        /// <summary>
        /// Reconstruct an object from the cache-able representation. At the very least this method
        /// should perform a deep copy if the type is mutable.
        /// </summary>
        /// <param name="cached">the object to be cached.</param>
        /// <param name="owner">the owner of the cached object.</param>
        /// <returns>a reconstructed boolean from the cache-able representation.</returns>
        public object Assemble(object cached, object owner) => DeepCopy(cached);

        /// <summary>
        /// Return a deep copy of the persistent state, stopping at entities and at collections.
        /// </summary>
        /// <param name="value">generally a collection element or entity field.</param>
        /// <returns>a copy of the collection element or entity field.</returns>
        public object DeepCopy(object value) => value;

        /// <summary>
        /// Transform the object into its cacheable representation. At the very least this method
        /// should perform a deep copy if the type is mutable. That may not be enough for some
        /// implementations, however; for example, associations must be cached as identifier values.
        /// </summary>
        /// <param name="value">the object to be cached.</param>
        /// <returns>a cacheable representation of the object.</returns>
        public object Disassemble(object value) => DeepCopy(value);

        /// <summary>
        /// Compare two instances of the class mapped by this type for persistent "equality" i.e.
        /// equality of persistent state.
        /// </summary>
        /// <param name="x">string to compare 1.</param>
        /// <param name="y">string to compare 2.</param>
        /// <returns><c>true</c> if objects are equals; otherwise, <c>false</c>.</returns>
        public new bool Equals(object x, object y)
        {
            if (x == null || y == null)
            {
                return false;
            }

            return x.Equals(y);
        }

        /// <summary>
        /// Get a hash code for the instance, consistent with persistence "equality".
        /// </summary>
        /// <param name="x">The object to calculate the hash code.</param>
        /// <returns>the hash code.</returns>
        public int GetHashCode(object x) => x?.GetHashCode() ?? 0;

        /// <summary>
        /// Retrieve an instance of the mapped class from a ADO.Net result set. Implementers should
        /// handle possibility of null values.
        /// </summary>
        /// <param name="rs">A Database DataReader.</param>
        /// <param name="names">Column Names.</param>
        /// <param name="session">NHibernate Session.</param>
        /// <param name="owner">The Containing Entity.</param>
        /// <returns>A boolean object containing <c>true</c> or <c>false</c>.</returns>
        public object NullSafeGet(DbDataReader rs, string[] names, ISessionImplementor session, object owner)
        {
            names = Check.NotNull(names, nameof(names));
            var result = NHibernateUtil.String.NullSafeGet(rs, names[0], session);

            if (result == null)
            {
                return false;
            }

            byte b;
            try
            {
                if (result is string)
                {
                    b = byte.Parse(result as string, CultureInfo.InvariantCulture);
                }
                else
                {
                    b = (byte)result;
                }
            }
            catch (ArgumentNullException)
            {
                return false;
            }

            return b == 1;
        }

        /// <summary>
        /// Write an instance of the mapped class to a prepared statement. Handle possibility of null
        /// values. A multi-column type should be written to parameters starting from index.
        /// </summary>
        /// <param name="cmd">a Database Command.</param>
        /// <param name="value">the object to write.</param>
        /// <param name="index">command parameter index.</param>
        /// <param name="session">the NHibernate session.</param>
        public void NullSafeSet(DbCommand cmd, object value, int index, ISessionImplementor session)
        {
            cmd = Check.NotNull(cmd, nameof(cmd));
            if (value == null)
            {
                ((IDataParameter)cmd.Parameters[index]).Value = DBNull.Value;
            }
            else
            {
                var boolValue = (bool)value;
                ((IDataParameter)cmd.Parameters[index]).Value = boolValue ? (byte)1 : (byte)0;
            }
        }

        /// <summary>
        /// During merge, replace the existing (target) value in the entity we are merging to with a
        /// new (original) value from the detached entity we are merging. For immutable objects, or
        /// null values, it is safe to simply return the first parameter. For mutable objects, it is
        /// safe to return a copy of the first parameter. For objects with component values, it might
        /// make sense to recursively replace component values.
        /// </summary>
        /// <param name="original">the value from the detached entity being merged.</param>
        /// <param name="target">the value in the managed entity.</param>
        /// <param name="owner">the managed entity.</param>
        /// <returns>Returns the first parameter because it is immutable.</returns>
        public object Replace(object original, object target, object owner)
        {
            return original;
        }
    }
}
