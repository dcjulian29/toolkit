using System;
using System.Diagnostics.CodeAnalysis;
using ToolKit.DirectoryServices.ActiveDirectory;
using Xunit;

namespace UnitTests.DirectoryServices.ActiveDirectory
{
    [SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1600:ElementsMustBeDocumented",
        Justification = "Test Suites do not need XML Documentation.")]
    public class AdDateTimeTests
    {
        [Fact]
        public void ToADsLargeInteger_Should_ReturnExpectedResult()
        {
            // Arrange
            var date = new DateTime(2015, 10, 15, 10, 32, 00, DateTimeKind.Utc);
            var expected = new LargeInteger()
            {
                HighPart = 30476084,
                LowPart = -1185116160
            };

            // Act
            var actual = AdDateTime.ToADsLargeInteger(date);

            // Assert
            Assert.Equal(expected.HighPart, actual.HighPart);
            Assert.Equal(expected.LowPart, actual.LowPart);
        }

        [Fact]
        public void ToDateTime_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = new DateTime(2015, 10, 15, 10, 32, 00, DateTimeKind.Utc);
            var date = new LargeInteger()
            {
                HighPart = 30476084,
                LowPart = -1185116160
            };

            // Act
            var actual = AdDateTime.ToDateTime(date);

            // Assert
            Assert.Equal(expected.ToString("s"), actual.ToString("s"));
        }

        [Fact]
        public void ToDateTime_Should_ReturnMaxDate_When_ProvidedWithLargeIntegerThatIsAllOnes()
        {
            // Arrange
            var expected = DateTime.MaxValue;
            var date = new LargeInteger()
            {
                HighPart = 2147483647,
                LowPart = -1
            };

            // Act
            var actual = AdDateTime.ToDateTime(date);

            // Assert
            Assert.Equal(expected.ToString("s"), actual.ToString("s"));
        }

        [Fact]
        public void ToLdapDateTimeShould_ReturnExpectedResult()
        {
            // Arrange
            var expected = "20151015103200.0Z";
            var date = new DateTime(2015, 10, 15, 10, 32, 00);

            // Act
            var actual = AdDateTime.ToLdapDateTime(date);

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
