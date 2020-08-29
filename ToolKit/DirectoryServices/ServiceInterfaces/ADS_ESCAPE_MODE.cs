using System.Diagnostics.CodeAnalysis;

namespace ToolKit.DirectoryServices.ServiceInterfaces
{
    /// <summary>
    /// Specifies whether special characters are escaped, unescaped, or untouched.
    /// </summary>
    [SuppressMessage(
        "Microsoft.Naming",
        "CA1707:IdentifiersShouldNotContainUnderscores",
        Justification = "This enumeration represents Win32API which include underscores.")]
    [SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1650:ElementDocumentationMustBeSpelledCorrectly",
        Justification = "This class contains acronyms or abbreviations.")]
    public enum ADS_ESCAPE_MODE
    {
        /// <summary>
        /// The default escape mode provides a convenient option to specify the escape mode. It has
        /// the effect of minimal escape operation appropriate for a chosen format. Thus, the
        /// default behavior depends on the value that ADS_FORMAT_ENUM uses to retrieve the
        /// directory paths. <table><tr><th>Retrieved path format</th><th>Default escaped mode</th></tr><tr><td>ADS_FORMAT_X500</td><td>ADS_ESCAPEDMODE_ON</td></tr><tr><td>ADS_FORMAT_X500_NO_SERVER</td><td>ADS_ESCAPEDMODE_ON</td></tr><tr><td>ADS_FORMAT_WINDOWS</td><td>ADS_ESCAPEDMODE_ON</td></tr><tr><td>ADS_FORMAT_WINDOWS_NO_SERVER</td><td>ADS_ESCAPEDMODE_ON</td></tr><tr><td>ADS_FORMAT_X500_DN</td><td>ADS_ESCAPEDMODE_OFF</td></tr><tr><td>ADS_FORMAT_X500_PARENT</td><td>ADS_ESCAPEDMODE_OFF</td></tr><tr><td>ADS_FORMAT_WINDOWS_DN</td><td>ADS_ESCAPEDMODE_OFF</td></tr><tr><td>ADS_FORMAT_WINDOWS_PARENT</td><td>ADS_ESCAPEDMODE_OFF</td></tr><tr><td>ADS_FORMAT_LEAF</td><td>ADS_ESCAPEDMODE_ON</td></tr></table>
        /// </summary>
        DEFAULT = 1,

        /// <summary>
        /// All special characters display in the escape format; for example,
        /// "CN=date\=yy\/mm\/dd\,weekday" displays as is.
        /// </summary>
        ON = 2,

        /// <summary>
        /// ADSI special characters display in the unescaped format; for example,
        /// "CN=date\=yy\/mm\/dd\,weekday" displays as "CN=date\=yy/mm/dd\,weekday".
        /// </summary>
        OFF = 3,

        /// <summary>
        /// ADSI and LDAP special characters display in the unescaped format; for example,
        /// "CN=date\=yy\/mm\/dd\,weekday" displays as "CN=date=yy/mm/dd,weekday".
        /// </summary>
        OFF_EX = 4
    }
}
