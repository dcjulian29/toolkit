//// ReSharper disable InconsistentNaming

namespace ToolKit.DirectoryServices.ServiceInterfaces
{
    /// <summary>
    /// Specifies the type of values in a pathname object.
    /// </summary>
    public enum ADS_FORMAT
    {
        /// <summary>
        /// Returns the full path in Windows format, for example, "LDAP://servername/o=internet/…/cn=bar".
        /// </summary>
        WINDOWS = 1,

        /// <summary>
        /// Returns Windows format without server, for example, "LDAP://o=internet/…/cn=bar".
        /// </summary>
        WINDOWS_NO_SERVER = 2,

        /// <summary>
        /// Returns Windows format of the distinguished name only, for example, "o=internet/…/cn=bar".
        /// </summary>
        WINDOWS_DN = 3,

        /// <summary>
        /// Returns Windows format of Parent only, for example, "o=internet/…".
        /// </summary>
        WINDOWS_PARENT = 4,

        /// <summary>
        /// Returns the full path in X.500 format, for example, "LDAP://servername/cn=bar,…,o=internet".
        /// </summary>
        X500 = 5,

        /// <summary>
        /// Returns the path without server in X.500 format, for example, "LDAP://cn=bar,…,o=internet".
        /// </summary>
        X500_NO_SERVER = 6,

        /// <summary>
        /// Returns only the distinguished name in X.500 format. For example, "cn=bar,…,o=internet".
        /// </summary>
        X500_DN = 7,

        /// <summary>
        /// Returns only the parent in X.500 format, for example, "…,o=internet".
        /// </summary>
        X500_PARENT = 8,

        /// <summary>
        /// Returns the server name, for example, "servername".
        /// </summary>
        SERVER = 9,

        /// <summary>
        /// Returns the name of the provider, for example, "LDAP".
        /// </summary>
        PROVIDER = 10,

        /// <summary>
        /// Returns the name of the leaf, for example, "cn=bar".
        /// </summary>
        LEAF = 11
    }
}
