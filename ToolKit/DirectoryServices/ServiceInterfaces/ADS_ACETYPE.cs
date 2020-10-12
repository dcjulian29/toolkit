using System.Diagnostics.CodeAnalysis;

namespace ToolKit.DirectoryServices.ServiceInterfaces
{
    /// <summary>
    /// Specifies the ACE type.
    /// </summary>
    [SuppressMessage(
        "Microsoft.Naming",
        "CA1707:IdentifiersShouldNotContainUnderscores",
        Justification = "This enumeration represents Win32API which include underscores.")]
    public enum ADS_ACETYPE
    {
        /// <summary>
        /// The ACE is of the standard ACCESS ALLOWED type, where the ObjectType and
        /// InheritedObjectType fields are NULL.
        /// </summary>
        ACCESS_ALLOWED = 0,

        /// <summary>
        /// The ACE is of the standard system-audit type, where the ObjectType and
        /// InheritedObjectType fields are NULL.
        /// </summary>
        ACCESS_DENIED = 0x1,

        /// <summary>
        /// The ACE is of the standard system type, where the ObjectType and InheritedObjectType
        /// fields are NULL.
        /// </summary>
        SYSTEM_AUDIT = 0x2,

        /// <summary>
        /// The ACE grants access to an object or a sub-object of the object, such as a property set
        /// or property. ObjectType or InheritedObjectType or both contain a GUID that identifies a
        /// property set, property, extended right, or type of child object. Windows NT 4.0: Not used.
        /// </summary>
        ACCESS_ALLOWED_OBJECT = 0x5,

        /// <summary>
        /// The ACE denies access to an object or a sub-object of the object, such as a property set
        /// or property. ObjectType or InheritedObjectType or both contain a GUID that identifies a
        /// property set, property, extended right, or type of child object. Windows NT 4.0: Not used.
        /// </summary>
        ACCESS_DENIED_OBJECT = 0x6,

        /// <summary>
        /// The ACE audits access to an object or a sub-object of the object, such as a property set
        /// or property. ObjectType or InheritedObjectType or both contain a GUID that identifies a
        /// property set, property, extended right, or type of child object. Windows NT 4.0: Not used.
        /// </summary>
        SYSTEM_AUDIT_OBJECT = 0x7,

        /// <summary>
        /// This enumeration value is not used in ADSI.
        /// </summary>
        SYSTEM_ALARM_OBJECT = 0x8,

        /// <summary>
        /// Same functionality as ADS_ACETYPE_ACCESS_ALLOWED, but used with applications that use
        /// AUTHZ to verify ACEs. Windows NT 4.0: Not used.
        /// </summary>
        ACCESS_ALLOWED_CALLBACK = 0x9,

        /// <summary>
        /// Same functionality as ADS_ACETYPE_ACCESS_DENIED, but used with applications that use
        /// AUTHZ to verify ACEs. Windows NT 4.0: Not used.
        /// </summary>
        ACCESS_DENIED_CALLBACK = 0xA,

        /// <summary>
        /// Same functionality as ADS_ACETYPE_ACCESS_ALLOWED_OBJECT, but used with applications that
        /// use AUTHZ to verify ACEs. Windows NT 4.0: Not used.
        /// </summary>
        ACCESS_ALLOWED_CALLBACK_OBJECT = 0xB,

        /// <summary>
        /// Same functionality as ADS_ACETYPE_ACCESS_DENIED_OBJECT, but used with applications that
        /// use AUTHZ to check ACEs. Windows NT 4.0: Not used.
        /// </summary>
        ACCESS_DENIED_CALLBACK_OBJECT = 0xC,

        /// <summary>
        /// Same functionality as ADS_ACETYPE_SYSTEM_AUDIT, but used with applications that use
        /// AUTHZ to check ACEs. Windows NT 4.0: Not used.
        /// </summary>
        SYSTEM_AUDIT_CALLBACK = 0xD,

        /// <summary>
        /// This enumeration value is not used in ADSI.
        /// </summary>
        SYSTEM_ALARM_CALLBACK = 0xE,

        /// <summary>
        /// Same functionality as ADS_ACETYPE_SYSTEM_AUDIT_OBJECT, but used with applications that
        /// use AUTHZ to verify ACEs. Windows NT 4.0: Not used.
        /// </summary>
        SYSTEM_AUDIT_CALLBACK_OBJECT = 0xF,

        /// <summary>
        /// This enumeration value is not used in ADSI.
        /// </summary>
        SYSTEM_ALARM_CALLBACK_OBJECT = 0x10
    }
}
