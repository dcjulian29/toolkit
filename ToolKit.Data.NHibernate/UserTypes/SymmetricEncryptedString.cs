using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Common.Logging;
using NHibernate;
using NHibernate.Engine;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;
using ToolKit.Cryptography;

namespace ToolKit.Data.NHibernate.UserTypes
{
    /// <summary>
    /// Use symmetric encryption to store the string encrypted in the database and "translate" when
    /// the object is saved or retrieved from the database.
    /// </summary>
    public class SymmetricEncryptedString : IUserType, IParameterizedType
    {
        private static EncryptionData _encryptionKey;
        private static EncryptionData _initializationVector;
        private static ILog _log = LogManager.GetLogger<SymmetricEncryptedString>();
        private readonly SymmetricEncryption _encryptor = new SymmetricEncryption(SymmetricEncryption.Provider.Rijndael);

        /// <summary>
        /// Initializes static members of the <see cref="SymmetricEncryptedString"/> class.
        /// </summary>
        static SymmetricEncryptedString()
        {
            var hash = SHA256Hash.Create().Compute(new EncryptionData(Environment.MachineName));
            _encryptionKey = new EncryptionData(hash);
            _initializationVector = new EncryptionData(hash);
        }

        /// <summary>
        /// Gets or sets the encryption key to use when saving or reading from database.
        /// </summary>
        public static EncryptionData EncryptionKey
        {
            get => _encryptionKey;

            set
            {
                _log.Info(m => m("Changing the Encryption Key for SymmetricEncryptedStrings."));

                _encryptionKey = value;
            }
        }

        /// <summary>
        /// Gets or sets the initialization vector to use when saving or reading from database.
        /// </summary>
        public static EncryptionData InitializationVector
        {
            get => _initializationVector;

            set
            {
                _log.Info(m => m("Changing the Initialization Vector for SymmetricEncryptedStrings."));

                _initializationVector = value;
            }
        }

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
        public Type ReturnedType => typeof(String);

        /// <summary>
        /// Gets the SQL types for the columns mapped by this type. In this case just a SQL Type will
        /// be returned: <seealso cref="DbType.String"/>
        /// </summary>
        public SqlType[] SqlTypes
        {
            get
            {
                var types = new SqlType[1];
                types[0] = new SqlType(DbType.String);

                return types;
            }
        }

        /// <summary>
        /// Reconstruct an object from the cacheable representation. At the very least this method
        /// should perform a deep copy if the type is mutable. (optional operation)
        /// </summary>
        /// <param name="cached">the object to be cached</param>
        /// <param name="owner">the owner of the cached object</param>
        /// <returns>a reconstructed string from the cacheable representation</returns>
        public object Assemble(object cached, object owner) => DeepCopy(cached);

        /// <summary>
        /// Return a deep copy of the persistent state, stopping at entities and at collections.
        /// </summary>
        /// <param name="value">generally a collection element or entity field</param>
        /// <returns>a copy of the collection element or entity field</returns>
        public object DeepCopy(object value) => value == null ? null : String.Copy((String)value);

        /// <inheritdoc/>
        /// <summary>
        /// Transform the object into its cacheable representation. At the very least this method
        /// should perform a deep copy if the type is mutable. That may not be enough for some
        /// implementations, however; for example, associations must be cached as identifier values.
        /// (optional operation)
        /// </summary>
        /// <param name="value">the object to be cached</param>
        /// <returns>a cacheable representation of the object</returns>
        public object Disassemble(object value) => DeepCopy(value);

        /// <inheritdoc/>
        /// <summary>
        /// Compare two instances of the class mapped by this type for persistent "equality" or
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

        /// <inheritdoc/>
        /// <summary>
        /// Get a hash code for the instance, consistent with persistence "equality"
        /// </summary>
        /// <param name="x">The object to calculate the hash code</param>
        /// <returns>the hash code.</returns>
        public int GetHashCode(object x) => x == null ? 0 : x.GetHashCode();

        /// <inheritdoc/>
        /// <summary>
        /// Retrieve an instance of the mapped class from a ADO.Net result set. Classes that inherit
        /// from this class should handle possibility of null values.
        /// </summary>
        /// <param name="rs">a DbDataReader</param>
        /// <param name="names">column names</param>
        /// <param name="owner">the containing entity</param>
        /// <returns>Trimmed string or null</returns>
        /// <param name="session">the NHibernate session</param>
        public object NullSafeGet(DbDataReader rs, string[] names, ISessionImplementor session, object owner)
        {
            var resultString = (string)NHibernateUtil.String.NullSafeGet(rs, names[0], session);

            if (resultString == null)
            {
                return null;
            }

            var data = new EncryptionData
            {
                Base64 = resultString
            };

            _encryptor.Key = _encryptionKey;
            _encryptor.InitializationVector = _initializationVector;

            return _encryptor.Decrypt(data).Text;
        }

        /// <inheritdoc/>
        /// <summary>
        /// Write an instance of the mapped class to a prepared statement. Handle possibility of null
        /// values. A multi-column type should be written to parameters starting from index.
        /// </summary>
        /// <param name="cmd">an object that implements the Database Command interface</param>
        /// <param name="value">the object to write</param>
        /// <param name="index">command parameter index</param>
        /// <param name="session">the NHibernate session</param>
        public void NullSafeSet(DbCommand cmd, object value, int index, ISessionImplementor session)
        {
            if (value == null)
            {
                NHibernateUtil.String.NullSafeSet(cmd, null, index, session);
                return;
            }

            _encryptor.Key = _encryptionKey;
            _encryptor.InitializationVector = _initializationVector;

            var data = _encryptor.Encrypt(new EncryptionData((string)value));
            value = data.Base64;

            NHibernateUtil.String.NullSafeSet(cmd, value, index, session);
        }

        /// <summary>
        /// During merge, replace the existing (target) value in the entity we are merging to with a
        /// new (original) value from the detached entity we are merging. For immutable objects, or
        /// null values, it is safe to simply return the first parameter. For mutable objects, it is
        /// safe to return a copy of the first parameter. For objects with component values, it might
        /// make sense to recursively replace component values.
        /// </summary>
        /// <param name="original">the value from the detached entity being merged</param>
        /// <param name="target">the value in the managed entity</param>
        /// <param name="owner">the managed entity</param>
        /// <returns>Returns the first parameter because it is immutable</returns>
        public object Replace(object original, object target, object owner) => original;

        /// <inheritdoc/>
        /// <summary>
        /// Gets called by Hibernate to pass the configured type parameters to the implementation.
        /// </summary>
        /// <param name="parameters">a dictionary key/value pairs of parameters to configure.</param>
        public void SetParameterValues(IDictionary<string, string> parameters)
        {
            // Method intentionally left empty.
        }
    }
}
