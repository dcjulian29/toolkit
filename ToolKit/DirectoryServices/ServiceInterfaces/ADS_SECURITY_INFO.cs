using System;
using System.Diagnostics.CodeAnalysis;

namespace ToolKit.DirectoryServices.ServiceInterfaces
{
    /// <summary>
    /// Specifies the options for examining security data.
    /// </summary>
    [SuppressMessage(
        "Microsoft.Naming",
        "CA1707:IdentifiersShouldNotContainUnderscores",
        Justification = "This enumeration represents Win32API which include underscores.")]
    [Flags]
    public enum ADS_SECURITY_INFO
    {
        /// <summary>
        /// Reads or sets the owner data.
        /// </summary>
        OWNER = 0x1,

        /// <summary>
        /// Reads or sets the group data.
        /// </summary>
        GROUP = 0x2,

        /// <summary>
        /// Reads or sets the discretionary access-control list data.
        /// </summary>
        DACL = 0x4,

        /// <summary>
        /// Reads or sets the system access-control list data.
        /// </summary>
        SACL = 0x8
    }
}
