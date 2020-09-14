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
            const string expected = "&";
            const string source = "&amp;";

            // Act
            var actual = XmlEncoder.Decode(source);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Decode_Should_ReturnCorrectlyEncodedString_When_StringContainsDoubleQuote()
        {
            // Arrange
            const string expected = "\"";
            const string source = "&quot;";

            // Act
            var actual = XmlEncoder.Decode(source);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Decode_Should_ReturnCorrectlyEncodedString_When_StringContainsGreaterThan()
        {
            // Arrange
            const string expected = ">";
            const string source = "&gt;";

            // Act
            var actual = XmlEncoder.Decode(source);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Decode_Should_ReturnCorrectlyEncodedString_When_StringContainsLessThan()
        {
            // Arrange
            const string expected = "<";
            const string source = "&lt;";

            // Act
            var actual = XmlEncoder.Decode(source);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Decode_Should_ReturnCorrectlyEncodedString_When_StringContainsNonPrintableInDecimal()
        {
            // Arrange
            const string expected = "\r";
            const string source = "&#13;";

            // Act
            var actual = XmlEncoder.Decode(source);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Decode_Should_ReturnCorrectlyEncodedString_When_StringContainsNonPrintableInHexaDecimal()
        {
            // Arrange
            const string expected = "\n";
            const string source = "&#x0A;";

            // Act
            var actual = XmlEncoder.Decode(source);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Decode_Should_ReturnCorrectlyEncodedString_When_StringContainsSingleQuote()
        {
            // Arrange
            const string expected = "\'";
            const string source = "&apos;";

            // Act
            var actual = XmlEncoder.Decode(source);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Decode_Should_ReturnCorrectlyEncodedString_When_StringContainsTwoSpaces()
        {
            // Arrange
            const string expected = "  ";
            const string source = "&nbsp; ";

            // Act
            var actual = XmlEncoder.Decode(source);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Decode_Should_ReturnCorrectlyEncodedString_When_StringIsANormalString()
        {
            // Arrange
            const string expected = "Normal";
            const string source = "Normal";

            // Act
            var actual = XmlEncoder.Decode(source);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Decode_Should_ReturnInputString_When_InputStringDoesNotContainEntity()
        {
            // Arrange
            const string expected = "&NotAnEntity";

            // Act
            var actual = XmlEncoder.Decode(expected);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Encode_Should_ReturnCorrectlyEncodedString_When_StringContainsAnAmpersand()
        {
            // Arrange
            const string expected = "&amp;";
            const string source = "&";

            // Act
            var actual = XmlEncoder.Encode(source);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Encode_Should_ReturnCorrectlyEncodedString_When_StringContainsDoubleQuote()
        {
            // Arrange
            const string expected = "&quot;";
            const string source = "\"";

            // Act
            var actual = XmlEncoder.Encode(source);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Encode_Should_ReturnCorrectlyEncodedString_When_StringContainsGreaterThan()
        {
            // Arrange
            const string expected = "&gt;";
            const string source = ">";

            // Act
            var actual = XmlEncoder.Encode(source);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Encode_Should_ReturnCorrectlyEncodedString_When_StringContainsLessThan()
        {
            // Arrange
            const string expected = "&lt;";
            const string source = "<";

            // Act
            var actual = XmlEncoder.Encode(source);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Encode_Should_ReturnCorrectlyEncodedString_When_StringContainsNonPrintable()
        {
            // Arrange
            const string expected = "&#13;";
            const string source = "\r";

            // Act
            var actual = XmlEncoder.Encode(source);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Encode_Should_ReturnCorrectlyEncodedString_When_StringContainsSingleQuote()
        {
            // Arrange
            const string expected = "&apos;";
            const string source = "\'";

            // Act
            var actual = XmlEncoder.Encode(source);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Encode_Should_ReturnCorrectlyEncodedString_When_StringContainsTwoSpaces()
        {
            // Arrange
            const string expected = "&nbsp; ";
            const string source = "  ";

            // Act
            var actual = XmlEncoder.Encode(source);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Encode_Should_ReturnCorrectlyEncodedString_When_StringIsANormalString()
        {
            // Arrange
            const string expected = "Normal";
            const string source = "Normal";

            // Act
            var actual = XmlEncoder.Encode(source);

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
