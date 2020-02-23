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
    public class DatabaseBaseTests
    {
        [Fact]
        public void Instance_Should_BeTheExpectedType()
        {
            // Arrange and Act
            _ = new DbTest();

            // Assert
            _ = Assert.IsAssignableFrom<DatabaseBase>(DatabaseBase.Instance);
        }

        [Fact]
        public void UnitTest_Should_BeTrue()
        {
            // Arrange, Act
            _ = new DbTest();

            // Assert
            Assert.True(DatabaseBase.UnitTests);
        }

        public class DbTest : DatabaseBase
        {
            public DbTest()
            {
                UnitTests = true;
                _instance = this;
            }

            public override void InitializeDatabase(Action initialization)
            {
                initialization();
            }
        }
    }
}
