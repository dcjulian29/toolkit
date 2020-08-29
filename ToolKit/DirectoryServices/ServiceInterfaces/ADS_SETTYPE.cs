using System.Diagnostics.CodeAnalysis;

namespace ToolKit.DirectoryServices.ServiceInterfaces
{
    /// <summary>
    /// Specifies the path format in IADsPathname::Set.
    /// </summary>
    [SuppressMessage(
        "Microsoft.Naming",
        "CA1707:IdentifiersShouldNotContainUnderscores",
        Justification = "This enumeration represents Win32API which include underscores.")]
    [SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1650:ElementDocumentationMustBeSpelledCorrectly",
        Justification = "This class contains references to Win32 API.")]
    public enum ADS_SETTYPE
    {
        /// <summary>
        /// Sets the full path, for example, "LDAP://servername/o=internet/…/cn=bar".
        /// </summary>
        FULL = 1,

        /// <summary>
        /// Updates the provider only, for example, "LDAP".
        /// </summary>
        PROVIDER = 2,

        /// <summary>
        /// Updates the server name only, for example, "servername".
        /// </summary>
        SERVER = 3,

        /// <summary>
        /// Updates the distinguished name only, for example, "o=internet/…/cn=bar".
        /// </summary>
        DN = 4
    }
}
