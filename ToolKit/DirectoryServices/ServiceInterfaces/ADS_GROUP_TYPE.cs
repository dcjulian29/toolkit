using System;

//// ReSharper disable InconsistentNaming

namespace ToolKit.DirectoryServices.ServiceInterfaces
{
    /// <summary>
    /// Specifies the group type of the member.
    /// </summary>
    [Flags]
    public enum ADS_GROUP_TYPE : uint
    {
        /// <summary>
        /// Specifies a group that can contain accounts from the same domain and other global groups
        /// from the same domain. This type of group can be exported to a different domain.
        /// </summary>
        GLOBAL_GROUP = 0x00000002,

        /// <summary>
        /// Specifies a group that can contain accounts from any domain, other domain local groups
        /// from the same domain, global groups from any domain, and universal groups. This type of
        /// group should not be included in access-control lists of resources in other domains. This
        /// type of group is intended for use with the LDAP provider.
        /// </summary>
        DOMAIN_LOCAL_GROUP = 0x00000004,

        /// <summary>
        /// Specifies a group that is identical to the ADS_GROUP_TYPE_DOMAIN_LOCAL_GROUP group, but
        /// is intended for use with the WinNT provider.
        /// </summary>
        LOCAL_GROUP = 0x00000004,

        /// <summary>
        /// Specifies a group that can contain accounts from any domain, global groups from any
        /// domain, and other universal groups. This type of group cannot contain domain local groups.
        /// </summary>
        UNIVERSAL_GROUP = 0x00000008,

        /// <summary>
        /// Specifies a group that is security enabled. This group can be used to apply an
        /// access-control list on an ADSI object or a file system.
        /// </summary>
        SECURITY_ENABLED = 0x80000000
    }
}
