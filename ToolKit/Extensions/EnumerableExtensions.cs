using ToolKit.Validation;

namespace System.Collections.Generic
{
    /// <summary>
    /// Extensions for any class that inherits from IEnumerable.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Extends an enumerable collect to provide the ability to call an action on each member.
        /// </summary>
        /// <typeparam name="T">The type of object stored in the enumerable collection.</typeparam>
        /// <param name="enumberable">The enumerable collection.</param>
        /// <param name="action">The action to execute.</param>
        public static void Each<T>(this IEnumerable<T> enumberable, Action<T> action)
        {
            Check.NotNull(enumberable, nameof(enumberable));
            Check.NotNull(action, nameof(action));

            foreach (var item in enumberable)
            {
                action(item);
            }
        }
    }
}
