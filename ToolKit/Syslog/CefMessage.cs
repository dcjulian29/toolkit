using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Net;
using System.Text;
using ToolKit.Validation;

namespace ToolKit.Syslog
{
    /// <summary>
    /// Represents a Common Event Format SYSLOG Message. This implementation is based on
    /// documentation obtained from ArcSight, Inc. The document was released on July 17, 2009 and is
    /// designated as revision 15.
    /// </summary>
    public class CefMessage : IMessage
    {
        private readonly Dictionary<string, string> _extensions = new Dictionary<string, string>();

        private string _deviceProduct = string.Empty;

        private string _deviceVendor = string.Empty;

        private string _deviceVersion = string.Empty;

        private string _name = string.Empty;

        private int _severity;

        private string _signatureId = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="CefMessage" /> class.
        /// </summary>
        public CefMessage()
        {
            Facility = Facility.User;
            Level = Level.Information;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CefMessage" /> class.
        /// </summary>
        /// <param name="facility">The facility.</param>
        /// <param name="level">The level.</param>
        public CefMessage(Facility facility, Level level)
        {
            Facility = facility;
            Level = level;
        }

        /// <summary>
        /// Gets or sets the application level protocol, example values are: HTTP, HTTPS, SSHv2,
        /// Telnet, POP, IMAP, IMAPS, etc.
        /// </summary>
        public string ApplicationProtocol
        {
            get => ReturnExtensionValue("app"); set => AddExtension("app", value, 31);
        }

        /// <summary>
        /// Gets or sets the number of bytes transferred inbound. Inbound relative to the source to
        /// destination relationship, meaning that data was flowing from source to destination.
        /// </summary>
        public int BytesIn
        {
            get => ToInteger("in"); set => AddExtension("in", Check.NotNegative(value, nameof(value)), 0);
        }

        /// <summary>
        /// Gets or sets the number of bytes transferred outbound. Outbound relative to the source
        /// to destination relationship, meaning that data was flowing from destination to source.
        /// </summary>
        public int BytesOut
        {
            get => ToInteger("out"); set => AddExtension("out", Check.NotNegative(value, nameof(value)), 0);
        }

        /// <summary>
        /// Gets or sets the address of destination that the event refers to in an IP network. The
        /// format is an IPv4 address. Example: "192.168.10.1".
        /// </summary>
        public IPAddress DestinationAddress
        {
            get => IPAddress.Parse(ReturnExtensionValue("dst")); set => AddExtension("dst", value, 0);
        }

        /// <summary>
        /// Gets or sets the destination's DNS domain part of the complete fully qualified domain
        /// name (FQDN).
        /// </summary>
        public string DestinationDnsDomain
        {
            get => ReturnExtensionValue("destinationDnsDomain");

            set => AddExtension("destinationDnsDomain", value, 255);
        }

        /// <summary>
        /// Gets or sets the name of the destination host that an event refers to in an IP network.
        /// The format should be a fully qualified domain name associated with the destination node,
        /// when a node is available.
        /// </summary>
        public string DestinationHostName
        {
            get => ReturnExtensionValue("dhost"); set => AddExtension("dhost", value, 1023);
        }

        /// <summary>
        /// Gets or sets the name of the destination MAC address. Six colon-separated hexadecimal
        /// numbers. Example: "00:0D:60:AF:1B:61".
        /// </summary>
        public string DestinationMacAddress
        {
            get => ReturnExtensionValue("dmac"); set => AddExtension("dmac", value, 0);
        }

        /// <summary>
        /// Gets or sets the Windows domain name of the destination address.
        /// </summary>
        public string DestinationNtDomain
        {
            get => ReturnExtensionValue("dntdom"); set => AddExtension("dntdom", value, 255);
        }

        /// <summary>
        /// Gets or sets the destination port. The valid port numbers are between 0 and 65535.
        /// </summary>
        public int DestinationPort
        {
            get => ToInteger("dpt");

            set
            {
                if ((value < 0) || (value > 65535))
                {
                    throw new ArgumentException("Valid port numbers are between 0 and 65535");
                }

                AddExtension("dpt", value, 0);
            }
        }

        /// <summary>
        /// Gets or sets the name of the destination process which is the event's destination. For
        /// example: "telnetd", or "sshd".
        /// </summary>
        public string DestinationProcessName
        {
            get => ReturnExtensionValue("dproc"); set => AddExtension("dproc", value, 1023);
        }

        /// <summary>
        /// Gets or sets the destination user by ID. For example, in UNIX, the root user is
        /// generally associated with user ID 0.
        /// </summary>
        public string DestinationUserId
        {
            get => ReturnExtensionValue("duid"); set => AddExtension("duid", value, 1023);
        }

        /// <summary>
        /// Gets or sets the name of the destination user by name. This is the user associated with
        /// the event's destination. E-mail addresses are also mapped into the UserName fields. The
        /// recipient is a candidate to put into destinationUserName.
        /// </summary>
        public string DestinationUserName
        {
            get => ReturnExtensionValue("duser"); set => AddExtension("duser", value, 1023);
        }

        /// <summary>
        /// Gets or sets the destination user privileges. The allowed values are: "Administrator",
        /// "User", and "Guest". This identifies the destination user's privileges. In UNIX, for
        /// example, activity executed on the root user would be identified with
        /// destinationUserPrivileges of "Administrator". This is an idealized and simplified view
        /// on privileges and can be extended in the future.
        /// </summary>
        public string DestinationUserPrivileges
        {
            get => ReturnExtensionValue("dpriv"); set => AddExtension("dpriv", value, 1023);
        }

        /// <summary>
        /// Gets or sets the action mentioned in the event.
        /// </summary>
        public string DeviceAction
        {
            get => ReturnExtensionValue("act"); set => AddExtension("act", value, 63);
        }

        /// <summary>
        /// Gets or sets the device address of the device that an event refers to in an IP network.
        /// The format is an IPv4 address. Example: "192.168.10.1".
        /// </summary>
        public IPAddress DeviceAddress
        {
            get => IPAddress.Parse(ReturnExtensionValue("dvc")); set => AddExtension("dvc", value, 0);
        }

        /// <summary>
        /// Gets or sets the direction, if any, the communication that was observed has taken.
        /// </summary>
        public string DeviceDirection
        {
            get => ReturnExtensionValue("deviceDirection"); set => AddExtension("deviceDirection", value, 0);
        }

        /// <summary>
        /// Gets or sets the device's DNS domain part of the complete fully qualified domain name (FQDN).
        /// </summary>
        public string DeviceDnsDomain
        {
            get => ReturnExtensionValue("deviceDnsDomain"); set => AddExtension("deviceDnsDomain", value, 255);
        }

        /// <summary>
        /// Gets or sets the category assigned by the originating device. Devices often times use
        /// their own categorization schema to classify events.
        /// </summary>
        public string DeviceEventCategory
        {
            get => ReturnExtensionValue("cat"); set => AddExtension("cat", value, 1023);
        }

        /// <summary>
        /// Gets or sets the name of the device host. The format should be a fully qualified domain
        /// name associated with the device node, when a node is available.
        /// </summary>
        public string DeviceHostName
        {
            get => ReturnExtensionValue("dvchost"); set => AddExtension("dvchost", value, 100);
        }

        /// <summary>
        /// Gets or sets the device interface on which the packet or data entered the device.
        /// </summary>
        public string DeviceInboundInterface
        {
            get => ReturnExtensionValue("deviceInboundInterface");

            set => AddExtension("deviceInboundInterface", value, 15);
        }

        /// <summary>
        /// Gets or sets the name of the device's MAC address. Six colon-separated hexadecimal
        /// numbers. Example: "00:0D:60:AF:1B:61".
        /// </summary>
        public string DeviceMacAddress
        {
            get => ReturnExtensionValue("deviceMacAddress"); set => AddExtension("deviceMacAddress", value, 0);
        }

        /// <summary>
        /// Gets or sets the device's Windows domain name.
        /// </summary>
        public string DeviceNtDomain
        {
            get => ReturnExtensionValue("deviceNtDomain"); set => AddExtension("deviceNtDomain", value, 255);
        }

        /// <summary>
        /// Gets or sets the device interface on which the packet or data left the device.
        /// </summary>
        public string DeviceOutboundInterface
        {
            get => ReturnExtensionValue("deviceOutboundInterface");

            set => AddExtension("deviceOutboundInterface", value, 15);
        }

        /// <summary>
        /// Gets or sets the name of the destination process which is the event's destination. For
        /// example: "telnetd", or "sshd".
        /// </summary>
        public string DeviceProcessName
        {
            get => ReturnExtensionValue("deviceProcessName");

            set => AddExtension("deviceProcessName", value, 1023);
        }

        /// <summary>
        /// Gets or sets the Device Product. Device Vendor, Device Product and Device Version are
        /// strings that uniquely identify the type of sending device. No two products may use the
        /// same device-vendor and device-product pair. There is no central authority managing these
        /// pairs. Event producers have to ensure that they assign unique name pairs.
        /// </summary>
        public string DeviceProduct
        {
            get => _deviceProduct;

            set
            {
                Check.NotNull(value, nameof(value));
                Check.NotEmpty(value, "The value specified for a set operation is equal to Empty.");

                _deviceProduct = Encode(value, true);
            }
        }

        /// <summary>
        /// Gets or sets the Device Vendor. Device Vendor, Device Product and Device Version are
        /// strings that uniquely identify the type of sending device. No two products may use the
        /// same device-vendor and device-product pair. There is no central authority managing these
        /// pairs. Event producers have to ensure that they assign unique name pairs.
        /// </summary>
        public string DeviceVendor
        {
            get => _deviceVendor;

            set
            {
                Check.NotNull(value, nameof(value));
                Check.NotEmpty(value, "The value specified for a set operation is equal to Empty.");

                _deviceVendor = Encode(value, true);
            }
        }

        /// <summary>
        /// Gets or sets the Device Version. Device Vendor, Device Product and Device Version are
        /// strings that uniquely identify the type of sending device. No two products may use the
        /// same device-vendor and device-product pair. There is no central authority managing these
        /// pairs. Event producers have to ensure that they assign unique name pairs.
        /// </summary>
        public string DeviceVersion
        {
            get => _deviceVersion;

            set
            {
                Check.NotNull(value, nameof(value));
                Check.NotEmpty(value, "The value specified for a set operation is equal to Empty.");

                _deviceVersion = Encode(value, true);
            }
        }

        /// <summary>
        /// Gets or sets the time at which the activity related to the event ended.
        /// </summary>
        public DateTime EndTime
        {
            get => FromCefDateString("end"); set => AddExtension("end", ToCefDateString(value), 0);
        }

        /// <summary>
        /// Gets or sets the event category assigned by the originating device. Devices oftentimes
        /// use their won categorization schema to classify events.
        /// </summary>
        public string EventCategory
        {
            get => ReturnExtensionValue("cat"); set => AddExtension("cat", value, 1023);
        }

        /// <summary>
        /// Gets or sets how many times was this same event observed.
        /// </summary>
        public int EventCount
        {
            get => ToInteger("cnt"); set => AddExtension("cnt", Check.NotNegative(value, nameof(value)), 0);
        }

        /// <summary>
        /// Gets or sets the facility of the message.
        /// </summary>
        public Facility Facility { get; set; }

        /// <summary>
        /// Gets or sets the time when the file was created.
        /// </summary>
        public DateTime FileCreateTime
        {
            get => FromCefDateString("fileCreateTime");
            set => AddExtension("fileCreateTime", ToCefDateString(value), 0);
        }

        /// <summary>
        /// Gets or sets the hash of the file.
        /// </summary>
        public string FileHash
        {
            get => ReturnExtensionValue("fileHash"); set => AddExtension("fileHash", value, 255);
        }

        /// <summary>
        /// Gets or sets the Id of the file. This could be the inode.
        /// </summary>
        public string FileId
        {
            get => ReturnExtensionValue("fileId"); set => AddExtension("fileId", value, 1023);
        }

        /// <summary>
        /// Gets or sets the time when the file was last modified.
        /// </summary>
        public DateTime FileModificationTime
        {
            get => FromCefDateString("fileModificationTime");

            set => AddExtension("fileModificationTime", ToCefDateString(value), 0);
        }

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        public string FileName
        {
            get => ReturnExtensionValue("fname"); set => AddExtension("fname", value, 1023);
        }

        /// <summary>
        /// Gets or sets the full path to the file, including the file name itself.
        /// </summary>
        public string FilePath
        {
            get => ReturnExtensionValue("filePath"); set => AddExtension("filePath", value, 1023);
        }

        /// <summary>
        /// Gets or sets the permissions of the file.
        /// </summary>
        public string FilePermission
        {
            get => ReturnExtensionValue("filePermission"); set => AddExtension("filePermission", value, 1023);
        }

        /// <summary>
        /// Gets or sets the size of the file.
        /// </summary>
        public int FileSize
        {
            get => ToInteger("fsize"); set => AddExtension("fsize", value, 0);
        }

        /// <summary>
        /// Gets or sets the type of the file. (pipe, socket, etc.)
        /// </summary>
        public string FileType
        {
            get => ReturnExtensionValue("fileType"); set => AddExtension("fileType", value, 1023);
        }

        /// <summary>
        /// Gets or sets the level of the message.
        /// </summary>
        public Level Level { get; set; }

        /// <summary>
        /// Gets or sets an arbitrary message giving more details about the event. Multi-line
        /// entries can be produced by using \n as the new-line separator.
        /// </summary>
        public string Message
        {
            get => ReturnExtensionValue("msg"); set => AddExtension("msg", value, 1023);
        }

        /// <summary>
        /// Gets or sets the Name of the Event. Name is a string representing a human-readable and
        /// understandable description of the event. The event name should not contain information
        /// that is specifically mentioned in other fields. For example: "Port scan from 10.0.0.1
        /// targeting 20.1.1.1" is not a good event name. It should be: "Port scan". The other
        /// information is redundant and can be picked up from the other fields.
        /// </summary>
        public string Name
        {
            get => _name;

            set
            {
                Check.NotNull(value, nameof(value));
                Check.NotEmpty(value, "The value specified for a set operation is equal to Empty.");

                _name = Encode(value, true);
            }
        }

        /// <summary>
        /// Gets or sets the time when the old file was created.
        /// </summary>
        public DateTime OldFileCreateTime
        {
            get => FromCefDateString("oldFileCreateTime");

            set => AddExtension("oldFileCreateTime", ToCefDateString(value), 0);
        }

        /// <summary>
        /// Gets or sets the hash of the old file.
        /// </summary>
        public string OldFileHash
        {
            get => ReturnExtensionValue("oldFileHash"); set => AddExtension("oldFileHash", value, 255);
        }

        /// <summary>
        /// Gets or sets the Id of the old file. This could be the inode.
        /// </summary>
        public string OldFileId
        {
            get => ReturnExtensionValue("oldFileId"); set => AddExtension("oldFileId", value, 1023);
        }

        /// <summary>
        /// Gets or sets the time when the old file was last modified.
        /// </summary>
        public DateTime OldFileModificationTime
        {
            get => FromCefDateString("oldFileModificationTime");

            set => AddExtension("oldFileModificationTime", ToCefDateString(value), 0);
        }

        /// <summary>
        /// Gets or sets the full path to the old file, including the old file name itself.
        /// </summary>
        public string OldFilePath
        {
            get => ReturnExtensionValue("oldFilePath"); set => AddExtension("oldFilePath", value, 1023);
        }

        /// <summary>
        /// Gets or sets the permissions of the old file.
        /// </summary>
        public string OldFilePermission
        {
            get => ReturnExtensionValue("oldFilePermission");

            set => AddExtension("oldFilePermission", value, 1023);
        }

        /// <summary>
        /// Gets or sets the type of the old file. (pipe, socket, etc.)
        /// </summary>
        public string OldFileType
        {
            get => ReturnExtensionValue("oldFileType"); set => AddExtension("oldFileType", value, 1023);
        }

        /// <summary>
        /// Gets or sets the time at which the event related to the activity was received.
        /// </summary>
        public DateTime ReceiptTime
        {
            get => FromCefDateString("rt"); set => AddExtension("rt", ToCefDateString(value), 0);
        }

        /// <summary>
        /// Gets or sets the User-Agent associated with the request.
        /// </summary>
        public string RequestClientApplication
        {
            get => ReturnExtensionValue("requestClientApplication");

            set => AddExtension("requestClientApplication", value, 1023);
        }

        /// <summary>
        /// Gets or sets the cookies associated with the request.
        /// </summary>
        public string RequestCookies
        {
            get => ReturnExtensionValue("requestCookies"); set => AddExtension("requestCookies", value, 1023);
        }

        /// <summary>
        /// Gets or sets the method used to access a URL. Possible values: "POST", "GET", ...
        /// </summary>
        public string RequestMethod
        {
            get => ReturnExtensionValue("requestMethod"); set => AddExtension("requestMethod", value, 1023);
        }

        /// <summary>
        /// Gets or sets the request URL. In the case of an HTTP request, this field contains the
        /// URL accessed. The URL should contain the protocol as well, e.g., "http://www.security.com".
        /// </summary>
        [SuppressMessage(
            "Design",
            "CA1056:URI-like properties should not be strings",
            Justification = "A strin object is just fine here!")]
        public string RequestUrl
        {
            get => ReturnExtensionValue("request"); set => AddExtension("request", value, 1023);
        }

        /// <summary>
        /// Gets or sets the severity. Severity is an integer and reflects the importance of the
        /// event. Only numbers from 0 to 10 are allowed, where 10 indicates the most important event.
        /// </summary>
        public int Severity
        {
            get => _severity;

            set
            {
                if ((value < 0) || (value > 10))
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                _severity = value;
            }
        }

        /// <summary>
        /// Gets or sets the Signature ID. The Signature ID is a unique identifier per event-type
        /// and identifies the type of event being reported. In the intrusion detection system (IDS)
        /// world, each signature or rule that detects certain activity has a unique signature ID
        /// assigned. This is a requirement for other types of devices as well, and helps
        /// correlation engines deal with the events.
        /// </summary>
        public string SignatureId
        {
            get => _signatureId;

            set
            {
                Check.NotNull(value, nameof(value));
                Check.NotEmpty(value, "The value specified for a set operation is equal to Empty.");

                _signatureId = Encode(value, true);
            }
        }

        /// <summary>
        /// Gets or sets the address of source that the event refers to in an IP network. The format
        /// is an IPv4 address. Example: "192.168.10.1".
        /// </summary>
        public IPAddress SourceAddress
        {
            get => IPAddress.Parse(ReturnExtensionValue("src")); set => AddExtension("src", value, 0);
        }

        /// <summary>
        /// Gets or sets the source DNS domain part of the complete fully qualified domain name (FQDN).
        /// </summary>
        public string SourceDnsDomain
        {
            get => ReturnExtensionValue("sourceDnsDomain"); set => AddExtension("sourceDnsDomain", value, 255);
        }

        /// <summary>
        /// Gets or sets the name of the source host that an event refers to in an IP network. The
        /// format should be a fully qualified domain name associated with the source node, when a
        /// node is available.
        /// </summary>
        public string SourceHostName
        {
            get => ReturnExtensionValue("shost"); set => AddExtension("shost", value, 1023);
        }

        /// <summary>
        /// Gets or sets the name of the source MAC address. Six colon-separated hexadecimal
        /// numbers. Example: "00:0D:60:AF:1B:61".
        /// </summary>
        public string SourceMacAddress
        {
            get => ReturnExtensionValue("smac"); set => AddExtension("smac", value, 0);
        }

        /// <summary>
        /// Gets or sets the Windows domain name of the source address.
        /// </summary>
        public string SourceNtDomain
        {
            get => ReturnExtensionValue("sntdom"); set => AddExtension("sntdom", value, 255);
        }

        /// <summary>
        /// Gets or sets the source port. The valid port numbers are between 0 and 65535.
        /// </summary>
        public int SourcePort
        {
            get => ToInteger("spt");

            set
            {
                if ((value < 0) || (value > 65535))
                {
                    throw new ArgumentException("Valid port numbers are between 0 and 65535", nameof(value));
                }

                AddExtension("spt", value, 0);
            }
        }

        /// <summary>
        /// Gets or sets the source user by ID. For example, in UNIX, the root user is generally
        /// associated with user ID 0.
        /// </summary>
        public string SourceUserId
        {
            get => ReturnExtensionValue("suid"); set => AddExtension("suid", value, 1023);
        }

        /// <summary>
        /// Gets or sets the name of the source user by name. This is the user associated with the
        /// event's source. E-mail addresses are also mapped into the UserName fields. The recipient
        /// is a candidate to put into sourceUserName.
        /// </summary>
        public string SourceUserName
        {
            get => ReturnExtensionValue("suser"); set => AddExtension("suser", value, 1023);
        }

        /// <summary>
        /// Gets or sets the source user privileges. The allowed values are: "Administrator",
        /// "User", and "Guest". This identifies the source user's privileges. In UNIX, for example,
        /// activity executed on the root user would be identified with sourceUserPrivileges of
        /// "Administrator". This is an idealized and simplified view on privileges and can be
        /// extended in the future.
        /// </summary>
        public string SourceUserPrivileges
        {
            get => ReturnExtensionValue("spriv"); set => AddExtension("spriv", value, 1023);
        }

        /// <summary>
        /// Gets or sets the time at which the activity related to the event started.
        /// </summary>
        public DateTime StartTime
        {
            get => FromCefDateString("start"); set => AddExtension("start", ToCefDateString(value), 0);
        }

        /// <summary>
        /// Gets or sets the text of the message. CEF:Version|Device Vendor|Device Product|Device
        /// Version|Signature ID|Name|Severity|Extensions.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// The operation attempted is invalid for the state of the object.
        /// </exception>
        public string Text
        {
            get
            {
                if (string.IsNullOrEmpty(DeviceVendor))
                {
                    throw new InvalidOperationException("Device Vendor is missing.");
                }

                if (string.IsNullOrEmpty(DeviceProduct))
                {
                    throw new InvalidOperationException("Device Product is missing.");
                }

                if (string.IsNullOrEmpty(DeviceVersion))
                {
                    throw new InvalidOperationException("Device Version is missing.");
                }

                if (string.IsNullOrEmpty(SignatureId))
                {
                    throw new InvalidOperationException("Signature ID is missing.");
                }

                if (string.IsNullOrEmpty(Name))
                {
                    throw new InvalidOperationException("Name is missing.");
                }

                var builder = new StringBuilder();

                builder.Append("CEF:").Append(Version);
                builder.Append('|').Append(DeviceVendor);
                builder.Append('|').Append(DeviceProduct);
                builder.Append('|').Append(DeviceVersion);
                builder.Append('|').Append(SignatureId);
                builder.Append('|').Append(Name);
                builder.Append('|').Append(Severity);

                foreach (KeyValuePair<string, string> kvp in _extensions)
                {
                    builder.Append('|').Append(kvp.Key).Append('=').Append(kvp.Value);
                }

                return builder.ToString();
            }

            set => throw new InvalidOperationException("You cannot directly set the text for a CEF Message.");
        }

        /// <summary>
        /// Gets or sets the Layer-4 protocol used. The possible values are protocol names such as
        /// TCP or UDP.
        /// </summary>
        public string TransportProtocol
        {
            get => ReturnExtensionValue("proto"); set => AddExtension("proto", value, 31);
        }

        /// <summary>
        /// Gets the CEF version. The CEF version identifies the version of the CEF format and
        /// determine what the properties of this message represent.
        /// </summary>
        public int Version { get; } = 15;

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() => Text;

        private static string Encode(string textToEncode, bool prefix)
        {
            Check.NotNull(textToEncode, nameof(textToEncode));

            textToEncode = textToEncode.Trim();

            if (textToEncode.Length == 0)
            {
                return textToEncode;
            }

            if (textToEncode.Contains("\\"))
            {
                textToEncode = textToEncode.Replace("\\", "\\\\");
            }

            if (textToEncode.Contains("|") && prefix)
            {
                textToEncode = textToEncode.Replace("|", "\\|");
            }

            if (textToEncode.Contains("=") && !prefix)
            {
                textToEncode = textToEncode.Replace("=", "\\=");
            }

            if (textToEncode.Contains("\r\n"))
            {
                textToEncode = textToEncode.Replace("\r\n", "\n");
            }

            if (textToEncode.Contains("\r"))
            {
                textToEncode = textToEncode.Replace("\r", "\n");
            }

            return textToEncode;
        }

        private static string ToCefDateString(DateTime value)
                    => value.ToString("MMM dd yyyy HH:mm:ss", CultureInfo.InvariantCulture);

        private void AddExtension(string keyName, object value, int maximumLength)
        {
            Check.NotNull(value, nameof(value));

            var stringValue = Convert.ToString(value, CultureInfo.InvariantCulture);

            Check.NotEmpty(stringValue, "The value specified for a set operation is equal to Empty.");

            if ((maximumLength > 0) && (stringValue.Length > maximumLength))
            {
                stringValue = stringValue.Substring(0, maximumLength);
            }

            _extensions[keyName] = Encode(stringValue, false);
        }

        private DateTime FromCefDateString(string keyName)
            => DateTime.Parse(ReturnExtensionValue(keyName), CultureInfo.CurrentCulture);

        private string ReturnExtensionValue(string keyName)
            => _extensions.ContainsKey(keyName) ? _extensions[keyName] : null;

        private int ToInteger(string keyName)
            => Convert.ToInt32(ReturnExtensionValue(keyName), CultureInfo.InvariantCulture);
    }
}
