using System.Diagnostics.CodeAnalysis;
using ToolKit;
using Xunit;

namespace UnitTests
{
    [SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1600:ElementsMustBeDocumented",
        Justification = "Test Suites do not need XML Documentation.")]
    public class HexEncodingTest
    {
        [Fact]
        public void GetByteCountTest()
        {
            // Arrange
            var expected = 2;

            // Act
            var actual = HexEncoding.GetByteCount("FFA4");

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void IsHexDigitTest()
        {
            Assert.True(HexEncoding.IsHexDigit('F'));
            Assert.False(HexEncoding.IsHexDigit('z'));
        }

        [Fact]
        public void IsHexFormatTest()
        {
            Assert.True(HexEncoding.IsHexFormat("FFAA34"));
            Assert.False(HexEncoding.IsHexFormat("JULIAN"));
        }

        [Fact]
        public void ToBytesTest()
        {
            // Arrange
            byte[] expected = { 255, 170, 50 };

            // Act
            var actual = HexEncoding.ToBytes("FFAA32");

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToStringTest()
        {
            // Arrange
            var expected = "FFAA32";
            byte[] bytes = { 255, 170, 50 };

            // Act
            var actual = HexEncoding.ToString(bytes);

            // Arrange
            Assert.Equal(expected, actual);
        }
    }
}
