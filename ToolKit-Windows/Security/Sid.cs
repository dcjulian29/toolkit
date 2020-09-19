using System.Security.Principal;

namespace ToolKit.Security
{
    /// <summary>
    /// This class provides functions related to Security Identifiers (SIDs).
    /// </summary>
    public static class Sid
    {
        /// <summary>
        /// Gets the SID for the built-in administrators group on the machine. This is not the same
        /// as the built-in Administrator *account*.
        /// </summary>
        /// <value>The administrators.</value>
        public static string Administrators
        {
            get
            {
                return "S-1-5-32-544";
            }
        }

        /// <summary>
        /// Gets the SID for any user logged on without an identity, for instance via an anonymous
        /// network session. Note that users logged in using the Built-in Guest account are neither
        /// authenticated nor anonymous. Available on XP and later.
        /// </summary>
        /// <value>The anonymous logged on user.</value>
        public static string AnonymousLoggedonUser
        {
            get
            {
                return "S-1-5-7";
            }
        }

        /// <summary>
        /// Gets the SID for any user recognized by the local machine or by a domain. Note that
        /// users logged in using the Built-in Guest account are not authenticated. However, members
        /// of the Guests group with individual accounts on the machine or domain are authenticated.
        /// </summary>
        /// <value>The authenticated users.</value>
        public static string AuthenticatedUsers
        {
            get
            {
                return "S-1-5-11";
            }
        }

        /// <summary>
        /// Gets the SID for group covering users logging in using the local or domain guest
        /// account. This is not the same as the built-in Guest *account*.
        /// </summary>
        /// <value>The built-in guests.</value>
        public static string BuiltinGuests
        {
            get
            {
                return "S-1-5-32-546";
            }
        }

        /// <summary>
        /// Gets the SID for group covering all local user accounts, and users on the domain.
        /// </summary>
        /// <value>The built-in user.</value>
        public static string BuiltinUser
        {
            get
            {
                return "S-1-5-32-545";
            }
        }

        /// <summary>
        /// Gets the SID for users who initially logged onto the machine "interactively", such as
        /// local logon and Remote Desktops logon.
        /// </summary>
        /// <value>The interactive users.</value>
        public static string InteractiveUsers
        {
            get
            {
                return "S-1-5-4";
            }
        }

        /// <summary>
        /// Gets the SID for a predefined account for services that presents user credentials for
        /// local resources and anonymous credentials for network access. Available on XP and later.
        /// </summary>
        /// <value>The local service.</value>
        public static string LocalService
        {
            get
            {
                return "S-1-5-19";
            }
        }

        /// <summary>
        /// Gets the SID for users accessing the machine remotely, without interactive desktop
        /// access (i.e., file sharing or RPC calls).
        /// </summary>
        /// <value>The network logon user.</value>
        public static string NetworkLogonUser
        {
            get
            {
                return "S-1-5-2";
            }
        }

        /// <summary>
        /// Gets the SID for a predefined account for services that presents user credentials for
        /// local resources and the machine ID for network access. Available on XP and later.
        /// </summary>
        /// <value>The network service.</value>
        public static string NetworkService
        {
            get
            {
                return "S-1-5-20";
            }
        }

        /// <summary>
        /// Gets the SID for the OS itself (including its user mode components.)
        /// </summary>
        /// <value>The system.</value>
        public static string System
        {
            get
            {
                return "S-1-5-18";
            }
        }

        /// <summary>
        /// Gets the SID for interactive Users who *initially* logged onto the machine specifically
        /// via Terminal Services or Remote Desktop.
        /// </summary>
        /// <value>The terminal server users.</value>
        public static string TerminalServerUsers
        {
            get
            {
                return "S-1-5-13";
            }
        }

        /// <summary>
        /// Gets the SID for prior to Windows XP, this SID covers every session: authenticated,
        /// anonymous, and the Built-in Guest account. For Windows XP and later, this SID does not
        /// cover anonymous logon sessions - only authenticated and the Built-in Guest account.
        /// </summary>
        /// <value>The world.</value>
        public static string World
        {
            get
            {
                return "S-1-1-0";
            }
        }

        /// <summary>
        /// Convert the string SID to binary.
        /// </summary>
        /// <param name="stringSid">The string SID.</param>
        /// <returns>byte array of the SID.</returns>
        public static byte[] ToBinary(string stringSid)
        {
            var sid = new SecurityIdentifier(stringSid);
            var sidBytes = new byte[sid.BinaryLength];
            sid.GetBinaryForm(sidBytes, 0);

            return sidBytes;
        }

        /// <summary>
        /// Convert the binary SID to a string in Hexadecimal format.
        /// </summary>
        /// <param name="binarySid">The binary SID.</param>
        /// <returns>string in Hexadecimal format.</returns>
        public static string ToHex(byte[] binarySid)
        {
            return HexEncoding.ToString(binarySid);
        }

        /// <summary>
        /// Convert the string formatted SID to a string in Hexadecimal format.
        /// </summary>
        /// <param name="stringSid">The string sid.</param>
        /// <returns>string in Hexadecimal format.</returns>
        public static string ToHex(string stringSid)
        {
            return ToHex(ToBinary(stringSid));
        }

        /// <summary>
        /// Translates the SID into its name in the DOMAIN\USER format.
        /// </summary>
        /// <param name="binarySid">The binary SID.</param>
        /// <returns>string containing the name of the SID.</returns>
        public static string ToName(byte[] binarySid)
        {
            return ToName(ToString(binarySid));
        }

        /// <summary>
        /// Translates the SID into its name in the DOMAIN\USER format.
        /// </summary>
        /// <param name="stringSid">The string SID.</param>
        /// <returns>string containing the name of the SID.</returns>
        public static string ToName(string stringSid)
        {
            var sid = new SecurityIdentifier(stringSid);
            var nac = (NTAccount)sid.Translate(typeof(NTAccount));

            return nac.ToString();
        }

        /// <summary>
        /// Convert the binary SID to a string format.
        /// </summary>
        /// <param name="binarySid">The binary sid.</param>
        /// <returns>string containing a string representation of the SID.</returns>
        public static string ToString(byte[] binarySid)
        {
            var sid = new SecurityIdentifier(binarySid, 0);

            return sid.ToString();
        }
    }
}
