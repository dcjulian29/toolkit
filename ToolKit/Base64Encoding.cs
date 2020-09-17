using System;
using Common.Logging;

namespace ToolKit
{
    /// <summary>
    /// A class that handles Base64 encoding and decoding.
    /// </summary>
    public static class Base64Encoding
    {
        private static readonly ILog _log = LogManager.GetLogger("Base64Encoding");

        /// <summary>
        /// Creates a byte array from the Base64 encoded string.
        /// </summary>
        /// <param name="data">string to convert to byte array.</param>
        /// <returns>byte array, in the same left-to-right order as the data.</returns>
        public static byte[] ToBytes(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return Array.Empty<byte>();
            }

            try
            {
                return Convert.FromBase64String(data);
            }
            catch (FormatException fex)
            {
                _log.Error($"The provided string ({data}) does not appear to be base64 encoded.", fex);
                throw;
            }
        }

        /// <summary>
        /// Creates a base64 encoded string from the byte array.
        /// </summary>
        /// <param name="bytes">byte array to convert to string.</param>
        /// <returns>string, in the same left-to-right order as the byte array.</returns>
        public static string ToString(byte[] bytes)
        {
            if ((bytes == null) || (bytes.Length == 0))
            {
                return string.Empty;
            }

            return Convert.ToBase64String(bytes);
        }
    }
}
