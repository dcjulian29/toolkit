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
    public class IpV4NetworkTests
    {
        [Fact]
        public void Address_Should_ReturnCorrectAddress()
        {
            // Arrange
            var expected = new IpV4Address(192, 169, 103, 129);

            // Act
            var network = new IpV4Network(expected, new IpV4Address(255, 255, 255, 0));

            // Assert
            Assert.Equal(expected, network.Address);
        }

        [Fact]
        public void Bitmask_Should_ReturnCorrectBitMask()
        {
            // Arrange
            var expected = 24;
            var mask = new IpV4Address(255, 255, 255, 0);

            // Act
            var network = new IpV4Network(new IpV4Address(192, 169, 103, 129), mask);

            // Assert
            Assert.Equal(expected, network.Bitmask);
        }

        [Fact]
        public void Broadcast_Should_ReturnCorrectBroadcast()
        {
            // Arrange
            var expected = new IpV4Address(192, 168, 59, 15);
            var address = new IpV4Address(192, 168, 59, 13);
            var mask = new IpV4Address(255, 255, 255, 240);

            // Act
            var network = new IpV4Network(address, mask);

            // Assert
            Assert.Equal(expected, network.Broadcast);
        }

        [Fact]
        public void Constructor_Should_ConvertMaskIntoAppropiateMask_When_MaskInFirstOctet()
        {
            // Arrange
            var mask = 4;
            var address = new IpV4Address(4, 120, 0, 1);
            var expected = "240.0.0.0";

            // Act
            var network = new IpV4Network(address, mask);

            // Assert
            Assert.Equal(expected, network.Netmask.ToString());
        }

        [Fact]
        public void Constructor_Should_ConvertMaskIntoAppropiateMask_When_MaskInSecondOctet()
        {
            // Arrange
            var mask = 12;
            var address = new IpV4Address(4, 120, 0, 1);
            var expected = "255.240.0.0";

            // Act
            var network = new IpV4Network(address, mask);

            // Assert
            Assert.Equal(expected, network.Netmask.ToString());
        }

        [Fact]
        public void Constructor_Should_ConvertMaskIntoAppropiateMask_When_MaskInThirdOctet()
        {
            // Arrange
            var mask = 21;
            var address = new IpV4Address(172, 16, 30, 1);
            var expected = "255.255.248.0";

            // Act
            var network = new IpV4Network(address, mask);

            // Assert
            Assert.Equal(expected, network.Netmask.ToString());
        }

        [Fact]
        public void Constructor_Should_ConvertMaskIntoAppropiateMask_When_MaskInFourthOctet()
        {
            // Arrange
            var mask = 27;
            var address = new IpV4Address(192, 168, 0, 1);
            var expected = "255.255.255.224";

            // Act
            var network = new IpV4Network(address, mask);

            // Assert
            Assert.Equal(expected, network.Netmask.ToString());
        }

        [Fact]
        public void Netmask_Should_ReturnCorrectNetMask()
        {
            // Arrange
            var expected = new IpV4Address(255, 255, 255, 0);

            // Act
            var network = new IpV4Network(new IpV4Address(192, 169, 103, 129), expected);

            // Assert
            Assert.Equal(expected, network.Netmask);
        }

        [Fact]
        public void NetworkId_Should_ReturnCorrectNetWorkId()
        {
            // Arrange
            var expected = new IpV4Address(192, 168, 103, 0);
            var address = new IpV4Address(192, 168, 103, 129);
            var mask = new IpV4Address(255, 255, 255, 0);

            // Act
            var network = new IpV4Network(address, mask);

            // Assert
            Assert.Equal(expected, network.NetworkId);
        }

        [Fact]
        public void MaximumAddress_Should_ReturnCorrectMaximumAddress()
        {
            // Arrange
            var expected = new IpV4Address("192.168.195.190");
            var address = new IpV4Address(192, 168, 195, 166);
            var mask = new IpV4Address(255, 255, 255, 224);

            // Act
            var network = new IpV4Network(address, mask);

            // Assert
            Assert.Equal(expected, network.MaximumAddress);
        }

        [Fact]
        public void MinumumAddress_Should_ReturnCorrectMinumumAddress()
        {
            // Arrange
            var expected = new IpV4Address("192.168.195.161");
            var address = new IpV4Address(192, 168, 195, 166);
            var mask = new IpV4Address(255, 255, 255, 224);

            // Act
            var network = new IpV4Network(address, mask);

            // Assert
            Assert.Equal(expected, network.MinumumAddress);
        }

        [Fact]
        public void NumberOfHosts_Should_ReturnCorrectNumberOfHosts()
        {
            // Arrange
            var expected = 30;
            var address = new IpV4Address(192, 168, 195, 166);
            var mask = new IpV4Address(255, 255, 255, 224);

            // Act
            var network = new IpV4Network(address, mask);

            // Assert
            Assert.Equal(expected, network.NumberOfHosts);
        }
    }
}
