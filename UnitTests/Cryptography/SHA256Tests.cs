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
    public class SHA256Tests
    {
        private static readonly string _assemblyPath =
            Path.GetDirectoryName(Assembly.GetAssembly(typeof(SHA256Tests)).Location)
            + Path.DirectorySeparatorChar;

        [Fact]
        public void SHA256_Should_CalculateCorrectHash()
        {
            // Arrange
            var expected = "8475D18AB750605A1B00381287B3E91D395082F25832B0A22F1F87DD2BD89A71";

            // Act
            var actual = SHA256Hash.Create().Compute("This is a Test of the Hash Function");

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SHA256_Should_CalculateCorrectHash_When_ProvidedEncryptionData()
        {
            // Arrange
            var data = new EncryptionData("This is a Test of the Hash Function");
            var expected = "8475D18AB750605A1B00381287B3E91D395082F25832B0A22F1F87DD2BD89A71";

            // Act
            var actual = SHA256Hash.Create().Compute(data);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SHA256_Should_CalculateCorrectHash_When_ProvidedStream()
        {
            // Arrange
            var expected = "A5882E2BAC0505CAE5E302B494AADD9591B02F2834AAE8047D7BF7671AF84800";
            var actual = String.Empty;

            // Act
            using (var sr = new StreamReader($"{_assemblyPath}gettysburg.txt"))
            {
                actual = SHA256Hash.Create().Compute(sr.BaseStream);
            }

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SHA256_Should_CalculateCorrectHash_When_ProvidedWithBytes()
        {
            // Arrange
            var data = new byte[]
            {
                0x55, 0x6e, 0x69, 0x74, 0x54, 0x65, 0x73, 0x74
            };
            var expected = "9F629BE9A63456097B80045FAD64ED7F49EDECCBD689CF69D8CC8296BB5276F3";

            // Act
            var actual = SHA256Hash.Create().Compute(data);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SHA256_Should_CalculateCorrectHash_When_Salted()
        {
            // Arrange
            var data = new EncryptionData("This is a Test of the Hash Function");
            var salt = new EncryptionData("Salty!");
            var expected = "709F35EB69B0E0AE7C6BA8A3F05C83215C96FBB7DA3DF7204702D4BBA82F468B";

            // Act
            var actual = SHA256Hash.Create().Compute(data, salt);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SHA256_Should_CalculateCorrectHashBytes()
        {
            // Arrange
            var expected = new byte[]
            {
                0x84, 0x75, 0xd1, 0x8a, 0xb7, 0x50, 0x60, 0x5a, 0x1b, 0x00,
                0x38, 0x12, 0x87, 0xb3, 0xe9, 0x1d, 0x39, 0x50, 0x82, 0xf2,
                0x58, 0x32, 0xb0, 0xa2, 0x2f, 0x1f, 0x87, 0xdd, 0x2b, 0xd8,
                0x9a, 0x71
            };

            // Act
            var actual = SHA256Hash.Create().ComputeToBytes("This is a Test of the Hash Function");

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SHA256_Should_CalculateCorrectHashBytes_When_ProvidedEncryptionData()
        {
            // Arrange
            var data = new EncryptionData("This is a Test of the Hash Function");
            var expected = new byte[]
            {
                0x84, 0x75, 0xd1, 0x8a, 0xb7, 0x50, 0x60, 0x5a, 0x1b, 0x00,
                0x38, 0x12, 0x87, 0xb3, 0xe9, 0x1d, 0x39, 0x50, 0x82, 0xf2,
                0x58, 0x32, 0xb0, 0xa2, 0x2f, 0x1f, 0x87, 0xdd, 0x2b, 0xd8,
                0x9a, 0x71
            };

            // Act
            var actual = SHA256Hash.Create().ComputeToBytes(data);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SHA256_Should_CalculateCorrectHashBytes_When_ProvidedStream()
        {
            // Arrange
            var expected = new byte[]
            {
                0xa5, 0x88, 0x2e, 0x2b, 0xac, 0x05, 0x05, 0xca, 0xe5, 0xe3,
                0x02, 0xb4, 0x94, 0xaa, 0xdd, 0x95, 0x91, 0xb0, 0x2f, 0x28,
                0x34, 0xaa, 0xe8, 0x04, 0x7d, 0x7b, 0xf7, 0x67, 0x1a, 0xf8,
                0x48, 0x00
            };

            byte[] actual;

            // Act
            using (var sr = new StreamReader($"{ _assemblyPath }gettysburg.txt"))
            {
                actual = SHA256Hash.Create().ComputeToBytes(sr.BaseStream);
            }

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SHA256_Should_CalculateCorrectHashBytes_When_ProvidedWithBytes()
        {
            // Arrange
            var data = new byte[]
            {
                0x55, 0x6e, 0x69, 0x74, 0x54, 0x65, 0x73, 0x74
            };

            var expected = new byte[]
            {
                0x9f, 0x62, 0x9b, 0xe9, 0xa6, 0x34, 0x56, 0x09, 0x7b, 0x80,
                0x04, 0x5f, 0xad, 0x64, 0xed, 0x7f, 0x49, 0xed, 0xec, 0xcb,
                0xd6, 0x89, 0xcf, 0x69, 0xd8, 0xcc, 0x82, 0x96, 0xbb, 0x52,
                0x76, 0xf3
            };

            // Act
            var actual = SHA256Hash.Create().ComputeToBytes(data);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SHA256_Should_CalculateCorrectHashBytes_When_Salted()
        {
            // Arrange
            var data = new EncryptionData("This is a Test of the Hash Function");
            var salt = new EncryptionData("Salty!");
            var expected = new byte[]
            {
                0x70, 0x9f, 0x35, 0xeb, 0x69, 0xb0, 0xe0, 0xae, 0x7c, 0x6b,
                0xa8, 0xa3, 0xf0, 0x5c, 0x83, 0x21, 0x5c, 0x96, 0xfb, 0xb7,
                0xda, 0x3d, 0xf7, 0x20, 0x47, 0x02, 0xd4, 0xbb, 0xa8, 0x2f,
                0x46, 0x8b
            };

            // Act
            var actual = SHA256Hash.Create().ComputeToBytes(data, salt);

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
