using System;

//// ReSharper disable InconsistentNaming

namespace ToolKit.DirectoryServices.ServiceInterfaces
{
    /// <summary>
    /// Specifies flags used for manipulating user properties.
    /// </summary>
    [Flags]
    public enum ADS_USER_FLAG
    {
        /// <summary>
        /// The logon script is executed. This flag does not work for the ADSI LDAP provider on
        /// either read or write operations. For the ADSI WinNT provider, this flag is read-only
        /// data, and it cannot be set for user objects.
        /// </summary>
        SCRIPT = 0x1,

        /// <summary>
        /// The user account is disabled.
        /// </summary>
        ACCOUNTDISABLE = 0x2,

        /// <summary>
        /// The home directory is required.
        /// </summary>
        HOMEDIR_REQUIRED = 0x8,

        /// <summary>
        /// The account is currently locked out.
        /// </summary>
        LOCKOUT = 0x10,

        /// <summary>
        /// No password is required.
        /// </summary>
        PASSWD_NOTREQD = 0x20,

        /// <summary>
        /// The user cannot change the password. This flag can be read, but not set directly. For
        /// more information and a code example that shows how to prevent a user from changing the
        /// password, see User Cannot Change Password.
        /// </summary>
        PASSWD_CANT_CHANGE = 0x40,

        /// <summary>
        /// The user can send an encrypted password.
        /// </summary>
        ENCRYPTED_TEXT_PASSWORD_ALLOWED = 0x80,

        /// <summary>
        /// This is an account for users whose primary account is in another domain. This account
        /// provides user access to this domain, but not to any domain that trusts this domain. Also
        /// known as a local user account.
        /// </summary>
        TEMP_DUPLICATE_ACCOUNT = 0x100,

        /// <summary>
        /// This is a default account type that represents a typical user.
        /// </summary>
        NORMAL_ACCOUNT = 0x200,

        /// <summary>
        /// This is a permit to trust account for a system domain that trusts other domains.
        /// </summary>
        INTERDOMAIN_TRUST_ACCOUNT = 0x800,

        /// <summary>
        /// This is a computer account for a Microsoft Windows NT Workstation/Windows 2000
        /// Professional or Windows NT Server/Windows 2000 Server that is a member of this domain.
        /// </summary>
        WORKSTATION_TRUST_ACCOUNT = 0x1000,

        /// <summary>
        /// This is a computer account for a system backup domain controller that is a member of
        /// this domain.
        /// </summary>
        SERVER_TRUST_ACCOUNT = 0x2000,

        /// <summary>
        /// When set, the password will not expire on this account.
        /// </summary>
        DONT_EXPIRE_PASSWD = 0x10000,

        /// <summary>
        /// This is an Majority Node Set (MNS) logon account. With MNS, you can configure a
        /// multi-node Windows cluster without using a common shared disk.
        /// </summary>
        MNS_LOGON_ACCOUNT = 0x20000,

        /// <summary>
        /// When set, this flag will force the user to log on using a smart card.
        /// </summary>
        SMARTCARD_REQUIRED = 0x40000,

        /// <summary>
        /// When set, the service account (user or computer account), under which a service runs, is
        /// trusted for Kerberos delegation. Any such service can impersonate a client requesting
        /// the service. To enable a service for Kerberos delegation, set this flag on the
        /// userAccountControl property of the service account.
        /// </summary>
        TRUSTED_FOR_DELEGATION = 0x80000,

        /// <summary>
        /// When set, the security context of the user will not be delegated to a service even if
        /// the service account is set as trusted for Kerberos delegation.
        /// </summary>
        NOT_DELEGATED = 0x100000,

        /// <summary>
        /// Restrict this principal to use only Data Encryption Standard (DES) encryption types for
        /// keys. <blockquote>Active Directory Client Extension: Not supported.</blockquote>
        /// </summary>
        USE_DES_KEY_ONLY = 0x200000,

        /// <summary>
        /// This account does not require Kerberos preauthentication for logon. <blockquote>Active
        /// Directory Client Extension: Not supported.</blockquote>
        /// </summary>
        DONT_REQUIRE_PREAUTH = 0x400000,

        /// <summary>
        /// The user password has expired. This flag is created by the system using data from the
        /// password last set attribute and the domain policy. It is read-only and cannot be set. To
        /// manually set a user password as expired, use the NetUserSetInfo function with the
        /// USER_INFO_3 (usri3_password_expired member) or USER_INFO_4 (usri4_password_expired
        /// member) structure. <blockquote>Active Directory Client Extension: Not supported.</blockquote>
        /// </summary>
        PASSWORD_EXPIRED = 0x800000,

        /// <summary>
        /// The account is enabled for delegation. This is a security-sensitive setting; accounts
        /// with this option enabled should be strictly controlled. This setting enables a service
        /// running under the account to assume a client identity and authenticate as that user to
        /// other remote servers on the network. <blockquote>Active Directory Client Extension: Not supported.</blockquote>
        /// </summary>
        TRUSTED_TO_AUTHENTICATE_FOR_DELEGATION = 0x1000000
    }
}
