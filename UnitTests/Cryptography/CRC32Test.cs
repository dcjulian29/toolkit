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
    public class CRC32Test
    {
        private static readonly string _assemblyPath =
            Path.GetDirectoryName(Assembly.GetAssembly(typeof(CRC32Test)).Location)
            + Path.DirectorySeparatorChar;

        [Fact]
        public void CRC32_Should_CalculateCorrectHash()
        {
            // Arrange
            var expected = "84A3C5DC";

            // Act
            var actual = CRC32.Create().Compute("This is a Test of the Hash Function");

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CRC32_Should_CalculateCorrectHash_When_ProvidedEncryptionData()
        {
            // Arrange
            var data = new EncryptionData("This is a Test of the Hash Function");
            var expected = "84A3C5DC";

            // Act
            var actual = CRC32.Create().Compute(data);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CRC32_Should_CalculateCorrectHash_When_ProvidedStream()
        {
            // Arrange
            var expected = "8893EF97";
            var actual = String.Empty;

            // Act
            using (var sr = new StreamReader($"{_assemblyPath}gettysburg.txt"))
            {
                actual = CRC32.Create().Compute(sr.BaseStream);
            }

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CRC32_Should_CalculateCorrectHash_When_ProvidedWithBytes()
        {
            // Arrange
            var data = new byte[]
            {
                0x55, 0x6e, 0x69, 0x74, 0x54, 0x65, 0x73, 0x74
            };
            var expected = "D9CD03C0";

            // Act
            var actual = CRC32.Create().Compute(data);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CRC32_Should_CalculateCorrectHash_When_Salted()
        {
            // Arrange
            var data = new EncryptionData("This is a Test of the Hash Function");
            var salt = new EncryptionData("Salty!");
            var expected = "833862F5";

            // Act
            var actual = CRC32.Create().Compute(data, salt);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CRC32_Should_CalculateCorrectHashBytes()
        {
            // Arrange
            var expected = new byte[]
            {
                0x84, 0xa3, 0xc5, 0xdc
            };

            // Act
            var actual = CRC32.Create().ComputeToBytes("This is a Test of the Hash Function");

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CRC32_Should_CalculateCorrectHashBytes_When_ProvidedEncryptionData()
        {
            // Arrange
            var data = new EncryptionData("This is a Test of the Hash Function");
            var expected = new byte[]
            {
                0x84, 0xa3, 0xc5, 0xdc
            };

            // Act
            var actual = CRC32.Create().ComputeToBytes(data);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CRC32_Should_CalculateCorrectHashBytes_When_ProvidedStream()
        {
            // Arrange
            var expected = new byte[]
            {
                0x88, 0x93, 0xef, 0x97
            };

            byte[] actual;

            // Act
            using (var sr = new StreamReader($"{ _assemblyPath }gettysburg.txt"))
            {
                actual = CRC32.Create().ComputeToBytes(sr.BaseStream);
            }

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CRC32_Should_CalculateCorrectHashBytes_When_ProvidedWithBytes()
        {
            // Arrange
            var data = new byte[]
            {
                0x55, 0x6e, 0x69, 0x74, 0x54, 0x65, 0x73, 0x74
            };

            var expected = new byte[]
            {
                0xd9, 0xcd, 0x03, 0xc0
            };

            // Act
            var actual = CRC32.Create().ComputeToBytes(data);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CRC32_Should_CalculateCorrectHashBytes_When_Salted()
        {
            // Arrange
            var data = new EncryptionData("This is a Test of the Hash Function");
            var salt = new EncryptionData("Salty!");
            var expected = new byte[]
            {
                0x83, 0x38, 0x62, 0xf5
            };

            // Act
            var actual = CRC32.Create().ComputeToBytes(data, salt);

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
