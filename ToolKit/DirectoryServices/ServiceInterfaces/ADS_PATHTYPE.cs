//// ReSharper disable InconsistentNaming

namespace ToolKit.DirectoryServices.ServiceInterfaces
{
    /// <summary>
    /// Specifies the type of object on which the security descriptor is modified.
    /// </summary>
    public enum ADS_PATHTYPE
    {
        /// <summary>
        /// Indicates that the security descriptor will be retrieved or set on a file object.
        /// </summary>
        FILE = 1,

        /// <summary>
        /// Indicates that the security descriptor will be retrieved or set on a file share object.
        /// </summary>
        FILESHARE = 2,

        /// <summary>
        /// Indicates that the security descriptor will be retrieved or set on a registry key object.
        /// </summary>
        REGISTRY = 3
    }
}
