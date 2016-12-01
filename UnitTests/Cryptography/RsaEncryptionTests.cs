﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Security.Cryptography;
using ToolKit.Cryptography;
using Xunit;

namespace UnitTests.Cryptography
{
    [SuppressMessage(
         "StyleCop.CSharp.DocumentationRules",
         "SA1600:ElementsMustBeDocumented",
         Justification = "Test Suites do not need XML Documentation.")]
    public class RsaEncryptionTests
    {
        private static string _targetString =
            "The instinct of nearly all societies is to lock up anybody who is truly free. "
            + "First, society begins by trying to beat you up. If this fails, they try to poison you. "
            + "If this fails too, they finish by loading honors on your head."
            + " - Jean Cocteau (1889-1963)";

        private static string secret = SHA256Hash.Create().Compute(_targetString);

        [Fact]
        public void Decrypt_Should_ReturnExpectedResult_When_KeyIsStoredInCertificate()
        {
            // Arrange
            var cert = Directory.GetCurrentDirectory() + @"\RsaEncrypt";

            var publicKey = RsaPublicKey.LoadFromCertificateFile(cert + ".cer");
            var privateKey = RsaPrivateKey.LoadFromCertificateFile(cert + ".pfx", "password");

            var e1 = new RsaEncryption();
            var e2 = new RsaEncryption();

            // Act
            var encryptedData = e1.Encrypt(new EncryptionData(secret), publicKey);
            var decryptedData = e2.Decrypt(encryptedData, privateKey);

            // Assert
            Assert.Equal(decryptedData.Text, secret);
        }

        [Fact]
        public void Decrypt_Should_ReturnExpectedResult_When_KeyIsStoredInConfig()
        {
            // Arrange
            var publicKey = RsaPublicKey.LoadFromConfig();
            var privateKey = RsaPrivateKey.LoadFromConfig();

            var e1 = new RsaEncryption();
            var e2 = new RsaEncryption();

            // Act
            var encryptedData = e1.Encrypt(new EncryptionData(secret), publicKey);
            var decryptedData = e2.Decrypt(encryptedData, privateKey);

            // Assert
            Assert.Equal(decryptedData.Text, secret);
        }

        [Fact]
        public void Decrypt_Should_ReturnExpectedResult_When_KeyIsStoredInXml()
        {
            // Arrange
            var publicKeyXml = "<RSAKeyValue>" +
                               "<Modulus>0D59Km2Eo9oopcm7Y2wOXx0TRRXQFybl9HHe/ve47Qcf2EoKbs9nkuMmhCJlJ" +
                               "zrq6ZJzgQSEbpVyaWn8OHq0I50rQ13dJsALEquhlfwVWw6Hit7qRvveKlOAGfj8xdkaXJ" +
                               "LYS1tA06tKHfYxgt6ysMBZd0DIedYoE1fe3VlLZyE=</Modulus>" +
                               "<Exponent>AQAB</Exponent>" +
                               "</RSAKeyValue>";

            var privateKeyXml = "<RSAKeyValue>" +
                                "<Modulus>0D59Km2Eo9oopcm7Y2wOXx0TRRXQFybl9HHe/ve47Qcf2EoKbs9nkuMmhCJlJ" +
                                "zrq6ZJzgQSEbpVyaWn8OHq0I50rQ13dJsALEquhlfwVWw6Hit7qRvveKlOAGfj8xdkaXJ" +
                                "LYS1tA06tKHfYxgt6ysMBZd0DIedYoE1fe3VlLZyE=</Modulus>" +
                                "<Exponent>AQAB</Exponent>" +
                                "<P>/1cvDks8qlF1IXKNwcXW8tjTlhjidjGtbT9k7FCYug+P6ZBDfqhUqfvjgLFF" +
                                "/+dAkoofNqliv89b8DRy4gS4qQ==</P>" +
                                "<Q>0Mgq7lyvmVPR1r197wnba1bWbJt8W2Ki8ilUN6lX6Lkk04ds9y3A0txy0ESya7dyg" +
                                "9NLscfU3NQMH8RRVnJtuQ==</Q>" +
                                "<DP>+uwfRumyxSDlfSgInFqh/+YKD5+GtGXfKtO4hu4xF+8BGqJ1YXtkL" +
                                "+Njz2zmADOt5hOr1tigPSQ2EhhIqUnAeQ==</DP>" +
                                "<DQ>M5Ofd28SOjCIwCHjwG+Q8v1qzz3CBNljI6uuEGoXO3ix" +
                                "bkggVRfKcMzg2C6AXTfeZE6Ifoy9OyhvLlHTPiXakQ==</DQ>" +
                                "<InverseQ>yQIJMLdL6kU4VK7M5b5PqWS8XzkgxfnaowRs9mhSEDdwwWPtUXO8aQ9G3" +
                                "zuiDUqNq9j5jkdt77+c2stBdV97ew==</InverseQ>" +
                                "<D>HOpQXu/OFyJXuo2EY43BgRt8bX9V4aEZFRQqrqSfHOp8VYASasiJzS+VTYupGAVqUP" +
                                "xw5V1HNkOyG0kIKJ+BG6BpIwLIbVKQn/ROs7E3/vBdg2+QXKhikMz/4gY" +
                                "x2oEsXW2kzN1GMRop2lrrJZJNGE/eG6i4lQ1/inj1Tk/sqQE=</D>" +
                                "</RSAKeyValue>";

            var publicKey = new RsaPublicKey(publicKeyXml);
            var privateKey = new RsaPrivateKey(privateKeyXml);

            var e1 = new RsaEncryption();
            var e2 = new RsaEncryption();

            // Act
            var encryptedData = e1.Encrypt(new EncryptionData(secret), publicKey);
            var decryptedData = e2.Decrypt(encryptedData, privateKey);

            // Assert
            Assert.Equal(decryptedData.Text, secret);
        }

        [Fact]
        public void Decrypt_Should_ReturnExpectedResult_When_UsingDefaultKeys()
        {
            // Arrange
            var e1 = new RsaEncryption();
            var e2 = new RsaEncryption();

            // Act
            var encryptedData = e1.Encrypt(new EncryptionData(secret));
            var decryptedData = e2.Decrypt(encryptedData);

            // Assert
            Assert.Equal(decryptedData.Text, secret);
        }

        [Fact]
        public void Decrypt_Should_ReturnExpectedResult_When_UsingDefaultKeySizeAndGeneratedKeys()
        {
            // Arrange
            var publicKey = new RsaPublicKey();
            var privateKey = new RsaPrivateKey();

            var e1 = new RsaEncryption();
            var e2 = new RsaEncryption();
            e1.GenerateNewKeyset(ref publicKey, ref privateKey);

            // Act
            var encryptedData = e1.Encrypt(new EncryptionData(secret), publicKey);
            var decryptedData = e2.Decrypt(encryptedData, privateKey);

            // Assert
            Assert.Equal(decryptedData.Text, secret);
        }

        [Fact]
        public void Decrypt_Should_ReturnExpectedResult_When_UsingExplicitKeySizeAndGeneratedKeys()
        {
            // Arrange
            var publicKey = new RsaPublicKey();
            var privateKey = new RsaPrivateKey();

            var e1 = new RsaEncryption(4096);
            var e2 = new RsaEncryption(4096);
            e1.GenerateNewKeyset(ref publicKey, ref privateKey);

            // Act
            var encryptedData = e1.Encrypt(new EncryptionData(secret), publicKey);
            var decryptedData = e2.Decrypt(encryptedData, privateKey);

            // Assert
            Assert.Equal(decryptedData.Text, secret);
        }

        [Fact]
        public void DefaultPrivateKey_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "AQAB";

            // Act
            var key = new RsaEncryption();

            // Assert
            Assert.Equal(expected, key.DefaultPublicKey.Exponent);
        }

        [Fact]
        public void DefaultPublicKey_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "ksvo/EqBn9XRzvH826npSQdCYv1G5gyEnzQeC4qPidEm"
                         + "Ub6Yan12cWYlt4CsK5umYGwWmRSL20Ufc+gnZQo6Pw==";

            // Act
            var key = new RsaEncryption();

            // Assert
            Assert.Equal(expected, key.DefaultPrivateKey.PrimeExponentP);
        }

        [Fact]
        public void Encrypt_Should_ThrowException_When_EncryptingToMuchData()
        {
            // Arrange
            var publicKey = new RsaPublicKey();
            var privateKey = new RsaPrivateKey();
            var e1 = new RsaEncryption();

            // Act
            e1.GenerateNewKeyset(ref publicKey, ref privateKey);

            // Assert
            Assert.Throws<CryptographicException>(() =>
            {
                e1.Encrypt(new EncryptionData(_targetString), publicKey);
            });
        }

        [Fact]
        public void Encrypt_Should_ThrowException_When_KeyIsNull()
        {
            // Arrange
            var publicKey = new RsaPublicKey(); ;
            var privateKey = new RsaPrivateKey();
            var e1 = new RsaEncryption();
            e1.GenerateNewKeyset(ref publicKey, ref privateKey);
            EncryptionData data = null;

            // Act & Assert
            Assert.Throws<NullReferenceException>(() =>
            {
                e1.Encrypt(data, publicKey);
            });
        }

        [Fact]
        public void GenerateNewKeyset_Should_ThrowException_When_PrivateKeyIsNull()
        {
            // Arrange
            var key = new RsaEncryption();
            var publicKey = new RsaPublicKey();
            RsaPrivateKey privateKey = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                key.GenerateNewKeyset(ref publicKey, ref privateKey);
            });
        }

        [Fact]
        public void GenerateNewKeyset_Should_ThrowException_When_PublicKeyIsNull()
        {
            // Arrange
            var key = new RsaEncryption();
            var privateKey = new RsaPrivateKey();
            RsaPublicKey publicKey = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                key.GenerateNewKeyset(ref publicKey, ref privateKey);
            });
        }

        [Fact]
        public void KeySizeMaxBits_Should_ExpectedResult()
        {
            // Arrange
            var expected = 16384;

            // Act
            var rsa = new RsaEncryption();

            // Assert
            Assert.Equal(expected, rsa.KeySizeMaxBits);
        }

        [Fact]
        public void KeySizeMinBits_Should_ExpectedResult()
        {
            // Arrange
            var expected = 384;

            // Act
            var rsa = new RsaEncryption();

            // Assert
            Assert.Equal(expected, rsa.KeySizeMinBits);
        }

        [Fact]
        public void KeySizeStepBits_Should_ExpectedResult()
        {
            // Arrange
            var expected = 8;

            // Act
            var rsa = new RsaEncryption();

            // Assert
            Assert.Equal(expected, rsa.KeySizeStepBits);
        }

        [Fact]
        public void Sign_Should_CorrectlyCreateProperSignature()
        {
            // Arrange
            var secretData = new EncryptionData(secret);

            var xml = File.ReadAllText("privateKey.xml");
            var privateKey = RsaPrivateKey.LoadFromXml(xml);

            var expected = "kZmV1cUO91lpOQkgz5HLbWsfeXabJOPfcWjH72EytH95AAJEVq+nonJm9A" +
                           "UjHy53VAIagJFJYiORcgsHC1klkppM71hRD1xUs70ggPiMIcTv/CDij3" +
                           "6FYxGd7n9GAh5LikojbWJxJHc3A5LqnAwSBBfOfY2K4gY5lZ3rSmhNHDM=";

            var e1 = new RsaEncryption();

            // Act

            var signature = e1.Sign(secretData, privateKey);

            // Assert
            Assert.Equal(signature.Base64, expected);
        }

        [Fact]
        public void Verify_Should_ReturnFalse_When_ValidatingChangedSignedData()
        {
            // Arrange
            var secretData = new EncryptionData(secret);

            var publicKey = new RsaPublicKey();
            var privateKey = new RsaPrivateKey();

            var e1 = new RsaEncryption();
            e1.GenerateNewKeyset(ref publicKey, ref privateKey);

            // Act
            var signature = e1.Sign(secretData, privateKey);
            secretData.Text = secretData.Text + "3";
            var actual = e1.Verify(secretData, signature, publicKey);

            // Assert
            Assert.False(actual);
        }

        [Fact]
        public void Verify_Should_ReturnTrue_When_ValidatingUnChangedSignedData()
        {
            // Arrange
            var secretData = new EncryptionData(secret);

            var publicKey = new RsaPublicKey();
            var privateKey = new RsaPrivateKey();

            var signature = default(EncryptionData);

            var e1 = new RsaEncryption();
            e1.GenerateNewKeyset(ref publicKey, ref privateKey);

            // Act
            signature = e1.Sign(secretData, privateKey);
            var actual = e1.Verify(secretData, signature, publicKey);

            // Assert
            Assert.True(actual);
        }
    }
}