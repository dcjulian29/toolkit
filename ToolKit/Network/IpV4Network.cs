using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolKit.Network
{
    /// <summary>
    /// A representation of an IP version 4 address
    /// </summary>
    public class IpV4Network
    {
        private IpV4Address _address;
        private IpV4Address _mask;

        /// <summary>
        /// Initializes a new instance of the <see cref="IpV4Network"/> class.
        /// </summary>
        /// <param name="address">
        /// The IPV4 address.
        /// </param>
        /// <param name="mask">
        /// The subnet mask in IPV4 format.
        /// </param>
        public IpV4Network(IpV4Address address, IpV4Address mask)
        {
            _address = address;
            _mask = mask;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IpV4Network"/> class.
        /// </summary>
        /// <param name="address">
        /// The IPV4 address.
        /// </param>
        /// <param name="mask">
        /// The subnet mask in IPV4 format.
        /// </param>
        public IpV4Network(IpV4Address address, int mask)
        {
            _address = address;

            var octet1 = 0;
            var octet2 = 0;
            var octet3 = 0;
            var octet4 = 0;

            var bits = String.Format("{0}{1}", new String('1', mask), new string('0', 32 - mask));

            if (mask >= 1 && mask <= 8)
            {
                octet1 = Convert.ToInt32(bits.Substring(0, 8), 2);
            }
            else if (mask >= 9 && mask <= 16)
            {
                octet1 = 255;
                octet2 = Convert.ToInt32(bits.Substring(8, 8), 2);
            }
            else if (mask >= 17 && mask <= 24)
            {
                octet1 = 255;
                octet2 = 255;
                octet3 = Convert.ToInt32(bits.Substring(16, 8), 2);
            }
            else if (mask >= 25 && mask <= 32)
            {
                octet1 = 255;
                octet2 = 255;
                octet3 = 255;
                octet4 = Convert.ToInt32(bits.Substring(24), 2);
            }

            _mask = new IpV4Address(octet1, octet2, octet3, octet4);
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="IpV4Network"/> class from being created. 
        /// </summary>
        private IpV4Network()
        {
        }

        /// <summary>
        /// Gets the Internet Protocol (IP) V4 Address.
        /// </summary>
        public IpV4Address Address
        {
            get
            {
                return _address;
            }
        }

        /// <summary>
        /// Gets the CIDR representation of the bit mask of the network.
        /// </summary>
        public int Bitmask
        {
            get
            {
                return _mask.ToBinary().Count(f => f == '1');
            }
        }

        /// <summary>
        /// Gets the Internet Protocol (IP) V4 Broadcast Address..
        /// </summary>
        public IpV4Address Broadcast
        {
            get
            {
                var retval = new StringBuilder();
                var address = ConvertToOctets(_address);
                var mask = ConvertToOctets(_mask);
                var inverted = new List<int>();

                mask.Each(m => inverted.Add(255 - m));

                for (int i = 0; i < 4; i++)
                {
                    retval.AppendFormat("{0}.", address[i] | inverted[i]);
                }

                return new IpV4Address(retval.ToString(0, retval.Length - 1));
            }
        }

        /// <summary>
        /// Gets the Network Mask.
        /// </summary>
        public IpV4Address Netmask
        {
            get
            {
                return _mask;
            }
        }

        /// <summary>
        /// Gets the Internet Protocol (IP) V4 Network Id Address..
        /// </summary>
        public IpV4Address NetworkId
        {
            get
            {
                var retval = new StringBuilder();
                var address = ConvertToOctets(_address);
                var mask = ConvertToOctets(_mask);

                for (int i = 0; i < 4; i++)
                {
                    retval.AppendFormat("{0}.", address[i] & mask[i]);
                }

                return new IpV4Address(retval.ToString(0, retval.Length - 1));
            }
        }

        /// <summary>
        /// Gets the Maximum Host IPV4 Address for the Network
        /// </summary>
        public IpV4Address MaximumAddress
        {
            get
            {
                var broadcast = Broadcast;
                var octets = broadcast.ToString().Split('.');

                return new IpV4Address(
                    Convert.ToInt32(octets[0]),
                    Convert.ToInt32(octets[1]),
                    Convert.ToInt32(octets[2]),
                    Convert.ToInt32(octets[3]) - 1);
            }
        }

        /// <summary>
        /// Gets the Minimum Host IPV4 Address for the Network
        /// </summary>
        public IpV4Address MinumumAddress
        {
            get
            {
                var network = NetworkId;
                var octets = network.ToString().Split('.');

                return new IpV4Address(
                    Convert.ToInt32(octets[0]),
                    Convert.ToInt32(octets[1]),
                    Convert.ToInt32(octets[2]),
                    Convert.ToInt32(octets[3]) + 1);
            }
        }

        /// <summary>
        /// Gets the Number of Hosts for the Network
        /// </summary>
        public int NumberOfHosts
        {
            get
            {
                return Convert.ToInt32(Math.Pow(2, _mask.ToBinary().Count(f => f == '0')) - 2);
            }
        }

        private IReadOnlyList<int> ConvertToOctets(IpV4Address address)
        {
            var binary = address.ToBinary(true);
            var octets = binary.Split('.');
            var integers = new List<int>();

            octets.Each(octet => integers.Add(Convert.ToInt32(octet, 2)));

            return integers;
        }
    }
}
