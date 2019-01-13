using System.Diagnostics.CodeAnalysis;
using ToolKit.Xml;
using Xunit;

namespace UnitTests.Xml
{
    [SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1600:ElementsMustBeDocumented",
        Justification = "Test Suites do not need XML Documentation.")]
    public class XmlEncoderTests
    {
        [Fact]
        public void Decode_Should_ReturnCorrectlyEncodedString_When_StringContainsAnAmpersand()
        {
            // Arrange
            var expected = "&";
            var source = "&amp;";

            // Act
            var actual = XmlEncoder.Decode(source);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Decode_Should_ReturnCorrectlyEncodedString_When_StringContainsDoubleQuote()
        {
            // Arrange
            var expected = "\"";
            var source = "&quot;";

            // Act
            var actual = XmlEncoder.Decode(source);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Decode_Should_ReturnCorrectlyEncodedString_When_StringContainsGreaterThan()
        {
            // Arrange
            var expected = ">";
            var source = "&gt;";

            // Act
            var actual = XmlEncoder.Decode(source);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Decode_Should_ReturnCorrectlyEncodedString_When_StringContainsLessThan()
        {
            // Arrange
            var expected = "<";
            var source = "&lt;";

            // Act
            var actual = XmlEncoder.Decode(source);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Decode_Should_ReturnCorrectlyEncodedString_When_StringContainsNonPrintableInDecimal()
        {
            // Arrange
            var expected = "\r";
            var source = "&#13;";

            // Act
            var actual = XmlEncoder.Decode(source);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Decode_Should_ReturnCorrectlyEncodedString_When_StringContainsNonPrintableInHexaDecimal()
        {
            // Arrange
            var expected = "\n";
            var source = "&#x0A;";

            // Act
            var actual = XmlEncoder.Decode(source);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Decode_Should_ReturnCorrectlyEncodedString_When_StringContainsSingleQuote()
        {
            // Arrange
            var expected = "\'";
            var source = "&apos;";

            // Act
            var actual = XmlEncoder.Decode(source);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Decode_Should_ReturnCorrectlyEncodedString_When_StringContainsTwoSpaces()
        {
            // Arrange
            var expected = "  ";
            var source = "&nbsp; ";

            // Act
            var actual = XmlEncoder.Decode(source);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Decode_Should_ReturnCorrectlyEncodedString_When_StringIsANormalString()
        {
            // Arrange
            var expected = "Normal";
            var source = "Normal";

            // Act
            var actual = XmlEncoder.Decode(source);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Decode_Should_ReturnInputString_When_InputStringDoesNotContainEntity()
        {
            // Arrange
            var expected = "&NotAnEntity";

            // Act
            var actual = XmlEncoder.Decode(expected);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Encode_Should_ReturnCorrectlyEncodedString_When_StringContainsAnAmpersand()
        {
            // Arrange
            var expected = "&amp;";
            var source = "&";

            // Act
            var actual = XmlEncoder.Encode(source);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Encode_Should_ReturnCorrectlyEncodedString_When_StringContainsDoubleQuote()
        {
            // Arrange
            var expected = "&quot;";
            var source = "\"";

            // Act
            var actual = XmlEncoder.Encode(source);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Encode_Should_ReturnCorrectlyEncodedString_When_StringContainsGreaterThan()
        {
            // Arrange
            var expected = "&gt;";
            var source = ">";

            // Act
            var actual = XmlEncoder.Encode(source);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Encode_Should_ReturnCorrectlyEncodedString_When_StringContainsLessThan()
        {
            // Arrange
            var expected = "&lt;";
            var source = "<";

            // Act
            var actual = XmlEncoder.Encode(source);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Encode_Should_ReturnCorrectlyEncodedString_When_StringContainsNonPrintable()
        {
            // Arrange
            var expected = "&#13;";
            var source = "\r";

            // Act
            var actual = XmlEncoder.Encode(source);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Encode_Should_ReturnCorrectlyEncodedString_When_StringContainsSingleQuote()
        {
            // Arrange
            var expected = "&apos;";
            var source = "\'";

            // Act
            var actual = XmlEncoder.Encode(source);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Encode_Should_ReturnCorrectlyEncodedString_When_StringContainsTwoSpaces()
        {
            // Arrange
            var expected = "&nbsp; ";
            var source = "  ";

            // Act
            var actual = XmlEncoder.Encode(source);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Encode_Should_ReturnCorrectlyEncodedString_When_StringIsANormalString()
        {
            // Arrange
            var expected = "Normal";
            var source = "Normal";

            // Act
            var actual = XmlEncoder.Encode(source);

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
