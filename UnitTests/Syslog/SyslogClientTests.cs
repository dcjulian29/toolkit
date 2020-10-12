using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using ToolKit.Syslog;
using Xunit;

namespace UnitTests.Syslog
{
    [SuppressMessage(
            "StyleCop.CSharp.DocumentationRules",
            "SA1600:ElementsMustBeDocumented",
            Justification = "Test Suites do not need XML Documentation.")]
    public class SyslogClientTests
    {
        private static Random random = new Random();

        [Fact]
        public void IncludeProcessInfo_Should_AcceptSettingToFalse()
        {
            // Arrange
            var client = new SyslogClient();

            // Act
            client.IncludeProcessInfo = false;

            // Assert
            Assert.False(client.IncludeProcessInfo);
        }

        [Fact]
        public void IncludeProcessInfo_Should_HaveDefaultValue()
        {
            // Arrange & Act
            var client = new SyslogClient();

            // Assert
            Assert.True(client.IncludeProcessInfo);
        }

        [Fact]
        public void Port_Should_AcceptProperPortNumbers()
        {
            // Arrange
            var client = new SyslogClient();

            // Act
            client.Port = 8514;

            // Assert
            Assert.Equal(8514, client.Port);
        }

        [Fact]
        public void Port_Should_HaveDefaultValue()
        {
            // Arrange & Act
            var client = new SyslogClient();

            // Assert
            Assert.Equal(514, client.Port);
        }

        [Fact]
        public void Port_Should_HaveProvidedValue()
        {
            // Arrange & Act
            var client = new SyslogClient("syslog.contoso.com", 8514);

            // Assert
            Assert.Equal(8514, client.Port);
        }

        [Fact]
        public void Port_Should_ThrowArgumentOutOfRangeException_When_AttemptingToSetTo65536()
        {
            // Arrange
            var client = new SyslogClient();

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => client.Port = 65536);
        }

        [Fact]
        public void Port_Should_ThrowArgumentOutOfRangeException_When_AttemptingToSetToNegative()
        {
            // Arrange
            var client = new SyslogClient();

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => client.Port = -1);
        }

        [Fact]
        public void Send_Should_CommunicateCorrectly_When_NotIncludingProcessInfo()
        {
            // Arrange
            var expected = @"\<\d+\>\S{3}\s\d+\s\d+\:\d+\:\d+\s\S+\s[^\[]+$";
            var port = random.Next(49152, 65535);
            var client = new SyslogClient()
            {
                Port = port,
                IncludeProcessInfo = false
            };

            var message = new Message("Unit Test Are Awesome!");

            var server = new MockUdpServer(port);

            // Act
            client.Send(message);
            while (!server.Done)
            {
                Thread.Sleep(100); // Multi-threaded programming will bite you!
            }

            // Assert
            Assert.Matches(expected, server.MessageRecieved);
        }

        [Fact]
        public void Send_Should_CommunicateCorrectlyOverTcp()
        {
            // Arrange
            var expected = @"\<\d+\>\S{3}\s\d+\s\d+\:\d+\:\d+\s\S+\s\S+\[\d+\]\:\s.*";
            var port = random.Next(49152, 65535);
            var client = new SyslogClient()
            {
                Port = port
            }.UseTcp();

            var message = new Message("Unit Test Are Awesome!");

            var server = new MockTcpServer(port);

            // Act
            client.Send(message);

            while (!server.Done)
            {
                Thread.Sleep(100); // Multi-threaded programming will bite you!
            }

            // Assert
            Assert.Matches(expected, server.MessageRecieved);
        }

        [Fact]
        public void Send_Should_CommunicateCorrectlyOverUdp()
        {
            // Arrange
            var expected = @"\<\d+\>\S{3}\s\d+\s\d+\:\d+\:\d+\s\S+\s\S+\[\d+\]\:\s.*";
            var port = random.Next(49152, 65535);
            var client = new SyslogClient()
            {
                Port = port
            }.UseUdp();

            var message = new Message("Unit Test Are Awesome!");

            var server = new MockUdpServer(port);

            // Act
            client.Send(message);

            while (!server.Done)
            {
                Thread.Sleep(100); // Multi-threaded programming will bite you!
            }

            // Assert
            Assert.Matches(expected, server.MessageRecieved);
        }

        [Fact]
        public void Server_Should_AcceptProperServerName()
        {
            // Arrange
            var client = new SyslogClient();

            // Act
            client.Server = "syslogserver.contoso.com";

            // Assert
            Assert.Equal("syslogserver.contoso.com", client.Server);
        }

        [Fact]
        public void Server_Should_HaveDefaultValue()
        {
            // Arrange
            var client = new SyslogClient();

            // Act & Assert
            Assert.Equal("127.0.0.1", client.Server);
        }

        [Fact]
        public void Server_Should_HaveProvidedValue()
        {
            // Arrange
            var client = new SyslogClient("syslogserver.contoso.com");

            // Act & Assert
            Assert.Equal("syslogserver.contoso.com", client.Server);
        }

        [Fact]
        public void Server_Should_ThrowArgumentException_When_AttemptingToSetToEmptyString()
        {
            // Arrange
            var client = new SyslogClient();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => client.Server = string.Empty);
        }

        [Fact]
        public void Server_Should_ThrowArgumentNullException_When_AttemptingToSetToNull()
        {
            // Arrange
            var client = new SyslogClient();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => client.Server = null);
        }

        [Fact]
        public void TruncateTagIfNeeded_Should_TruncateWhenProcessNameTooLong()
        {
            // Arrange
            const string expected = "12345678901234567890123456[5434]: ";
            const string tag = "12345678901234567890123456789012345";
            const string id = "[5434]";

            // Act
            var actual = SyslogClient.TruncateTagIfNeeded(tag, id);

            // Assert
            Assert.Equal(expected, actual);
        }

        private class MockTcpServer
        {
            private readonly TcpListener _server;

            public MockTcpServer(int port)
            {
                _server = new TcpListener(IPAddress.Loopback, port);
                _server.Start();
                _server.BeginAcceptTcpClient(Listen, _server);
            }

            public bool Done { get; set; }

            public string MessageRecieved { get; set; }

            private void Listen(IAsyncResult result)
            {
                var client = _server.EndAcceptTcpClient(result);

                var stream = client.GetStream();

                using (var reader = new StreamReader(stream, Encoding.ASCII))
                {
                    MessageRecieved = reader.ReadToEnd();
                }

                Done = true;
            }
        }

        private class MockUdpServer
        {
            private readonly UdpClient _server;

            private IPEndPoint _endpoint;

            public MockUdpServer(int port)
            {
                _endpoint = new IPEndPoint(IPAddress.Loopback, port);
                _server = new UdpClient(_endpoint);
                _server.BeginReceive(Listen, _server);
            }

            public bool Done { get; set; }

            public string MessageRecieved { get; set; }

            private void Listen(IAsyncResult result)
            {
                var message = _server.EndReceive(result, ref _endpoint);

                MessageRecieved = Encoding.ASCII.GetString(message).Trim('\0').Trim('\n');

                Done = true;
            }
        }
    }
}
