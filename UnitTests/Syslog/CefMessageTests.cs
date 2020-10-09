using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using ToolKit.Syslog;
using Xunit;

namespace UnitTests.Syslog
{
    [SuppressMessage(
            "StyleCop.CSharp.DocumentationRules",
            "SA1600:ElementsMustBeDocumented",
            Justification = "Test Suites do not need XML Documentation.")]
    public class CefMessageTests
    {
        [Fact]
        public void ApplicationProtocol_Should_ReturnExpectedResult()
        {
            // Arrange
            const string expected = "CEF:15|Julian|ToolBox|2010.12.01|TestSig|Test Case|5|app=HTTPS";
            var msg = GenerateCefMessage();

            // Act
            msg.ApplicationProtocol = "HTTPS";
            var actual = msg.Text;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ApplicationProtocol_Should_ThrowArgumentException_When_EmptyStringPassed()
        {
            // Arrange
            var msg = GenerateCefMessage();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => msg.ApplicationProtocol = string.Empty);
        }

        [Fact]
        public void ApplicationProtocol_Should_ThrowArgumentNullException_When_NullPassed()
        {
            // Arrange
            var msg = GenerateCefMessage();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => msg.ApplicationProtocol = null);
        }

        [Fact]
        public void BytesIn_Should_ReturnExpectedResult()
        {
            // Arrange
            const string expected = "CEF:15|Julian|ToolBox|2010.12.01|TestSig|Test Case|5|in=42";
            var msg = GenerateCefMessage();

            // Act
            msg.BytesIn = 42;
            var actual = msg.Text;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void BytesIn_Should_ThrowArgumentException_When_NegativeNumberPassed()
        {
            // Arrange
            var msg = GenerateCefMessage();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => msg.BytesIn = -1);
        }

        [Fact]
        public void BytesOut_Should_ReturnExpectedResult()
        {
            // Arrange
            const string expected = "CEF:15|Julian|ToolBox|2010.12.01|TestSig|Test Case|5|out=42";
            var msg = GenerateCefMessage();

            // Act
            msg.BytesOut = 42;
            var actual = msg.Text;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void BytesOut_Should_ThrowArgumentException_When_NegativeNumberPassed()
        {
            // Arrange
            var msg = GenerateCefMessage();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => msg.BytesOut = -1);
        }

        [Fact]
        public void DestinationAddress_Should_ReturnExpectedResult()
        {
            // Arrange
            const string expected = "CEF:15|Julian|ToolBox|2010.12.01|TestSig|Test Case|5|dst=10.10.10.11";
            var msg = GenerateCefMessage();

            // Act
            msg.DestinationAddress = IPAddress.Parse("10.10.10.11");
            var actual = msg.Text;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DestinationAddress_Should_ThrowArgumentNullException_When_NullPassed()
        {
            // Arrange
            var msg = GenerateCefMessage();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => msg.DestinationAddress = null);
        }

        [Fact]
        public void DestinationDnsDomain_Should_ReturnExpectedPropertyValue()
        {
            // Arrange
            const string expected = "julianscorner.com";
            var msg = GenerateCefMessage();

            // Act
            msg.DestinationDnsDomain = "julianscorner.com";
            var actual = msg.DestinationDnsDomain;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DestinationDnsDomain_Should_ReturnExpectedResult()
        {
            // Arrange
            const string expected = "CEF:15|Julian|ToolBox|2010.12.01|TestSig|Test Case|5|destinationDnsDomain=julianscorner.com";
            var msg = GenerateCefMessage();

            // Act
            msg.DestinationDnsDomain = "julianscorner.com";
            var actual = msg.Text;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DestinationDnsDomain_Should_ThrowArgumentException_When_EmptyStringPassed()
        {
            // Arrange
            var msg = GenerateCefMessage();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => msg.DestinationDnsDomain = string.Empty);
        }

        [Fact]
        public void DestinationDnsDomain_Should_ThrowArgumentNullException_When_NullPassed()
        {
            // Arrange
            var msg = GenerateCefMessage();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => msg.DestinationDnsDomain = null);
        }

        [Fact]
        public void DestinationHostName_Should_ReturnExpectedResult()
        {
            // Arrange
            const string expected = "CEF:15|Julian|ToolBox|2010.12.01|TestSig|Test Case|5|dhost=server";
            var msg = GenerateCefMessage();

            // Act
            msg.DestinationHostName = "server";
            var actual = msg.Text;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DestinationHostName_Should_ThrowArgumentException_When_EmptyStringPassed()
        {
            // Arrange
            var msg = GenerateCefMessage();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => msg.DestinationHostName = string.Empty);
        }

        [Fact]
        public void DestinationHostName_Should_ThrowArgumentNullException_When_NullPassed()
        {
            // Arrange
            var msg = GenerateCefMessage();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => msg.DestinationHostName = null);
        }

        [Fact]
        public void DestinationMacAddress_Should_ReturnExpectedResult()
        {
            // Arrange
            const string expected = "CEF:15|Julian|ToolBox|2010.12.01|TestSig|Test Case|5|dmac=00:0D:AF:1B:61";
            var msg = GenerateCefMessage();

            // Act
            msg.DestinationMacAddress = "00:0D:AF:1B:61";
            var actual = msg.Text;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DestinationMacAddress_Should_ThrowArgumentException_When_EmptyStringPassed()
        {
            // Arrange
            var msg = GenerateCefMessage();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => msg.DestinationMacAddress = string.Empty);
        }

        [Fact]
        public void DestinationMacAddress_Should_ThrowArgumentNullException_When_NullPassed()
        {
            // Arrange
            var msg = GenerateCefMessage();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => msg.DestinationMacAddress = null);
        }

        [Fact]
        public void DestinationNtDomain_Should_ReturnExpectedResult()
        {
            // Arrange
            const string expected = "CEF:15|Julian|ToolBox|2010.12.01|TestSig|Test Case|5|dntdom=Contoso";
            var msg = GenerateCefMessage();

            // Act
            msg.DestinationNtDomain = "Contoso";
            var actual = msg.Text;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DestinationNtDomain_Should_ThrowArgumentException_When_EmptyStringPassed()
        {
            // Arrange
            var msg = GenerateCefMessage();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => msg.DestinationNtDomain = string.Empty);
        }

        [Fact]
        public void DestinationNtDomain_Should_ThrowArgumentNullException_When_NullPassed()
        {
            // Arrange
            var msg = GenerateCefMessage();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => msg.DestinationNtDomain = null);
        }

        [Fact]
        public void DestinationPort_Should_ReturnExpectedPropertyValue()
        {
            // Arrange
            const int expected = 42;
            var msg = GenerateCefMessage();

            // Act
            msg.DestinationPort = 42;
            var actual = msg.DestinationPort;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DestinationPort_Should_ReturnExpectedResult()
        {
            // Arrange
            const string expected = "CEF:15|Julian|ToolBox|2010.12.01|TestSig|Test Case|5|dpt=42";
            var msg = GenerateCefMessage();

            // Act
            msg.DestinationPort = 42;
            var actual = msg.Text;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DestinationPort_Should_ThrowArgumentException_When_NegativeNumberPassed()
        {
            // Arrange
            var msg = GenerateCefMessage();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => msg.DestinationPort = -1);
        }

        [Fact]
        public void DestinationPort_Should_ThrowArgumentException_When_NumberAbovePortsPassed()
        {
            // Arrange
            var msg = GenerateCefMessage();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => msg.DestinationPort = 65536);
        }

        [Fact]
        public void DestinationProcessName_Should_ReturnExpectedResult()
        {
            // Arrange
            const string expected = "CEF:15|Julian|ToolBox|2010.12.01|TestSig|Test Case|5|dproc=explorer";
            var msg = GenerateCefMessage();

            // Act
            msg.DestinationProcessName = "explorer";
            var actual = msg.Text;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DestinationProcessName_Should_ThrowArgumentException_When_EmptyStringPassed()
        {
            // Arrange
            var msg = GenerateCefMessage();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => msg.DestinationProcessName = string.Empty);
        }

        [Fact]
        public void DestinationProcessName_Should_ThrowArgumentNullException_When_NullPassed()
        {
            // Arrange
            var msg = GenerateCefMessage();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => msg.DestinationProcessName = null);
        }

        [Fact]
        public void DestinationUserId_Should_ReturnExpectedResult()
        {
            // Arrange
            const string expected = "CEF:15|Julian|ToolBox|2010.12.01|TestSig|Test Case|5|duid=1001";
            var msg = GenerateCefMessage();

            // Act
            msg.DestinationUserId = "1001";
            var actual = msg.Text;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DestinationUserId_Should_ThrowArgumentException_When_EmptyStringPassed()
        {
            // Arrange
            var msg = GenerateCefMessage();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => msg.DestinationUserId = string.Empty);
        }

        [Fact]
        public void DestinationUserId_Should_ThrowArgumentNullException_When_NullPassed()
        {
            // Arrange
            var msg = GenerateCefMessage();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => msg.DestinationUserId = null);
        }

        [Fact]
        public void DestinationUserName_Should_ReturnExpectedResult()
        {
            // Arrange
            const string expected = "CEF:15|Julian|ToolBox|2010.12.01|TestSig|Test Case|5|duser=julian";
            var msg = GenerateCefMessage();

            // Act
            msg.DestinationUserName = "julian";
            var actual = msg.Text;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DestinationUserName_Should_ThrowArgumentException_When_EmptyStringPassed()
        {
            // Arrange
            var msg = GenerateCefMessage();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => msg.DestinationUserName = string.Empty);
        }

        [Fact]
        public void DestinationUserName_Should_ThrowArgumentNullException_When_NullPassed()
        {
            // Arrange
            var msg = GenerateCefMessage();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => msg.DestinationUserName = null);
        }

        [Fact]
        public void DestinationUserPrivileges_Should_ReturnExpectedResult()
        {
            // Arrange
            const string expected = "CEF:15|Julian|ToolBox|2010.12.01|TestSig|Test Case|5|dpriv=Administrator";
            var msg = GenerateCefMessage();

            // Act
            msg.DestinationUserPrivileges = "Administrator";
            var actual = msg.Text;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DestinationUserPrivileges_Should_ThrowArgumentException_When_EmptyStringPassed()
        {
            // Arrange
            var msg = GenerateCefMessage();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => msg.DestinationUserPrivileges = string.Empty);
        }

        [Fact]
        public void DestinationUserPrivileges_Should_ThrowArgumentNullException_When_NullPassed()
        {
            // Arrange
            var msg = GenerateCefMessage();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => msg.DestinationUserPrivileges = null);
        }

        [Fact]
        public void DeviceAction_Should_ReturnExpectedResult()
        {
            // Arrange
            const string expected = "CEF:15|Julian|ToolBox|2010.12.01|TestSig|Test Case|5|act=BLOCK";
            var msg = GenerateCefMessage();

            // Act
            msg.DeviceAction = "BLOCK";
            var actual = msg.Text;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DeviceAction_Should_ThrowArgumentException_When_EmptyStringPassed()
        {
            // Arrange
            var msg = GenerateCefMessage();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => msg.DeviceAction = string.Empty);
        }

        [Fact]
        public void DeviceAction_Should_ThrowArgumentNullException_When_NullPassed()
        {
            // Arrange
            var msg = GenerateCefMessage();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => msg.DeviceAction = null);
        }

        [Fact]
        public void DeviceAddress_Should_ReturnExpectedResult()
        {
            // Arrange
            const string expected = "CEF:15|Julian|ToolBox|2010.12.01|TestSig|Test Case|5|dvc=10.10.10.11";
            var msg = GenerateCefMessage();

            // Act
            msg.DeviceAddress = IPAddress.Parse("10.10.10.11");
            var actual = msg.Text;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DeviceAddress_Should_ThrowArgumentNullException_When_NullPassed()
        {
            // Arrange
            var msg = GenerateCefMessage();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => msg.DeviceAddress = null);
        }

        [Fact]
        public void DeviceDirection_Should_ReturnExpectedResult()
        {
            // Arrange
            const string expected = "CEF:15|Julian|ToolBox|2010.12.01|TestSig|Test Case|5|deviceDirection=IN";
            var msg = GenerateCefMessage();

            // Act
            msg.DeviceDirection = "IN";
            var actual = msg.Text;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DeviceDirection_Should_ThrowArgumentException_When_EmptyStringPassed()
        {
            // Arrange
            var msg = GenerateCefMessage();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => msg.DeviceDirection = string.Empty);
        }

        [Fact]
        public void DeviceDirection_Should_ThrowArgumentNullException_When_NullPassed()
        {
            // Arrange
            var msg = GenerateCefMessage();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => msg.DeviceDirection = null);
        }

        [Fact]
        public void DeviceDnsDomain_Should_ReturnExpectedResult()
        {
            // Arrange
            const string expected = "CEF:15|Julian|ToolBox|2010.12.01|TestSig|Test Case|5|deviceDnsDomain=julianscorner.com";
            var msg = GenerateCefMessage();

            // Act
            msg.DeviceDnsDomain = "julianscorner.com";
            var actual = msg.Text;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DeviceDnsDomain_Should_ThrowArgumentException_When_EmptyStringPassed()
        {
            // Arrange
            var msg = GenerateCefMessage();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => msg.DeviceDnsDomain = string.Empty);
        }

        [Fact]
        public void DeviceDnsDomain_Should_ThrowArgumentNullException_When_NullPassed()
        {
            // Arrange
            var msg = GenerateCefMessage();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => msg.DeviceDnsDomain = null);
        }

        [Fact]
        public void DeviceEventCategory_Should_ReturnExpectedResult()
        {
            // Arrange
            const string expected = "CEF:15|Julian|ToolBox|2010.12.01|TestSig|Test Case|5|cat=Security";
            var msg = GenerateCefMessage();

            // Act
            msg.DeviceEventCategory = "Security";
            var actual = msg.Text;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DeviceEventCategory_Should_ThrowArgumentException_When_EmptyStringPassed()
        {
            // Arrange
            var msg = GenerateCefMessage();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => msg.DeviceEventCategory = string.Empty);
        }

        [Fact]
        public void DeviceEventCategory_Should_ThrowArgumentNullException_When_NullPassed()
        {
            // Arrange
            var msg = GenerateCefMessage();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => msg.DeviceEventCategory = null);
        }

        [Fact]
        public void DeviceHostName_Should_ReturnExpectedResult()
        {
            // Arrange
            const string expected = "CEF:15|Julian|ToolBox|2010.12.01|TestSig|Test Case|5|dvchost=server";
            var msg = GenerateCefMessage();

            // Act
            msg.DeviceHostName = "server";
            var actual = msg.Text;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DeviceHostName_Should_ThrowArgumentException_When_EmptyStringPassed()
        {
            // Arrange
            var msg = GenerateCefMessage();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => msg.DeviceHostName = string.Empty);
        }

        [Fact]
        public void DeviceHostName_Should_ThrowArgumentNullException_When_NullPassed()
        {
            // Arrange
            var msg = GenerateCefMessage();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => msg.DeviceHostName = null);
        }

        [Fact]
        public void DeviceInboundInterface_Should_ReturnExpectedPropertyValue()
        {
            // Arrange
            const string expected = "eth0";
            var msg = GenerateCefMessage();

            // Act
            msg.DeviceInboundInterface = "eth0";
            var actual = msg.DeviceInboundInterface;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DeviceInboundInterface_Should_ReturnExpectedResult()
        {
            // Arrange
            const string expected = "CEF:15|Julian|ToolBox|2010.12.01|TestSig|Test Case|5|deviceInboundInterface=eth0";
            var msg = GenerateCefMessage();

            // Act
            msg.DeviceInboundInterface = "eth0";
            var actual = msg.Text;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DeviceInboundInterface_Should_ThrowArgumentException_When_EmptyStringPassed()
        {
            // Arrange
            var msg = GenerateCefMessage();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => msg.DeviceInboundInterface = string.Empty);
        }

        [Fact]
        public void DeviceInboundInterface_Should_ThrowArgumentNullException_When_NullPassed()
        {
            // Arrange
            var msg = GenerateCefMessage();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => msg.DeviceInboundInterface = null);
        }

        [Fact]
        public void DeviceMacAddress_Should_ReturnExpectedResult()
        {
            // Arrange
            const string expected = "CEF:15|Julian|ToolBox|2010.12.01|TestSig|Test Case|5|deviceMacAddress=00:0D:60:AF:1B:61";
            var msg = GenerateCefMessage();

            // Act
            msg.DeviceMacAddress = "00:0D:60:AF:1B:61";
            var actual = msg.Text;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DeviceMacAddress_Should_ThrowArgumentException_When_EmptyStringPassed()
        {
            // Arrange
            var msg = GenerateCefMessage();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => msg.DeviceMacAddress = string.Empty);
        }

        [Fact]
        public void DeviceMacAddress_Should_ThrowArgumentNullException_When_NullPassed()
        {
            // Arrange
            var msg = GenerateCefMessage();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => msg.DeviceMacAddress = null);
        }

        [Fact]
        public void DeviceNtDomain_Should_ReturnExpectedResult()
        {
            // Arrange
            const string expected = "CEF:15|Julian|ToolBox|2010.12.01|TestSig|Test Case|5|deviceNtDomain=Contoso";
            var msg = GenerateCefMessage();

            // Act
            msg.DeviceNtDomain = "Contoso";
            var actual = msg.Text;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DeviceNtDomain_Should_ThrowArgumentException_When_EmptyStringPassed()
        {
            // Arrange
            var msg = GenerateCefMessage();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => msg.DeviceNtDomain = string.Empty);
        }

        [Fact]
        public void DeviceNtDomain_Should_ThrowArgumentNullException_When_NullPassed()
        {
            // Arrange
            var msg = GenerateCefMessage();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => msg.DeviceNtDomain = null);
        }

        [Fact]
        public void DeviceOutboundInterface_Should_ReturnExpectedPropertyValue()
        {
            // Arrange
            const string expected = "eth1";
            var msg = GenerateCefMessage();

            // Act
            msg.DeviceOutboundInterface = "eth1";
            var actual = msg.DeviceOutboundInterface;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DeviceOutboundInterface_Should_ReturnExpectedResult()
        {
            // Arrange
            const string expected = "CEF:15|Julian|ToolBox|2010.12.01|TestSig|Test Case|5|deviceOutboundInterface=eth1";
            var msg = GenerateCefMessage();

            // Act
            msg.DeviceOutboundInterface = "eth1";
            var actual = msg.Text;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DeviceOutboundInterface_Should_ThrowArgumentException_When_EmptyStringPassed()
        {
            // Arrange
            var msg = GenerateCefMessage();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => msg.DeviceOutboundInterface = string.Empty);
        }

        [Fact]
        public void DeviceOutboundInterface_Should_ThrowArgumentNullException_When_NullPassed()
        {
            // Arrange
            var msg = GenerateCefMessage();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => msg.DeviceOutboundInterface = null);
        }

        [Fact]
        public void DeviceProcessName_Should_ReturnExpectedPropertyValue()
        {
            // Arrange
            const string expected = "systemd";
            var msg = GenerateCefMessage();

            // Act
            msg.DeviceProcessName = "systemd";
            var actual = msg.DeviceProcessName;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DeviceProcessName_Should_ReturnExpectedResult()
        {
            // Arrange
            const string expected = "CEF:15|Julian|ToolBox|2010.12.01|TestSig|Test Case|5|deviceProcessName=systemd";
            var msg = GenerateCefMessage();

            // Act
            msg.DeviceProcessName = "systemd";
            var actual = msg.Text;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DeviceProcessName_Should_ThrowArgumentException_When_EmptyStringPassed()
        {
            // Arrange
            var msg = GenerateCefMessage();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => msg.DeviceProcessName = string.Empty);
        }

        [Fact]
        public void DeviceProcessName_Should_ThrowArgumentNullException_When_NullPassed()
        {
            // Arrange
            var msg = GenerateCefMessage();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => msg.DeviceProcessName = null);
        }

        [Fact]
        public void Encoding_Should_EscapeBackslashInExtension()
        {
            // Arrange
            const string expected = @"CEF:15|Julian|ToolBox|2010.12.01|TestSig|Test Case|5|act=An\\Action";
            var msg = GenerateCefMessage();

            // Act - Add backslash to an extension...
            msg.DeviceAction = @"An\Action";

            // Assert
            Assert.Equal(expected, msg.Text);
        }

        [Fact]
        public void Encoding_Should_EscapeBackslashInPrefix()
        {
            // Arrange & Act
            const string expected = @"CEF:15|Julian|Tool\\Box|2010.12.01|TestSig|Test|5";
            var msg = new CefMessage
            {
                DeviceVendor = "Julian",
                DeviceProduct = @"Tool\Box",
                DeviceVersion = "2010.12.01",
                SignatureId = "TestSig",
                Name = "Test",
                Severity = 5
            };

            // Assert
            Assert.Equal(expected, msg.Text);
        }

        [Fact]
        public void Encoding_Should_EscapeConvertMultilineInExtension()
        {
            // Arrange
            const string expected = "CEF:15|Julian|ToolBox|2010.12.01|TestSig|Test Case|5|act=An\nAct\nCat\nHere";
            var msg = GenerateCefMessage();

            // Act - Add multi-line to an extension...
            msg.DeviceAction = "An\r\nAct\rCat\nHere";

            // Assert
            Assert.Equal(expected, msg.Text);
        }

        [Fact]
        public void Encoding_Should_EscapeEqualInExtension()
        {
            // Arrange
            const string expected = @"CEF:15|Julian|ToolBox|2010.12.01|TestSig|Test Case|5|act=An\=Action";
            var msg = GenerateCefMessage();

            // Act - Add equal to an extension...
            msg.DeviceAction = "An=Action";

            // Assert
            Assert.Equal(expected, msg.Text);
        }

        [Fact]
        public void Encoding_Should_EscapePipeInPrefix()
        {
            // Arrange & Act
            const string expected = "CEF:15|Julian|Tool\\|Box|2010.12.01|TestSig|Test|5";
            var msg = new CefMessage
            {
                DeviceVendor = "Julian",
                DeviceProduct = "Tool|Box",
                DeviceVersion = "2010.12.01",
                SignatureId = "TestSig",
                Name = "Test",
                Severity = 5
            };

            // Assert
            Assert.Equal(expected, msg.Text);
        }

        [Fact]
        public void EndTime_Should_ReturnExpectedResult()
        {
            // Arrange
            const string expected = "CEF:15|Julian|ToolBox|2010.12.01|TestSig|Test Case|5|end=Jan 01 1970 03:14:15";
            var msg = GenerateCefMessage();

            // Act
            msg.EndTime = new DateTime(1970, 1, 1, 3, 14, 15, DateTimeKind.Utc);
            var actual = msg.Text;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void EventCategory_Should_ReturnExpectedResult()
        {
            // Arrange
            const string expected = "CEF:15|Julian|ToolBox|2010.12.01|TestSig|Test Case|5|cat=Security";
            var msg = GenerateCefMessage();

            // Act
            msg.EventCategory = "Security";
            var actual = msg.Text;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void EventCategory_Should_ThrowArgumentException_When_EmptyStringPassed()
        {
            // Arrange
            var msg = GenerateCefMessage();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => msg.EventCategory = string.Empty);
        }

        [Fact]
        public void EventCategory_Should_ThrowArgumentNullException_When_NullPassed()
        {
            // Arrange
            var msg = GenerateCefMessage();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => msg.EventCategory = null);
        }

        [Fact]
        public void EventCount_Should_ReturnExpectedResult()
        {
            // Arrange
            const string expected = "CEF:15|Julian|ToolBox|2010.12.01|TestSig|Test Case|5|cnt=42";
            var msg = GenerateCefMessage();

            // Act
            msg.EventCount = 42;
            var actual = msg.Text;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void EventCount_Should_ThrowArgumentException_When_NegativeNumberPassed()
        {
            // Arrange
            var msg = GenerateCefMessage();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => msg.EventCount = -1);
        }

        [Fact]
        public void Facility_Should_BeSystem5_When_ConstructorWithValuesUsed()
        {
            // Arrange
            const Facility expected = Facility.System5;
            var msg = new CefMessage(Facility.System5, Level.Emergency);

            // Act
            var actual = msg.Facility;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FileCreateTime_Should_ReturnExpectedResult()
        {
            // Arrange
            const string expected = "CEF:15|Julian|ToolBox|2010.12.01|TestSig|Test Case|5|fileCreateTime=Jan 01 1970 03:14:15";
            var msg = GenerateCefMessage();

            // Act
            msg.FileCreateTime = new DateTime(1970, 1, 1, 3, 14, 15, DateTimeKind.Utc);
            var actual = msg.Text;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FileHash_Should_ReturnExpectedResult()
        {
            // Arrange
            const string expected = "CEF:15|Julian|ToolBox|2010.12.01|TestSig|Test Case|5|fileHash=E05B25105D7C0814F051BB81299D836B423FEA6E";
            var msg = GenerateCefMessage();

            // Act
            msg.FileHash = "E05B25105D7C0814F051BB81299D836B423FEA6E";
            var actual = msg.Text;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FileHash_Should_ThrowArgumentException_When_EmptyStringPassed()
        {
            // Arrange
            var msg = GenerateCefMessage();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => msg.FileHash = string.Empty);
        }

        [Fact]
        public void FileHash_Should_ThrowArgumentNullException_When_NullPassed()
        {
            // Arrange
            var msg = GenerateCefMessage();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => msg.FileHash = null);
        }

        [Fact]
        public void FileId_Should_ReturnExpectedResult()
        {
            // Arrange
            const string expected = "CEF:15|Julian|ToolBox|2010.12.01|TestSig|Test Case|5|fileId=34543";
            var msg = GenerateCefMessage();

            // Act
            msg.FileId = "34543";
            var actual = msg.Text;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FileId_Should_ThrowArgumentException_When_EmptyStringPassed()
        {
            // Arrange
            var msg = GenerateCefMessage();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => msg.FileId = string.Empty);
        }

        [Fact]
        public void FileId_Should_ThrowArgumentNullException_When_NullPassed()
        {
            // Arrange
            var msg = GenerateCefMessage();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => msg.FileId = null);
        }

        [Fact]
        public void FileModificationTime_Should_ReturnExpectedResult()
        {
            // Arrange
            const string expected = "CEF:15|Julian|ToolBox|2010.12.01|TestSig|Test Case|5|fileModificationTime=Jan 01 1970 03:14:15";
            var msg = GenerateCefMessage();

            // Act
            msg.FileModificationTime = new DateTime(1970, 1, 1, 3, 14, 15, DateTimeKind.Utc);
            var actual = msg.Text;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Level_Should_BeEmergency_When_ConstructorWithValuesUsed()
        {
            // Arrange
            const Level expected = Level.Emergency;
            var msg = new CefMessage(Facility.System5, Level.Emergency);

            // Act
            var actual = msg.Level;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void OldFileCreateTime_Should_ReturnExpectedResult()
        {
            // Arrange
            const string expected = "CEF:15|Julian|ToolBox|2010.12.01|TestSig|Test Case|5|oldFileCreateTime=Jan 01 1970 03:14:15";
            var msg = GenerateCefMessage();

            // Act
            msg.OldFileCreateTime = new DateTime(1970, 1, 1, 3, 14, 15, DateTimeKind.Utc);
            var actual = msg.Text;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void OldFileModificationTime_Should_ReturnExpectedResult()
        {
            // Arrange
            const string expected = "CEF:15|Julian|ToolBox|2010.12.01|TestSig|Test Case|5|oldFileModificationTime=Jan 01 1970 03:14:15";
            var msg = GenerateCefMessage();

            // Act
            msg.OldFileModificationTime = new DateTime(1970, 1, 1, 3, 14, 15, DateTimeKind.Utc);
            var actual = msg.Text;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ReceiptTime_Should_ReturnExpectedResult()
        {
            // Arrange
            const string expected = "CEF:15|Julian|ToolBox|2010.12.01|TestSig|Test Case|5|rt=Jan 01 1970 03:14:15";
            var msg = GenerateCefMessage();

            // Act
            msg.ReceiptTime = new DateTime(1970, 1, 1, 3, 14, 15, DateTimeKind.Utc);
            var actual = msg.Text;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void StartTime_Should_ReturnExpectedResult()
        {
            // Arrange
            const string expected = "CEF:15|Julian|ToolBox|2010.12.01|TestSig|Test Case|5|start=Jan 01 1970 03:14:15";
            var msg = GenerateCefMessage();

            // Act
            msg.StartTime = new DateTime(1970, 1, 1, 3, 14, 15, DateTimeKind.Utc);
            var actual = msg.Text;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Text_Should_ReturnExpectedMessage()
        {
            // Arrange
            const string expected = "CEF:15|Julian|ToolBox|2010.12.01|TestSig|Test Case|5";
            var msg = new CefMessage()
            {
                DeviceVendor = "Julian",
                DeviceProduct = "ToolBox",
                DeviceVersion = "2010.12.01",
                SignatureId = "TestSig",
                Name = "Test Case",
                Severity = 5
            };

            // Act
            var actual = msg.Text;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Text_Should_ReturnZeroSeverity_When_SeverityIsMissing()
        {
            // Arrange
            const string expected = "CEF:15|Julian|ToolBox|2010.12.01|TestSig|Test Case|0";
            var msg = new CefMessage()
            {
                DeviceVendor = "Julian",
                DeviceProduct = "ToolBox",
                DeviceVersion = "2010.12.01",
                SignatureId = "TestSig",
                Name = "Test Case"
            };

            // Act
            var actual = msg.Text;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Text_Should_ThrowInvalidOperationException_When_DeviceProductIsMissing()
        {
            // Arrange
            var msg = new CefMessage()
            {
                DeviceVendor = "Julian",
                DeviceVersion = "2010.12.01",
                SignatureId = "TestSig",
                Name = "Test Case",
                Severity = 5
            };

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => msg.Text);
        }

        [Fact]
        public void Text_Should_ThrowInvalidOperationException_When_DeviceVendorIsMissing()
        {
            // Arrange
            var msg = new CefMessage()
            {
                DeviceProduct = "ToolBox",
                DeviceVersion = "2010.12.01",
                SignatureId = "TestSig",
                Name = "Test Case",
                Severity = 5
            };

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => msg.Text);
        }

        [Fact]
        public void Text_Should_ThrowInvalidOperationException_When_DeviceVersionIsMissing()
        {
            // Arrange
            var msg = new CefMessage()
            {
                DeviceVendor = "Julian",
                DeviceProduct = "ToolBox",
                SignatureId = "TestSig",
                Name = "Test Case",
                Severity = 5
            };

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => msg.Text);
        }

        [Fact]
        public void Text_Should_ThrowInvalidOperationException_When_NameIsMissing()
        {
            // Arrange
            var msg = new CefMessage()
            {
                DeviceVendor = "Julian",
                DeviceProduct = "ToolBox",
                DeviceVersion = "2010.12.01",
                SignatureId = "TestSig",
                Severity = 5
            };

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => msg.Text);
        }

        [Fact]
        public void Text_Should_ThrowInvalidOperationException_When_SignatureIdIsMissing()
        {
            // Arrange
            var msg = new CefMessage()
            {
                DeviceVendor = "Julian",
                DeviceProduct = "ToolBox",
                DeviceVersion = "2010.12.01",
                Name = "Test Case",
                Severity = 5
            };

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => msg.Text);
        }

        [Fact]
        public void Version_Should_Be15()
        {
            // Arrange
            var msg = new CefMessage();

            // Act & Assert
            Assert.Equal(15, msg.Version);
        }

        private CefMessage GenerateCefMessage()
        {
            return new CefMessage()
            {
                DeviceVendor = "Julian",
                DeviceProduct = "ToolBox",
                DeviceVersion = "2010.12.01",
                SignatureId = "TestSig",
                Name = "Test Case",
                Severity = 5
            };
        }
    }
}
