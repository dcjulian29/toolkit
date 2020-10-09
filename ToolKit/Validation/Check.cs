using System;

namespace ToolKit.Validation
{
    /// <summary>
    /// Provide common parameter checking.
    /// </summary>
    public static class Check
    {
        /// <summary>
        /// Check to validate that the string is not null or empty.
        /// </summary>
        /// <param name="value">The parameter's value.</param>
        /// <param name="parameterName">The parameter's string representation.</param>
        /// <returns>The value of the parameter if it is not <c>null</c> or empty.</returns>
        public static string NotEmpty(string value, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(parameterName);
            }

            return value;
        }

        /// <summary>
        /// Check to validate that the string is not null or empty.
        /// </summary>
        /// <param name="value">The parameter's value.</param>
        /// <param name="parameterName">The parameter's string representation.</param>
        /// <param name="message">The exception message to use.</param>
        /// <returns>The value of the parameter if it is not <c>null</c> or empty.</returns>
        public static string NotEmpty(string value, string parameterName, string message)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(message, parameterName);
            }

            return value;
        }

        /// <summary>
        /// Check to validate that the number is not negative.
        /// </summary>
        /// <param name="value">The parameter's value.</param>
        /// <param name="parameterName">The parameter name.</param>
        /// <returns>The value of the parameter if it is not negative.</returns>
        public static int NotNegative(int value, string parameterName)
        {
            if (value < 0)
            {
                throw new ArgumentException(parameterName);
            }

            return value;
        }

        /// <summary>
        /// Check to validate that the number is not negative.
        /// </summary>
        /// <param name="value">The parameter's value.</param>
        /// <param name="parameterName">The parameter name.</param>
        /// <param name="message">The exception message to use.</param>
        /// <returns>The value of the parameter if it is not negative.</returns>
        public static int NotNegative(int value, string parameterName, string message)
        {
            if (value < 0)
            {
                throw new ArgumentException(message, parameterName);
            }

            return value;
        }

        /// <summary>
        /// Check to validate that the parameter is not null.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="value">The parameter's value.</param>
        /// <param name="parameterName">The parameter's string representation.</param>
        /// <returns>The value of the parameter if it is not <c>null</c>.</returns>
        public static T NotNull<T>(T value, string parameterName)
            where T : class => value ?? throw new ArgumentNullException(parameterName);

        /// <summary>
        /// Check to validate that the parameter is not null.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="value">The parameter's value.</param>
        /// <param name="parameterName">The parameter's string representation.</param>
        /// <returns>The value of the parameter if it is not <c>null</c>.</returns>
        public static T? NotNull<T>(T? value, string parameterName)
            where T : struct
        {
            if (!value.HasValue)
            {
                throw new ArgumentNullException(parameterName);
            }

            return value;
        }
    }
}
