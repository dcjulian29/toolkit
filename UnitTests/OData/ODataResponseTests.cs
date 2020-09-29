using System.Diagnostics.CodeAnalysis;
using System.IO;
using ToolKit.OData;
using Xunit;

namespace UnitTests.OData
{
    [SuppressMessage(
            "StyleCop.CSharp.DocumentationRules",
            "SA1600:ElementsMustBeDocumented",
            Justification = "Test Suites do not need XML Documentation.")]
    public class ODataResponseTests
    {
        [Fact]
        public void Context_Should_ContainExpectedValue()
        {
            // Arrange
            const string expected
                = "https://analytics.dev.azure.com/Contoso/Enterprise/_odata/v3.0-preview/$metadata#WorkItems";
            var json = File.ReadAllText("odata.response.workitem.json");

            // Act
            var actual = ODataResponse.Create(json);

            // Assert
            Assert.Equal(expected, actual.Context);
        }

        [Fact]
        public void Status_Should_ContainExpectedValue()
        {
            // Arrange
            var json = File.ReadAllText("odata.response.workitem.json");

            // Act
            var actual = ODataResponse.Create(json);

            // Assert
            Assert.Equal(200, actual.StatusCode);
        }

        [Fact]
        public void Values_Should_ContainExpectedNumberOfValues()
        {
            // Arrange
            var json = File.ReadAllText("odata.response.workitem.json");

            // Act
            var actual = ODataResponse.Create(json);

            // Assert
            Assert.Equal(10, actual.Value.Count);
        }

        [Fact]
        public void Warning_Should_ContainExpectedNumberOfValues()
        {
            // Arrange
            var json = File.ReadAllText("odata.response.workitem.json");

            // Act
            var actual = ODataResponse.Create(json);

            // Assert
            Assert.Single(actual.Warnings);
        }
    }
}
