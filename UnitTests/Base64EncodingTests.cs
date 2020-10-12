using System.Text;
using ToolKit;
using Xunit;

namespace UnitTests
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1600:ElementsMustBeDocumented",
        Justification = "Test Suites do not need XML Documentation.")]
    public class Base64EncodingTests
    {
        #region -- Test SetUp and TearDown --
        #endregion
        #region -- Test Cases --
        [Fact]
        public void ConvertToBytes_Should_EncodeWhenProvidedWithBase64String()
        {
            // Arrange
            var expected = Encoding.Unicode.GetBytes("Now is the time for all good men to come");
            var base64 = "TgBvAHcAIABpAHMAIAB0AGgAZQAgAHQAaQBtAGUAIABmAG8AcgAgAG" +
                         "EAbABsACAAZwBvAG8AZAAgAG0AZQBuACAAdABvACAAYwBvAG0AZQA=";

            // Act
            var actual = Base64Encoding.ToBytes(base64);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ConvertToString_Should_EncodeWhenProvidedWithBytes()
        {
            // Arrange
            var expected = "TgBvAHcAIABpAHMAIAB0AGgAZQAgAHQAaQBtAGUAIABmAG8AcgAgAG" +
                           "EAbABsACAAZwBvAG8AZAAgAG0AZQBuACAAdABvACAAYwBvAG0AZQA=";
            var b = Encoding.Unicode.GetBytes("Now is the time for all good men to come");

            // Act
            var actual = Base64Encoding.ToString(b);

            // Assert
            Assert.Equal(expected, actual);
        }
        #endregion
        #region -- Supporting Test Classes --
        #endregion
    }
}
