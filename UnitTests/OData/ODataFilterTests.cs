using System;
using System.Diagnostics.CodeAnalysis;
using ToolKit.OData;
using Xunit;

namespace UnitTests.OData
{
    [SuppressMessage(
           "StyleCop.CSharp.DocumentationRules",
           "SA1600:ElementsMustBeDocumented",
           Justification = "Test Suites do not need XML Documentation.")]
    public class ODataFilterTests
    {
        [Fact]
        public void And_Should_ReturnExpectedString()
        {
            // Arrange
            const string expected = "(Address eq 'Redmond') and (Enrolled eq 1)";
            var filter1 = new ODataFilter()
                .EqualTo("Address", "Redmond");
            var filter2 = new ODataFilter()
                .EqualTo("Enrolled", "1");

            // Act
            filter1 = filter1.And(filter2);
            var actual = filter1.ToString();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ComplexFilter_Should_ReturnExpectedString()
        {
            // Arrange

            const string expected = "(not ((startswith(Title, 'Contoso')) and ((Age gt 5) and (State eq 'Open')))) or (round(price) eq 42)";

            var filter = new ODataFilter()
                .StartsWith("Title", "Contoso")
                .And(new ODataFilter()
                    .GreaterThan("Age", 5)
                    .EqualTo("State", "Open"))
                .Not()
                .Or(new ODataFilter("round(price) eq 42"));

            // Act
            var actual = filter.ToString();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Concat_Should_ReturnExpectedString()
        {
            // Arrange
            const string expected = "concat(FirstName, LastName) eq 'JulianEasterling'";
            var filter = new ODataFilter()
                .Concat("FirstName", "LastName", "JulianEasterling");

            // Act
            var actual = filter.ToString();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Contains_Should_ReturnExpectedString()
        {
            // Arrange
            const string expected = "contains(Title, 'Contoso')";
            var filter = new ODataFilter()
                .Contains("Title", "Contoso");

            // Act
            var actual = filter.ToString();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void EndsWith_Should_ReturnExpectedString()
        {
            // Arrange
            const string expected = "endswith(Title, 'Contoso')";
            var filter = new ODataFilter()
                .EndsWith("Title", "Contoso");

            // Act
            var actual = filter.ToString();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Equallto_Should_ReturnExpectedString()
        {
            // Arrange
            const string expected = "Address eq 'Redmond'";
            var filter = new ODataFilter()
                .EqualTo("Address", "Redmond");

            // Act
            var actual = filter.ToString();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void EqualTo_Should_ReturnExpectedString_When_MultipleExpressionsAreUsed()
        {
            // Arrange
            const string expected = "(Address eq 'Redmond') and (Enrolled eq 1)";
            var filter = new ODataFilter()
                .EqualTo("Address", "Redmond")
                .EqualTo("Enrolled", "1");

            // Act
            var actual = filter.ToString();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void EqualTo_Should_ReturnExpectedString_When_NumberAsStringIsUsed()
        {
            // Arrange
            const string expected = "Enrolled eq 1";
            var filter = new ODataFilter()
                .EqualTo("Enrolled", "1");

            // Act
            var actual = filter.ToString();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void EqualTo_Should_ReturnExpectedString_When_NumberIsUsed()
        {
            // Arrange
            const string expected = "Id eq 45";
            var filter = new ODataFilter()
                .EqualTo("Id", 45);

            // Act
            var actual = filter.ToString();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GreaterThan_Should_ReturnExpectedString()
        {
            // Arrange
            const string expected = "Age gt 17";
            var filter = new ODataFilter()
                .GreaterThan("Age", 17);

            // Act
            var actual = filter.ToString();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GreaterThanOrEqual_Should_ReturnExpectedString()
        {
            // Arrange
            const string expected = "Age ge 65";
            var filter = new ODataFilter()
                .GreaterThanOrEqual("Age", 65);

            // Act
            var actual = filter.ToString();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void IndexOf_Should_ReturnExpectedString()
        {
            // Arrange
            const string expected = "indexof(Title, 'Contoso') eq 5";
            var filter = new ODataFilter()
                .IndexOf("Title", "Contoso", 5);

            // Act
            var actual = filter.ToString();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void LessThan_Should_ReturnExpectedString()
        {
            // Arrange
            const string expected = "Age lt 18";
            var filter = new ODataFilter()
                .LessThan("Age", 18);

            // Act
            var actual = filter.ToString();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void LessThanOrEqual_Should_ReturnExpectedString()
        {
            // Arrange
            const string expected = "Age le 13";
            var filter = new ODataFilter()
                .LessThanOrEqual("Age", 13);

            // Act
            var actual = filter.ToString();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Not_Should_ReturnExpectedString()
        {
            // Arrange
            const string expected = "not (Address eq 'Redmond')";
            var filter = new ODataFilter()
                .EqualTo("Address", "Redmond").Not();

            // Act
            var actual = filter.ToString();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void NotEqualTo_Should_ReturnExpectedString()
        {
            // Arrange
            const string expected = "Address ne 'Redmond'";
            var filter = new ODataFilter()
                .NotEqualTo("Address", "Redmond");

            // Act
            var actual = filter.ToString();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void NotEqualTo_Should_ReturnExpectedString_When_NumberIsUsed()
        {
            // Arrange
            const string expected = "Id ne 45";
            var filter = new ODataFilter()
                .NotEqualTo("Id", 45);

            // Act
            var actual = filter.ToString();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Or_Should_ReturnExpectedString()
        {
            // Arrange
            const string expected = "(Address eq 'Redmond') or (Enrolled eq 1)";
            var filter1 = new ODataFilter()
                .EqualTo("Address", "Redmond");
            var filter2 = new ODataFilter()
                .EqualTo("Enrolled", "1");

            // Act
            filter1 = filter1.Or(filter2);
            var actual = filter1.ToString();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void StartsWith_Should_ReturnExpectedString()
        {
            // Arrange
            const string expected = "startswith(Title, 'Contoso')";
            var filter = new ODataFilter()
                .StartsWith("Title", "Contoso");

            // Act
            var actual = filter.ToString();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Substring_Should_ReturnExpectedString()
        {
            // Arrange
            const string expected = "substringof('Contoso', Title) eq true";
            var filter = new ODataFilter()
                .Substring("Title", "Contoso");

            // Act
            var actual = filter.ToString();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToLower_Should_ReturnExpectedString()
        {
            // Arrange
            const string expected = "tolower(Title) eq 'contoso'";
            var filter = new ODataFilter()
                .ToLower("Title", "Contoso");

            // Act
            var actual = filter.ToString();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToString_Should_ReturnEmptyStringWhenFilterIsEmpty()
        {
            // Arrange
            var filter = new ODataFilter();

            // Act
            var actual = filter.ToString();

            // Assert
            Assert.True(string.IsNullOrWhiteSpace(actual));
        }

        [Fact]
        public void ToString_Should_ReturnExpectedString_When_ComplexExpressionIsUsed()
        {
            // Arrange
            const string expected = "((Address eq 'Redmond') and (Enrolled ne 1)) and (id eq 1)";
            var filter = new ODataFilter()
                .EqualTo("Address", "Redmond")
                .NotEqualTo("Enrolled", "1");
            var filter1 = new ODataFilter().EqualTo("id", "1");

            // Act
            filter = filter.And(filter1);

            var actual = filter.ToString();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToString_Should_ReturnExpectedString_When_RawFilterProvided()
        {
            // Arrange
            const string expected = "length(Name) gt 10";
            var filter = new ODataFilter("length(Name) gt 10");

            // Act
            var actual = filter.ToString();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToUpper_Should_ReturnExpectedString()
        {
            // Arrange
            const string expected = "toupper(Title) eq 'CONTOSO'";
            var filter = new ODataFilter()
                .ToUpper("Title", "Contoso");

            // Act
            var actual = filter.ToString();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Trim_Should_ReturnExpectedString()
        {
            // Arrange
            const string expected = "trim(Title) eq 'Contoso'";
            var filter = new ODataFilter()
                .Trim("Title", "Contoso");

            // Act
            var actual = filter.ToString();

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
