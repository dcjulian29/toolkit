using System;

//// ReSharper disable InconsistentNaming

namespace ToolKit.DirectoryServices.ServiceInterfaces
{
    /// <summary>
    /// Specifies the security level used in authenticating a client.
    /// </summary>
    [Flags]
    public enum ADS_AUTHENTICATION : uint
    {
        /// <summary>
        /// Requests secure authentication. When this flag is set, the WinNT provider uses NT LAN
        /// Manager (NTLM) to authenticate the client. Active Directory will use Kerberos, and
        /// possibly NTLM, to authenticate the client. When the user name and password are NULL,
        /// ADSI binds to the object using the security context of the calling thread, which is
        /// either the security context of the user account under which the application is running
        /// or of the client user account that the calling thread represents.
        /// </summary>
        SECURE_AUTHENTICATION = 0x1,

        /// <summary>
        /// Requires ADSI to use encryption for data exchange over the network.
        /// Note: This option is not supported by the WinNT provider.
        /// </summary>
        USE_ENCRYPTION = 0x2,

        /// <summary>
        /// The channel is encrypted using Secure Sockets Layer (SSL). Active Directory requires
        /// that the Certificate Server be installed to support SSL. If this flag is not combined
        /// with the ADS_SECURE_AUTHENTICATION flag and the supplied credentials are NULL, the bind
        /// will be performed anonymously. If this flag is combined with the
        /// ADS_SECURE_AUTHENTICATION flag and the supplied credentials are NULL, then the
        /// credentials of the calling thread are used.
        /// Note: This option is not supported by the WinNT provider.
        /// </summary>
        USE_SSL = 0x2,

        /// <summary>
        /// A write-able domain controller is not required. For a Windows NT 4.0 network, ADSI will
        /// attempt to connect to either a primary domain controller (PDC) or a backup domain
        /// controller (BDC) because BDCs are read-only. On a Windows Server 2003 or Windows 2000
        /// network, all servers are writable, so this flag has no affect.
        /// </summary>
        READONLY_SERVER = 0x4,

        /// <summary>
        /// This flag is deprecated.
        /// </summary>
        PROMPT_CREDENTIALS = 0x8,

        /// <summary>
        /// Request no authentication. The providers may attempt to bind the client, as an anonymous
        /// user, to the target object. The WinNT provider does not support this flag. Active
        /// Directory establishes a connection between the client and the targeted object, but will
        /// not perform authentication. Setting this flag amounts to requesting an anonymous
        /// binding, which indicates all users as the security context.
        /// </summary>
        NO_AUTHENTICATION = 0x10,

        /// <summary>
        /// When this flag is set, ADSI will not attempt to query the objectClass property and thus
        /// will only expose the base interfaces supported by all ADSI objects instead of the full
        /// object support. A user can use this option to increase the performance in a series of
        /// object manipulations that involve only methods of the base interfaces. However, ADSI
        /// will not verify that any of the requested objects actually exist on the server. For more
        /// information, see Fast Binding Options for Batch Write/Modify Operations. This option is
        /// also useful for binding to non-Active Directory directory services, for example Exchange
        /// 5.5, where the objectClass query would fail.
        /// </summary>
        FAST_BIND = 0x20,

        /// <summary>
        /// Verifies data integrity. The ADS_SECURE_AUTHENTICATION flag must also be set also to use signing.
        /// Note: This option is not supported by the WinNT provider.
        /// </summary>
        USE_SIGNING = 0x40,

        /// <summary>
        /// Encrypts data using Kerberos. The ADS_SECURE_AUTHENTICATION flag must also be set to use sealing.
        /// Note: This option is not supported by the WinNT provider.
        /// </summary>
        USE_SEALING = 0x80,

        /// <summary>
        /// Enables ADSI to delegate the user security context, which is necessary for moving
        /// objects across domains.
        /// </summary>
        USE_DELEGATION = 0x100,

        /// <summary>
        /// Windows 2000 SP1 and later: Specify this flag when using the LDAP provider if your
        /// ADsPath includes a server name. Do not use this flag for paths that include a domain
        /// name or for a serverless bind. If you specify a server name without also specifying this
        /// flag, unnecessary network traffic can result.
        /// </summary>
        SERVER_BIND = 0x200,

        /// <summary>
        /// Specify this flag to turn referral chasing off for the life of the connection. However,
        /// even when this flag is specified, ADSI still allows the setting of referral chasing
        /// behavior for container enumeration when set using ADS_OPTION_REFERRALS in
        /// ADS_OPTION_ENUM (as documented in container enumeration wtih referral chasing in
        /// IADsObjectOptions::SetOption) and searching separately (as documented in Referral
        /// Chasing with IDirectorySearch).
        /// Note: This option is not supported by the WinNT provider.
        /// </summary>
        NO_REFERRAL_CHASING = 0x400,

        /// <summary>
        /// The enumeration value is Reserved.
        /// </summary>
        AUTH_RESERVED = 0x80000000
    }
}
