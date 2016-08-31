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
    public class SHA384Tests
    {
        [Fact]
        public void SHA384_Should_CalculateCorrectHash()
        {
            // Arrange
            var expected = "6DB892F1AAA48AB98DBDB9569C90D81EF8913981FCCFC93F08079801"
                           + "5580E9561B75651004806DF6EBF159F423364D5A";

            // Act
            var actual = SHA384Hash.Create().Compute("This is a Test of the Hash Function");

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SHA384_Should_CalculateCorrectHash_When_ProvidedEncryptionData()
        {
            // Arrange
            var data = new EncryptionData("This is a Test of the Hash Function");
            var expected = "6DB892F1AAA48AB98DBDB9569C90D81EF8913981FCCFC93F08079801"
                           + "5580E9561B75651004806DF6EBF159F423364D5A";

            // Act
            var actual = SHA384Hash.Create().Compute(data);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SHA384_Should_CalculateCorrectHash_When_ProvidedStream()
        {
            // Arrange
            var expected = "5B02DA411990DA6D407B133CBCA66A3034CA80E0A2C4FDA1BA8E6488"
                           + "85E817234C33D65BACD4B7DBAAEE16C99920A2EE";
            var actual = String.Empty;

            // Act
            using (var sr = new StreamReader("gettysburg.txt"))
            {
                actual = SHA384Hash.Create().Compute(sr.BaseStream);
            }

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SHA384_Should_CalculateCorrectHash_When_ProvidedWithBytes()
        {
            // Arrange
            var data = new byte[]
            {
                0x55, 0x6e, 0x69, 0x74, 0x54, 0x65, 0x73, 0x74
            };
            var expected = "6BFFD31117710EE30109D8EF1422D5AA1E6EB0ED9F04AD871F503A2E"
                           + "49AA511472983BCD7584B972AED8A7C25B1ACFE8";

            // Act
            var actual = SHA384Hash.Create().Compute(data);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SHA384_Should_CalculateCorrectHash_When_Salted()
        {
            // Arrange
            var data = new EncryptionData("This is a Test of the Hash Function");
            var salt = new EncryptionData("Salty!");
            var expected = "E746B2B99D3C27422CB8DEF10C4D397675B0314E818E4300AA92475F"
                           + "258B22B8BA87A24E78F5B6ADEE035E1798A9EEA0";

            // Act
            var actual = SHA384Hash.Create().Compute(data, salt);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SHA384_Should_CalculateCorrectHashBytes()
        {
            // Arrange
            var expected = new byte[]
            {
                0x6d, 0xb8, 0x92, 0xf1, 0xaa, 0xa4, 0x8a, 0xb9, 0x8d, 0xbd,
                0xb9, 0x56, 0x9c, 0x90, 0xd8, 0x1e, 0xf8, 0x91, 0x39, 0x81,
                0xfc, 0xcf, 0xc9, 0x3f, 0x08, 0x07, 0x98, 0x01, 0x55, 0x80,
                0xe9, 0x56, 0x1b, 0x75, 0x65, 0x10, 0x04, 0x80, 0x6d, 0xf6,
                0xeb, 0xf1, 0x59, 0xf4, 0x23, 0x36, 0x4d, 0x5a
            };

            // Act
            var actual = SHA384Hash.Create().ComputeToBytes("This is a Test of the Hash Function");

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SHA384_Should_CalculateCorrectHashBytes_When_ProvidedEncryptionData()
        {
            // Arrange
            var data = new EncryptionData("This is a Test of the Hash Function");
            var expected = new byte[]
            {
                0x6d, 0xb8, 0x92, 0xf1, 0xaa, 0xa4, 0x8a, 0xb9, 0x8d, 0xbd,
                0xb9, 0x56, 0x9c, 0x90, 0xd8, 0x1e, 0xf8, 0x91, 0x39, 0x81,
                0xfc, 0xcf, 0xc9, 0x3f, 0x08, 0x07, 0x98, 0x01, 0x55, 0x80,
                0xe9, 0x56, 0x1b, 0x75, 0x65, 0x10, 0x04, 0x80, 0x6d, 0xf6,
                0xeb, 0xf1, 0x59, 0xf4, 0x23, 0x36, 0x4d, 0x5a
            };

            // Act
            var actual = SHA384Hash.Create().ComputeToBytes(data);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SHA384_Should_CalculateCorrectHashBytes_When_ProvidedStream()
        {
            // Arrange
            var expected = new byte[]
            {
                0x5b, 0x02, 0xda, 0x41, 0x19, 0x90, 0xda, 0x6d, 0x40, 0x7b,
                0x13, 0x3c, 0xbc, 0xa6, 0x6a, 0x30, 0x34, 0xca, 0x80, 0xe0,
                0xa2, 0xc4, 0xfd, 0xa1, 0xba, 0x8e, 0x64, 0x88, 0x85, 0xe8,
                0x17, 0x23, 0x4c, 0x33, 0xd6, 0x5b, 0xac, 0xd4, 0xb7, 0xdb,
                0xaa, 0xee, 0x16, 0xc9, 0x99, 0x20, 0xa2, 0xee
            };

            byte[] actual;

            // Act
            using (var sr = new StreamReader("gettysburg.txt"))
            {
                actual = SHA384Hash.Create().ComputeToBytes(sr.BaseStream);
            }

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SHA384_Should_CalculateCorrectHashBytes_When_ProvidedWithBytes()
        {
            // Arrange
            var data = new byte[]
            {
                0x55, 0x6e, 0x69, 0x74, 0x54, 0x65, 0x73, 0x74
            };

            var expected = new byte[]
            {
                0x6b, 0xff, 0xd3, 0x11, 0x17, 0x71, 0x0e, 0xe3, 0x01, 0x09,
                0xd8, 0xef, 0x14, 0x22, 0xd5, 0xaa, 0x1e, 0x6e, 0xb0, 0xed,
                0x9f, 0x04, 0xad, 0x87, 0x1f, 0x50, 0x3a, 0x2e, 0x49, 0xaa,
                0x51, 0x14, 0x72, 0x98, 0x3b, 0xcd, 0x75, 0x84, 0xb9, 0x72,
                0xae, 0xd8, 0xa7, 0xc2, 0x5b, 0x1a, 0xcf, 0xe8
            };

            // Act
            var actual = SHA384Hash.Create().ComputeToBytes(data);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SHA384_Should_CalculateCorrectHashBytes_When_Salted()
        {
            // Arrange
            var data = new EncryptionData("This is a Test of the Hash Function");
            var salt = new EncryptionData("Salty!");
            var expected = new byte[]
            {
                0xe7, 0x46, 0xb2, 0xb9, 0x9d, 0x3c, 0x27, 0x42, 0x2c, 0xb8,
                0xde, 0xf1, 0x0c, 0x4d, 0x39, 0x76, 0x75, 0xb0, 0x31, 0x4e,
                0x81, 0x8e, 0x43, 0x00, 0xaa, 0x92, 0x47, 0x5f, 0x25, 0x8b,
                0x22, 0xb8, 0xba, 0x87, 0xa2, 0x4e, 0x78, 0xf5, 0xb6, 0xad,
                0xee, 0x03, 0x5e, 0x17, 0x98, 0xa9, 0xee, 0xa0
            };

            // Act
            var actual = SHA384Hash.Create().ComputeToBytes(data, salt);

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
