using System;
using System.Data;
using Common.Logging;
using NHibernate;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;

namespace ToolKit.Data.NHibernate.UserTypes
{
    /// <summary>
    /// Translates Null DateTime columns in database to a valid DateTime value.
    /// </summary>
    public class NullableDateTime : IUserType
    {
        private static ILog _log = LogManager.GetLogger<NullableDateTime>();

        /// <summary>
        /// Gets a value indicating whether this instance is mutable.
        /// </summary>
        /// <value><c>true</c> if this instance is mutable; otherwise, <c>false</c>.</value>
        public bool IsMutable
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the type returned by <c>NullSafeGet</c>.
        /// </summary>
        /// <value>The type returned by <c>NullSafeGet</c>.</value>
        public Type ReturnedType
        {
            get
            {
                return typeof(DateTime);
            }
        }

        /// <summary>
        /// Gets the SQL types for the columns mapped by this type. In this case just a SQL Type
        /// will be returned: <seealso cref="DbType.DateTime"/>
        /// </summary>
        public SqlType[] SqlTypes
        {
            get
            {
                SqlType[] types = new SqlType[1];
                types[0] = new SqlType(DbType.DateTime);
                return types;
            }
        }

        /// <summary>
        /// Reconstruct an object from the cacheable representation. At the very least this method
        /// should perform a deep copy if the type is mutable. (optional operation)
        /// </summary>
        /// <param name="cached">the object to be cached</param>
        /// <param name="owner">the owner of the cached object</param>
        /// <returns>a reconstructed DateTime from the cacheable representation</returns>
        public object Assemble(object cached, object owner)
        {
            return DeepCopy(cached);
        }

        /// <summary>
        /// Return a deep copy of the persistent state, stopping at entities and at collections.
        /// </summary>
        /// <param name="value">generally a collection element or entity field</param>
        /// <returns>a copy of the collection element or entity field</returns>
        public object DeepCopy(object value)
        {
            return value;
        }

        /// <summary>
        /// Transform the object into its cacheable representation. At the very least this method
        /// should perform a deep copy if the type is mutable. That may not be enough for some
        /// implementations, however; for example, associations must be cached as identifier values.
        /// (optional operation)
        /// </summary>
        /// <param name="value">the object to be cached</param>
        /// <returns>a cacheable representation of the object</returns>
        public object Disassemble(object value)
        {
            return DeepCopy(value);
        }

        /// <summary>
        /// Compare two instances of the class mapped by this type for persistent "equality" i.e.
        /// equality of persistent state
        /// </summary>
        /// <param name="x">string to compare 1</param>
        /// <param name="y">string to compare 2</param>
        /// <returns><c>true</c> if objects are equals; otherwise, <c>false</c></returns>
        public new bool Equals(object x, object y)
        {
            if (x == null || y == null)
            {
                return false;
            }

            return x.Equals(y);
        }

        /// <summary>
        /// Get a hash code for the instance, consistent with persistence "equality"
        /// </summary>
        /// <param name="x">The object to calculate the hash code</param>
        /// <returns>the hash code.</returns>
        public int GetHashCode(object x)
        {
            return x == null ? 0 : x.GetHashCode();
        }

        /// <summary>
        /// Retrieve an instance of the mapped class from a ADO.Net result set. Implementers should
        /// handle possibility of null values.
        /// </summary>
        /// <param name="dataReader">an IDataReader</param>
        /// <param name="names">column names</param>
        /// <param name="owner">the containing entity</param>
        /// <returns>A DateTime object or null</returns>
        public object NullSafeGet(IDataReader dataReader, string[] names, object owner)
        {
            var result = NHibernateUtil.String.NullSafeGet(dataReader, names[0]);

            var retval = DateTime.MinValue;

            if (result == null)
            {
                return retval;
            }

            try
            {
                retval = DateTime.Parse(Convert.ToString(result));
            }
            catch (FormatException fex)
            {
                _log.Error("Invalid Date format!", fex);
                throw;
            }

            return retval;
        }

        /// <summary>
        /// Write an instance of the mapped class to a prepared statement. Handle possibility of
        /// null values. A multi-column type should be written to parameters starting from index.
        /// </summary>
        /// <param name="cmd">a Database Command</param>
        /// <param name="value">the object to write</param>
        /// <param name="index">command parameter index</param>
        public void NullSafeSet(IDbCommand cmd, object value, int index)
        {
            if (value == null)
            {
                ((IDataParameter)cmd.Parameters[index]).Value = DBNull.Value;
            }
            else
            {
                try
                {
                    ((IDataParameter)cmd.Parameters[index]).Value = DateTime.Parse(value.ToString());
                }
                catch (FormatException fex)
                {
                    _log.Error("Invalid Date format!", fex);
                    throw;
                }
            }
        }

        /// <summary>
        /// During merge, replace the existing (target) value in the entity we are merging to with a
        /// new (original) value from the detached entity we are merging. For immutable objects, or
        /// null values, it is safe to simply return the first parameter. For mutable objects, it is
        /// safe to return a copy of the first parameter. For objects with component values, it
        /// might make sense to recursively replace component values.
        /// </summary>
        /// <param name="original">the value from the detached entity being merged</param>
        /// <param name="target">the value in the managed entity</param>
        /// <param name="owner">the managed entity</param>
        /// <returns>Returns the first parameter because it is immutable</returns>
        public object Replace(object original, object target, object owner)
        {
            return original;
        }
    }
}
