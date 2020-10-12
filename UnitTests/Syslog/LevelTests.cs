using System.Diagnostics.CodeAnalysis;
using ToolKit.Syslog;
using Xunit;

namespace UnitTests.Syslog
{
    [SuppressMessage(
            "StyleCop.CSharp.DocumentationRules",
            "SA1600:ElementsMustBeDocumented",
            Justification = "Test Suites do not need XML Documentation.")]
    public class LevelTests
    {
        [Fact]
        public void Alert_Should_BeEqualTo1()
        {
            // Act & Assert
            Assert.Equal(1, (int)Level.Alert);
        }

        [Fact]
        public void Critical_Should_BeEqualTo2()
        {
            // Act & Assert
            Assert.Equal(2, (int)Level.Critical);
        }

        [Fact]
        public void Debug_Should_BeEqualTo7()
        {
            // Act & Assert
            Assert.Equal(7, (int)Level.Debug);
        }

        [Fact]
        public void Emergency_Should_BeEqualTo0()
        {
            // Act & Assert
            Assert.Equal(0, (int)Level.Emergency);
        }

        [Fact]
        public void Error_Should_BeEqualTo3()
        {
            // Act & Assert
            Assert.Equal(3, (int)Level.Error);
        }

        [Fact]
        public void Information_Should_BeEqualTo6()
        {
            // Act & Assert
            Assert.Equal(6, (int)Level.Information);
        }

        [Fact]
        public void Notice_Should_BeEqualTo5()
        {
            // Act & Assert
            Assert.Equal(5, (int)Level.Notice);
        }

        [Fact]
        public void Warning_Should_BeEqualTo4()
        {
            // Act & Assert
            Assert.Equal(4, (int)Level.Warning);
        }
    }
}
