using System.Text;
using ToolKit.Cryptography;
using Xunit;

namespace UnitTests.Cryptography
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1600:ElementsMustBeDocumented",
        Justification = "Test Suites do not need XML Documentation.")]
    public class EncryptionDataTests
    {
        [Fact]
        public void Base64_Should_AcceptCorrectData()
        {
            // Arrange
            var expected = "UnitTest";
            var data = new EncryptionData
            {
                Base64 = "VW5pdFRlc3Q="
            };

            // Act
            var actual = data.Text;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Bytes_Should_AcceptCorrectData()
        {
            // Arrange
            var expected = "UnitTest";
            var data = new EncryptionData
            {
                Bytes = new byte[]
                {
                    0x55, 0x6e, 0x69, 0x74, 0x54, 0x65, 0x73, 0x74
                }
            };

            // Act
            var actual = data.Text;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Bytes_Should_ReturnExpectedLength_When_MaximumBytesSpecified()
        {
            // Arrange
            var data = new EncryptionData("UnitTest")
            {
                MaximumBytes = 2
            };

            // Act
            var actual = data.Bytes;

            // Assert
            Assert.Equal(2, actual.Length);
        }

        [Fact]
        public void Bytes_Should_ReturnExpectedLength_When_MaximumBytesSpecifiedAndBytesLess()
        {
            // Arrange
            var data = new EncryptionData("UnitTest")
            {
                MaximumBytes = 512
            };

            // Act
            var actual = data.Bytes;

            // Assert
            Assert.Equal(8, actual.Length);
        }

        [Fact]
        public void Bytes_Should_ReturnExpectedLength_When_MinimumBytesSpecified()
        {
            // Arrange
            var data = new EncryptionData("UnitTest")
            {
                MinimumBytes = 1024
            };

            // Act
            var actual = data.Bytes;

            // Assert
            Assert.Equal(1024, actual.Length);
        }

        [Fact]
        public void Bytes_Should_ReturnExpectedLength_When_MinimumBytesSpecifiedAndBytesMore()
        {
            // Arrange
            var data = new EncryptionData("UnitTest")
            {
                MinimumBytes = 2
            };

            // Act
            var actual = data.Bytes;

            // Assert
            Assert.Equal(8, actual.Length);
        }

        [Fact]
        public void Bytes_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = new byte[]
            {
                0x55, 0x6e, 0x69, 0x74, 0x54, 0x65, 0x73, 0x74
            };

            var data = new EncryptionData("UnitTest");

            // Act
            var actual = data.Bytes;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void EncodingToUse_Should_ReturnCorrectEncoder_When_EncoderPassedInConstructor()
        {
            // Arrange
            var expected = Encoding.ASCII;
            var data = new EncryptionData("data", Encoding.ASCII);

            // Act
            var actual = data.EncodingToUse;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void EncodingToUse_Should_ReturnUTF8_When_DefaultConstructorIsUsed()
        {
            // Arrange
            var expected = Encoding.UTF8;
            var data = new EncryptionData();

            // Act
            var actual = data.EncodingToUse;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void EqualOperator_Should_ReturnTrue_When_ValuesAreSame()
        {
            // Arrange
            var encrypt1 = new EncryptionData("password");
            var encrypt2 = new EncryptionData("password");

            // Act
            var actual = encrypt1 == encrypt2;

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void Equals_Should_ReturnFalse_When_OtherObjectIsDifferentType()
        {
            // Arrange
            var encrypt = new EncryptionData();
            var other = new CRC32();

            // Act
            var actual = encrypt.Equals(other);

            // Assert
            Assert.False(actual);
        }

        [Fact]
        public void Equals_Should_ReturnFalse_When_OtherObjectIsNull()
        {
            // Arrange
            var encrypt1 = new EncryptionData("password");

            // Act
            var actual = encrypt1.Equals(null);

            // Assert
            Assert.False(actual);
        }

        [Fact]
        public void Equals_Should_ReturnTrue_When_ObjectIsSame()
        {
            // Arrange
            var encrypt1 = new EncryptionData("password");

            // Act
            var actual = encrypt1.Equals(encrypt1);

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void Equals_Should_ReturnTrue_When_ValuesAreSame()
        {
            // Arrange
            var encrypt1 = new EncryptionData("password");
            var encrypt2 = new EncryptionData("password");

            // Act
            var actual = encrypt1.Equals(encrypt2);

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void GetHashCode_Should_ReturnExpectedResult()
        {
            // Arrange
            var encrypt = new EncryptionData("password");

            // Act
            var actual = encrypt.GetHashCode();

            // Assert
            Assert.True(actual == 4548);
        }

        [Fact]
        public void GetHashCode_Should_ReturnExpectedResult_When_EncryptionDataIsNull()
        {
            // Arrange
            EncryptionData encrypt = new EncryptionData();

            // Act
            var actual = encrypt.GetHashCode();

            // Assert
            Assert.True(actual == 0);
        }

        [Fact]
        public void Hex_Should_ReturnCorrectResult()
        {
            // Arrange
            var expected = "UnitTest";
            var data = new EncryptionData();

            // Act
            data.Hex = "556E697454657374";

            // Assert
            Assert.Equal(expected, data.Text);
        }

        [Fact]
        public void IsEmpty_Should_ReturnTrue_When_ByteLengthZero()
        {
            // Arrange
            var data = new EncryptionData(string.Empty);

            // Act
            var actual = data.IsEmpty;

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void IsEmpty_Should_ReturnTrue_When_NoBytes()
        {
            // Arrange
            var data = new EncryptionData();

            // Act
            var actual = data.IsEmpty;

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void MaximumBits_Should_ReturnCorrectNumber()
        {
            // Arrange
            var data = new EncryptionData();
            data.MaximumBytes = 1024;

            // Act
            var actual = data.MaximumBits;

            // Assert
            Assert.Equal(8192, actual);
        }

        [Fact]
        public void MaximumBytes_Should_ReturnCorrectNumber()
        {
            // Arrange
            var data = new EncryptionData();
            data.MaximumBits = 8192;

            // Act
            var actual = data.MaximumBytes;

            // Assert
            Assert.Equal(1024, actual);
        }

        [Fact]
        public void MaximumBytes_Should_ReturnZero_When_DefaultConstructorIsUsed()
        {
            // Arrange
            var data = new EncryptionData();

            // Act
            var actual = data.MaximumBytes;

            // Assert
            Assert.Equal(0, actual);
        }

        [Fact]
        public void MinimumBits_Should_ReturnCorrectNumber()
        {
            // Arrange
            var data = new EncryptionData();
            data.MinimumBytes = 1024;

            // Act
            var actual = data.MinimumBits;

            // Assert
            Assert.Equal(8192, actual);
        }

        [Fact]
        public void MinimumByte_Should_ReturnCorrectNumber()
        {
            // Arrange
            var data = new EncryptionData();
            data.MinimumBits = 8192;

            // Act
            var actual = data.MinimumBytes;

            // Assert
            Assert.Equal(1024, actual);
        }

        [Fact]
        public void MinimumBytes_Should_ReturnZero_When_DefaultConstructorIsUsed()
        {
            // Arrange
            var data = new EncryptionData();

            // Act
            var actual = data.MinimumBytes;

            // Assert
            Assert.Equal(0, actual);
        }

        [Fact]
        public void NotEqualOperator_Should_ReturnTrueWhenValuesAreDifferent()
        {
            // Arrange
            var encrypt1 = new EncryptionData("password1");
            var encrypt2 = new EncryptionData("password2");

            // Act
            var actual = encrypt1 != encrypt2;

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void Text_Should_ReturnEmptyString_When_ObjectEmpty()
        {
            // Arrange
            var data = new EncryptionData();

            // Act
            var actual = data.Text;

            // Assert
            Assert.Equal(string.Empty, actual);
        }

        [Fact]
        public void ToBase64_Should_ReturnCorrectResult()
        {
            // Arrange
            var expected = "VW5pdFRlc3Q=";
            var data = new EncryptionData("UnitTest");

            // Act
            var actual = data.ToBase64();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToHex_Should_ReturnCorrectResult()
        {
            // Arrange
            var expected = "556E697454657374";
            var data = new EncryptionData("UnitTest");

            // Act
            var actual = data.ToHex();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToString_Should_ReturnCorrectResult()
        {
            // Arrange
            var expected = "UnitTest";
            var bytes = new byte[]
            {
                0x55, 0x6e, 0x69, 0x74, 0x54, 0x65, 0x73, 0x74
            };

            var data = new EncryptionData(bytes);

            // Act
            var actual = data.ToString();

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
