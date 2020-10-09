using System;
using System.Diagnostics.CodeAnalysis;
using ToolKit.Syslog;
using Xunit;

namespace UnitTests.Syslog
{
    [SuppressMessage(
            "StyleCop.CSharp.DocumentationRules",
            "SA1600:ElementsMustBeDocumented",
            Justification = "Test Suites do not need XML Documentation.")]
    public class MessageTests
    {
        [Fact]
        public void Facility_Should_BeSystem5_When_ConstructorWithMessageUsed()
        {
            // Arrange
            const Facility expected = Facility.User;
            var msg = new Message(Guid.NewGuid().ToString());

            // Act
            var actual = msg.Facility;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Facility_Should_BeSystem5_When_ConstructorWithValuesUsed()
        {
            // Arrange
            const Facility expected = Facility.System5;
            var msg = new Message(Facility.System5, Level.Emergency, "Message");

            // Act
            var actual = msg.Facility;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Facility_Should_BeUser_When_DefaultConstructorUsed()
        {
            // Arrange
            const Facility expected = Facility.User;
            var msg = new Message();

            // Act
            var actual = msg.Facility;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Facility_Should_BeUser_When_LevelIsChanged()
        {
            // Arrange
            const Facility expected = Facility.Cron;
            var msg = new Message
            {
                Facility = Facility.Cron
            };

            // Act
            var actual = msg.Facility;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Level_Should_BeEmergency_When_ConstructorWithMessageUsed()
        {
            // Arrange
            const Level expected = Level.Information;
            var msg = new Message(Guid.NewGuid().ToString());

            // Act
            var actual = msg.Level;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Level_Should_BeEmergency_When_ConstructorWithValuesUsed()
        {
            // Arrange
            const Level expected = Level.Emergency;
            var msg = new Message(Facility.System5, Level.Emergency, "Message");

            // Act
            var actual = msg.Level;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Level_Should_BeUser_When_DefaultConstructorUsed()
        {
            // Arrange
            const Level expected = Level.Information;
            var msg = new Message();

            // Act
            var actual = msg.Level;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Level_Should_BeUser_When_LevelIsChanged()
        {
            // Arrange
            const Level expected = Level.Alert;
            var msg = new Message
            {
                Level = Level.Alert
            };

            // Act
            var actual = msg.Level;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Text_Should_BeEmpty_When_DefaultConstructorUsed()
        {
            // Arrange
            var expected = string.Empty;
            var msg = new Message();

            // Act
            var actual = msg.Text;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Text_Should_BeExpected_When_ConstructorWithMessageUsed()
        {
            // Arrange
            var expected = Guid.NewGuid().ToString();
            var msg = new Message(expected);

            // Act
            var actual = msg.Text;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Text_Should_BeExpected_When_ConstructorWithValuesUsed()
        {
            // Arrange
            var expected = Guid.NewGuid().ToString();
            var msg = new Message(Facility.System5, Level.Emergency, expected);

            // Act
            var actual = msg.Text;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToString_Should_BeExpected_When_ConstructorWithMessageUsed()
        {
            // Arrange
            var expected = Guid.NewGuid().ToString();
            var msg = new Message(expected);

            // Act
            var actual = msg.ToString();

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
