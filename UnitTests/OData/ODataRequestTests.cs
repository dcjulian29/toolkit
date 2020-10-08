using System.Diagnostics.CodeAnalysis;
using ToolKit.OData;
using Xunit;

namespace UnitTests.OData
{
    [SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1600:ElementsMustBeDocumented",
        Justification = "Test Suites do not need XML Documentation.")]
    public class ODataRequestTests
    {
        [Fact]
        public void ToString_Should_ReturnCorrectBaseUrl()
        {
            // Arrange
            const string expected = "https://analytics.dev.azure.com/Contoso/Enterprise/_odata/v3.0-preview/";

            var request = new ODataRequest(expected);

            // Act
            var actual = request.ToString();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToString_Should_ReturnCorrectBaseUrl_When_AllQueryParametersUsed()
        {
            // Arrange
            const string baseUrl = "https://analytics.dev.azure.com/Contoso/Enterprise/_odata/v3.0-preview/";
            var expected = $"{baseUrl}WorkItems?$select=WorkItemId,Title,WorkItemType,State,CreatedDate&$filter=startswith(Area/AreaPath,'Enterprise')&$orderby=CreatedDate";

            var request = new ODataRequest(baseUrl);

            // Act
            request.AddProperty("WorkItemId");
            request.AddProperty("Title");
            request.AddProperty("WorkItemType");
            request.AddProperty("State");
            request.AddProperty("CreatedDate");

            request.AddSorting("CreatedDate");

            request.Entity = "WorkItems";

            request.Filter = new ODataFilter().StartsWith("Area/AreaPath", "Enterprise");

            var actual = request.ToString();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToString_Should_ReturnCorrectString_When_UrlIsNotProvided()
        {
            // Arrange
            const string expected = "WorkItems?$select=WorkItemId,Title,State,CreatedDate&$orderby=CreatedDate";

            var request = new ODataRequest();

            // Act
            request.AddProperty("WorkItemId");
            request.AddProperty("Title");
            request.AddProperty("State");
            request.AddProperty("CreatedDate");

            request.AddSorting("CreatedDate");

            request.Entity = "WorkItems";

            var actual = request.ToString();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToString_Should_ReturnCorrectURL_When_DesendingSortingIsUsed()
        {
            var expected = "https://analytics.dev.azure.com/Contoso/Enterprise/_odata/v3.0-preview/";

            var request = new ODataRequest(expected);

            expected = $"{expected}?$orderby=CreatedDate desc";

            // Act
            request.AddSorting("CreatedDate", true);

            var actual = request.ToString();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToString_Should_ReturnCorrectURL_When_DuplicateSortingIsUsed()
        {
            var expected = "https://analytics.dev.azure.com/Contoso/Enterprise/_odata/v3.0-preview/";

            var request = new ODataRequest(expected);

            expected = $"{expected}?$orderby=CreatedDate";

            // Act
            request.AddSorting("CreatedDate");
            request.AddSorting("CreatedDate");

            var actual = request.ToString();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToString_Should_ReturnCorrectURL_When_EntityIsUsed()
        {
            var expected = "https://analytics.dev.azure.com/Contoso/Enterprise/_odata/v3.0-preview/";

            var request = new ODataRequest(expected);

            expected = $"{expected}WorkItems";

            // Act
            request.Entity = "WorkItems";

            var actual = request.ToString();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToString_Should_ReturnCorrectURL_When_FilterIsUsed()
        {
            var expected = "https://analytics.dev.azure.com/Contoso/Enterprise/_odata/v3.0-preview/";

            var request = new ODataRequest(expected);

            expected = $"{expected}?$filter=startswith(Area/AreaPath,'Enterprise')";

            // Act
            request.Filter = new ODataFilter().StartsWith("Area/AreaPath", "Enterprise");

            var actual = request.ToString();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToString_Should_ReturnCorrectURL_When_MultipleSortingIsUsed()
        {
            var expected = "https://analytics.dev.azure.com/Contoso/Enterprise/_odata/v3.0-preview/";

            var request = new ODataRequest(expected);

            expected = $"{expected}?$orderby=WorkItemType,CreatedDate desc";

            // Act
            request.AddSorting("WorkItemType");
            request.AddSorting("CreatedDate", true);

            var actual = request.ToString();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToString_Should_ReturnCorrectURL_When_PropertiesIsUsed()
        {
            var expected = "https://analytics.dev.azure.com/Contoso/Enterprise/_odata/v3.0-preview/";

            var request = new ODataRequest(expected);

            expected = $"{expected}?$select=WorkItemId,Title,WorkItemType,State,CreatedDate";

            // Act
            request.AddProperty("WorkItemId");
            request.AddProperty("Title");
            request.AddProperty("WorkItemType");
            request.AddProperty("State");
            request.AddProperty("CreatedDate");

            var actual = request.ToString();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToString_Should_ReturnCorrectURL_When_SamePropertiesIsAddedTwice()
        {
            var expected = "https://analytics.dev.azure.com/Contoso/Enterprise/_odata/v3.0-preview/";

            var request = new ODataRequest(expected);

            expected = $"{expected}?$select=WorkItemId,Title";

            // Act
            request.AddProperty("WorkItemId");
            request.AddProperty("Title");
            request.AddProperty("WorkItemId");

            var actual = request.ToString();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToString_Should_ReturnCorrectURL_When_SortingIsUsed()
        {
            var expected = "https://analytics.dev.azure.com/Contoso/Enterprise/_odata/v3.0-preview/";

            var request = new ODataRequest(expected);

            expected = $"{expected}?$orderby=CreatedDate";

            // Act
            request.AddSorting("CreatedDate");

            var actual = request.ToString();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToString_Should_ReturnCorrectValue_When_SkippingEntities()
        {
            // Arrange
            var expected = "https://analytics.dev.azure.com/Contoso/Enterprise/_odata/v3.0-preview/";

            var request = new ODataRequest(expected);

            expected = $"{expected}?$skip=200";

            // Act
            request.Skip(200);
            var actual = request.ToString();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToString_Should_ReturnCorrectValue_When_TakingAndSkippingEntities()
        {
            // Arrange
            var expected = "https://analytics.dev.azure.com/Contoso/Enterprise/_odata/v3.0-preview/";

            var request = new ODataRequest(expected);

            expected = $"{expected}?$top=15&$skip=200";

            // Act
            request.Take(15);
            request.Skip(200);
            var actual = request.ToString();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToString_Should_ReturnCorrectValue_When_TakingEntities()
        {
            // Arrange
            var expected = "https://analytics.dev.azure.com/Contoso/Enterprise/_odata/v3.0-preview/";

            var request = new ODataRequest(expected);

            expected = $"{expected}?$top=15";

            // Act
            request.Take(15);
            var actual = request.ToString();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToString_Should_ReturnNullWhenNotInitialized()
        {
            // Arrange
            var request = new ODataRequest();

            // Act
            var actual = request.ToString();

            // Assert
            Assert.True(string.IsNullOrWhiteSpace(actual));
        }

        [Fact]
        public void Url_Should_ReturnCorrectValue()
        {
            // Arrange
            const string expected = "https://analytics.dev.azure.com/Contoso/Enterprise/_odata/v3.0-preview/";

            var request = new ODataRequest(expected);

            // Act
            var actual = request.Url;

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
