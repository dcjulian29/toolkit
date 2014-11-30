using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolKit.Network;
using Xunit;

namespace UnitTests.Network
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Test Suites do not need XML Documentation.")]
    public class Ipv4AddressTests
    {
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
