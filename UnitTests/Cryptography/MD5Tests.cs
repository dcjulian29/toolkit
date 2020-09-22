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
    public class MD5Tests
    {
        [Fact]
        public void MD5_Should_CalculateCorrectHash()
        {
            // Arrange
            var expected = "B28DD27794B25EB89CC2554182D06A3E";

            // Act
            var actual = MD5Hash.Create().Compute("This is a Test of the Hash Function");

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void MD5_Should_CalculateCorrectHash_When_ProvidedEncryptionData()
        {
            // Arrange
            var data = new EncryptionData("This is a Test of the Hash Function");
            var expected = "B28DD27794B25EB89CC2554182D06A3E";

            // Act
            var actual = MD5Hash.Create().Compute(data);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void MD5_Should_CalculateCorrectHash_When_ProvidedStream()
        {
            // Arrange
            var expected = "CD4123AC876D1F206E710E25EA33C723";
            var actual = String.Empty;

            // Act
            using (var sr = new StreamReader("gettysburg.txt"))
            {
                actual = MD5Hash.Create().Compute(sr.BaseStream);
            }

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void MD5_Should_CalculateCorrectHash_When_ProvidedWithBytes()
        {
            // Arrange
            var data = new byte[]
            {
                0x55, 0x6e, 0x69, 0x74, 0x54, 0x65, 0x73, 0x74
            };
            var expected = "37ADC7DB47085615AF6389C9C50AF7B9";

            // Act
            var actual = MD5Hash.Create().Compute(data);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void MD5_Should_CalculateCorrectHash_When_Salted()
        {
            // Arrange
            var data = new EncryptionData("This is a Test of the Hash Function");
            var salt = new EncryptionData("Salty!");
            var expected = "AFFC26020A68F34D168D9B56C935FA82";

            // Act
            var actual = MD5Hash.Create().Compute(data, salt);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void MD5_Should_CalculateCorrectHashBytes()
        {
            // Arrange
            var expected = new byte[]
            {
                0xb2, 0x8d, 0xd2, 0x77, 0x94, 0xb2, 0x5e, 0xb8, 0x9c, 0xc2, 0x55, 0x41, 0x82, 0xd0, 0x6a, 0x3e
            };

            // Act
            var actual = MD5Hash.Create().ComputeToBytes("This is a Test of the Hash Function");

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void MD5_Should_CalculateCorrectHashBytes_When_ProvidedEncryptionData()
        {
            // Arrange
            var data = new EncryptionData("This is a Test of the Hash Function");
            var expected = new byte[]
            {
                0xb2, 0x8d, 0xd2, 0x77, 0x94, 0xb2, 0x5e, 0xb8, 0x9c, 0xc2, 0x55, 0x41, 0x82, 0xd0, 0x6a, 0x3e
            };

            // Act
            var actual = MD5Hash.Create().ComputeToBytes(data);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void MD5_Should_CalculateCorrectHashBytes_When_ProvidedStream()
        {
            // Arrange
            var expected = new byte[]
            {
                0xcd, 0x41, 0x23, 0xac, 0x87, 0x6d, 0x1f, 0x20, 0x6e, 0x71, 0x0e, 0x25, 0xea, 0x33, 0xc7, 0x23
            };

            byte[] actual;

            // Act
            using (var sr = new StreamReader("gettysburg.txt"))
            {
                actual = MD5Hash.Create().ComputeToBytes(sr.BaseStream);
            }

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void MD5_Should_CalculateCorrectHashBytes_When_ProvidedWithBytes()
        {
            // Arrange
            var data = new byte[]
            {
                0x55, 0x6e, 0x69, 0x74, 0x54, 0x65, 0x73, 0x74
            };

            var expected = new byte[]
            {
                0x37, 0xad, 0xc7, 0xdb, 0x47, 0x08, 0x56, 0x15, 0xaf, 0x63, 0x89, 0xc9, 0xc5, 0x0a, 0xf7, 0xb9
            };

            // Act
            var actual = MD5Hash.Create().ComputeToBytes(data);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void MD5_Should_CalculateCorrectHashBytes_When_Salted()
        {
            // Arrange
            var data = new EncryptionData("This is a Test of the Hash Function");
            var salt = new EncryptionData("Salty!");
            var expected = new byte[]
            {
                0xaf, 0xfc, 0x26, 0x02, 0x0a, 0x68, 0xf3, 0x4d, 0x16, 0x8d, 0x9b, 0x56, 0xc9, 0x35, 0xfa, 0x82
            };

            // Act
            var actual = MD5Hash.Create().ComputeToBytes(data, salt);

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}

#pragma warning restore 618
