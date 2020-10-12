using System;
using System.Diagnostics.CodeAnalysis;
using ToolKit.Data;
using Xunit;

namespace UnitTests.Data
{
    [SuppressMessage(
           "StyleCop.CSharp.DocumentationRules",
           "SA1600:ElementsMustBeDocumented",
           Justification = "Test Suites do not need XML Documentation.")]
    public class NameValueTests
    {
        [Fact]
        public void ToString_Should_ReturnExpectedResult_When_ConstructorUsed()
        {
            // Arrange
            const string expected = "TestName=TestValue";
            var pair = new NameValue("TestName", "TestValue");

            // Act
            var actual = pair.ToString();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToString_Should_ReturnExpectedResult_When_PropertiesUsed()
        {
            // Arrange
            const string expected = "TestName=TestValue";
            var pair = new NameValue()
            {
                Name = "TestName",
                Value = "TestValue"
            };

            // Act
            var actual = pair.ToString();

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
