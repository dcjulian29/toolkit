using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ToolKit.Cryptography
{
    /// <summary>
    /// Represents encryption/decryption methods that utilize a RSA keys to encrypt/decrypt a
    /// password used to symmetrically encrypt the payload.
    /// </summary>
    [SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1650:ElementDocumentationMustBeSpelledCorrectly",
        Justification = "Encryption tend to have names not in standard dictionaries...")]
    public class ASymmetricEncryption
    {
        private readonly int _keySize = 256;
        private readonly RsaPrivateKey _privateKey;
        private readonly RsaPublicKey _publicKey;
        private EncryptionData _password;

        /// <summary>
        /// Initializes a new instance of the <see cref="ASymmetricEncryption"/> class.
        /// </summary>
        /// <param name="key">
        /// The private key that will be used to decrypt the password used to decrypt the password.
        /// </param>
        /// <exception cref="ArgumentNullException">An RSA public key must be provided!</exception>
        public ASymmetricEncryption(RsaPublicKey key)
        {
            _publicKey = key ?? throw
                             new ArgumentNullException(
                                 nameof(key),
                                 "An RSA public key must be provided!");

            var random = CryptoRandomNumber.Next();

            var generator = new PasswordGenerator(random)
            {
                IncludeExtended = false
            };

            _password = new EncryptionData(generator.Generate(32));

            EncryptedPassword = new RsaEncryption().Encrypt(_password, _publicKey);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ASymmetricEncryption"/> class.
        /// </summary>
        /// <param name="key">
        /// The private key that will be used to decrypt the password used to decrypt the password.
        /// </param>
        /// <param name="password">The password to use during the symmetric part of the encryption.</param>
        /// <exception cref="ArgumentNullException">
        /// password - A password must be provided! or key - An RSA public key must be provided!
        /// </exception>
        public ASymmetricEncryption(RsaPublicKey key, EncryptionData password)
        {
            if (password.IsEmpty)
            {
                throw new ArgumentNullException(nameof(password), "A password must be provided!");
            }

            if (password.Text.Length > 32)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(password),
                    "RSA Encryption limits the password to 32 characters");
            }

            _publicKey = key ?? throw
                             new ArgumentNullException(
                                 nameof(key),
                                 "An RSA private key must be provided!");

            _password = Equals(password.EncodingToUse, Encoding.UTF8)
                    ? password
                    : new EncryptionData(password.Text, Encoding.UTF8);

            EncryptedPassword = new RsaEncryption().Encrypt(_password, _publicKey);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ASymmetricEncryption"/> class.
        /// </summary>
        /// <param name="key">
        /// The private key that will be used to decrypt the password used to decrypt the payload.
        /// </param>
        /// <exception cref="ArgumentNullException">An RSA private key must be provided!</exception>
        public ASymmetricEncryption(RsaPrivateKey key)
        {
            _privateKey = key ?? throw
                              new ArgumentNullException(
                                  nameof(key),
                                  "An RSA private key must be provided!");

            EncryptedPassword = new EncryptionData(new byte[256]);
        }

        /// <summary>
        /// Gets the encrypted password.
        /// </summary>
        public EncryptionData EncryptedPassword
        {
            get;
        }

        /// <summary>
        /// Decrypts the specified data using preset key and preset initialization vector
        /// </summary>
        /// <param name="encryptedData">The encrypted data.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The specified separator between password and payload wasn't found.
        /// </exception>
        /// <returns>the decrypted data.</returns>
        public EncryptionData Decrypt(EncryptionData encryptedData)
        {
            if (_privateKey == null)
            {
                throw new InvalidOperationException(
                    "In order to decrypt, you must provide the private key.");
            }

            var iv = new byte[16];
            EncryptionData payload;

            try
            {
                payload = ExtractParts(encryptedData);
            }
            catch (CryptographicException ex)
            {
                throw new CryptographicException("Wrong RSA Private Key Provided!", ex);
            }

            var password = _password.Bytes;

            Buffer.BlockCopy(password, 0, iv, 0, 16);

            var decrypt = new SymmetricEncryption(SymmetricEncryption.Provider.Rijndael)
            {
                Key = new EncryptionData(password),
                InitializationVector = new EncryptionData(iv)
            };

            var decryptedData = decrypt.Decrypt(payload);

            return decryptedData;
        }

        /// <summary>
        /// Decrypts the specified stream using preset key and preset initialization vector
        /// </summary>
        /// <param name="encryptedStream">The encrypted stream.</param>
        /// <returns>the decrypted data.</returns>
        public EncryptionData Decrypt(Stream encryptedStream)
        {
            if (_privateKey == null)
            {
                throw new InvalidOperationException(
                    "In order to decrypt, you must provide the private key.");
            }

            var b = new byte[encryptedStream.Length];

            _ = encryptedStream.Read(b, 0, (int)encryptedStream.Length);

            return Decrypt(new EncryptionData(b));
        }

        /// <summary>
        /// Encrypts the specified Data using preset key and preset initialization vector
        /// </summary>
        /// <param name="plainData">The data to encrypt.</param>
        /// <returns>the encrypted data.</returns>
        public EncryptionData Encrypt(EncryptionData plainData)
        {
            if (plainData.IsEmpty)
            {
                throw new ArgumentException("Invalid Encrypted Data!");
            }

            if (_publicKey == null)
            {
                throw new InvalidOperationException(
                    "In order to encrypt, you must provide the public key.");
            }

            var iv = new byte[16];

            var password = _password.Bytes;

            if (password.Length < 32)
            {
                password = password.Concat(new byte[32 - password.Length]).ToArray();
            }

            Buffer.BlockCopy(password, 0, iv, 0, 16);

            var encrypt = new SymmetricEncryption(SymmetricEncryption.Provider.Rijndael)
            {
                Key = new EncryptionData(password),
                InitializationVector = new EncryptionData(iv)
            };

            var encryptedData = encrypt.Encrypt(plainData);

            return CombineEncrypted(encryptedData);
        }

        /// <summary>
        /// Encrypts the stream to memory using provided key and provided initialization vector
        /// </summary>
        /// <param name="plainStream">The stream to preform the cryptographic function on.</param>
        /// <returns>the encrypted data.</returns>
        public EncryptionData Encrypt(Stream plainStream)
        {
            if (!plainStream.CanRead)
            {
                throw new ArgumentException("Can not read from the stream!");
            }

            if (_publicKey == null)
            {
                throw new InvalidOperationException(
                    "In order to encrypt, you must provide the public key.");
            }

            var iv = new byte[16];

            var password = _password.Bytes;

            Buffer.BlockCopy(password, 0, iv, 0, 16);

            var encrypt = new SymmetricEncryption(SymmetricEncryption.Provider.Rijndael)
            {
                Key = new EncryptionData(password),
                InitializationVector = new EncryptionData(iv)
            };

            var encryptedData = encrypt.Encrypt(plainStream);

            return CombineEncrypted(encryptedData);
        }

        private EncryptionData CombineEncrypted(EncryptionData payload)
        {
            var passwordLength = EncryptedPassword.Bytes.Length;
            var payloadLength = payload.Bytes.Length;
            var b = new byte[passwordLength + payloadLength];

            Buffer.BlockCopy(EncryptedPassword.Bytes, 0, b, 0, passwordLength);
            Buffer.BlockCopy(payload.Bytes, 0, b, passwordLength, payloadLength);

            return new EncryptionData(b);
        }

        private EncryptionData ExtractParts(EncryptionData encryptedData)
        {
            if (encryptedData.IsEmpty)
            {
                throw new ArgumentException("Encrypted data is empty!");
            }

            if (encryptedData.Bytes.Length < 257)
            {
                throw new ArgumentException("Improper Encryption Data!");
            }

            var password = new Byte[256];

            Buffer.BlockCopy(encryptedData.Bytes, 0, password, 0, 256);

            var payloadLength = encryptedData.Bytes.Length - 256;
            var payload = new byte[payloadLength];

            Buffer.BlockCopy(encryptedData.Bytes, 256, payload, 0, payloadLength);

            _password = new RsaEncryption().Decrypt(new EncryptionData(password), _privateKey);

            return new EncryptionData(payload);
        }
    }
}
