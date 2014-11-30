using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolKit.Network
{
    /// <summary>
    /// A representation of an IP version 4 address
    /// </summary>
    public class IpV4Address
    {
        private int _firstOctet;
        private int _secondOctet;
        private int _thirdOctet;
        private int _fourthOctet;

        /// <summary>
        /// Initializes a new instance of the <see cref="IpV4Address"/> class.
        /// </summary>
        /// <param name="octet1">
        /// The first octet of the IPV4 address.
        /// </param>
        /// <param name="octet2">
        /// The second octet of the IPV4 address.
        /// </param>
        /// <param name="octet3">
        /// The third octet of the IPV4 address.
        /// </param>
        /// <param name="octet4">
        /// The fourth octet of the IPV4 address.
        /// </param>
        public IpV4Address(int octet1, int octet2, int octet3, int octet4)
        {
            if (
                (octet1 > 255 || octet1 < 0) || (octet2 > 255 || octet2 < 0) || 
                (octet3 > 255 || octet3 < 0) || (octet4 > 255 || octet4 < 0))
            {
                throw new ArgumentException("Octet should be equal or between 0 and 255");    
            }

            _firstOctet = octet1;
            _secondOctet = octet2;
            _thirdOctet = octet3;
            _fourthOctet = octet4;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IpV4Address"/> class.
        /// </summary>
        /// <param name="address">
        /// The IPV4 address in 4 octet form
        /// </param>
        public IpV4Address(string address)
        {
            var octets = address.Split('.');

            if (octets.Length != 4)
            {
                throw new ArgumentException("Address should contain 4 octets");
            }

            var octet1 = Convert.ToInt32(octets[0]);
            var octet2 = Convert.ToInt32(octets[1]);
            var octet3 = Convert.ToInt32(octets[2]);
            var octet4 = Convert.ToInt32(octets[3]);

            if (
                (octet1 > 255 || octet1 < 0) || (octet2 > 255 || octet2 < 0) ||
                (octet3 > 255 || octet3 < 0) || (octet4 > 255 || octet4 < 0))
            {
                throw new ArgumentException("Octet should be equal or between 0 and 255");
            }

            _firstOctet = octet1;
            _secondOctet = octet2;
            _thirdOctet = octet3;
            _fourthOctet = octet4;
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="IpV4Address"/> class from being created. 
        /// </summary>
        private IpV4Address()
        {
        }

        /// <summary>
        /// Converts the address to binary representation.
        /// </summary>
        /// <param name="includePeriod">
        /// Indicates weather to use a period between octets.
        /// </param>
        /// <returns>
        /// A binary representation of this IP address.
        /// </returns>
        public string ToBinary(bool includePeriod = false)
        {
            var separator = String.Empty;
            if (includePeriod)
            {
                separator = ".";
            }

            return String.Format(
                "{0}{4}{1}{4}{2}{4}{3}",
                Convert.ToString(_firstOctet, 2).PadLeft(8, '0'),
                Convert.ToString(_secondOctet, 2).PadLeft(8, '0'),
                Convert.ToString(_thirdOctet, 2).PadLeft(8, '0'),
                Convert.ToString(_fourthOctet, 2).PadLeft(8, '0'),
                separator);
        }

        /// <summary>
        /// Converts the address to string representation.
        /// </summary>
        /// <returns>
        /// A string representation of this IP address.
        /// </returns>
        public override string ToString()
        {
            return String.Format("{0}.{1}.{2}.{3}", _firstOctet, _secondOctet, _thirdOctet, _fourthOctet);
        }
    }
}
