using System;
using System.Diagnostics.CodeAnalysis;

namespace ToolKit.DirectoryServices.ServiceInterfaces
{
    /// <summary>
    /// Specifies the types of attributes represented by an attributeSchema object.
    /// </summary>
    [SuppressMessage(
        "Microsoft.Naming",
        "CA1707:IdentifiersShouldNotContainUnderscores",
        Justification = "This enumeration represents Win32API which include underscores.")]
    [Flags]
    public enum ADS_SYSTEM_FLAG : uint
    {
        /// <summary>
        /// Identifies an object that cannot be deleted.
        /// </summary>
        DISALLOW_DELETE = 0x80000000,

        /// <summary>
        /// For objects in the configuration partition, if this flag is set, the object can be
        /// renamed; otherwise, the object cannot be renamed. By default, this flag is not set on
        /// new objects created under the configuration partition, and you can set this flag only
        /// during object creation.
        /// </summary>
        CONFIG_ALLOW_RENAME = 0x40000000,

        /// <summary>
        /// For objects in the configuration partition, if this flag is set, the object can be
        /// moved; otherwise, the object cannot be moved. By default, this flag is not set on new
        /// objects created under the configuration partition, and you can set this flag only during
        /// object creation.
        /// </summary>
        CONFIG_ALLOW_MOVE = 0x20000000,

        /// <summary>
        /// For objects in the configuration partition, if this flag is set, the object can be moved
        /// with restrictions; otherwise, the object cannot be moved. By default, this flag is not
        /// set on new objects created under the configuration partition, and you can set this flag
        /// only during object creation.
        /// </summary>
        CONFIG_ALLOW_LIMITED_MOVE = 0x10000000,

        /// <summary>
        /// Identifies a domain object that cannot be renamed.
        /// </summary>
        DOMAIN_DISALLOW_RENAME = 0x08000000,

        /// <summary>
        /// Identifies a domain object that cannot be moved.
        /// </summary>
        DOMAIN_DISALLOW_MOVE = 0x04000000,

        /// <summary>
        /// Naming context is in NTDS.
        /// </summary>
        CR_NTDS_NC = 0x00000001,

        /// <summary>
        /// Naming context is a domain.
        /// </summary>
        CR_NTDS_DOMAIN = 0x00000002,

        /// <summary>
        /// If this flag is set in the systemFlags attribute of an attributeSchema object, the
        /// attribute is not to be replicated.
        /// </summary>
        ATTR_NOT_REPLICATED = 0x00000001,

        /// <summary>
        /// If this flag is set in the systemFlags attribute of an attributeSchema object, the
        /// attribute is a constructed property.
        /// </summary>
        ATTR_IS_CONSTRUCTED = 0x00000004
    }
}
