using System.Diagnostics.CodeAnalysis;

namespace ToolKit.DirectoryServices.ServiceInterfaces
{
    /// <summary>
    /// Specifies the behavior of alias dereferencing.
    /// </summary>
    [SuppressMessage(
        "Microsoft.Naming",
        "CA1707:IdentifiersShouldNotContainUnderscores",
        Justification = "This enumeration represents Win32API which include underscores.")]
    public enum ADS_DEREF
    {
        /// <summary>
        /// Does not dereference aliases when searching or locating the base object of the search.
        /// </summary>
        NEVER = 0,

        /// <summary>
        /// Dereferences aliases when searching subordinates of the base object, but not when
        /// locating the base itself.
        /// </summary>
        SEARCHING = 1,

        /// <summary>
        /// Dereferences aliases when locating the base object of the search, but not when searching
        /// its subordinates.
        /// </summary>
        FINDING = 2,

        /// <summary>
        /// Dereferences aliases when both searching subordinates and locating the base object of
        /// the search.
        /// </summary>
        ALWAYS = 3
    }
}
