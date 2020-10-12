using ToolKit.Validation;

namespace System
{
    /// <summary>
    /// This class provides extensions to enumerations.
    /// </summary>
    public static class EnumerationExtensions
    {
        /// <summary>
        /// Clears a flag within a flag based enumeration.
        /// </summary>
        /// <typeparam name="T">The type of enumeration.</typeparam>
        /// <param name="target">The target enumeration.</param>
        /// <param name="value">The value of the type of enumeration.</param>
        /// <returns>an enumeration where the flag is cleared.</returns>
        public static T ClearFlag<T>(this Enum target, T value)
        {
            Check.NotNull(target, nameof(target));

            if (target.GetType() != value.GetType())
            {
                throw new InvalidOperationException("Enumeration Type Mismatch!");
            }

            try
            {
                return (T)(object)((int)(object)target & ~(int)(object)value);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Could not remove value from enumerated type '{typeof(T).Name}'.", ex);
            }
        }

        /// <summary>
        /// Determines if the enumeration contains the provided flag enabled.
        /// </summary>
        /// <typeparam name="T">The type of enumeration.</typeparam>
        /// <param name="target">The target enumeration.</param>
        /// <param name="value">The value of the type of enumeration.</param>
        /// <returns>
        ///   <c>true</c> if the enumeration contains the provided flag enabled;
        ///   otherwise, <c>false</c>.
        /// </returns>
        public static bool Contains<T>(this Enum target, T value)
        {
            Check.NotNull(target, nameof(target));

            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("Argument is not an Enumeration!");
            }

            if (target.GetType() != value.GetType())
            {
                throw new InvalidOperationException("Enumeration Type Mismatch!");
            }

            try
            {
                return ((int)(object)target & (int)(object)value) == (int)(object)value;
            }
            catch (InvalidCastException)
            {
                return false;
            }
        }

        /// <summary>
        /// Determines if the enumeration is the provided enumeration value.
        /// </summary>
        /// <typeparam name="T">The type of enumeration.</typeparam>
        /// <param name="target">The target enumeration.</param>
        /// <param name="value">The value of the type of enumeration.</param>
        /// <returns>
        ///   <c>true</c> if the enumeration is the provided enumeration value; otherwise, <c>false</c>.
        /// </returns>
        public static bool Is<T>(this Enum target, T value)
        {
            Check.NotNull(target, nameof(target));

            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("Argument is not an Enumeration!");
            }

            if (target.GetType() != value.GetType())
            {
                throw new InvalidOperationException("Enumeration Type Mismatch!");
            }

            try
            {
                return (int)(object)target == (int)(object)value;
            }
            catch (InvalidCastException)
            {
                return false;
            }
        }

        /// <summary>
        /// Sets a flag within a flag based enumeration.
        /// </summary>
        /// <typeparam name="T">The type of enumeration.</typeparam>
        /// <param name="target">The target enumeration.</param>
        /// <param name="value">The value of the type of enumeration.</param>
        /// <returns>an enumeration where the flag is set.</returns>
        public static T SetFlag<T>(this Enum target, T value)
        {
            Check.NotNull(target, nameof(target));

            if (target.GetType() != value.GetType())
            {
                throw new InvalidOperationException("Enumeration Type Mismatch!");
            }

            try
            {
                return (T)(object)((int)(object)target | (int)(object)value);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Could not append value from enumerated type '{typeof(T).Name}'.", ex);
            }
        }
    }
}
