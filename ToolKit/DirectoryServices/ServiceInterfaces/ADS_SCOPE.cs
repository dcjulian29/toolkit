using System.Diagnostics.CodeAnalysis;

namespace ToolKit.DirectoryServices.ServiceInterfaces
{
    /// <summary>
    /// Specifies the scope of a directory search.
    /// </summary>
    [SuppressMessage(
        "Microsoft.Naming",
        "CA1707:IdentifiersShouldNotContainUnderscores",
        Justification = "This enumeration represents Win32API which include underscores.")]
    public enum ADS_SCOPE
    {
        /// <summary>
        /// Limits the search to the base object. The result contains, at most, one object.
        /// </summary>
        BASE = 0,

        /// <summary>
        /// Searches one level of the immediate children, excluding the base object.
        /// </summary>
        ONELEVEL = 1,

        /// <summary>
        /// Searches the whole sub-tree, including all the children and the base object itself.
        /// </summary>
        SUBTREE = 2
    }
}
