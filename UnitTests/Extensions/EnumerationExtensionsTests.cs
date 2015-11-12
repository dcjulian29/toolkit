using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace UnitTests.Extensions
{
    [SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1600:ElementsMustBeDocumented",
        Justification = "Test Suites do not need XML Documentation.")]
    public class EnumerationExtensionsTests
    {
        private enum Color
        {
            Red,
            Blue,
            Green
        }

        [Flags]
        private enum Groups
        {
            Global = 0x00000002,
            Domain = 0x00000004,
            Local = 0x00000004,
            Universal = 0x00000008
        }

        [Flags]
        private enum UserFlag
        {
            Disabled = 0x2,
            LockedOut = 0x10,
            Password = 0x20,
            Encrypted = 0x80,
            Duplicate = 0x100,
            Normal = 0x200,
            Workstation = 0x1000,
            Trusted = 0x80000,
            NotRequired = 0x100000,
            Delagated = 0x1000000
        }

        [Fact]
        public void ClearFlag_Should_ClearTheFlag_When_FlagExist()
        {
            // Arrange
            var userFlag = UserFlag.Disabled | UserFlag.LockedOut;

            // Act
            userFlag = userFlag.ClearFlag(UserFlag.Disabled);

            // Assert
            Assert.False(userFlag.Contains(UserFlag.Disabled));
        }

        [Fact]
        public void ClearFlag_Should_ThrowInvalidOperationException_When_EnumarationFlagDoesNotExist()
        {
            // Arrange
            var userFlag = UserFlag.Disabled | UserFlag.LockedOut;

            // Act

            // Assert
            Assert.Throws<InvalidOperationException>(() =>
            {
                userFlag.ClearFlag(Groups.Domain);
            });
        }

        [Fact]
        public void ClearFlag_Should_ThrowInvalidOperationException_When_ValueOfFlagDoesNotExist()
        {
            // Arrange
            var userFlag = UserFlag.Disabled | UserFlag.LockedOut;

            // Act

            // Assert
            Assert.Throws<InvalidOperationException>(() =>
            {
                userFlag = (UserFlag)userFlag.ClearFlag(0x2000000);
            });
        }

        [Fact]
        public void Contains_Should_ReturnFalse_When_FlagDoesNotExist()
        {
            // Arrange
            var userFlag = UserFlag.Disabled | UserFlag.LockedOut;

            // Act

            // Assert
            Assert.False(userFlag.Contains(UserFlag.Password));
        }

        [Fact]
        public void Contains_Should_ReturnTrue_When_FlagDoesExist()
        {
            // Arrange
            var userFlag = UserFlag.Disabled | UserFlag.LockedOut;

            // Act

            // Assert
            Assert.True(userFlag.Contains(UserFlag.Disabled));
        }

        [Fact]
        public void Contains_Should_ThrowArgumentException_When_FlagDoesNotExistsInEnumeration()
        {
            // Arrange
            var userFlag = UserFlag.Disabled | UserFlag.LockedOut;

            // Act

            // Assert
            Assert.Throws<ArgumentException>(() =>
            {
                var result = userFlag.Contains("noEnum");
            });
        }

        [Fact]
        public void Contains_Should_ThrowInvalidOperationException_When_EnumerationTypeIsDifferent()
        {
            // Arrange
            var userFlag = UserFlag.Disabled | UserFlag.LockedOut;

            // Act

            // Assert
            Assert.Throws<InvalidOperationException>(() =>
            {
                var result = userFlag.Contains(Groups.Domain);
            });
        }

        [Fact]
        public void Is_Should_ReturnFalse_When_ProvidedEnumerationDoesNotEqualEnumeration()
        {
            // Arrange
            var color = Color.Blue;

            // Act

            // Assert
            Assert.False(color.Is(Color.Red));
        }

        [Fact]
        public void Is_Should_ReturnTrue_When_ProvidedEnumerationDoesEqualEnumeration()
        {
            // Arrange
            var color = Color.Blue;

            // Act

            // Assert
            Assert.True(color.Is(Color.Blue));
        }

        [Fact]
        public void Is_Should_ThrowArgumentException_When_EnumerationValueDoesNotExist()
        {
            // Arrange
            var color = Color.Blue;

            // Act

            // Assert
            Assert.Throws<ArgumentException>(() =>
            {
                var result = color.Is("-1");
            });
        }

        [Fact]
        public void Is_Should_ThrowInvalidOperationException_When_EnumerationIsNotSameType()
        {
            // Arrange
            var userFlag = UserFlag.Disabled | UserFlag.LockedOut;

            // Act

            // Assert
            Assert.Throws<InvalidOperationException>(() =>
            {
                var result = userFlag.Contains(Color.Green);
            });
        }

        [Fact]
        public void SetFlag_Should_SetTheFlag_When_FlagExistInEnumeration()
        {
            // Arrange
            var userFlags = UserFlag.Normal;

            // Act
            userFlags = userFlags.SetFlag(UserFlag.NotRequired);

            // Assert
            Assert.True(userFlags.Contains(UserFlag.NotRequired));
        }

        [Fact]
        public void SetFlag_Should_ThrowInvalidOperationException_When_EnumerationTypeIsDifferent()
        {
            // Arrange
            var userFlags = UserFlag.Normal;

            // Act

            // Assert
            Assert.Throws<InvalidOperationException>(() =>
            {
                userFlags.SetFlag(Color.Blue);
            });
        }

        [Fact]
        public void SetFlag_Should_ThrowInvalidOperationException_When_ValueOfFlagDoesNotExist()
        {
            // Arrange
            var userFlags = UserFlag.Normal;

            // Act

            // Assert
            Assert.Throws<InvalidOperationException>(() =>
            {
                userFlags.SetFlag("nonenum");
            });
        }
    }
}
