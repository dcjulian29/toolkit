using System.IO;

namespace ToolKit.Cryptography
{
    /// <summary>
    /// Hash functions are fundamental to modern cryptography. These functions map binary strings of
    /// an arbitrary length to small binary strings of a fixed length, known as hash values. A
    /// cryptographic hash function has the property that it is computationally infeasible to find
    /// two distinct inputs that hash to the same value. Hash functions are commonly used with
    /// digital signatures and for data integrity.
    /// </summary>
    public interface IHash
    {
        /// <summary>
        /// Calculates hash for a stream.
        /// </summary>
        /// <param name="data">The data to be used to hash.</param>
        /// <returns>a string containing the hash of the data provided.</returns>
        string Compute(Stream data);

        /// <summary>
        /// Calculates hash for an array of bytes.
        /// </summary>
        /// <param name="data">The data to be used to hash.</param>
        /// <returns>a string containing the hash of the data provided.</returns>
        string Compute(byte[] data);

        /// <summary>
        /// Calculates hash for a string.
        /// </summary>
        /// <param name="data">The data to be used to hash.</param>
        /// <returns>a string containing the hash of the data provided.</returns>
        string Compute(string data);

        /// <summary>
        /// Calculates hash for data contained within an <see cref="EncryptionData"/>.
        /// </summary>
        /// <param name="data">The data to be used to hash.</param>
        /// <returns>a string containing the hash of the data provided.</returns>
        string Compute(EncryptionData data);

        /// <summary>
        /// Calculates hash for data contained within an <see cref="EncryptionData"/> with a prefixed
        /// salt value contained within an <see cref="EncryptionData"/>. A "salt" is random data
        /// prefixed to every hashed value to prevent common dictionary attacks.
        /// </summary>
        /// <param name="data">The data to be used to hash.</param>
        /// <param name="salt">The salt to use during the hash.</param>
        /// <returns>a string containing the hash of the data provided.</returns>
        string Compute(EncryptionData data, EncryptionData salt);

        /// <summary>
        /// Calculates hash for a stream.
        /// </summary>
        /// <param name="data">The data to be used to hash.</param>
        /// <returns>a byte array containing the hash of the data provided.</returns>
        byte[] ComputeToBytes(Stream data);

        /// <summary>
        /// Calculates hash for an array of bytes.
        /// </summary>
        /// <param name="data">The data to be used to hash.</param>
        /// <returns>a byte array containing the hash of the data provided.</returns>
        byte[] ComputeToBytes(byte[] data);

        /// <summary>
        /// Calculates hash for a string.
        /// </summary>
        /// <param name="data">The data to be used to hash.</param>
        /// <returns>a string containing the hash of the data provided.</returns>
        byte[] ComputeToBytes(string data);

        /// <summary>
        /// Calculates hash for data contained within an <see cref="EncryptionData"/>.
        /// </summary>
        /// <param name="data">The data to be used to hash.</param>
        /// <returns>a byte array containing the hash of the data provided.</returns>
        byte[] ComputeToBytes(EncryptionData data);

        /// <summary>
        /// Calculates hash for data contained within an <see cref="EncryptionData"/> with a prefixed
        /// salt value contained within an <see cref="EncryptionData"/>. A "salt" is random data
        /// prefixed to every hashed value to prevent common dictionary attacks.
        /// </summary>
        /// <param name="data">The data to be used to hash.</param>
        /// <param name="salt">The salt to use during the hash.</param>
        /// <returns>a byte array containing the hash of the data provided.</returns>
        byte[] ComputeToBytes(EncryptionData data, EncryptionData salt);
    }
}
