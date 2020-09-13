using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ToolKit.Cryptography
{
    /// <summary>
    /// A class to simplify Diffie-Hellman key exchange.
    /// </summary>
    /// <seealso cref="ToolKit.DisposableObject"/>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules",
        "SA1650:ElementDocumentationMustBeSpelledCorrectly",
        Justification = "Can't change the person's name")]
    public class DiffieHellman : DisposableObject
    {
        private readonly ECDiffieHellmanCng _dh;
        private readonly Aes _encryptor;

        /// <inheritdoc/>
        public DiffieHellman()
        {
            _encryptor = new AesCryptoServiceProvider()
            {
                Padding = PaddingMode.Zeros
            };

            _dh = new ECDiffieHellmanCng
            {
                KeyDerivationFunction = ECDiffieHellmanKeyDerivationFunction.Hash,
                HashAlgorithm = CngAlgorithm.Sha256
            };

            PublicKey = new EncryptionData(_dh.PublicKey.ToByteArray());
        }

        /// <summary>
        /// Gets the initialization vector to use.
        /// </summary>
        public EncryptionData IV => new EncryptionData(_encryptor.IV);

        /// <summary>
        /// Gets the public key of the remote system.
        /// </summary>
        public EncryptionData PublicKey { get; }

        /// <summary>
        /// Decrypts the specified secret to send from other side.
        /// </summary>
        /// <param name="publicKey">The public key of the other side.</param>
        /// <param name="encrypted">The encrypted data.</param>
        /// <param name="iv">The initialization vector of the other side.</param>
        /// <returns>The decrypted data.</returns>
        [SuppressMessage("Microsoft.Usage",
            "CA2202:Do not dispose objects multiple times",
            Justification = "Why does it think it gets disposed multiple times?")]
        public EncryptionData Decrypt(EncryptionData publicKey, EncryptionData encrypted, EncryptionData iv)
        {
            if (publicKey == null)
            {
                throw new ArgumentNullException(nameof(publicKey));
            }

            if (encrypted == null)
            {
                throw new ArgumentNullException(nameof(encrypted));
            }

            if (iv == null)
            {
                throw new ArgumentNullException(nameof(iv));
            }

            var decryptedMessage = new EncryptionData
            {
                EncodingToUse = Encoding.UTF8
            };

            var key = CngKey.Import(publicKey.Bytes, CngKeyBlobFormat.EccPublicBlob);
            var derivedKey = _dh.DeriveKeyMaterial(key);

            _encryptor.Key = derivedKey;
            _encryptor.IV = iv.Bytes;

            MemoryStream stream = null;

            try
            {
                stream = new MemoryStream();

                using (var decryption = _encryptor.CreateDecryptor())
                {
                    using (var cryptStream = new CryptoStream(stream, decryption, CryptoStreamMode.Write))
                    {
                        cryptStream.Write(encrypted.Bytes, 0, encrypted.Bytes.Length);

                        decryptedMessage.Bytes = stream.ToArray();
                    }
                }
            }
            finally
            {
                stream?.Dispose();
            }

            return decryptedMessage;
        }

        /// <summary>
        /// Encrypts the specified secret to send to other side.
        /// </summary>
        /// <param name="publicKey">The public key.</param>
        /// <param name="secretMessage">The secret.</param>
        /// <returns>The encrypted data.</returns>
        [SuppressMessage("Microsoft.Usage",
            "CA2202:Do not dispose objects multiple times",
            Justification = "Why does it think it gets disposed multiple times?")]
        public EncryptionData Encrypt(EncryptionData publicKey, EncryptionData secretMessage)
        {
            if (publicKey == null)
            {
                throw new ArgumentNullException(nameof(publicKey));
            }

            if (secretMessage == null)
            {
                throw new ArgumentNullException(nameof(secretMessage));
            }

            var encryptedMessage = new EncryptionData()
            {
                EncodingToUse = Encoding.UTF8
            };

            var key = CngKey.Import(publicKey.Bytes, CngKeyBlobFormat.EccPublicBlob);
            var derivedKey = _dh.DeriveKeyMaterial(key);

            _encryptor.Key = derivedKey;

            using (var stream = new MemoryStream())
            {
                using (var encryption = _encryptor.CreateEncryptor())
                {
                    using (var cryptStream = new CryptoStream(stream, encryption, CryptoStreamMode.Write))
                    {
                        var cipher = secretMessage.Bytes;
                        cryptStream.Write(cipher, 0, cipher.Length);
                    }

                    encryptedMessage.Bytes = stream.ToArray();
                }
            }

            return encryptedMessage;
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

            _encryptor?.Dispose();

            _dh?.Dispose();
        }
    }
}
