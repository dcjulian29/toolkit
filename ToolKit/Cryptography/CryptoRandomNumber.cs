using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ToolKit.Cryptography
{
    /// <summary>
    /// Utilizes a cryptographic Random Number Generator (RNG) that produces a sequence of numbers
    /// that meet certain statistical requirements for randomness.
    /// </summary>
    public static class CryptoRandomNumber
    {
        private static readonly RNGCryptoServiceProvider _provider = new RNGCryptoServiceProvider();

        /// <summary>
        /// Returns a non-negative random integer.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that is greater than or equal to 0 and less than <see cref="int.MaxValue"/>.
        /// </returns>
        public static int Next()
        {
            var byteArray = new byte[4];
            _provider.GetBytes(byteArray);

            return Math.Abs(BitConverter.ToInt32(byteArray, 0));
        }

        /// <summary>
        /// Returns a non-negative random integer that is less than the specified maximum..
        /// </summary>
        /// <param name="maxValue">The exclusive upper bound of the random number to be generated.</param>
        /// <returns>
        /// A 32-bit signed integer that is greater than or equal to 0 and less than <see cref="int.MaxValue"/>.
        /// </returns>
        public static int Next(int maxValue)
        {
            if (maxValue < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxValue), "maxValue is less than 0!");
            }

            return GetNumber(0, maxValue);
        }

        /// <summary>
        /// Returns a non-negative random integer that is within a specified range.
        /// </summary>
        /// <param name="minValue">The inclusive lower bound of the random number return.</param>
        /// <param name="maxValue">The exclusive upper bound of the random number to be generated.</param>
        /// <returns>
        /// A 32-bit signed integer that is greater than or equal to 0 and less than <see cref="int.MaxValue"/>.
        /// </returns>
        public static int Next(int minValue, int maxValue)
        {
            if (maxValue < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxValue), "maxValue is less than 0!");
            }

            if (minValue < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxValue), "minValue is less than 0!");
            }

            if (maxValue < minValue)
            {
                throw new ArgumentOutOfRangeException(nameof(maxValue), "maxValue is less than minValue!");
            }

            if (minValue == maxValue)
            {
                return minValue;
            }

            return GetNumber(minValue, maxValue);
        }

        /// <summary>
        /// Fills the elements of a specified array of bytes with random numbers.
        /// </summary>
        /// <param name="bytes">An array of bytes to contain the random numbers.</param>
        /// <exception cref="ArgumentNullException">Bytes is null!.</exception>
        public static void NextBytes(byte[] bytes)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException(nameof(bytes), "Bytes is null!");
            }

            _provider.GetBytes(bytes);
        }

        /// <summary>
        /// Returns a random floating-point number that is greater than or equal to 0.0, and less
        /// than 1.0.
        /// </summary>
        /// <returns>
        /// A double-precision floating point number that is greater than or equal to 0.0, and less
        /// than 1.0.
        /// </returns>
        public static double NextDouble()
        {
            var bytes = new byte[8];
            NextBytes(bytes);

            var sb = new StringBuilder("0.");

            var numbers = ConvertToIntegers(bytes);

            numbers.Each(n => sb.Append(n));

            return Convert.ToDouble(sb.ToString(), CultureInfo.CurrentCulture);
        }

        [ExcludeFromCodeCoverage]
        private static IEnumerable<int> ConvertToIntegers(IEnumerable<byte> bytes)
        {
            return bytes.Select(Convert.ToInt32).ToArray();
        }

        [ExcludeFromCodeCoverage]
        private static int GetNumber(int minValue, int maxValue)
        {
            int number;

            do
            {
                number = Next();
            }
            while ((number < 0) || (number >= maxValue) || (number < minValue));

            return number;
        }
    }
}
