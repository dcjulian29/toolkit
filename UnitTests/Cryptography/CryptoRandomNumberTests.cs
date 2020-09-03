using System;
using System.Collections.Generic;
using ToolKit.Cryptography;
using Xunit;

namespace UnitTests.Cryptography
{
    public class CryptoRandomNumberTests
    {
        [Fact]
        public void Next_Should_RetrunMinValue_When_MaxValueIsEqual()
        {
            // Arrange
            var minValue = 2;
            var maxValue = 2;

            // Act
            var number = CryptoRandomNumber.Next(minValue, maxValue);

            // Assert
            Assert.Equal(minValue, number);
        }

        [Fact]
        public void Next_Should_ReturnNonNegativeNumber()
        {
            // Arrange & Act
            var number = CryptoRandomNumber.Next();

            // Assert
            Assert.True(number > 0);
        }

        [Fact]
        public void Next_Should_ThrowException_When_MaxValueIsLessThanMinValue()
        {
            // Arrange
            var maxValue = 2;
            var minValue = 4;

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(
                () =>
                {
                    var n = CryptoRandomNumber.Next(minValue, maxValue);
                });
        }

        [Fact]
        public void Next_Should_ThrowException_When_NegativeMaxValueAndPositiveMinValue()
        {
            // Arrange
            var maxValue = -1;
            var minValue = 2;

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(
                () =>
                {
                    var n = CryptoRandomNumber.Next(minValue, maxValue);
                });
        }

        [Fact]
        public void Next_Should_ThrowException_When_NegativeMinValueAndPositiveMaxValue()
        {
            // Arrange
            var maxValue = 2;
            var minValue = -1;

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(
                () =>
                {
                    var n = CryptoRandomNumber.Next(minValue, maxValue);
                });
        }

        [Fact]
        public void Next_Should_ThrowException_When_NegativeNumberIsProvidedForMaxValue()
        {
            // Arrange
            var maxValue = -1;

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(
                () =>
                {
                    var n = CryptoRandomNumber.Next(maxValue);
                });
        }

        [Fact]
        public void Next_Sould_ReturnNonNegativeNumberLessThanOrEqualToMaxValue()
        {
            // Arrange
            var maxValue = 1000;

            // Act
            var number = CryptoRandomNumber.Next(maxValue);

            // Assert
            Assert.True((number > 0) && (number <= maxValue));
        }

        [Fact]
        public void Next_Sould_ReturnNonNegativeNumberLessThanOrEqualToMaxValueAndGreaterThanMinValue()
        {
            // Arrange
            var maxValue = 1000;
            var minValue = 900;

            // Act
            var number = CryptoRandomNumber.Next(minValue, maxValue);

            // Assert
            Assert.True((number > 0) && (number <= maxValue));
        }

        [Fact]
        public void NextBytes_Should_ReturnExpectedRandomBytes()
        {
            // Arrange
            var bytes = new byte[6];

            // Act
            CryptoRandomNumber.NextBytes(bytes);

            // Assert
            bytes.Each(b => Assert.True(b > 0));
        }

        [Fact]
        public void NextBytes_Should_ThrowException_When_BytesPassedInIsNull()
        {
            // Arrange
            byte[] bytes = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(
                () =>
                {
                    CryptoRandomNumber.NextBytes(bytes);
                });
        }

        [Fact]
        public void NextDouble_Should_ReturnNonNegativeDoubleNumber()
        {
            // Arrange && Act
            var number = CryptoRandomNumber.NextDouble();

            // Assert
            Assert.True((number > 0.0) && (number < 1.0));
        }
    }
}
