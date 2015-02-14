using System;
using System.Diagnostics.CodeAnalysis;
using ToolKit.Validation;
using Xunit;

namespace UnitTests.Validation
{
    [SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1600:ElementsMustBeDocumented",
        Justification = "Test Suites do not need XML Documentation.")]
    public class CheckTests
    {
        [Fact]
        public void NotEmpty_Should_ReturnValue_When_StringIsNotEmpty()
        {
            // Arrange
            var expected = "A String Value";

            // Act
            var actual = Check.NotEmpty(expected, "name");

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void NotEmpty_Should_ThrowArgumentException_When_StringIsEmpty()
        {
            // Arrange
            var name = String.Empty;

            // Assert
            Assert.Throws<ArgumentException>(() =>
            {
                Check.NotEmpty(name, "name");
            });
        }

        [Fact]
        public void NotNull_Should_ReturnValue_When_NullableTypeHasValue()
        {
            // Arrange
            DateTime? expected = DateTime.Now;

            // Act
            var actual = Check.NotNull(expected, "today");

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void NotNull_Should_ReturnValue_When_ObjectIsNotNull()
        {
            // Arrange
            var expected = "A String Value";

            // Act
            var actual = Check.NotNull(expected, "name");

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void NotNull_Should_ThrowArgumentNullException_When_ObjectIsNull()
        {
            // Arrange
            String name = null;

            // Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                Check.NotNull(name, "name");
            });
        }

        [Fact]
        public void NotNull_Should_ThrowArgumentNullException_When_NullableTypeIsNull()
        {
            // Arrange
            DateTime? today = null;

            // Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                Check.NotNull(today, "today");
            });
        }
    }
}
