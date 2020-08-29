using System.Diagnostics.CodeAnalysis;

namespace ToolKit.DirectoryServices.ServiceInterfaces
{
    /// <summary>
    /// Specifies the ways to update property values in the property cache.
    /// </summary>
    [SuppressMessage(
        "Microsoft.Naming",
        "CA1707:IdentifiersShouldNotContainUnderscores",
        Justification = "This enumeration represents Win32API which include underscores.")]
    public enum ADS_PROPERTY_OPERATION
    {
        /// <summary>
        /// Instructs the directory service to remove all the property value(s) from the object.
        /// </summary>
        CLEAR = 1,

        /// <summary>
        /// Instructs the directory service to replace the current value(s) with the specified value(s).
        /// </summary>
        UPDATE = 2,

        /// <summary>
        /// Instructs the directory service to append the specified value(s) to the existing
        /// values(s). When the ADS_PROPERTY_APPEND operation is specified, the new attribute
        /// value(s) are automatically committed to the directory service and removed from the local
        /// cache. This forces the local cache to be updated from the directory service the next
        /// time the attribute value(s) are retrieved.
        /// </summary>
        APPEND = 3,

        /// <summary>
        /// Instructs the directory service to delete the specified value(s) from the object.
        /// </summary>
        DELETE = 4
    }
}
