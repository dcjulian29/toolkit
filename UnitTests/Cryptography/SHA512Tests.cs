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
    public class SHA512Tests
    {
        [Fact]
        public void SHA512Hash_Should_CalculateCorrectHash()
        {
            // Arrange
            var expected = "EA0B57CBECB912E69EFCC662D979D432D22590BF9782B1F8238C87F5C3F1F83E"
                           + "31C2476BF0F7C0DD4F4DBAA565A72127DF4D10611E2A9D49667DE861E5BD4A94";

            // Act
            var actual = SHA512Hash.Create().Compute("This is a Test of the Hash Function");

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SHA512Hash_Should_CalculateCorrectHash_When_ProvidedEncryptionData()
        {
            // Arrange
            var data = new EncryptionData("This is a Test of the Hash Function");
            var expected = "EA0B57CBECB912E69EFCC662D979D432D22590BF9782B1F8238C87F5C3F1F83E"
                           + "31C2476BF0F7C0DD4F4DBAA565A72127DF4D10611E2A9D49667DE861E5BD4A94";

            // Act
            var actual = SHA512Hash.Create().Compute(data);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SHA512Hash_Should_CalculateCorrectHash_When_ProvidedStream()
        {
            // Arrange
            var expected = "92D11A96C22A0D346219F41D96906DD7A97872B782E1B9596A26D9FF1F540C23"
                           + "86AD6E0E35641458BFD86862B63599F77103B38FADA89F2DB93BFF62CB82BE30";
            var actual = String.Empty;

            // Act
            using (var sr = new StreamReader("gettysburg.txt"))
            {
                actual = SHA512Hash.Create().Compute(sr.BaseStream);
            }

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SHA512Hash_Should_CalculateCorrectHash_When_ProvidedWithBytes()
        {
            // Arrange
            var data = new byte[]
            {
                0x55, 0x6e, 0x69, 0x74, 0x54, 0x65, 0x73, 0x74
            };
            var expected = "E67958D980D4534131BA29EE59DEF22DF37656D8D433F7AF5667E7E88BC38282"
                           + "325EE4BD278827BC5376466A5F753E0CB8ABF7768EF8B4B819F32A64AA585EBB";

            // Act
            var actual = SHA512Hash.Create().Compute(data);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SHA512Hash_Should_CalculateCorrectHash_When_Salted()
        {
            // Arrange
            var data = new EncryptionData("This is a Test of the Hash Function");
            var salt = new EncryptionData("Salty!");
            var expected = "95688DA4EEA5DF0AB4330A785FFBF9B4FE16B7A661A48935CE864C92CC28B587"
                           + "91F258F94AD745563D26752A2B7D5B2ABC6B0B5F87B2C602CF6BF1B485507C88";

            // Act
            var actual = SHA512Hash.Create().Compute(data, salt);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SHA512Hash_Should_CalculateCorrectHashBytes()
        {
            // Arrange
            var expected = new byte[]
            {
                0xea, 0x0b, 0x57, 0xcb, 0xec, 0xb9, 0x12, 0xe6, 0x9e, 0xfc,
                0xc6, 0x62, 0xd9, 0x79, 0xd4, 0x32, 0xd2, 0x25, 0x90, 0xbf,
                0x97, 0x82, 0xb1, 0xf8, 0x23, 0x8c, 0x87, 0xf5, 0xc3, 0xf1,
                0xf8, 0x3e, 0x31, 0xc2, 0x47, 0x6b, 0xf0, 0xf7, 0xc0, 0xdd,
                0x4f, 0x4d, 0xba, 0xa5, 0x65, 0xa7, 0x21, 0x27, 0xdf, 0x4d,
                0x10, 0x61, 0x1e, 0x2a, 0x9d, 0x49, 0x66, 0x7d, 0xe8, 0x61,
                0xe5, 0xbd, 0x4a, 0x94
            };

            // Act
            var actual = SHA512Hash.Create().ComputeToBytes("This is a Test of the Hash Function");

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SHA512Hash_Should_CalculateCorrectHashBytes_When_ProvidedEncryptionData()
        {
            // Arrange
            var data = new EncryptionData("This is a Test of the Hash Function");
            var expected = new byte[]
            {
                0xea, 0x0b, 0x57, 0xcb, 0xec, 0xb9, 0x12, 0xe6, 0x9e, 0xfc,
                0xc6, 0x62, 0xd9, 0x79, 0xd4, 0x32, 0xd2, 0x25, 0x90, 0xbf,
                0x97, 0x82, 0xb1, 0xf8, 0x23, 0x8c, 0x87, 0xf5, 0xc3, 0xf1,
                0xf8, 0x3e, 0x31, 0xc2, 0x47, 0x6b, 0xf0, 0xf7, 0xc0, 0xdd,
                0x4f, 0x4d, 0xba, 0xa5, 0x65, 0xa7, 0x21, 0x27, 0xdf, 0x4d,
                0x10, 0x61, 0x1e, 0x2a, 0x9d, 0x49, 0x66, 0x7d, 0xe8, 0x61,
                0xe5, 0xbd, 0x4a, 0x94
            };

            // Act
            var actual = SHA512Hash.Create().ComputeToBytes(data);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SHA512Hash_Should_CalculateCorrectHashBytes_When_ProvidedStream()
        {
            // Arrange
            var expected = new byte[]
            {
                0x92, 0xd1, 0x1a, 0x96, 0xc2, 0x2a, 0x0d, 0x34, 0x62, 0x19,
                0xf4, 0x1d, 0x96, 0x90, 0x6d, 0xd7, 0xa9, 0x78, 0x72, 0xb7,
                0x82, 0xe1, 0xb9, 0x59, 0x6a, 0x26, 0xd9, 0xff, 0x1f, 0x54,
                0x0c, 0x23, 0x86, 0xad, 0x6e, 0x0e, 0x35, 0x64, 0x14, 0x58,
                0xbf, 0xd8, 0x68, 0x62, 0xb6, 0x35, 0x99, 0xf7, 0x71, 0x03,
                0xb3, 0x8f, 0xad, 0xa8, 0x9f, 0x2d, 0xb9, 0x3b, 0xff, 0x62,
                0xcb, 0x82, 0xbe, 0x30
            };

            byte[] actual;

            // Act
            using (var sr = new StreamReader("gettysburg.txt"))
            {
                actual = SHA512Hash.Create().ComputeToBytes(sr.BaseStream);
            }

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SHA512Hash_Should_CalculateCorrectHashBytes_When_ProvidedWithBytes()
        {
            // Arrange
            var data = new byte[]
            {
                0x55, 0x6e, 0x69, 0x74, 0x54, 0x65, 0x73, 0x74
            };

            var expected = new byte[]
            {
                0xe6, 0x79, 0x58, 0xd9, 0x80, 0xd4, 0x53, 0x41, 0x31, 0xba,
                0x29, 0xee, 0x59, 0xde, 0xf2, 0x2d, 0xf3, 0x76, 0x56, 0xd8,
                0xd4, 0x33, 0xf7, 0xaf, 0x56, 0x67, 0xe7, 0xe8, 0x8b, 0xc3,
                0x82, 0x82, 0x32, 0x5e, 0xe4, 0xbd, 0x27, 0x88, 0x27, 0xbc,
                0x53, 0x76, 0x46, 0x6a, 0x5f, 0x75, 0x3e, 0x0c, 0xb8, 0xab,
                0xf7, 0x76, 0x8e, 0xf8, 0xb4, 0xb8, 0x19, 0xf3, 0x2a, 0x64,
                0xaa, 0x58, 0x5e, 0xbb
            };

            // Act
            var actual = SHA512Hash.Create().ComputeToBytes(data);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SHA512Hash_Should_CalculateCorrectHashBytes_When_Salted()
        {
            // Arrange
            var data = new EncryptionData("This is a Test of the Hash Function");
            var salt = new EncryptionData("Salty!");
            var expected = new byte[]
            {
                0x95, 0x68, 0x8d, 0xa4, 0xee, 0xa5, 0xdf, 0x0a, 0xb4, 0x33,
                0x0a, 0x78, 0x5f, 0xfb, 0xf9, 0xb4, 0xfe, 0x16, 0xb7, 0xa6,
                0x61, 0xa4, 0x89, 0x35, 0xce, 0x86, 0x4c, 0x92, 0xcc, 0x28,
                0xb5, 0x87, 0x91, 0xf2, 0x58, 0xf9, 0x4a, 0xd7, 0x45, 0x56,
                0x3d, 0x26, 0x75, 0x2a, 0x2b, 0x7d, 0x5b, 0x2a, 0xbc, 0x6b,
                0x0b, 0x5f, 0x87, 0xb2, 0xc6, 0x02, 0xcf, 0x6b, 0xf1, 0xb4,
                0x85, 0x50, 0x7c, 0x88
            };

            // Act
            var actual = SHA512Hash.Create().ComputeToBytes(data, salt);

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
