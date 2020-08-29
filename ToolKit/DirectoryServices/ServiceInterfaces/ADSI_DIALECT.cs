using System;
using System.Diagnostics.CodeAnalysis;

namespace ToolKit.DirectoryServices.ServiceInterfaces
{
    /// <summary>
    /// Specifies available ADSI query dialects.
    /// </summary>
    [SuppressMessage(
        "Microsoft.Naming",
        "CA1707:IdentifiersShouldNotContainUnderscores",
        Justification = "This enumeration represents Win32API which include underscores.")]
    [Flags]
    public enum ADSI_DIALECT
    {
        /// <summary>
        /// ADSI queries are based on the LDAP dialect.
        /// </summary>
        LDAP = 0,

        /// <summary>
        /// ADSI queries are based on the SQL dialect.
        /// </summary>
        SQL = 0x1
    }
}
