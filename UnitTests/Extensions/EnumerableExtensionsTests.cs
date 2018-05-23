using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace UnitTests.Extensions
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1600:ElementsMustBeDocumented",
        Justification = "Test Suites do not need XML Documentation.")]
    public class EnumerableExtensionsTests
    {
        [Fact]
        public void Each_Should_ExecuteAction()
        {
            // Arrange
            IEnumerable<char> enumerable = new[] { 'a', 'b', 'c' };
            var sb = new StringBuilder();

            // Act
            enumerable.Each(c =>
            {
                sb.Append("1");
                sb.Append("2");
            });

            // Assert
            Assert.Equal("121212", sb.ToString());
        }

        [Fact]
        public void Each_Should_IterateOverEachItem()
        {
            // Arrange
            IEnumerable<char> enumerable = new[] { 'a', 'b', 'c' };
            var sb = new StringBuilder();

            // Act
            enumerable.Each(c => sb.Append(c));

            // Assert
            Assert.Equal("abc", sb.ToString());
        }
    }
}
