using System;
using System.IO;
using System.Reflection;
using ToolKit.Cryptography;
using Xunit;

namespace UnitTests.Cryptography
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1600:ElementsMustBeDocumented",
        Justification = "Test Suites do not need XML Documentation.")]
    public class SymmetricEncryptionTests
    {
        private static readonly string _assemblyPath =
            Path.GetDirectoryName(Assembly.GetAssembly(typeof(SymmetricEncryptionTests)).Location)
            + Path.DirectorySeparatorChar;

        private readonly EncryptionData _targetData;

        private readonly string _targetString;

        public SymmetricEncryptionTests()
        {
            _targetString = "The instinct of nearly all societies is to lock up anybody who is truly free. " +
                            "First, society begins by trying to beat you up. If this fails, they try to poison you. " +
                            "If this fails too, they finish by loading honors on your head." +
                            " - Jean Cocteau (1889-1963)";
            _targetData = new EncryptionData(_targetString);
        }

        [Fact]
        public void Constructor_Should_ThrowExceptionWhenInvalidProvider()
        {
            // Arrange
            var provider = (SymmetricEncryption.Provider)8;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new SymmetricEncryption(provider));
        }

        [Fact]
        public void Decrypt_Should_ReturnExpectedResult_When_ProvidedKeyAndInitializationVector()
        {
            // Arrange
            var e1 = new SymmetricEncryption(SymmetricEncryption.Provider.Rijndael);
            var e2 = new SymmetricEncryption(SymmetricEncryption.Provider.Rijndael);
            var iv = new EncryptionData("vector");
            var key = new EncryptionData("privatekey");

            // Act
            var encrypted = e1.Encrypt(new EncryptionData(_targetString), key, iv);
            var decrypted = e2.Decrypt(encrypted, key, iv);

            // -- the data will be padded to the encryption block size, so we need to trim it back down.
            var actual = decrypted.Text.Substring(0, _targetData.Bytes.Length);

            // Assert
            Assert.Equal(_targetString, actual);
        }

        [Fact]
        public void Decrypt_Should_ReturnWrongData_When_CorrectInitializationVectorIsNotProvided()
        {
            // Arrange
            var e1 = new SymmetricEncryption(SymmetricEncryption.Provider.Rijndael);
            var e2 = new SymmetricEncryption(SymmetricEncryption.Provider.Rijndael)
            {
                Key = e1.Key
            };

            // Act
            var encrypted = e1.Encrypt(_targetData);
            var actual = e2.Decrypt(encrypted);

            // Assert
            Assert.NotEqual(_targetData.Hex, actual.Hex);
        }

        [Fact]
        public void Decrypt_Should_ReturnWrongData_When_ProvidedWrongKey()
        {
            // Arrange
            var e1 = new SymmetricEncryption(SymmetricEncryption.Provider.Rijndael);
            var e2 = new SymmetricEncryption(SymmetricEncryption.Provider.Rijndael)
            {
                InitializationVector = e1.InitializationVector
            };

            // Act
            var key = new EncryptionData("SecretKey");
            var wrong = new EncryptionData("wrongkey");
            var encrypted = e1.Encrypt(_targetData, key);
            var actual = e2.Decrypt(encrypted, wrong);

            // Assert
            Assert.NotEqual(_targetData.Hex, actual.Hex);
        }

        [Fact]
        public void Encrypt_Should_UseRandomInitializationVector_When_NoInitializationVectorProvided()
        {
            // Arrange
            var e1 = new SymmetricEncryption(SymmetricEncryption.Provider.Rijndael);
            var expected = e1.InitializationVector;

            // Act
            e1.Encrypt(_targetData);

            // Assert
            Assert.Equal(expected, e1.InitializationVector);
        }

        [Fact]
        public void Encrypt_Should_UseRandomKey_When_NoKeyProvided()
        {
            // Arrange
            var e1 = new SymmetricEncryption(SymmetricEncryption.Provider.Rijndael);
            var expected = e1.Key;

            // Act
            e1.Encrypt(_targetData);

            // Assert
            Assert.Equal(expected, e1.Key);
        }

        [Fact]
        public void EncryptWithRijndael_Should_ReturnExpectedResult_When_ProvidedExplicitKey()
        {
            // Arrange
            var e1 = new SymmetricEncryption(SymmetricEncryption.Provider.Rijndael);
            var e2 = new SymmetricEncryption(SymmetricEncryption.Provider.Rijndael)
            {
                InitializationVector = e1.InitializationVector
            };

            var key = new EncryptionData("SecretKey");

            // Act
            var encrypted = e1.Encrypt(_targetData, key);
            var decrypted = e2.Decrypt(encrypted, key);

            // -- the data will be padded to the encryption block size, so we need to trim it back down.
            var actual = decrypted.Text.Substring(0, _targetData.Bytes.Length);

            // Assert
            Assert.Equal(_targetString, actual);
        }

        [Fact]
        public void EncryptWithRijndael_Should_ReturnExpectedResult_When_ProvidedKeyAndinitializationVector()
        {
            // Arrange
            var key = new EncryptionData("SecretKey");
            var vector = new EncryptionData("InitializationVector");

            var e1 = new SymmetricEncryption(SymmetricEncryption.Provider.Rijndael)
            {
                InitializationVector = vector
            };

            var e2 = new SymmetricEncryption(SymmetricEncryption.Provider.Rijndael)
            {
                InitializationVector = vector
            };

            // Act
            var encrypted = e1.Encrypt(_targetData, key);
            var decrypted = e2.Decrypt(encrypted, key);

            // -- the data will be padded to the encryption block size, so we need to trim it back down.
            var actual = decrypted.Text.Substring(0, _targetData.Bytes.Length);

            // Assert
            Assert.Equal(_targetString, actual);
        }

        [Fact]
        public void EncryptWithRijndael_Should_ReturnExpectedResult_When_UsingAutoGeneratedKeys()
        {
            // Arrange
            var e1 = new SymmetricEncryption(SymmetricEncryption.Provider.Rijndael);
            var e2 = new SymmetricEncryption(SymmetricEncryption.Provider.Rijndael)
            {
                InitializationVector = e1.InitializationVector
            };

            // Act
            var encrypted = e1.Encrypt(new EncryptionData(_targetString));
            var decrypted = e2.Decrypt(encrypted, e1.Key);

            // -- the data will be padded to the encryption block size, so we need to trim it back down.
            var actual = decrypted.Text.Substring(0, _targetData.Bytes.Length);

            // Assert
            Assert.Equal(_targetString, actual);
        }

        [Fact]
        public void EncryptWithRijndael_Should_ReturnExpectedResult_When_UsingStreamAndExplicitKey()
        {
            // Arrange
            var expected = "4F32AB797F0FCC782AAC0B4F4E5B1693";
            var key = new EncryptionData("0nTheDownLow1");

            var e1 = new SymmetricEncryption(SymmetricEncryption.Provider.Rijndael);
            var e2 = new SymmetricEncryption(SymmetricEncryption.Provider.Rijndael)
            {
                InitializationVector = e1.InitializationVector
            };

            EncryptionData encrypted;
            EncryptionData decrypted;

            // Act
            // -- Encrypt to memory
            using (var sr = new StreamReader($"{_assemblyPath}sample.doc"))
            {
                encrypted = e1.Encrypt(sr.BaseStream, key);
            }

            // -- Write encrypted date to new binary file.
            using (var bw = new BinaryWriter(new StreamWriter($"{_assemblyPath}encrypted.dat").BaseStream))
            {
                bw.Write(encrypted.Bytes);
                bw.Close();
            }

            // -- Decrypt the binary file
            using (var sr = new StreamReader($"{_assemblyPath}encrypted.dat"))
            {
                decrypted = e2.Decrypt(sr.BaseStream, key);
            }

            File.Delete($"{_assemblyPath}encrypted.dat");

            var h = new Hash(Hash.Provider.MD5);
            var actual = h.Calculate(decrypted).Hex;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void EncryptWithRijndael_Should_ReturnExpectedResult_When_UsingStreamAndKeyAndInitializationVector()
        {
            // Arrange
            var expected = "4F32AB797F0FCC782AAC0B4F4E5B1693";
            var key = new EncryptionData("0nTheDownLow1");
            var iv = new EncryptionData("vector");

            var e1 = new SymmetricEncryption(SymmetricEncryption.Provider.Rijndael);
            var e2 = new SymmetricEncryption(SymmetricEncryption.Provider.Rijndael);

            EncryptionData encrypted;
            EncryptionData decrypted;

            // Act
            // -- Encrypt to memory
            using (var sr = new StreamReader($"{_assemblyPath}sample.doc"))
            {
                encrypted = e1.Encrypt(sr.BaseStream, key, iv);
            }

            // -- Write encrypted date to new binary file.
            using (var bw = new BinaryWriter(new StreamWriter($"{_assemblyPath}encrypted.dat").BaseStream))
            {
                bw.Write(encrypted.Bytes);
                bw.Close();
            }

            // -- Decrypt the binary file
            using (var sr = new StreamReader($"{_assemblyPath}encrypted.dat"))
            {
                decrypted = e2.Decrypt(sr.BaseStream, key, iv);
            }

            File.Delete($"{_assemblyPath}encrypted.dat");

            var h = new Hash(Hash.Provider.MD5);
            var actual = h.Calculate(decrypted).Hex;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void EncryptWithTripleDES_Should_ReturnExpectedResult_When_ProvidedKeysAndInitializationVector()
        {
            // Arrange
            var key = new EncryptionData("SecretKey");
            var vector = new EncryptionData("InitializationVector");

            var e1 = new SymmetricEncryption(SymmetricEncryption.Provider.TripleDES)
            {
                InitializationVector = vector
            };

            var e2 = new SymmetricEncryption(SymmetricEncryption.Provider.TripleDES)
            {
                InitializationVector = vector
            };

            // Act
            var encrypted = e1.Encrypt(_targetData, key);
            var decrypted = e2.Decrypt(encrypted, key);

            // -- the data will be padded to the encryption block size, so we need to trim it back down.
            var actual = decrypted.Text.Substring(0, _targetData.Bytes.Length);

            // Assert
            Assert.Equal(_targetString, actual);
        }

        [Fact]
        public void EncryptWithTripleDES_Should_ReturnExpectedResult_When_UsingAutoGeneratedKeys()
        {
            // Arrange
            var e1 = new SymmetricEncryption(SymmetricEncryption.Provider.TripleDES);
            var e2 = new SymmetricEncryption(SymmetricEncryption.Provider.TripleDES)
            {
                InitializationVector = e1.InitializationVector
            };

            // Act
            var encrypted = e1.Encrypt(new EncryptionData(_targetString));
            var decrypted = e2.Decrypt(encrypted, e1.Key);

            // -- the data will be padded to the encryption block size, so we need to trim it back down.
            var actual = decrypted.Text.Substring(0, _targetData.Bytes.Length);

            // Assert
            Assert.Equal(_targetString, actual);
        }

        [Fact]
        public void EncryptWithTripleDES_Should_ReturnExpectedResult_When_UsingStream()
        {
            // Arrange
            var expected = "B5293E7C53966F4388D6B420A27CEBD7";

            var e1 = new SymmetricEncryption(SymmetricEncryption.Provider.TripleDES)
            {
                Key = new EncryptionData("Password, Yo!"),
                InitializationVector = new EncryptionData("MyInitializationVector")
            };

            var e2 = new SymmetricEncryption(SymmetricEncryption.Provider.TripleDES)
            {
                Key = new EncryptionData("Password, Yo!"),
                InitializationVector = new EncryptionData("MyInitializationVector")
            };

            EncryptionData encrypted;
            EncryptionData decrypted;

            // Act
            // -- Encrypt to memory
            using (var sr = new StreamReader($"{_assemblyPath}gettysburg.txt"))
            {
                encrypted = e1.Encrypt(sr.BaseStream);
            }

            // -- Write encrypted date to new binary file.
            using (var bw = new BinaryWriter(new StreamWriter($"{_assemblyPath}encrypted.dat").BaseStream))
            {
                bw.Write(encrypted.Bytes);
                bw.Close();
            }

            // -- Decrypt the binary file
            using (var sr = new StreamReader($"{_assemblyPath}encrypted.dat"))
            {
                decrypted = e2.Decrypt(sr.BaseStream);
            }

            File.Delete($"{_assemblyPath}encrypted.dat");

            var h = new Hash(Hash.Provider.MD5);
            var actual = h.Calculate(decrypted).Hex;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void InitializationVector_Should_ReturnExpected_When_VectorProvided()
        {
            // Arrange
            var expected = new EncryptionData("UnitTest");
            var e1 = new SymmetricEncryption(SymmetricEncryption.Provider.Rijndael)
            {
                InitializationVector = expected
            };

            // Act
            var actual = e1.InitializationVector;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void KeySizeBits_Should_ReturnExpectedResult_When_KeySizeBytesIsSet()
        {
            // Arrange
            var expected = 128;
            var e1 = new SymmetricEncryption(SymmetricEncryption.Provider.Rijndael)
            {
                KeySizeBytes = 16
            };

            // Act
            var actual = e1.KeySizeBits;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void KeySizeBytes_Should_ReturnExpectedResult_When_KeySizeBitsIsSet()
        {
            // Arrange
            var expected = 16;
            var e1 = new SymmetricEncryption(SymmetricEncryption.Provider.Rijndael)
            {
                KeySizeBits = 128
            };

            // Act
            var actual = e1.KeySizeBytes;

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
