using System;

//// ReSharper disable InconsistentNaming

namespace ToolKit.DirectoryServices.ServiceInterfaces
{
    /// <summary>
    /// Specifies available ADSI query dialects.
    /// </summary>
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
