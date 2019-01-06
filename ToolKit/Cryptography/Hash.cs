using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Security.Cryptography;
using Common.Logging;

namespace ToolKit.Cryptography
{
    /// <summary>
    /// Hash functions are fundamental to modern cryptography. These functions map binary strings of
    /// an arbitrary length to small binary strings of a fixed length, known as hash values. A
    /// cryptographic hash function has the property that it is computationally infeasible to find
    /// two distinct inputs that hash to the same value. Hash functions are commonly used with
    /// digital signatures and for data integrity.
    /// </summary>
    public class Hash
    {
        private static ILog _log = LogManager.GetLogger<Hash>();

        private HashAlgorithm _hashAlgorithm;
        private EncryptionData _hashData = new EncryptionData();

        /// <summary>
        /// Initializes a new instance of the <see cref="Hash"/> class.
        /// </summary>
        /// <param name="provider">The Hash Algorithm Provider.</param>
        public Hash(Provider provider)
        {
            switch (provider)
            {
                case Provider.CRC32:
                    _log.Debug("Using CRC32 provider...");
                    _hashAlgorithm = new Crc32Algorithm();
                    break;

                case Provider.MD5:
                    _log.Debug("Using MD5 provider...");
                    _hashAlgorithm = new MD5CryptoServiceProvider();
                    break;

                case Provider.SHA1:
                    _log.Debug("Using SHA1 provider...");
                    _hashAlgorithm = new SHA1Managed();
                    break;

                case Provider.SHA256:
                    _log.Debug("Using SHA256 provider...");
                    _hashAlgorithm = new SHA256Managed();
                    break;

                case Provider.SHA384:
                    _log.Debug("Using SHA384 provider...");
                    _hashAlgorithm = new SHA384Managed();
                    break;

                case Provider.SHA512:
                    _log.Debug("Using SHA512 provider...");
                    _hashAlgorithm = new SHA512Managed();
                    break;

                default:
                    _log.Error(m => m("Invalid Provider Provided!"));
                    throw new ArgumentException("Invalid Provider Provided!");
            }
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="Hash"/> class from being created.
        /// </summary>
        [ExcludeFromCodeCoverage]
        private Hash()
        {
        }

        /// <summary>
        /// Type of hash; some are security oriented, others are fast and simple
        /// </summary>
        public enum Provider
        {
            /// <summary>
            /// Cyclic Redundancy Check provider, 32-bit
            /// </summary>
            // ReSharper disable once InconsistentNaming
            CRC32,

            /// <summary>
            /// Message Digest algorithm 5, 128-bit
            /// </summary>
            // ReSharper disable once InconsistentNaming
            MD5,

            /// <summary>
            /// Secure Hashing Algorithm provider, SHA-1 variant, 160-bit
            /// </summary>
            // ReSharper disable once InconsistentNaming
            SHA1,

            /// <summary>
            /// Secure Hashing Algorithm provider, SHA-2 variant, 256-bit
            /// </summary>
            // ReSharper disable once InconsistentNaming
            SHA256,

            /// <summary>
            /// Secure Hashing Algorithm provider, SHA-2 variant, 384-bit
            /// </summary>
            // ReSharper disable once InconsistentNaming
            SHA384,

            /// <summary>
            /// Secure Hashing Algorithm provider, SHA-2 variant, 512-bit
            /// </summary>
            // ReSharper disable once InconsistentNaming
            SHA512
        }

        /// <summary>
        /// Gets the previously calculated hash.
        /// </summary>
        public EncryptionData Value
        {
            get
            {
                return _hashData;
            }
        }

        /// <summary>
        /// Calculates hash on a stream of arbitrary length
        /// </summary>
        /// <param name="stream">The stream of data to read.</param>
        /// <returns>the hash of the data provided.</returns>
        public EncryptionData Calculate(Stream stream)
        {
            _hashData.Bytes = _hashAlgorithm.ComputeHash(stream);
            return _hashData;
        }

        /// <summary>
        /// Calculates hash for fixed length <see cref="EncryptionData"/>
        /// </summary>
        /// <param name="data">The data to be used to hash.</param>
        /// <returns>the hash of the data provided.</returns>
        public EncryptionData Calculate(EncryptionData data)
        {
            _hashData.Bytes = _hashAlgorithm.ComputeHash(data.Bytes);
            return _hashData;
        }

        /// <summary>
        /// Calculates hash for a string with a prefixed salt value. A "salt" is random data prefixed
        /// to every hashed value to prevent common dictionary attacks.
        /// </summary>
        /// <param name="data">The data to be used to hash.</param>
        /// <param name="salt">The salt to use during the hash.</param>
        /// <returns>the hash of the data provided.</returns>
        public EncryptionData Calculate(EncryptionData data, EncryptionData salt)
        {
            var nb = new byte[data.Bytes.Length + salt.Bytes.Length];
            salt.Bytes.CopyTo(nb, 0);
            data.Bytes.CopyTo(nb, salt.Bytes.Length);

            _hashData.Bytes = _hashAlgorithm.ComputeHash(nb);
            return _hashData;
        }

        /// <summary>
        /// Implements a cyclic redundancy check (CRC) hash algorithm.
        /// </summary>
        private sealed class Crc32Algorithm : HashAlgorithm
        {
            private static uint _defaultPolynomial = 0xEDB88320;
            private uint _hash;
            private uint _seed;
            private uint[] _table;

            /// <summary>
            /// Initializes a new instance of the <see cref="Crc32Algorithm"/> class.
            /// </summary>
            public Crc32Algorithm()
            {
                Initialize();
            }

            /// <summary>
            /// Initializes an implementation of the <see
            /// cref="T:System.Security.Cryptography.HashAlgorithm"/> class.
            /// </summary>
            public override void Initialize()
            {
                if (_table != null)
                {
                    return;
                }

                _table = new uint[256];

                for (var i = 0; i < 256; i++)
                {
                    var entry = (uint)i;

                    for (var j = 0; j < 8; j++)
                    {
                        entry = (entry & 1) == 1 ? (entry >> 1) ^ _defaultPolynomial : entry >> 1;
                    }

                    _table[i] = entry;
                }

                _seed = 0xFFFFFFFF;
                _hash = _seed;
            }

            /// <summary>
            /// Called by the base class to implement the hash algorithm.
            /// </summary>
            /// <param name="buffer">The buffer containing the data to calculate the CRC hash.</param>
            /// <param name="start">The start index of the buffer.</param>
            /// <param name="length">The length of the buffer.</param>
            protected override void HashCore(byte[] buffer, int start, int length)
            {
                _hash = CalculateHash(_table, _hash, buffer, start, length);
            }

            /// <summary>
            /// When overridden in a derived class, finalizes the hash computation after the last
            /// data is processed by the cryptographic stream object.
            /// </summary>
            /// <returns>The computed hash code.</returns>
            protected override byte[] HashFinal()
            {
                var hashBuffer = ToBigEndianBytes(~_hash);

                HashValue = hashBuffer;

                return hashBuffer;
            }

            private static uint CalculateHash(uint[] table, uint seed, byte[] buffer, int start, int size)
            {
                var crc = seed;

                for (var i = start; i < size; i++)
                {
                    unchecked
                    {
                        crc = (crc >> 8) ^ table[buffer[i] ^ crc & 0xff];
                    }
                }

                return crc;
            }

            private static byte[] ToBigEndianBytes(uint x)
            {
                return new[]
                {
                    (byte)(x >> 24 & 0xff),
                    (byte)(x >> 16 & 0xff),
                    (byte)(x >> 8 & 0xff),
                    (byte)(x & 0xff)
                };
            }
        }
    }
}
