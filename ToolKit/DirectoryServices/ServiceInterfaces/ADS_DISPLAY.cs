//// ReSharper disable InconsistentNaming

namespace ToolKit.DirectoryServices.ServiceInterfaces
{
    /// <summary>
    /// Specifies how a path is displayed.
    /// </summary>
    public enum ADS_DISPLAY
    {
        /// <summary>
        /// The path is displayed with both attributes and values. For example, CN=Jeff Smith.
        /// </summary>
        FULL = 1,

        /// <summary>
        /// The path is displayed with values only. For example, Jeff Smith.
        /// </summary>
        VALUE_ONLY = 2
    }
}
