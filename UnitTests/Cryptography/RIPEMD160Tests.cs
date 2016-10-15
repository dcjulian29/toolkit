using System;
using System.IO;
using ToolKit.Cryptography;
using Xunit;

namespace UnitTests.Cryptography
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1600:ElementsMustBeDocumented",
        Justification = "Test Suites do not need XML Documentation.")]
    public class RIPEMD160Tests
    {
        [Fact]
        public void RIPEMD160_Should_CalculateCorrectHash()
        {
            // Arrange
            var expected = "18F07AF57C498236D825A7340EFA84A3C12A4FA8";

            // Act
            var actual = RIPEMD160Hash.Create().Compute("This is a Test of the Hash Function");

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void RIPEMD160_Should_CalculateCorrectHash_When_ProvidedEncryptionData()
        {
            // Arrange
            var data = new EncryptionData("This is a Test of the Hash Function");
            var expected = "18F07AF57C498236D825A7340EFA84A3C12A4FA8";

            // Act
            var actual = RIPEMD160Hash.Create().Compute(data);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void RIPEMD160_Should_CalculateCorrectHash_When_ProvidedStream()
        {
            // Arrange
            var expected = "386970AEB0C5C1E35EEE6F1973C1359B2E0AC8A3";
            var actual = String.Empty;

            // Act
            using (var sr = new StreamReader("gettysburg.txt"))
            {
                actual = RIPEMD160Hash.Create().Compute(sr.BaseStream);
            }

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void RIPEMD160_Should_CalculateCorrectHash_When_ProvidedWithBytes()
        {
            // Arrange
            var data = new byte[]
            {
                0x55, 0x6e, 0x69, 0x74, 0x54, 0x65, 0x73, 0x74
            };
            var expected = "CEBAB863E03653FD2D1CD94AE31AEEAFB759612B";

            // Act
            var actual = RIPEMD160Hash.Create().Compute(data);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void RIPEMD160_Should_CalculateCorrectHash_When_Salted()
        {
            // Arrange
            var data = new EncryptionData("This is a Test of the Hash Function");
            var salt = new EncryptionData("Salty!");
            var expected = "EF2C691C97736211B77C4C75087DFFBB6C957B30";

            // Act
            var actual = RIPEMD160Hash.Create().Compute(data, salt);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void RIPEMD160_Should_CalculateCorrectHashBytes()
        {
            // Arrange
            var expected = new byte[]
            {
                0x18, 0xf0, 0x7a, 0xf5, 0x7c, 0x49, 0x82, 0x36, 0xd8, 0x25,
                0xa7, 0x34, 0x0e, 0xfa, 0x84, 0xa3, 0xc1, 0x2a, 0x4f, 0xa8
            };

            // Act
            var actual = RIPEMD160Hash.Create().ComputeToBytes("This is a Test of the Hash Function");

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void RIPEMD160_Should_CalculateCorrectHashBytes_When_ProvidedEncryptionData()
        {
            // Arrange
            var data = new EncryptionData("This is a Test of the Hash Function");
            var expected = new byte[]
            {
                0x18, 0xf0, 0x7a, 0xf5, 0x7c, 0x49, 0x82, 0x36, 0xd8, 0x25,
                0xa7, 0x34, 0x0e, 0xfa, 0x84, 0xa3, 0xc1, 0x2a, 0x4f, 0xa8
            };

            // Act
            var actual = RIPEMD160Hash.Create().ComputeToBytes(data);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void RIPEMD160_Should_CalculateCorrectHashBytes_When_ProvidedStream()
        {
            // Arrange
            var expected = new byte[]
            {
                0x38, 0x69, 0x70, 0xae, 0xb0, 0xc5, 0xc1, 0xe3, 0x5e, 0xee,
                0x6f, 0x19, 0x73, 0xc1, 0x35, 0x9b, 0x2e, 0x0a, 0xc8, 0xa3
            };

            byte[] actual;

            // Act
            using (var sr = new StreamReader("gettysburg.txt"))
            {
                actual = RIPEMD160Hash.Create().ComputeToBytes(sr.BaseStream);
            }

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void RIPEMD160_Should_CalculateCorrectHashBytes_When_ProvidedWithBytes()
        {
            // Arrange
            var data = new byte[]
            {
                0x55, 0x6e, 0x69, 0x74, 0x54, 0x65, 0x73, 0x74
            };

            var expected = new byte[]
            {
                0xce, 0xba, 0xb8, 0x63, 0xe0, 0x36, 0x53, 0xfd, 0x2d, 0x1c,
                0xd9, 0x4a, 0xe3, 0x1a, 0xee, 0xaf, 0xb7, 0x59, 0x61, 0x2b
            };

            // Act
            var actual = RIPEMD160Hash.Create().ComputeToBytes(data);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void RIPEMD160_Should_CalculateCorrectHashBytes_When_Salted()
        {
            // Arrange
            var data = new EncryptionData("This is a Test of the Hash Function");
            var salt = new EncryptionData("Salty!");
            var expected = new byte[]
            {
                0xef, 0x2c, 0x69, 0x1c, 0x97, 0x73, 0x62, 0x11, 0xb7, 0x7c,
                0x4c, 0x75, 0x08, 0x7d, 0xff, 0xbb, 0x6c, 0x95, 0x7b, 0x30
            };

            // Act
            var actual = RIPEMD160Hash.Create().ComputeToBytes(data, salt);

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
