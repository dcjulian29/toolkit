namespace ToolKit.Cryptography
{
    /// <summary>
    /// Abstraction of Data Protection API Protection Scopes.
    /// </summary>
    public enum DataProtectionKeyType
    {
        /// <summary>
        /// The protected data is associated with the current user. Only threads running under the
        /// current user context can unprotect the data.
        /// </summary>
        UserKey = 1,

        /// <summary>
        /// The protected data is associated with the machine context. Any process running on the
        /// computer can unprotect data. This enumeration value is usually used in server-specific
        /// applications that run on a server where untrusted users are not allowed access.
        /// </summary>
        MachineKey
    }
}
