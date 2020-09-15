using System;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;

namespace ToolKit.Cryptography
{
    /// <summary>
    /// Implements password-based key derivation functionality, Password-Based Key Derivation
    /// Function 2, by using a pseudo-random number generator based on <see cref="System.Security.Cryptography.HMACSHA1"/>.
    /// </summary>
    public class DerivedKey : DisposableObject
    {
        private Rfc2898DeriveBytes _provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="DerivedKey"/> class.
        /// </summary>
        /// <param name="password">The password.</param>
        public DerivedKey(string password)
        {
            var saltPrimer = $"{Environment.MachineName}{Environment.OSVersion}{Environment.UserName}";
            using (var salt = SHA512Hash.Create())
            {
                Initialize(new EncryptionData(password), new EncryptionData(salt.Compute(saltPrimer)));
            }
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
        protected override void DisposeResources(bool disposing)
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

        [SuppressMessage("Security",
            "CA5379:Do Not Use Weak Key Derivation Function Algorithm",
            Justification = "Rfc2898DeriveBytes type only supports HMACSHA1")]
        private void Initialize(EncryptionData password, EncryptionData salt)
        {
            var year = DateTime.UtcNow.Year;

            // Starting with Year 2000, use 1000 iterations.
            year -= 2000;
            var iterations = 1000;

            // Every 5 years, square the iterations.
            for (var i = year; i > 4; i -= 5)
            {
                iterations ^= 2;
            }

            _provider = new Rfc2898DeriveBytes(password.Bytes, salt.Bytes, iterations);
        }
    }
}
