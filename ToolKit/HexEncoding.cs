using System;
using System.Globalization;
using System.Linq;
using Common.Logging;

namespace ToolKit
{
    /// <summary>
    /// A class that handles Hex encoding and decoding.
    /// </summary>
    public class HexEncoding
    {
        private static ILog _log = LogManager.GetLogger<HexEncoding>();

        /// <summary>
        /// Gets the byte count of the hex string.
        /// </summary>
        /// <param name="hexString">The hex string.</param>
        /// <returns>the number of bytes of the hex values</returns>
        public static int GetByteCount(string hexString)
        {
            var numHexChars = hexString.Count(IsHexDigit);

            // If odd number of characters, discard last character
            if (numHexChars % 2 != 0)
            {
                numHexChars--;
            }

            return numHexChars / 2; // 2 characters per byte
        }

        /// <summary>
        /// Returns true is c is a hexadecimal digit (A-F, a-f, 0-9)
        /// </summary>
        /// <param name="c">Character to test</param>
        /// <returns>true if hex digit, false if not</returns>
        public static bool IsHexDigit(char c)
        {
            var numA = Convert.ToInt32('A');
            var num1 = Convert.ToInt32('0');
            c = Char.ToUpper(c);

            var numChar = Convert.ToInt32(c);

            if (numChar >= numA && numChar < (numA + 6))
            {
                return true;
            }

            if (numChar >= num1 && numChar < (num1 + 10))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Determines if given string is in proper hexadecimal string format
        /// </summary>
        /// <param name="hexString">The hex string.</param>
        /// <returns>true if hex formatted, false if not</returns>
        public static bool IsHexFormat(string hexString)
        {
            return hexString.All(IsHexDigit);
        }

        /// <summary>
        /// Creates a byte array from the hexadecimal string. Each two characters are combined to
        /// create one byte. First two hexadecimal characters become first byte in returned array.
        /// Non-hexadecimal characters are ignored.
        /// </summary>
        /// <param name="hexString">string to convert to byte array</param>
        /// <returns>byte array, in the same left-to-right order as the hexString</returns>
        public static byte[] ToBytes(string hexString)
        {
            // Remove all none A-F, 0-9, characters
            var strip = hexString.Where(IsHexDigit).Aggregate(String.Empty, (current, c) => current + c);

            // If odd number of characters, discard last character
            if (strip.Length % 2 != 0)
            {
                strip = strip.Substring(0, strip.Length - 1);
            }

            var byteLength = strip.Length / 2;
            var bytes = new byte[byteLength];
            var j = 0;

            for (var i = 0; i < bytes.Length; i++)
            {
                var hex = new string(new char[] { strip[j], strip[j + 1] });
                bytes[i] = byte.Parse(hex, NumberStyles.HexNumber);
                j = j + 2;
            }

            return bytes;
        }

        /// <summary>
        /// Creates a string from the hexadecimal byte array. Each byte is converted into two
        /// characters representing the hex value.
        /// </summary>
        /// <param name="bytes">byte array to convert to string</param>
        /// <returns>string, in the same left-to-right order as the byte array</returns>
        public static string ToString(byte[] bytes)
        {
            return bytes.Aggregate(String.Empty, (current, t) => current + t.ToString("X2"));
        }
    }
}
