using System;
using System.Diagnostics.CodeAnalysis;

namespace ToolKit.DirectoryServices.ServiceInterfaces
{
    /// <summary>
    /// Specifies the presence of the ObjectType or InheritedObjectType fields in an ACE.
    /// </summary>
    [SuppressMessage(
        "Microsoft.Naming",
        "CA1707:IdentifiersShouldNotContainUnderscores",
        Justification = "This enumeration represents Win32API which include underscores.")]
    [Flags]
    public enum ADS_FLAGTYPE
    {
        /// <summary>
        /// The ObjectType field is present in the ACE.
        /// </summary>
        OBJECT_TYPE_PRESENT = 0x1,

        /// <summary>
        /// The InheritedObjectType field is present in the ACE.
        /// </summary>
        INHERITED_OBJECT_TYPE_PRESENT = 0x2
    }
}
