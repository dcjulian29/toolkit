//// ReSharper disable InconsistentNaming

namespace ToolKit.DirectoryServices.ServiceInterfaces
{
    /// <summary>
    /// Used to identify the type of password encoding used with the ADS_OPTION_PASSWORD_METHOD
    /// option in the IADsObjectOptions::GetOption and IADsObjectOptions::SetOption methods.
    /// </summary>
    public enum ADS_PASSWORD_ENCODING
    {
        /// <summary>
        /// Passwords are encoded using SSL.
        /// </summary>
        REQUIRE_SSL = 0,

        /// <summary>
        /// Passwords are not encoded and are transmitted in plaintext.
        /// </summary>
        CLEAR = 1
    }
}
