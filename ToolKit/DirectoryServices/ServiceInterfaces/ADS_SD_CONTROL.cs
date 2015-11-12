using System;

//// ReSharper disable InconsistentNaming

namespace ToolKit.DirectoryServices.ServiceInterfaces
{
    /// <summary>
    /// Specifies that an access-control list (ACL) is to be protected when new permissions are
    /// recursively applied to a directory tree.
    /// </summary>
    [Flags]
    public enum ADS_SD_CONTROL
    {
        /// <summary>
        /// A default mechanism provides the owner security identifier (SID) of the security
        /// descriptor rather than the original provider of the security descriptor.
        /// </summary>
        SE_OWNER_DEFAULTED = 0x0001,

        /// <summary>
        /// A default mechanism provides the group SID of the security descriptor rather than the
        /// original provider of the security descriptor.
        /// </summary>
        SE_GROUP_DEFAULTED = 0x0002,

        /// <summary>
        /// The discretionary access-control list (DACL) is present in the security descriptor. If
        /// this flag is not set, or if this flag is set and the DACL is NULL, the security
        /// descriptor allows full access to everyone.
        /// </summary>
        SE_DACL_PRESENT = 0x0004,

        /// <summary>
        /// The security descriptor uses a default DACL built from the creator's access token.
        /// </summary>
        SE_DACL_DEFAULTED = 0x0008,

        /// <summary>
        /// The system access-control list (SACL) is present in the security descriptor.
        /// </summary>
        SE_SACL_PRESENT = 0x0010,

        /// <summary>
        /// The security descriptor uses a default SACL built from the creator's access token.
        /// </summary>
        SE_SACL_DEFAULTED = 0x0020,

        /// <summary>
        /// THE DACL of the security descriptor must be inherited.
        /// </summary>
        SE_DACL_AUTO_INHERIT_REQ = 0x0100,

        /// <summary>
        /// The SACL of the security descriptor must be inherited.
        /// </summary>
        SE_SACL_AUTO_INHERIT_REQ = 0x0200,

        /// <summary>
        /// The DACL of the security descriptor supports automatic propagation of inheritable
        /// access-control entries (ACEs) to existing child objects.
        /// </summary>
        SE_DACL_AUTO_INHERITED = 0x0400,

        /// <summary>
        /// The SACL of the security descriptor supports automatic propagation of inheritable ACEs
        /// to existing child objects.
        /// </summary>
        SE_SACL_AUTO_INHERITED = 0x0800,

        /// <summary>
        /// The security descriptor will not allow inheritable ACEs to modify the DACL.
        /// </summary>
        SE_DACL_PROTECTED = 0x1000,

        /// <summary>
        /// The security descriptor will not allow inheritable ACEs to modify the SACL.
        /// </summary>
        SE_SACL_PROTECTED = 0x2000,

        /// <summary>
        /// The security descriptor is of self-relative format with all the security information in
        /// a continuous block of memory.
        /// </summary>
        SE_SELF_RELATIVE = 0x8000
    }
}
