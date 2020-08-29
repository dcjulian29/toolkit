using System.Diagnostics.CodeAnalysis;

namespace ToolKit.DirectoryServices.ServiceInterfaces
{
    /// <summary>
    /// Specifies data types used to interpret an ADSI extended syntax string.
    /// </summary>
    [SuppressMessage(
        "Microsoft.Naming",
        "CA1707:IdentifiersShouldNotContainUnderscores",
        Justification = "This enumeration represents Win32API which include underscores.")]
    public enum ADS_TYPE
    {
        /// <summary>
        /// The data type is invalid.
        /// </summary>
        INVALID = 0,

        /// <summary>
        /// The string is of Distinguished Name (path) of a directory service object.
        /// </summary>
        DN_STRING = 1,

        /// <summary>
        /// The string is of the case-sensitive type.
        /// </summary>
        CASE_EXACT_STRING = 2,

        /// <summary>
        /// The string is of the case-insensitive type.
        /// </summary>
        CASE_IGNORE_STRING = 3,

        /// <summary>
        /// The string is displayable on screen or in print.
        /// </summary>
        PRINTABLE_STRING = 4,

        /// <summary>
        /// The string is of a numeral to be interpreted as text.
        /// </summary>
        NUMERIC_STRING = 5,

        /// <summary>
        /// The data is of a Boolean value.
        /// </summary>
        BOOLEAN = 6,

        /// <summary>
        /// The data is of an integer value.
        /// </summary>
        INTEGER = 7,

        /// <summary>
        /// The string is of a byte array.
        /// </summary>
        OCTET_STRING = 8,

        /// <summary>
        /// The data is of the universal time as expressed in Universal Time Coordinate (UTC).
        /// </summary>
        UTC_TIME = 9,

        /// <summary>
        /// The data is of a long integer value.
        /// </summary>
        LARGE_INTEGER = 10,

        /// <summary>
        /// The string is of a provider-specific string.
        /// </summary>
        PROV_SPECIFIC = 11,

        /// <summary>
        /// This enumeration value is not used in ADSI.
        /// </summary>
        OBJECT_CLASS = 12,

        /// <summary>
        /// The data is of a list of case insensitive strings.
        /// </summary>
        CASEIGNORE_LIST = 13,

        /// <summary>
        /// The data is of a list of octet strings.
        /// </summary>
        OCTET_LIST = 14,

        /// <summary>
        /// The string is of a directory path.
        /// </summary>
        PATH = 15,

        /// <summary>
        /// The string is of the postal address type.
        /// </summary>
        POSTALADDRESS = 16,

        /// <summary>
        /// The data is of a time stamp in seconds.
        /// </summary>
        TIMESTAMP = 17,

        /// <summary>
        /// The string is of a back link.
        /// </summary>
        BACKLINK = 18,

        /// <summary>
        /// The string is of a typed name.
        /// </summary>
        TYPEDNAME = 19,

        /// <summary>
        /// The data is of the Hold data structure.
        /// </summary>
        HOLD = 20,

        /// <summary>
        /// The string is of a net address.
        /// </summary>
        NETADDRESS = 21,

        /// <summary>
        /// The data is of a replica pointer.
        /// </summary>
        REPLICAPOINTER = 22,

        /// <summary>
        /// The string is of a fax number.
        /// </summary>
        FAXNUMBER = 23,

        /// <summary>
        /// The data is of an e-mail message.
        /// </summary>
        EMAIL = 24,

        /// <summary>
        /// The data is of Windows NT/Windows 2000 security descriptor as represented by a byte array.
        /// </summary>
        NT_SECURITY_DESCRIPTOR = 25,

        /// <summary>
        /// The data is of an undefined type.
        /// </summary>
        UNKNOWN = 26,

        /// <summary>
        /// The data is of ADS_DN_WITH_BINARY used for mapping a distinguished name to a non varying GUID.
        /// </summary>
        DN_WITH_BINARY = 27,

        /// <summary>
        /// The data is of ADS_DN_WITH_STRING used for mapping a distinguished name to a non-varying
        /// string value.
        /// </summary>
        DN_WITH_STRING = 28
    }
}
