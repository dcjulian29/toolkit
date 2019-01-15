using System;
using Common.Logging;

namespace ToolKit.DirectoryServices.ActiveDirectory
{
    /// <summary>
    /// This class is wrapper for the IADsLargeInteger value returned from Active Directory and
    /// represents a date time value. A IADsLargeInteger value is a 64-bit value that represents the
    /// number of 100-nanosecond intervals that have elapsed since 12:00 midnight, January 1, 1601
    /// A.D. (C.E.) Coordinated Universal Time (UTC).
    /// </summary>
    public static class AdDateTime
    {
        /// <summary>
        /// Converts a DateTime object into an IADsLargeInteger object
        /// </summary>
        /// <param name="dateTime">The DateTime object to convert</param>
        /// <returns>The UTC DateTime object represented as an IADsLargeInteger</returns>
        public static IADsLargeInteger ToADsLargeInteger(DateTime dateTime)
        {
            var int64Value = dateTime.ToFileTimeUtc();

            IADsLargeInteger largeIntValue = new LargeInteger
            {
                HighPart = (int)(int64Value >> 32),
                LowPart = (int)(int64Value & 0xFFFFFFFF)
            };

            return largeIntValue;
        }

        /// <summary>
        /// Converts an IADsLargeInteger object into a date and time object
        /// </summary>
        /// <param name="dateTimeObject">The IADsLargeInteger object</param>
        /// <returns>The IADsLargeInteger object represented as a DateTime object</returns>
        public static DateTime ToDateTime(object dateTimeObject)
        {
            var dateTime = dateTimeObject as LargeInteger;

            var high = (long)dateTime.HighPart;
            var low = (long)dateTime.LowPart;

            if ((low == -1) && (high == 2147483647))
            {
                // This number is due to Interop interpretation between unsigned and signed values
                // This low and high combination represents 64 1's
                return DateTime.MaxValue;
            }

            if (low < 0)
            {
                high++;
            }

            high = high << 32;

            return DateTime.FromFileTime(high + low).ToUniversalTime();
        }

        /// <summary>
        /// Convert the DateTime to a format understood by LDAP.
        /// </summary>
        /// <param name="when">The date and time to convert.</param>
        /// <returns>string containing an LDAP formatted date and time.</returns>
        public static string ToLdapDateTime(DateTime when)
        {
            return String.Format(
                "{0:0000}{1:00}{2:00}{3:00}{4:00}{5:00}.0Z",
                when.Year,
                when.Month,
                when.Day,
                when.Hour,
                when.Minute,
                when.Second);
        }
    }
}
