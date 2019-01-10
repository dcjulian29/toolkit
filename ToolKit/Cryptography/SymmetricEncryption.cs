using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Security.Cryptography;
using Common.Logging;

namespace ToolKit.Cryptography
{
    /// <summary>
    /// Symmetric-key algorithms are a class of algorithms for cryptography that use trivially
    /// related, often identical, cryptographic keys for both decryption and encryption. The
    /// encryption key is trivially related to the decryption key, in that they may be identical or
    /// there is a simple transformation to go between the two keys. The keys, in practice, represent
    /// a shared secret between two or more parties that can be used to maintain a private
    /// information link.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules",
        "SA1650:ElementDocumentationMustBeSpelledCorrectly",
        Justification = "Encryption tend to have names not in standard dictionaries...")]
    public class SymmetricEncryption
    {
        private readonly int _bufferSize = 2048;
        private readonly SymmetricAlgorithm _crypto;
        private EncryptionData _initializationVector;
        private EncryptionData _key;

        /// <summary>
        /// Initializes a new instance of the <see cref="SymmetricEncryption"/> class using the
        /// specified provider.
        /// </summary>
        /// <param name="provider">The cryptographic provider.</param>
        public SymmetricEncryption(Provider provider)
        {
            switch (provider)
            {
                case Provider.Rijndael:
                    _crypto = new RijndaelManaged();
                    break;

                case Provider.TripleDES:
                    _crypto = new TripleDESCryptoServiceProvider();
                    break;

                default:
                    throw new ArgumentException("Invalid Provider Provided!");
            }

            // Ensure that any IV or key can be used regardless of length
            _crypto.Padding = PaddingMode.Zeros;

            // Make sure key and IV are always set, no matter what
            Key = RandomKey();
            InitializationVector = RandomInitializationVector();
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="SymmetricEncryption"/> class from being created.
        /// </summary>
        [ExcludeFromCodeCoverage]
        private SymmetricEncryption()
        {
        }

        /// <summary>
        /// Types of available symmetric encryption algorithms. Some are considered insecure as of
        /// 2011 due the small key sizes
        /// </summary>
        public enum Provider
        {
            /// <summary>
            /// The Rijndael (also known as AES) provider supports keys of 128, 192, or 256 bits with
            /// a default of 256 bits
            /// </summary>
            // ReSharper disable once IdentifierTypo
            Rijndael,

            /// <summary>
            /// The TripleDES provider (also known as 3DES) supports keys of 128 or 192 bits with a
            /// default of 192 bits
            /// </summary>
            // ReSharper disable once InconsistentNaming
            TripleDES
        }

        /// <summary>
        /// Gets or sets the initialization vector.
        /// </summary>
        /// <value>The initialization vector.</value>
        /// <remark>
        /// Using the default Cipher Block Chaining (CBC) mode, all data blocks are processed using
        /// the value derived from the previous block; the first data block has no previous data
        /// block to use, so it needs an InitializationVector to feed the first block
        /// </remark>
        public EncryptionData InitializationVector
        {
            get => _initializationVector;

            set
            {
                _initializationVector = value;
                _initializationVector.MaximumBytes = _crypto.BlockSize / 8;
                _initializationVector.MinimumBytes = _crypto.BlockSize / 8;
            }
        }

        /// <summary>
        /// Gets or sets the key used to encrypt/decrypt data
        /// </summary>
        public EncryptionData Key
        {
            get => _key;

            set
            {
                _key = value;
                _key.MaximumBytes = _crypto.LegalKeySizes[0].MaxSize / 8;
                _key.MinimumBytes = _crypto.LegalKeySizes[0].MinSize / 8;
            }
        }

        /// <summary>
        /// Gets or sets the key size in bits. We use the default key size for any given provider; if
        /// you want to force a specific key size, set this property
        /// </summary>
        public int KeySizeBits
        {
            get => _crypto.KeySize;

            set
            {
                _crypto.KeySize = value;
                _key.MaximumBits = value;
            }
        }

        /// <summary>
        /// Gets or sets the key size in bytes. We use the default key size for any given provider;
        /// if you want to force a specific key size, set this property
        /// </summary>
        public int KeySizeBytes
        {
            get => _crypto.KeySize / 8;

            set
            {
                _crypto.KeySize = value * 8;
                _key.MaximumBytes = value;
            }
        }

        /// <summary>
        /// Decrypts the specified data using provided key and preset initialization vector
        /// </summary>
        /// <param name="encryptedData">The encrypted data.</param>
        /// <param name="key">The encryption key used to preform the cryptographic function on.</param>
        /// <returns>the decrypted data.</returns>
        public EncryptionData Decrypt(EncryptionData encryptedData, EncryptionData key)
        {
            Key = key;

            return Decrypt(encryptedData);
        }

        /// <summary>
        /// Decrypts the specified data using provided key and preset initialization vector
        /// </summary>
        /// <param name="encryptedData">The encrypted data.</param>
        /// <param name="key">The encryption key used to preform the cryptographic function on.</param>
        /// <param name="iv">The initialization vector.</param>
        /// <returns>the decrypted data.</returns>
        public EncryptionData Decrypt(EncryptionData encryptedData, EncryptionData key, EncryptionData iv)
        {
            Key = key;
            InitializationVector = iv;

            return Decrypt(encryptedData);
        }

        /// <summary>
        /// Decrypts the specified data using preset key and preset initialization vector
        /// </summary>
        /// <param name="encryptedData">The encrypted data.</param>
        /// <returns>the decrypted data.</returns>
        public EncryptionData Decrypt(EncryptionData encryptedData)
        {
            _crypto.Key = _key.Bytes;
            _crypto.IV = _initializationVector.Bytes;

            using (var ms = new MemoryStream(encryptedData.Bytes, 0, encryptedData.Bytes.Length))
            {
                var b = new byte[encryptedData.Bytes.Length];

                var cs = new CryptoStream(ms, _crypto.CreateDecryptor(), CryptoStreamMode.Read);

                try
                {
                    cs.Read(b, 0, encryptedData.Bytes.Length);
                }
                finally
                {
                    cs.Close();
                }

                return new EncryptionData(b);
            }
        }

        /// <summary>
        /// Decrypts the specified stream using provided key and preset initialization vector
        /// </summary>
        /// <param name="encryptedStream">The encrypted stream.</param>
        /// <param name="key">The encryption key used to preform the cryptographic function on.</param>
        /// <param name="iv">The initialization vector.</param>
        /// <returns>the decrypted data.</returns>
        public EncryptionData Decrypt(Stream encryptedStream, EncryptionData key, EncryptionData iv)
        {
            Key = key;
            InitializationVector = iv;

            return Decrypt(encryptedStream);
        }

        /// <summary>
        /// Decrypts the specified stream using provided key and preset initialization vector
        /// </summary>
        /// <param name="encryptedStream">The encrypted stream.</param>
        /// <param name="key">The encryption key used to preform the cryptographic function on.</param>
        /// <returns>the decrypted data.</returns>
        public EncryptionData Decrypt(Stream encryptedStream, EncryptionData key)
        {
            Key = key;

            return Decrypt(encryptedStream);
        }

        /// <summary>
        /// Decrypts the specified stream using preset key and preset initialization vector
        /// </summary>
        /// <param name="encryptedStream">The encrypted stream.</param>
        /// <returns>the decrypted data.</returns>
        public EncryptionData Decrypt(Stream encryptedStream)
        {
            _crypto.Key = _key.Bytes;
            _crypto.IV = _initializationVector.Bytes;

            using (var ms = new MemoryStream())
            {
                var b = new byte[_bufferSize + 1];

                using (CryptoStream cs = new CryptoStream(
                    encryptedStream,
                    _crypto.CreateDecryptor(),
                    CryptoStreamMode.Read))
                {
                    var i = cs.Read(b, 0, _bufferSize);

                    while (i > 0)
                    {
                        ms.Write(b, 0, i);
                        i = cs.Read(b, 0, _bufferSize);
                    }

                    cs.Close();
                }

                ms.Close();

                return new EncryptionData(ms.ToArray());
            }
        }

        /// <summary>
        /// Encrypts the specified Data using provided key
        /// </summary>
        /// <param name="d">The data to encrypt.</param>
        /// <param name="key">The encryption key used to preform the cryptographic function on.</param>
        /// <returns>the encrypted data.</returns>
        public EncryptionData Encrypt(EncryptionData d, EncryptionData key)
        {
            Key = key;
            return Encrypt(d);
        }

        /// <summary>
        /// Encrypts the specified Data using preset key and preset initialization vector
        /// </summary>
        /// <param name="d">The data to encrypt.</param>
        /// <returns>the encrypted data.</returns>
        public EncryptionData Encrypt(EncryptionData d)
        {
            _crypto.Key = _key.Bytes;
            _crypto.IV = _initializationVector.Bytes;

            using (var ms = new MemoryStream())
            {
                var cs = new CryptoStream(ms, _crypto.CreateEncryptor(), CryptoStreamMode.Write);
                cs.Write(d.Bytes, 0, d.Bytes.Length);
                cs.Close();
                ms.Close();

                return new EncryptionData(ms.ToArray());
            }
        }

        /// <summary>
        /// Encrypts the specified Data using provided key and provided initialization vector
        /// </summary>
        /// <param name="d">The data to encrypt.</param>
        /// <param name="key">The encryption key used to preform the cryptographic function on.</param>
        /// <param name="iv">The initialization vector.</param>
        /// <returns>the encrypted data.</returns>
        public EncryptionData Encrypt(EncryptionData d, EncryptionData key, EncryptionData iv)
        {
            InitializationVector = iv;
            Key = key;

            return Encrypt(d);
        }

        /// <summary>
        /// Encrypts the stream to memory using provided key and provided initialization vector
        /// </summary>
        /// <param name="s">The stream to preform the cryptographic function on.</param>
        /// <param name="key">The encryption key used to preform the cryptographic function on.</param>
        /// <param name="iv">The initialization vector .</param>
        /// <returns>the encrypted data.</returns>
        public EncryptionData Encrypt(Stream s, EncryptionData key, EncryptionData iv)
        {
            InitializationVector = iv;
            Key = key;

            return Encrypt(s);
        }

        /// <summary>
        /// Encrypts the stream to memory using specified key
        /// </summary>
        /// <param name="s">The stream to preform the cryptographic function on.</param>
        /// <param name="key">The encryption key used to preform the cryptographic function on.</param>
        /// <returns>the encrypted data.</returns>
        public EncryptionData Encrypt(Stream s, EncryptionData key)
        {
            Key = key;

            return Encrypt(s);
        }

        /// <summary>
        /// Encrypts the specified stream to memory using preset key and preset initialization vector
        /// </summary>
        /// <param name="s">The stream to preform the cryptographic function on.</param>
        /// <returns>the encrypted data.</returns>
        public EncryptionData Encrypt(Stream s)
        {
            _crypto.Key = _key.Bytes;
            _crypto.IV = _initializationVector.Bytes;

            using (var ms = new MemoryStream())
            {
                var b = new byte[_bufferSize + 1];

                var cs = new CryptoStream(ms, _crypto.CreateEncryptor(), CryptoStreamMode.Write);
                var i = s.Read(b, 0, _bufferSize);

                while (i > 0)
                {
                    cs.Write(b, 0, i);
                    i = s.Read(b, 0, _bufferSize);
                }

                cs.Close();
                ms.Close();

                return new EncryptionData(ms.ToArray());
            }
        }

        private EncryptionData RandomInitializationVector()
        {
            _crypto.GenerateIV();

            return new EncryptionData(_crypto.IV);
        }

        private EncryptionData RandomKey()
        {
            _crypto.GenerateKey();
            return new EncryptionData(_crypto.Key);
        }
    }
}
