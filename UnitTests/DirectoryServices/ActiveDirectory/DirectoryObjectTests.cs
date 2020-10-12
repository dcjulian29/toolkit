using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ToolKit.DirectoryServices.ActiveDirectory;
using Xunit;

namespace UnitTests.DirectoryServices.ActiveDirectory
{
    [SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1600:ElementsMustBeDocumented",
        Justification = "Test Suites do not need XML Documentation.")]
    public class DirectoryObjectTests
    {
        [Fact]
        public void NumberOfProperties_Should_ReturnTwo_When_TwoPropertiesExists()
        {
            // Arrange
            var expected = 2;

            var properties = new Dictionary<string, object>
            {
                { "name", "testObject" },
                { "type", 32 }
            };

            var obj = new DirectoryObject(properties);

            // Act
            var actual = obj.NumberOfProperties;

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
