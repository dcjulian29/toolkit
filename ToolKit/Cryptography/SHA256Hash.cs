using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace ToolKit.Cryptography
{
    /// <summary>
    /// Implementation of SHA-2 which is a cryptographic hash function designed by the National
    /// Security Agency and published by the NIST as a U.S. Federal Information Processing Standard.
    /// SHA stands for Secure Hash Algorithm. This implementation uses 256 bit (32 byte) block sizes.
    /// </summary>
    /// <remarks>
    /// Hash functions are fundamental to modern cryptography. These functions map binary strings of
    /// an arbitrary length to small binary strings of a fixed length, known as hash values. A
    /// cryptographic hash function has the property that it is computationally infeasible to find
    /// two distinct inputs that hash to the same value. Hash functions are commonly used with
    /// digital signatures and for data integrity.
    /// </remarks>
    [SuppressMessage(
        "Minor Code Smell",
        "S101:Types should be named in PascalCase",
        Justification = "Class names contains an acronym.")]
    public sealed class SHA256Hash : DisposableObject, IHash
    {
        private Hash _algorithm = new Hash(Hash.Provider.SHA256);

        /// <summary>
        /// Prevents a default instance of the <see cref="SHA256Hash"/> class from being created.
        /// </summary>
        [ExcludeFromCodeCoverage]
        private SHA256Hash()
        {
        }

        /// <summary>
        /// Creates an instance of the Hash Algorithm.
        /// </summary>
        /// <returns>an instance of the SHA256 Hash object.</returns>
        public static SHA256Hash Create()
        {
            return new SHA256Hash();
        }

        /// <summary>
        /// Calculates hash for a stream.
        /// </summary>
        /// <param name="data">The data to be used to hash.</param>
        /// <returns>a string containing the hash of the data provided.</returns>
        public string Compute(Stream data)
        {
            var hash = _algorithm.Calculate(data);

            return hash.Hex;
        }

        /// <summary>
        /// Calculates hash for an array of bytes.
        /// </summary>
        /// <param name="data">The data to be used to hash.</param>
        /// <returns>a string containing the hash of the data provided.</returns>
        public string Compute(byte[] data)
        {
            return _algorithm.Calculate(new EncryptionData(data)).Hex;
        }

        /// <summary>
        /// Calculates hash for a string.
        /// </summary>
        /// <param name="data">The data to be used to hash.</param>
        /// <returns>a string containing the hash of the data provided.</returns>
        public string Compute(string data)
        {
            return _algorithm.Calculate(new EncryptionData(data)).Hex;
        }

        /// <summary>
        /// Calculates hash for data contained within an <see cref="EncryptionData"/>.
        /// </summary>
        /// <param name="data">The data to be used to hash.</param>
        /// <returns>a string containing the hash of the data provided.</returns>
        public string Compute(EncryptionData data)
        {
            return _algorithm.Calculate(data).Hex;
        }

        /// <summary>
        /// Calculates hash for data contained within an <see cref="EncryptionData"/> with a prefixed
        /// salt value contained within an <see cref="EncryptionData"/>. A "salt" is random data
        /// prefixed to every hashed value to prevent common dictionary attacks.
        /// </summary>
        /// <param name="data">The data to be used to hash.</param>
        /// <param name="salt">The salt to use during the hash.</param>
        /// <returns>a string containing the hash of the data provided.</returns>
        public string Compute(EncryptionData data, EncryptionData salt)
        {
            return _algorithm.Calculate(data, salt).Hex;
        }

        /// <summary>
        /// Calculates hash for a stream.
        /// </summary>
        /// <param name="data">The data to be used to hash.</param>
        /// <returns>a byte array containing the hash of the data provided.</returns>
        public byte[] ComputeToBytes(Stream data)
        {
            return _algorithm.Calculate(data).Bytes;
        }

        /// <summary>
        /// Calculates hash for an array of bytes.
        /// </summary>
        /// <param name="data">The data to be used to hash.</param>
        /// <returns>a byte array containing the hash of the data provided.</returns>
        public byte[] ComputeToBytes(byte[] data)
        {
            return _algorithm.Calculate(new EncryptionData(data)).Bytes;
        }

        /// <summary>
        /// Calculates hash for a string.
        /// </summary>
        /// <param name="data">The data to be used to hash.</param>
        /// <returns>a string containing the hash of the data provided.</returns>
        public byte[] ComputeToBytes(string data)
        {
            return _algorithm.Calculate(new EncryptionData(data)).Bytes;
        }

        /// <summary>
        /// Calculates hash for data contained within an <see cref="EncryptionData"/>.
        /// </summary>
        /// <param name="data">The data to be used to hash.</param>
        /// <returns>a byte array containing the hash of the data provided.</returns>
        public byte[] ComputeToBytes(EncryptionData data)
        {
            return _algorithm.Calculate(data).Bytes;
        }

        /// <summary>
        /// Calculates hash for data contained within an <see cref="EncryptionData"/> with a prefixed
        /// salt value contained within an <see cref="EncryptionData"/>. A "salt" is random data
        /// prefixed to every hashed value to prevent common dictionary attacks.
        /// </summary>
        /// <param name="data">The data to be used to hash.</param>
        /// <param name="salt">The salt to use during the hash.</param>
        /// <returns>a byte array containing the hash of the data provided.</returns>
        public byte[] ComputeToBytes(EncryptionData data, EncryptionData salt)
        {
            return _algorithm.Calculate(data, salt).Bytes;
        }

        /// <summary>
        /// Disposes the resources used by the inherited class.
        /// </summary>
        /// <param name="disposing">
        /// <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release
        /// only unmanaged resources.
        /// </param>
        protected override void DisposeResources(bool disposing)
        {
            if (_algorithm != null)
            {
                _algorithm.Dispose();
                _algorithm = null;
            }
        }
    }
}
