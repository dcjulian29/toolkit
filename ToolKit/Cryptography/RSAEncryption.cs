using System;
using System.Security.Cryptography;
using System.Text;
using Common.Logging;

namespace ToolKit.Cryptography
{
    /// <summary>
    /// Asymmetric Encryption is a form of Encryption where keys come in pairs. What one key
    /// encrypts, only the other can decrypt. Frequently (but not necessarily), the keys are
    /// interchangeable, in the sense that if key A encrypts a message, then B can decrypt it, and if
    /// key B encrypts a message, then key A can decrypt it. Asymmetric Encryption is also known as
    /// Public Key Cryptography, since users typically create a matching key pair, and make one
    /// public while keeping the other secret.
    /// </summary>
    /// <remarks>
    /// Adapted from code originally written by Jeff Atwood. This code had no explicit license
    /// attached to it. If licensing is a concern, you should contact the original author.
    /// </remarks>
    public class RsaEncryption
    {
        private static ILog _log = LogManager.GetLogger<RsaEncryption>();

        /// <summary>
        /// Initializes a new instance of the <see cref="RsaEncryption"/> class.
        /// </summary>
        /// <param name="keySize">Size of the key in bits.</param>
        public RsaEncryption(int keySize)
        {
            KeySizeBits = keySize;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RsaEncryption"/> class.
        /// </summary>
        public RsaEncryption()
        {
        }

        /// <summary>
        /// Gets the default private key as stored in the *.config file
        /// </summary>
        public RsaPrivateKey DefaultPrivateKey => RsaPrivateKey.LoadFromEnvironment();

        /// <summary>
        /// Gets the default public key as stored in the *.config file.
        /// </summary>
        public RsaPublicKey DefaultPublicKey => RsaPublicKey.LoadFromEnvironment();

        /// <summary>
        /// Gets or sets the name of the key container used to store this key on disk; this is an
        /// unavoidable side effect of the underlying Microsoft CryptoAPI.
        /// </summary>
        public string KeyContainerName { get; set; } = "ToolKit.AsymmetricEncryption.DefaultContainerName";

        /// <summary>
        /// Gets the current key size, in bits.
        /// </summary>
        public int KeySizeBits { get; } = 1024;

        /// <summary>
        /// Gets the maximum supported key size, in bits.
        /// </summary>
        public int KeySizeMaxBits
        {
            get
            {
                var rsa = GetRsaProvider();
                var keyMaxSize = rsa.LegalKeySizes[0].MaxSize;
                rsa.Clear();

                return keyMaxSize;
            }
        }

        /// <summary>
        /// Gets the minimum supported key size, in bits.
        /// </summary>
        public int KeySizeMinBits
        {
            get
            {
                var rsa = GetRsaProvider();
                var keyMinSize = rsa.LegalKeySizes[0].MinSize;
                rsa.Clear();

                return keyMinSize;
            }
        }

        /// <summary>
        /// Gets the valid key step sizes, in bits.
        /// </summary>
        public int KeySizeStepBits
        {
            get
            {
                var rsa = GetRsaProvider();
                var keyStepBits = rsa.LegalKeySizes[0].SkipSize;
                rsa.Clear();

                return keyStepBits;
            }
        }

        /// <summary>
        /// Decrypts data using the default private key
        /// </summary>
        /// <param name="encryptedData">The data to be decrypted.</param>
        /// <returns>The decrypted data.</returns>
        public EncryptionData Decrypt(EncryptionData encryptedData)
        {
            return Decrypt(encryptedData, RsaPrivateKey.LoadFromEnvironment());
        }

        /// <summary>
        /// Decrypts data using the provided private key
        /// </summary>
        /// <param name="encryptedData">The data to be decrypted.</param>
        /// <param name="privateKey">The private key.</param>
        /// <returns>The decrypted data.</returns>
        public EncryptionData Decrypt(EncryptionData encryptedData, RsaPrivateKey privateKey)
        {
            var rsa = GetRsaProvider();
            rsa.ImportParameters(privateKey.ToParameters());

            // Be aware the RSACryptoServiceProvider reverses the order of encrypted bytes after
            // encryption and before decryption. In order to provide compatibility with other
            // providers, we reverse the order of the bytes to match what other providers output.
            Array.Reverse(encryptedData.Bytes);

            var decrypted = new EncryptionData(rsa.Decrypt(encryptedData.Bytes, false));
            rsa.Clear();

            return decrypted;
        }

        /// <summary>
        /// Encrypts data using the default public key
        /// </summary>
        /// <param name="data">The data to be encrypted..</param>
        /// <returns>The encrypted data.</returns>
        public EncryptionData Encrypt(EncryptionData data)
        {
            return Encrypt(data, DefaultPublicKey);
        }

        /// <summary>
        /// Encrypts data using the provided public key
        /// </summary>
        /// <param name="data">The data to be encrypted..</param>
        /// <param name="publicKey">The public key.</param>
        /// <returns>The encrypted data.</returns>
        public EncryptionData Encrypt(EncryptionData data, RsaPublicKey publicKey)
        {
            var rsa = GetRsaProvider();
            rsa.ImportParameters(publicKey.ToParameters());

            try
            {
                var encryptedBytes = rsa.Encrypt(data.Bytes, false);

                // Be aware the RSACryptoServiceProvider reverses the order of encrypted bytes after
                // encryption and before decryption. In order to provide compatibility with other
                // providers, we reverse the order of the bytes to match what other providers output.
                Array.Reverse(encryptedBytes);

                return new EncryptionData(encryptedBytes);
            }
            catch (CryptographicException ex)
            {
                _log.Error(m => m(ex.Message), ex);

                var sb = new StringBuilder();

                sb.Append("Your data is too large; RSA implementation in .Net is designed to encrypt ");
                sb.Append("relatively small amounts of data. The exact byte limit depends ");
                sb.Append("on the key size. To encrypt more data, use symmetric encryption ");
                sb.Append("and then encrypt that symmetric key with ");
                sb.Append("asymmetric encryption.");

                _log.Warn(sb.ToString());

                throw new CryptographicException(sb.ToString(), ex);
            }
            catch (Exception ex)
            {
                _log.Error(m => m(ex.ToString()), ex);

                throw;
            }
            finally
            {
                rsa.Clear();
            }
        }

        /// <summary>
        /// Generates a new public/private key pair as objects
        /// </summary>
        /// <param name="publicKey">The public key.</param>
        /// <param name="privateKey">The private key.</param>
        public void GenerateNewKeyset(ref RsaPublicKey publicKey, ref RsaPrivateKey privateKey)
        {
            if (publicKey == null)
            {
                throw new ArgumentNullException(nameof(publicKey));
            }

            if (privateKey == null)
            {
                throw new ArgumentNullException(nameof(privateKey));
            }

            var rsa = GetRsaProvider();

            var publicKeyXml = rsa.ToXmlString(false);
            var privateKeyXml = rsa.ToXmlString(true);

            rsa.Clear();

            publicKey = new RsaPublicKey(publicKeyXml);
            privateKey = new RsaPrivateKey(privateKeyXml);
        }

        /// <summary>
        /// Signs data using the provided private key
        /// </summary>
        /// <param name="dataToSign">The data to be signed.</param>
        /// <param name="privateKey">The private key.</param>
        /// <returns>The signature of the data as signed by the private key.</returns>
        public EncryptionData Sign(EncryptionData dataToSign, RsaPrivateKey privateKey)
        {
            var rsa = GetRsaProvider();
            rsa.ImportParameters(privateKey.ToParameters());

            var sig = rsa.SignData(dataToSign.Bytes, new SHA256Managed());
            rsa.Clear();

            return new EncryptionData(sig);
        }

        /// <summary>
        /// Verifies that the provided data has not changed since it was signed.
        /// </summary>
        /// <param name="data">The data to be validated.</param>
        /// <param name="signature">The signature to use to verify data.</param>
        /// <param name="publicKey">The public key.</param>
        /// <returns>
        /// <c>true</c> if the provided data has not changed since it was signed, otherwise <c>false</c>.
        /// </returns>
        public bool Verify(EncryptionData data, EncryptionData signature, RsaPublicKey publicKey)
        {
            var rsa = GetRsaProvider();
            rsa.ImportParameters(publicKey.ToParameters());

            var valid = rsa.VerifyData(data.Bytes, new SHA256Managed(), signature.Bytes);
            rsa.Clear();

            return valid;
        }

        private RSACryptoServiceProvider GetRsaProvider()
        {
            var csp = new CspParameters
            {
                KeyContainerName = KeyContainerName
            };

            var rsa = new RSACryptoServiceProvider(KeySizeBits, csp)
            {
                PersistKeyInCsp = false
            };

            RSACryptoServiceProvider.UseMachineKeyStore = true;

            return rsa;
        }
    }
}
