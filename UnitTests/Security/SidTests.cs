using System.Diagnostics.CodeAnalysis;
using ToolKit.Security;
using Xunit;

namespace UnitTests.Security
{
    [SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1600:ElementsMustBeDocumented",
        Justification = "Test Suites do not need XML Documentation.")]
    public class SidTests
    {
        [Fact]
        public void Administrators_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "S-1-5-32-544";

            // Act
            var actual = Sid.Administrators;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AnonymousLoggedonUser_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "S-1-5-7";

            // Act
            var actual = Sid.AnonymousLoggedonUser;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AuthenticatedUsers_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "S-1-5-11";

            // Act
            var actual = Sid.AuthenticatedUsers;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void BuiltinGuests_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "S-1-5-32-546";

            // Act
            var actual = Sid.BuiltinGuests;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void BuiltinUser_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "S-1-5-32-545";

            // Act
            var actual = Sid.BuiltinUser;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void InteractiveUsers_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "S-1-5-4";

            // Act
            var actual = Sid.InteractiveUsers;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void LocalService_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "S-1-5-19";

            // Act
            var actual = Sid.LocalService;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void NetworkLogonUser_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "S-1-5-2";

            // Act
            var actual = Sid.NetworkLogonUser;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void NetworkService_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "S-1-5-20";

            // Act
            var actual = Sid.NetworkService;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void System_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "S-1-5-18";

            // Act
            var actual = Sid.System;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TerminalServerUsers_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "S-1-5-13";

            // Act
            var actual = Sid.TerminalServerUsers;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToBinary_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = new byte[]
            {
                0x01, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x05,
                0x15, 0x00, 0x00, 0x00, 0x2E, 0x43, 0xAC, 0x40,
                0xC0, 0x85, 0x38, 0x5D, 0x07, 0xE5, 0x3B, 0x2B
            };

            var sid = "S-1-5-21-1085031214-1563985344-725345543";

            // Act
            var actual = Sid.ToBinary(sid);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToHex_Should_ReturnExpectedResult_When_ByteProvided()
        {
            // Arrange
            var expected = "0104000000000005150000002E43AC40C085385D07E53B2B";
            var sid = new byte[]
            {
                0x01, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x05,
                0x15, 0x00, 0x00, 0x00, 0x2E, 0x43, 0xAC, 0x40,
                0xC0, 0x85, 0x38, 0x5D, 0x07, 0xE5, 0x3B, 0x2B
            };

            // Act
            var actual = Sid.ToHex(sid);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToHex_Should_ReturnExpectedResult_When_StringProvided()
        {
            // Arrange
            var expected = "0104000000000005150000002E43AC40C085385D07E53B2B";
            var sid = "S-1-5-21-1085031214-1563985344-725345543";

            // Act
            var actual = Sid.ToHex(sid);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToName_Should_ReturnExpectedResult_When_ByteProvided()
        {
            // Arrange
            var expected = @"NT AUTHORITY\INTERACTIVE";
            var sid = new byte[] { 1, 1, 0, 0, 0, 0, 0, 5, 4, 0, 0, 0 };

            // Act
            var actual = Sid.ToName(sid);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToName_Should_ReturnExpectedResult_When_StringProvided()
        {
            // Arrange
            var expected = @"NT AUTHORITY\INTERACTIVE";
            var sid = "S-1-5-4";

            // Act
            var actual = Sid.ToName(sid);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToString_Should_ReturnExpectedResult_When_ByteProvided()
        {
            // Arrange
            var expected = "S-1-5-21-1085031214-1563985344-725345543";
            var sid = new byte[]
            {
                0x01, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x05,
                0x15, 0x00, 0x00, 0x00, 0x2E, 0x43, 0xAC, 0x40,
                0xC0, 0x85, 0x38, 0x5D, 0x07, 0xE5, 0x3B, 0x2B
            };

            // Act
            var actual = Sid.ToString(sid);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void World_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "S-1-1-0";

            // Act
            var actual = Sid.World;

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
