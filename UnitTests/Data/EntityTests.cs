using System;
using ToolKit.Data;
using Xunit;

namespace UnitTests.Data
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1600:ElementsMustBeDocumented",
        Justification = "Test Suites do not need XML Documentation.")]
    public class EntityTests
    {
        [Fact]
        public void EntityIdType_Should_BeAnInteger()
        {
            // Arrange
            var entity = new Car();

            // Act

            // Assert
            Assert.IsType(typeof(Int32), entity.Id);
        }

        internal class Car : Entity
        {

        }
    }
}
