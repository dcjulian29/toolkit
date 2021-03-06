using System;
using System.Diagnostics.CodeAnalysis;

namespace ToolKit.DirectoryServices.ServiceInterfaces
{
    /// <summary>
    /// Specifies the behavior of referral chasing.
    /// </summary>
    [SuppressMessage(
        "Microsoft.Naming",
        "CA1707:IdentifiersShouldNotContainUnderscores",
        Justification = "This enumeration represents Win32API which include underscores.")]
    [Flags]
    public enum ADS_CHASE_REFERRALS
    {
        /// <summary>
        /// The client should never chase the referred-to server. Setting this option prevents a
        /// client from contacting other servers in a referral process.
        /// </summary>
        NEVER = 0x00,

        /// <summary>
        /// The client chases only subordinate referrals which are a subordinate naming context in a
        /// directory tree. For example, if the base search is requested for "DC=Company,DC=Com",
        /// and the server returns a result set and a referral of "DC=Sales,DC=Company,DC=Com" on
        /// the Sales server, the client can contact the Sales server to continue the search. The
        /// ADSI LDAP provider always turns off this flag for paged searches.
        /// </summary>
        SUBORDINATE = 0x20,

        /// <summary>
        /// The client chases external referrals. For example, a client requests server A to perform
        /// a search for "DC=Company,DC=Com". However, server A does not contain the object, but
        /// knows that an independent server, B, owns it. It then refers the client to server B.
        /// </summary>
        EXTERNAL = 0x40,

        /// <summary>
        /// Referrals are chased for either the subordinate or external type.
        /// </summary>
        ALWAYS = 0x20 | 0x40
    }
}
