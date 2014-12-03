using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolKit.Network;
using Xunit;

namespace UnitTests.Network
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1600:ElementsMustBeDocumented",
        Justification = "Test Suites do not need XML Documentation.")]
    public class IpV4AddressTests
    {
        [Fact]
        public void CompareTo_Should_ReturnLessThan_When_AddressesIsLess()
        {
            // Arrange
            var address1 = new IpV4Address(192, 168, 103, 80);
            var address2 = new IpV4Address(192, 168, 103, 81);
            var expected = -1;

            // Act
            var actual = address1.CompareTo(address2);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CompareTo_Should_ReturnGreaterThan_When_AddressesIsGreater()
        {
            // Arrange
            var address1 = new IpV4Address(192, 168, 80, 80);
            var address2 = new IpV4Address(192, 168, 103, 80);
            var expected = 1;

            // Act
            var actual = address2.CompareTo(address1);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CompareTo_Should_ReturnEqual_When_AddressesIsEqual()
        {
            // Arrange
            var address1 = new IpV4Address(192, 168, 103, 80);
            var address2 = new IpV4Address(192, 168, 103, 80);
            var expected = 0;

            // Act
            var actual = address1.CompareTo(address2);

            // Assert
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void Constructor_Should_ThrowArgumentException_When_ProvidedWithInvalidOctets()
        {
            Assert.Throws<ArgumentException>(() =>
                {
                    var ip = new IpV4Address(192, 168, 267, 345);
                });
        }

        [Fact]
        public void Constructor_Should_ThrowArgumentException_When_ProvidedWithInvalidOctetsInString()
        {
            Assert.Throws<ArgumentException>(() =>
                {
                    var ip = new IpV4Address("192.168.267.345");
                });
        }

        [Fact]
        public void Constructor_Should_ThrowArgumentException_When_ProvidedWithTooFewOctetsInString()
        {
            Assert.Throws<ArgumentException>(() =>
                {
                    var ip = new IpV4Address("192.168.1");
                });
        }

        [Fact]
        public void Constructor_Should_ThrowArgumentException_When_ProvidedWithTooManyOctetsInString()
        {
            Assert.Throws<ArgumentException>(() =>
                {
                    var ip = new IpV4Address("192.168.0.1.3");
                });
        }

        [Fact]
        public void Equals_Should_ReturnTrue_When_BothAddressesAreEqual()
        {
            // Arrange
            var address1 = new IpV4Address(192, 168, 103, 80);
            var address2 = new IpV4Address(192, 168, 103, 80);

            // Act
            var actual = address1 == address2;

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void Equals_Should_ReturnFalse_When_BothAddressesAreNotEqual()
        {
            // Arrange
            var address1 = new IpV4Address(192, 168, 103, 80);
            var address2 = new IpV4Address(192, 168, 103, 81);

            // Act
            var actual = address1 == address2;

            // Assert
            Assert.False(actual);
        }

        [Fact]
        public void ToBinary_Should_ReturnCorrectBinary()
        {
            // Arrange
            var ip = new IpV4Address("192.168.250.123");
            var expected = "11000000101010001111101001111011";

            // Act
            var actual = ip.ToBinary();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToBinary_Should_ReturnCorrectBinaryWithPeriod()
        {
            // Arrange
            var ip = new IpV4Address("192.168.250.123");
            var expected = "11000000.10101000.11111010.01111011";

            // Act
            var actual = ip.ToBinary(true);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ToString_Should_ReturnCorrectString()
        {
            // Arrange
            var ip = new IpV4Address(192, 168, 250, 123);
            var expected = "192.168.250.123";

            // Act
            var actual = ip.ToString();

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
