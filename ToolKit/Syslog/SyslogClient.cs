using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Net.Sockets;
using System.Text;
using ToolKit.Validation;

namespace ToolKit.Syslog
{
    /// <summary>
    /// This class is used to send SYSLOG messages to a SYSLOG server.
    /// </summary>
    public class SyslogClient
    {
        private int _port;

        private string _server;

        private bool _useTcp;

        /// <summary>
        /// Initializes a new instance of the <see cref="SyslogClient" /> class.
        /// </summary>
        public SyslogClient()
        {
            Port = 514;
            Server = "127.0.0.1";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SyslogClient" /> class.
        /// </summary>
        /// <param name="server">The SYSLOG server to communicate with.</param>
        public SyslogClient(string server)
        {
            Port = 514;
            Server = server;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SyslogClient" /> class.
        /// </summary>
        /// <param name="server">The SYSLOG server to communicate with.</param>
        /// <param name="port">An integer that contains the port number on the SYSLOG server.</param>
        public SyslogClient(string server, int port)
        {
            Port = port;
            Server = server;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to include process info in message that is sent
        /// to the SYSLOG server.
        /// </summary>
        /// <value>
        /// <c>true</c> if process is to be included; otherwise, <c>false</c>. The default value is true.
        /// </value>
        public bool IncludeProcessInfo { get; set; } = true;

        /// <summary>
        /// Gets or sets the port to communicate with.
        /// </summary>
        public int Port
        {
            get => _port;

            set
            {
                if ((value < 0) || (value > 65535))
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(value),
                        "Valid port numbers are between 0 and 65535");
                }

                _port = value;
            }
        }

        /// <summary>
        /// Gets or sets the SYSLOG server to communicate with.
        /// </summary>
        public string Server
        {
            get => _server;

            set
            {
                Check.NotNull(value, nameof(value));
                Check.NotEmpty(value, "The value specified for a set operation is equal to Empty.");

                _server = value.Trim();
            }
        }

        /// <summary>
        /// Sends the message to the SYSLOG server.
        /// </summary>
        /// <param name="message">The SYSLOG message.</param>
        public void Send(IMessage message)
        {
            Check.NotNull(message, nameof(message));

            if (_useTcp)
            {
                var client = new TcpClient(_server, _port);
                using (var write = new StreamWriter(client.GetStream()))
                {
                    write.Write(PrepareMessageToSend(message));
                }

                client.Close();
                client.Dispose();
            }
            else
            {
                var bytesToSend = Encoding.ASCII.GetBytes(PrepareMessageToSend(message));
                using (var client = new UdpClient(_server, _port))
                {
                    client.Send(bytesToSend, bytesToSend.Length);
                }
            }
        }

        /// <summary>
        /// Uses TCP for communication with SYSLOG server.
        /// </summary>
        /// <returns>This client configured to use TCP.</returns>
        public SyslogClient UseTcp()
        {
            _useTcp = true;
            return this;
        }

        /// <summary>
        /// Uses UDP for communication with SYSLOG server.
        /// </summary>
        /// <returns>This client configured to use UDP.</returns>
        public SyslogClient UseUdp()
        {
            _useTcp = false;
            return this;
        }

        [SuppressMessage(
            "StyleCop.CSharp.DocumentationRules",
            "SA1600:Elements should be documented",
            Justification = "Exposed Only For Unit Testing")]
        internal static string TruncateTagIfNeeded(string tag, string processID)
        {
            if (tag.Length > 32 - processID.Length)
            {
                tag = tag.Substring(0, 32 - processID.Length);
            }

            return $"{tag}{processID}: ";
        }

        private string PrepareMessageToSend(IMessage message)
        {
            // The PRI value is calculated by first multiplying the Facility number by 8 and then
            // adding the numerical value of the Severity. For example, a kernel message
            // (Facility=0) with a Severity of Emergency (Severity=0) would have a Priority value of
            // 0. Also, a "Local Use 4" message (Facility=20) with a Severity of Notice (Severity=5)
            // would have a Priority value of 165.
            var pri = ((int)message.Facility * 8) + (int)message.Level;

            // The TIMESTAMP will be the current local time of the sender. Single digits in the date
            // (5 in this case) are preceded by a space in the TIMESTAMP format.
            var timestamp = DateTime.Now.ToString("MMM dd HH:mm:ss", CultureInfo.InvariantCulture);

            // The HOSTNAME will be the name of the device, as it is known by the relay. If the name
            // cannot be determined, the IP address of the device will be used.
            var hostname = Environment.MachineName.ToLower(CultureInfo.CurrentCulture);

            // The MSG part has two fields known as the TAG field and the CONTENT field. The value
            // in the TAG field will be the name of the program or process that generated the
            // message. The TAG is a string of alphanumeric characters that MUST NOT exceed 32
            // characters. The CONTENT contains the details of the message. This has traditionally
            // been a free form message that gives some detailed information of the event.
            var tag = string.Empty;

            if (IncludeProcessInfo)
            {
                tag = Process.GetCurrentProcess().ProcessName;
                var processID = $"[{Process.GetCurrentProcess().Id}]";

                tag = TruncateTagIfNeeded(tag, processID);
            }

            var msg = tag + message.Text;

            // The SYSLOG message is transmitted: <PRI>TIMESTAMP HOSTNAME MSG / \ TAG CONTENT
            //
            // <34>Oct 11 22:14:15 mymachine su: 'su root' failed for lonvick on /dev/pts/8
            return $"<{pri}>{timestamp} {hostname} {msg}\n";
        }
    }
}
