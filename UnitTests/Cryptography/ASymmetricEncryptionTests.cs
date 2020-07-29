using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using ToolKit.Cryptography;
using Xunit;

namespace UnitTests.Cryptography
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1600:ElementsMustBeDocumented",
        Justification = "Test Suites do not need XML Documentation.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "SonarCube.UnusedLocalVariables",
        "S1481",
        Justification = "Unused local variables should be removed")]
    public class ASymmetricEncryptionTests
    {
        private readonly RsaPrivateKey _privateKey;
        private readonly RsaPublicKey _publicKey;
        private readonly EncryptionData _targetData;
        private readonly string _targetString;

        public ASymmetricEncryptionTests()
        {
            _targetString = "Did you know that some of the most famous inventions would "
                            + "never have been made if the inventors had stopped at their "
                            + "first failure? For instance, Thomas Edison, inventor of "
                            + "the light bulb had 1,000 failed attempts at creating one "
                            + "light bulb that worked properly. - Vee Freir";

            _targetData = new EncryptionData(_targetString);

            _privateKey = RsaPrivateKey.LoadFromCertificateFile(
                $@"{Directory.GetCurrentDirectory()}\ASymmetricEncryption.pfx",
                "password");

            _publicKey = RsaPublicKey.LoadFromCertificateFile(
                $@"{Directory.GetCurrentDirectory()}\ASymmetricEncryption.pfx",
                "password");
        }

        [Fact]
        public void Constructor_Should_InitializeProperly_When_RSAPrivateKeyIsProvided()
        {
            // Arrange & Act
            var e1 = new ASymmetricEncryption(_privateKey);

            // Assert
            Assert.NotNull(e1);
        }

        [Fact]
        public void Constructor_Should_InitializeProperly_When_RSAPublicKeyAndPasswordProvided()
        {
            // Arrange
            var password = new EncryptionData("password");

            // Act
            var e1 = new ASymmetricEncryption(_publicKey, password);

            // Assert
            Assert.NotNull(e1);
        }

        [Fact]
        public void Constructor_Should_InitializeProperly_When_RSAPublicKeyIsProvided()
        {
            // Arrange & Act
            var e1 = new ASymmetricEncryption(_publicKey);

            // Assert
            Assert.NotNull(e1);
        }

        [Fact]
        public void Constructor_Should_ThrowArgumentOutOfRangeException_When_PasswordGreaterThan32Characters()
        {
            // Arrange
            var password = new EncryptionData("123456789012345678901234567890123");

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(
                () =>
                {
                    var e1 = new ASymmetricEncryption(_publicKey, password);
                });
        }

        [Fact]
        public void Constructor_Should_ThrowException_When_RSAPrivateKeyIsNotProvided()
        {
            // Arrange
            RsaPrivateKey key = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(
                () =>
                {
                    var e1 = new ASymmetricEncryption(key);
                });
        }

        [Fact]
        public void Constructor_Should_ThrowExceptionWhenNoRSAPublicKeyIsProvidedButPasswordIs()
        {
            // Arrange
            RsaPublicKey key = null;
            var password = new EncryptionData("password");

            // Act & Assert
            Assert.Throws<ArgumentNullException>(
                () =>
                {
                    var e1 = new ASymmetricEncryption(key, password);
                });
        }

        [Fact]
        public void Constructor_Should_ThrowExceptionWhenRSAPublicKeyIsNotProvided()
        {
            // Arrange
            RsaPublicKey key = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                var e1 = new ASymmetricEncryption(key);
            });
        }

        [Fact]
        public void Constructor_Should_ThrowExceptionWhenRSAPublicKeyIsProvidedButNoPassword()
        {
            // Arrange
            var password = new EncryptionData();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(
                () =>
                {
                    var e1 = new ASymmetricEncryption(_publicKey, password);
                });
        }

        [Fact]
        public void Decrypt_Should_ReturnExpectedResult_When_ProvidedCorrectPrivateKey()
        {
            // Arrange
            var e1 = new ASymmetricEncryption(_publicKey);
            var e2 = new ASymmetricEncryption(_privateKey);

            // Act
            var encrypted = e1.Encrypt(_targetData);
            var decrypted = e2.Decrypt(encrypted);

            // Assert
            Assert.True(_targetData.Text == decrypted.Text);
        }

        [Fact]
        public void Decrypt_Should_ReturnExpectedResult_When_ProvidedCorrectPrivateKeyAndUsingStream()
        {
            // Arrange
            var e1 = new ASymmetricEncryption(_publicKey);
            var e2 = new ASymmetricEncryption(_privateKey);

            // Act
            var encrypted = e1.Encrypt(_targetData);
            EncryptionData decrypted;

            using (var stream = new MemoryStream(encrypted.Bytes))
            {
                decrypted = e2.Decrypt(stream);
            }

            // Assert
            Assert.True(_targetData.Text == decrypted.Text);
        }

        [Fact]
        public void Decrypt_Should_ThrowException_When_ProvidedWrongPrivateKey()
        {
            // Arrange
            var wrongKey = RsaPrivateKey.LoadFromCertificateFile(
                $@"{Directory.GetCurrentDirectory()}\ASymmetricEncryptionWrong.pfx",
                "password");

            var e1 = new ASymmetricEncryption(_publicKey);
            var e2 = new ASymmetricEncryption(wrongKey);

            var encrypted = e1.Encrypt(_targetData);

            // Act & Assert
            Assert.Throws<CryptographicException>(() =>
            {
                var decrypted = e2.Decrypt(encrypted);
            });
        }

        [Fact]
        public void Decrypt_Should_ThrowException_When_NoEncryptedDataProvided()
        {
            // Arrange
            var e2 = new ASymmetricEncryption(_privateKey);

            var encrypted = new EncryptionData();

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
            {
                var decrypted = e2.Decrypt(encrypted);
            });
        }

        [Fact]
        public void Decrypt_Should_ThrowException_When_ImproperEncryptedDataProvided()
        {
            // Arrange
            var e2 = new ASymmetricEncryption(_privateKey);

            var encrypted = new EncryptionData(new byte[100]);

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
            {
                var decrypted = e2.Decrypt(encrypted);
            });
        }

        [Fact]
        public void Decrypt_Should_ThrowException_When_RSAPublicKeyIsProvided()
        {
            // Arrange
            var e1 = new ASymmetricEncryption(_publicKey);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(
                () =>
                {
                    var encrypted = e1.Decrypt(_targetData);
                });
        }

        [Fact]
        public void Decrypt_Should_ThrowException_When_RSAPublicKeyIsProvided_When_UsingStream()
        {
            // Arrange
            var e1 = new ASymmetricEncryption(_publicKey);
            EncryptionData encrypted;

            // Act & Assert
            Assert.Throws<InvalidOperationException>(
                () =>
                {
                    using (var sr = new StreamReader("sample.doc"))
                    {
                        encrypted = e1.Decrypt(sr.BaseStream);
                    }
                });
        }

        [Fact]
        public void Encrypt_Should_ReturnA32BytePassword()
        {
            // Arrange
            var encrypt = new ASymmetricEncryption(_publicKey);
            var encryptedPassword = new EncryptionData(new byte[256]);
            var rsa = new RsaEncryption();

            // Act
            var encryptedBytes = encrypt.Encrypt(_targetData);

            Buffer.BlockCopy(encryptedBytes.Bytes, 0, encryptedPassword.Bytes, 0, 256);

            var password = rsa.Decrypt(encryptedPassword, _privateKey);

            // Assert
            Assert.True(password.Bytes.Length == 32);
        }

        [Fact]
        public void Encrypt_Should_ReturnEncryptedData()
        {
            // Arrange
            var e1 = new ASymmetricEncryption(_publicKey);

            // Act
            var encrypted = e1.Encrypt(_targetData);

            // Assert
            Assert.False(encrypted.IsEmpty);
        }

        [Fact]
        public void Encrypt_Should_ReturnEncryptedData_When_UsingStream()
        {
            // Arrange
            var e1 = new ASymmetricEncryption(_publicKey);
            EncryptionData encrypted;

            // Act
            using (var sr = new StreamReader("sample.doc"))
            {
                encrypted = e1.Encrypt(sr.BaseStream);
            }

            // Assert
            Assert.False(encrypted.IsEmpty);
        }

        [Fact]
        public void Encrypt_Should_ReturnSamePassword_When_ExplicitPasswordProvidedAndPasswordLenghtLessThan16()
        {
            // Arrange
            var password = new EncryptionData("password");
            var encrypt = new ASymmetricEncryption(_publicKey, password);
            var encryptedPassword = new EncryptionData(new byte[256]);

            // Act
            var encryptedBytes = encrypt.Encrypt(_targetData);

            Buffer.BlockCopy(encryptedBytes.Bytes, 0, encryptedPassword.Bytes, 0, 256);

            var decryptedPassword = new RsaEncryption().Decrypt(encryptedPassword, _privateKey);

            // Assert
            Assert.True(decryptedPassword.Text == password.Text);
        }

        [Fact]
        public void Encrypt_Should_ReturnSamePassword_When_ExplicitPasswordProvidedAndPasswordLengthGreaterThan16()
        {
            // Arrange
            var password = new EncryptionData("A really long simple password");
            var encrypt = new ASymmetricEncryption(_publicKey, password);
            var encryptedPassword = new EncryptionData(new byte[256]);

            // Act
            var encryptedBytes = encrypt.Encrypt(_targetData);

            Buffer.BlockCopy(encryptedBytes.Bytes, 0, encryptedPassword.Bytes, 0, 256);

            var decryptedPassword = new RsaEncryption().Decrypt(encryptedPassword, _privateKey);

            // Assert
            Assert.True(decryptedPassword.Text == password.Text);
        }

        [Fact]
        public void Encrypt_Should_ReturnSamePassword_When_ExplicitPasswordProvidedAndPasswordWithNonUTF8Password()
        {
            // Arrange
            var password = new EncryptionData("A really long simple password", System.Text.Encoding.ASCII);
            var encrypt = new ASymmetricEncryption(_publicKey, password);
            var encryptedPassword = new EncryptionData(new byte[256]);

            // Act
            var encryptedBytes = encrypt.Encrypt(_targetData);

            Buffer.BlockCopy(encryptedBytes.Bytes, 0, encryptedPassword.Bytes, 0, 256);

            var decryptedPassword = new RsaEncryption().Decrypt(encryptedPassword, _privateKey);

            // Assert
            Assert.True(decryptedPassword.Text == password.Text);
        }

        [Fact]
        public void Encrypt_Should_ReturnTheSamePasswordAsEncryptedPassword()
        {
            // Arrange
            var encrypt = new ASymmetricEncryption(_publicKey);
            var encryptedPassword = new byte[256];
            var payloadPassword = new byte[256];

            Buffer.BlockCopy(encrypt.EncryptedPassword.Bytes, 0, encryptedPassword, 0, 256);

            // Act
            var encryptedBytes = encrypt.Encrypt(_targetData);

            Buffer.BlockCopy(encryptedBytes.Bytes, 0, payloadPassword, 0, 256);

            // Assert
            Assert.True(payloadPassword.SequenceEqual(encryptedPassword));
        }

        [Fact]
        public void Encrypt_Should_ThrowException_When_RSAPrivateKeyIsProvided()
        {
            // Arrange
            var e1 = new ASymmetricEncryption(_privateKey);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(
                () =>
                {
                    var encrypted = e1.Encrypt(_targetData);
                });
        }

        [Fact]
        public void Encrypt_Should_ThrowException_When_NoDataForPayloadProvided()
        {
            // Arrange
            var empty = new EncryptionData();
            var e1 = new ASymmetricEncryption(_privateKey);

            // Act & Assert
            Assert.Throws<ArgumentException>(
                () =>
                {
                    var encrypted = e1.Encrypt(empty);
                });
        }

        [Fact]
        public void Encrypt_Should_ThrowException_When_StreamIsNotReadable()
        {
            // Arrange
            var fileName = Guid.NewGuid().ToString();
            var e1 = new ASymmetricEncryption(_privateKey);
            var stream = new FileStream(fileName, FileMode.CreateNew, FileAccess.Write);

            // Act & Assert
            Assert.Throws<ArgumentException>(
                () =>
                {
                    var encrypted = e1.Encrypt(stream);
                });

            stream.Close();
            stream.Dispose();
        }

        [Fact]
        public void Encrypt_Should_ThrowException_When_RSAPrivateKeyIsProvided_When_UsingStream()
        {
            // Arrange
            var e1 = new ASymmetricEncryption(_privateKey);
            EncryptionData encrypted;

            // Act & Assert
            Assert.Throws<InvalidOperationException>(
                () =>
                {
                    using (var sr = new StreamReader("sample.doc"))
                    {
                        encrypted = e1.Encrypt(sr.BaseStream);
                    }
                });
        }

        [Fact]
        public void EncryptedPassword_Should_ReturnA256ByteEncryptedPassword()
        {
            // Arrange
            var encrypt = new ASymmetricEncryption(_publicKey);

            // Act
            var encryptedPassword = encrypt.EncryptedPassword;

            // Assert
            Assert.True(encryptedPassword.Bytes.Length == 256);
        }

        [Fact]
        public void Password_Should_ReturnValue_When_ConstructorIsCalled()
        {
            // Arrange & Act
            var encrypt = new ASymmetricEncryption(_publicKey);

            // Assert
            Assert.True(encrypt.EncryptedPassword.ToString().Length > 0);
        }
    }
}
