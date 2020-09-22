using System;
using System.IO;
using ToolKit.Cryptography;
using Xunit;

#pragma warning disable 618

namespace UnitTests.Cryptography
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1600:ElementsMustBeDocumented",
        Justification = "Test Suites do not need XML Documentation.")]
    public class SHA1Tests
    {
        [Fact]
        public void SHA1_Should_CalculateCorrectHash()
        {
            // Arrange
            var expected = "C922E6BAD109080CCA0BFD309A0322B8C08F4575";

            // Act
            var actual = SHA1Hash.Create().Compute("This is a Test of the Hash Function");

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SHA1_Should_CalculateCorrectHash_When_ProvidedEncryptionData()
        {
            // Arrange
            var data = new EncryptionData("This is a Test of the Hash Function");
            var expected = "C922E6BAD109080CCA0BFD309A0322B8C08F4575";

            // Act
            var actual = SHA1Hash.Create().Compute(data);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SHA1_Should_CalculateCorrectHash_When_ProvidedStream()
        {
            // Arrange
            var expected = "B29FE3AD6FD68C8B2C71C609948929525D2185D2";
            var actual = String.Empty;

            // Act
            using (var sr = new StreamReader("gettysburg.txt"))
            {
                actual = SHA1Hash.Create().Compute(sr.BaseStream);
            }

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SHA1_Should_CalculateCorrectHash_When_ProvidedWithBytes()
        {
            // Arrange
            var data = new byte[]
            {
                0x55, 0x6e, 0x69, 0x74, 0x54, 0x65, 0x73, 0x74
            };
            var expected = "CE04FE357BC67BE9BE8F97F0A07E36DC7B63EDFC";

            // Act
            var actual = SHA1Hash.Create().Compute(data);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SHA1_Should_CalculateCorrectHash_When_Salted()
        {
            // Arrange
            var data = new EncryptionData("This is a Test of the Hash Function");
            var salt = new EncryptionData("Salty!");
            var expected = "7C375585002045C5807EFC259C211D0318070186";

            // Act
            var actual = SHA1Hash.Create().Compute(data, salt);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SHA1_Should_CalculateCorrectHashBytes()
        {
            // Arrange
            var expected = new byte[]
            {
                0xc9, 0x22, 0xe6, 0xba, 0xd1, 0x09, 0x08, 0x0c, 0xca, 0x0b,
                0xfd, 0x30, 0x9a, 0x03, 0x22, 0xb8, 0xc0, 0x8f, 0x45, 0x75
            };

            // Act
            var actual = SHA1Hash.Create().ComputeToBytes("This is a Test of the Hash Function");

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SHA1_Should_CalculateCorrectHashBytes_When_ProvidedEncryptionData()
        {
            // Arrange
            var data = new EncryptionData("This is a Test of the Hash Function");
            var expected = new byte[]
            {
                0xc9, 0x22, 0xe6, 0xba, 0xd1, 0x09, 0x08, 0x0c, 0xca, 0x0b,
                0xfd, 0x30, 0x9a, 0x03, 0x22, 0xb8, 0xc0, 0x8f, 0x45, 0x75
            };

            // Act
            var actual = SHA1Hash.Create().ComputeToBytes(data);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SHA1_Should_CalculateCorrectHashBytes_When_ProvidedStream()
        {
            // Arrange
            var expected = new byte[]
            {
                0xb2, 0x9f, 0xe3, 0xad, 0x6f, 0xd6, 0x8c, 0x8b, 0x2c, 0x71,
                0xc6, 0x09, 0x94, 0x89, 0x29, 0x52, 0x5d, 0x21, 0x85, 0xd2
            };

            byte[] actual;

            // Act
            using (var sr = new StreamReader("gettysburg.txt"))
            {
                actual = SHA1Hash.Create().ComputeToBytes(sr.BaseStream);
            }

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SHA1_Should_CalculateCorrectHashBytes_When_ProvidedWithBytes()
        {
            // Arrange
            var data = new byte[]
            {
                0x55, 0x6e, 0x69, 0x74, 0x54, 0x65, 0x73, 0x74
            };

            var expected = new byte[]
            {
                0xce, 0x04, 0xfe, 0x35, 0x7b, 0xc6, 0x7b, 0xe9, 0xbe, 0x8f,
                0x97, 0xf0, 0xa0, 0x7e, 0x36, 0xdc, 0x7b, 0x63, 0xed, 0xfc
            };

            // Act
            var actual = SHA1Hash.Create().ComputeToBytes(data);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SHA1_Should_CalculateCorrectHashBytes_When_Salted()
        {
            // Arrange
            var data = new EncryptionData("This is a Test of the Hash Function");
            var salt = new EncryptionData("Salty!");
            var expected = new byte[]
            {
                0x7c, 0x37, 0x55, 0x85, 0x00, 0x20, 0x45, 0xc5, 0x80, 0x7e,
                0xfc, 0x25, 0x9c, 0x21, 0x1d, 0x03, 0x18, 0x07, 0x01, 0x86
            };

            // Act
            var actual = SHA1Hash.Create().ComputeToBytes(data, salt);

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}

#pragma warning restore 618
