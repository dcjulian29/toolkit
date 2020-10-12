using System.Diagnostics.CodeAnalysis;

namespace ToolKit.DirectoryServices.ServiceInterfaces
{
    /// <summary>
    /// Specifies the revision number of an ACE or ACL.
    /// </summary>
    [SuppressMessage(
        "Microsoft.Naming",
        "CA1707:IdentifiersShouldNotContainUnderscores",
        Justification = "This enumeration represents Win32API which include underscores.")]
    public enum ADS_SD_REVISION
    {
        /// <summary>
        /// The revision number of the ACE, or the ACL, for Active Directory.
        /// </summary>
        DS = 4
    }
}
