using System;
using System.Globalization;
using ToolKit.Validation;

namespace ToolKit.Network
{
    /// <summary>
    /// A representation of an IP version 4 address.
    /// </summary>
    public sealed class IpV4Address : IEquatable<IpV4Address>
    {
        private readonly int _firstOctet;
        private readonly int _secondOctet;
        private readonly int _thirdOctet;
        private readonly int _fourthOctet;

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
            if (octet1 > 255 || octet1 < 0 || (octet2 > 255 || octet2 < 0) ||
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
        /// The IPV4 address in 4 octet form.
        /// </param>
        public IpV4Address(string address)
        {
            Check.NotEmpty(address, nameof(address));

            var octets = address.Split('.');

            if (octets.Length != 4)
            {
                throw new ArgumentException("Address should contain 4 octets");
            }

            var octet1 = Convert.ToInt32(octets[0], CultureInfo.InvariantCulture);
            var octet2 = Convert.ToInt32(octets[1], CultureInfo.InvariantCulture);
            var octet3 = Convert.ToInt32(octets[2], CultureInfo.InvariantCulture);
            var octet4 = Convert.ToInt32(octets[3], CultureInfo.InvariantCulture);

            if (octet1 > 255 || octet1 < 0 || (octet2 > 255 || octet2 < 0) ||
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
        /// Indicates whether the current object is equal to another
        /// object of the same type.
        /// </summary>
        /// <param name="a">The first object of this type to compare.</param>
        /// <param name="b">The second object of this type to compare.</param>
        /// <returns>
        ///   <c>true</c> if the two objects are equal; otherwise, <c>false</c>.
        /// </returns>
        public static bool operator ==(IpV4Address a, IpV4Address b)
        {
            if (object.ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            return !(a is null) && !(b is null) && a.Equals(b);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another
        /// object of the same type.
        /// </summary>
        /// <param name="a">The first object of this type to compare.</param>
        /// <param name="b">The second object of this type to compare.</param>
        /// <returns>
        ///   <c>true</c> if the two objects are equal; otherwise, <c>false</c>.
        /// </returns>
        public static bool operator !=(IpV4Address a, IpV4Address b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// A 32-bit signed integer that indicates the relative order of the
        /// objects being compared. The return value has the following meanings:
        /// <list type="table">
        /// <listheader>
        /// <term>Value</term>
        /// <description>Meaning</description>
        /// </listheader>
        /// <item>
        /// <term>Less than zero</term>
        /// <description>This object is less than the <paramref name="other"/> parameter.</description>
        /// </item>
        /// <item>
        /// <term>Zero</term>
        /// <description>This object is equal to <paramref name="other"/>.</description>
        /// </item>
        /// <item>
        /// <term>Greater than zero</term>
        /// <description>This object is greater than <paramref name="other"/>.</description>
        /// </item>
        /// </list>
        /// </returns>
        public int CompareTo(IpV4Address other)
        {
            // If other is not a valid object reference, this instance is greater.
            if (other == null)
            {
                return 1;
            }

            if (Equals(other))
            {
                return 0;
            }

            var combined1 = string.Format(
                CultureInfo.InvariantCulture,
                "{0:D3}{1:D3}{2:D3}{3:D3}",
                _firstOctet,
                _secondOctet,
                _thirdOctet,
                _fourthOctet);
            var combined2 = string.Format(
                CultureInfo.InvariantCulture,
                "{0:D3}{1:D3}{2:D3}{3:D3}",
                other._firstOctet,
                other._secondOctet,
                other._thirdOctet,
                other._fourthOctet);

            return string.CompareOrdinal(combined1, combined2);
        }

        /// <summary>
        /// Determines whether the specified
        /// <see cref="object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="object"/>
        /// to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="object"/>
        ///   is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as IpV4Address);
        }

        /// <summary>
        /// Determines whether the specified IPV4 Address is equal to the current IPV4 Address.
        /// </summary>
        /// <param name="other">
        /// The IPV4 Address to compare with the current IPV4 Address.
        /// </param>
        /// <returns>
        /// <c>true</c> if the specified IPV4 Address is equal
        /// to the current IPV4 Address; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(IpV4Address other)
        {
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (other == null || GetType() != other.GetType())
            {
                return false;
            }

            return (_firstOctet == other._firstOctet) &&
                (_secondOctet == other._secondOctet) &&
                (_thirdOctet == other._thirdOctet) &&
                (_fourthOctet == other._fourthOctet);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms
        /// and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            const int Prime = 31;
            var result = 1;

            result = (Prime * result) + _firstOctet;
            result = (Prime * result) + _secondOctet;
            result = (Prime * result) + _thirdOctet;
            result = (Prime * result) + _fourthOctet;

            return result;
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
            var separator = string.Empty;
            if (includePeriod)
            {
                separator = ".";
            }

            return string.Format(
                CultureInfo.InvariantCulture,
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
            return string.Format(
                CultureInfo.InvariantCulture,
                "{0}.{1}.{2}.{3}",
                _firstOctet,
                _secondOctet,
                _thirdOctet,
                _fourthOctet);
        }
    }
}
