using System;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using Common.Logging;

namespace ToolKit.Cryptography
{
    /// <summary>
    /// Implements password-based key derivation functionality, Password-Based Key Derivation
    /// Function 2, by using a pseudo-random number generator based on <see cref="System.Security.Cryptography.HMACSHA1"/>.
    /// </summary>
    public class DerivedKey : IDisposable
    {
        private static ILog _log = LogManager.GetLogger<DerivedKey>();

        private Rfc2898DeriveBytes _provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="DerivedKey"/> class.
        /// </summary>
        /// <param name="password">The password.</param>
        public DerivedKey(string password)
        {
            var salt = SHA512Hash.Create().Compute(
                $"{Environment.MachineName}{Environment.OSVersion}{Environment.UserName}");

            Initialize(new EncryptionData(password), new EncryptionData(salt));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DerivedKey"/> class.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <param name="salt">The salt.</param>
        public DerivedKey(string password, string salt)
        {
            Initialize(new EncryptionData(password), new EncryptionData(salt));
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="DerivedKey"/> class.
        /// </summary>
        ~DerivedKey()
        {
            Dispose(false);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting
        /// unmanaged resources.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Returns the pseudo-random key for this object.
        /// </summary>
        /// <param name="size">The number of pseudo-random key bytes to generate.</param>
        /// <returns>The bytes of the derived key</returns>
        public byte[] GetBytes(int size)
        {
            return _provider.GetBytes(size);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing">
        /// <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only
        /// unmanaged resources.
        /// </param>
        [ExcludeFromCodeCoverage]
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            if (_provider == null)
            {
                return;
            }

            _provider.Dispose();
            _provider = null;
        }

        private void Initialize(EncryptionData password, EncryptionData salt)
        {
            var year = DateTime.UtcNow.Year;

            // Starting with Year 2000, use 1000 iterations.
            year = year - 2000;
            var iterations = 1000;

            // Every 5 years, square the iterations.
            for (var i = year; i > 4; i = i - 5)
            {
                iterations = iterations ^ 2;
            }

            _provider = new Rfc2898DeriveBytes(password.Bytes, salt.Bytes, iterations);
        }
    }
}
