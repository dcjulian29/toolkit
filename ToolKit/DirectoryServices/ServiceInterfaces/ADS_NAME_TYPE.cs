//// ReSharper disable InconsistentNaming

namespace ToolKit.DirectoryServices.ServiceInterfaces
{
    /// <summary>
    /// Specifies the format used to represent distinguished names.
    /// </summary>
    public enum ADS_NAME_TYPE
    {
        /// <summary>
        /// Name format as specified in RFC 1779. For example, "CN=Jeff Smith,CN=users,DC=Fabrikam,DC=com".
        /// </summary>
        RFC1779 = 1,

        /// <summary>
        /// Canonical name format. For example, "Fabrikam.com/Users/Jeff Smith".
        /// </summary>
        CANONICAL = 2,

        /// <summary>
        /// Account name format used in Windows NT 4.0. For example, "Fabrikam\JeffSmith".
        /// </summary>
        NT4 = 3,

        /// <summary>
        /// Display name format. For example, "Jeff Smith".
        /// </summary>
        DISPLAY = 4,

        /// <summary>
        /// Simple domain name format. For example, "JeffSmith@Fabrikam.com".
        /// </summary>
        DOMAIN_SIMPLE = 5,

        /// <summary>
        /// Simple enterprise name format. For example, "JeffSmith@Fabrikam.com".
        /// </summary>
        ENTERPRISE_SIMPLE = 6,

        /// <summary>
        /// Global Unique Identifier format. For example, "{95ee9fff-3436-11d1-b2b0-d15ae3ac8436}".
        /// </summary>
        GUID = 7,

        /// <summary>
        /// Unknown name type. The system will estimate the format. This element is a meaningful
        /// option only with the IADsNameTranslate.Set or the IADsNameTranslate.SetEx method, but
        /// not with the IADsNameTranslate.Get or IADsNameTranslate.GetEx method.
        /// </summary>
        UNKNOWN = 8,

        /// <summary>
        /// User principal name format. For example, "JeffSmith@Fabrikam.com".
        /// </summary>
        USER_PRINCIPAL_NAME = 9,

        /// <summary>
        /// Extended canonical name format. For example, "Fabrikam.com/Users Jeff Smith".
        /// </summary>
        CANONICAL_EX = 10,

        /// <summary>
        /// Service principal name format. For example, "www/www.fabrikam.com@fabrikam.com".
        /// </summary>
        SERVICE_PRINCIPAL_NAME = 11,

        /// <summary>
        /// A SID string, as defined in the Security Descriptor Definition Language (SDDL), for
        /// either the SID of the current object or one from the object SID history. For example,
        /// "O:AOG:DAD:(A;;RPWPCCDCLCSWRCWDWOGA;;;S-1-0-0)" For more information, see Security
        /// Descriptor String Format.
        /// </summary>
        SID_OR_SID_HISTORY_NAME = 12
    }
}
