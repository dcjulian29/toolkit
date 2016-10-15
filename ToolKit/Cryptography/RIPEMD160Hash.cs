using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace ToolKit.Cryptography
{
    /// <summary>
    /// RIPEMD-160 is a 160-bit cryptographic hash function. It is intended for use as a replacement
    /// for the 128-bit hash functions MD4, MD5, and RIPEMD. RIPEMD was developed in the framework of
    /// the EU project RIPE (RACE Integrity Primitives Evaluation, 1988-1992).
    /// </summary>
    /// <remarks>
    /// Hash functions are fundamental to modern cryptography. These functions map binary strings of
    /// an arbitrary length to small binary strings of a fixed length, known as hash values. A
    /// cryptographic hash function has the property that it is computationally infeasible to find
    /// two distinct inputs that hash to the same value. Hash functions are commonly used with
    /// digital signatures and for data integrity.
    /// </remarks>
    // ReSharper disable once InconsistentNaming
    public class RIPEMD160Hash : IHash
    {
        private readonly Hash _algorithm = new Hash(Hash.Provider.RIPEMD160);

        /// <summary>
        /// Prevents a default instance of the <see cref="RIPEMD160Hash"/> class from being created.
        /// </summary>
        [ExcludeFromCodeCoverage]
        private RIPEMD160Hash()
        {
        }

        /// <summary>
        /// Creates an instance of the Hash Algorithm.
        /// </summary>
        /// <returns>an instance of the SHA512 Hash object</returns>
        public static RIPEMD160Hash Create()
        {
            return new RIPEMD160Hash();
        }

        /// <summary>
        /// Calculates hash for a stream.
        /// </summary>
        /// <param name="data">The data to be used to hash.</param>
        /// <returns>a string containing the hash of the data provided.</returns>
        public string Compute(Stream data)
        {
            return _algorithm.Calculate(data).Hex;
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
    }
}
