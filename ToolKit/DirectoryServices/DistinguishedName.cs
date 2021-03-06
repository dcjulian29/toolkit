using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;
using Common.Logging;
using ToolKit.Data;
using ToolKit.Validation;

namespace ToolKit.DirectoryServices
{
    /// <summary>
    /// Distinguished names (DNs) are used to uniquely identify entries in an LDAP or X.500
    /// directory. DNs are user-oriented strings, typically used whenever you must add, modify or
    /// delete an entry in a directory using the LDAP programming interface. This class represents a
    /// Distinguished Name (RFC 2253) and provides access to the various parts of the Distinguished
    /// Name. For Active Directory Distinguished Names, it also provides resolution of the NetBIOS
    /// domain name.
    /// </summary>
    public class DistinguishedName
    {
        private static readonly ILog _log = LogManager.GetLogger<DistinguishedName>();

        private readonly List<NameValue> _components = new List<NameValue>();

        /// <summary>
        /// Initializes a new instance of the <see cref="DistinguishedName" /> class.
        /// </summary>
        /// <param name="distinguishedName">The distinguished name.</param>
        public DistinguishedName(string distinguishedName)
        {
            LdapServer = string.Empty;

            if (!string.IsNullOrWhiteSpace(distinguishedName))
            {
                Process(distinguishedName);
            }
        }

        /// <summary>
        /// Gets the name of the object in canonical format.
        /// </summary>
        /// <value>The name of the object in canonical format.</value>
        /// <remarks>
        /// Active Directory Canonical Name . By default, the Windows user interfaces display object
        /// names that use the canonical name, which lists the relative distinguished names from the
        /// root downward and without the RFC 1779 naming attribute descriptors; it uses the DNS
        /// domain name (the form of the name where the domain labels are separated by periods). If
        /// the name of an organizational unit contains a forward slash character (/), the system
        /// requires an escape character in the form of a backslash (\) to distinguish between
        /// forward slashes that separate elements of the canonical name and the forward slash that
        /// is part of the organizational unit name. The canonical name that appears in Active
        /// Directory Users and Computers properties pages displays the escape character immediately
        /// preceding the forward slash in the name of the organizational unit. For example, if the
        /// name of an organizational unit is Promotions/Northeast and the name of the domain is
        /// example.com, the canonical name is displayed as example.com/Promotions\/Northeast.
        /// </remarks>
        public string CanonicalName
        {
            get
            {
                if (string.IsNullOrEmpty(DnsDomain))
                {
                    return null;
                }

                var builder = new StringBuilder();
                builder.Append(DnsDomain);

                for (var i = _components.Count - 1; i >= 0; i--)
                {
                    if (_components[i].Name.ToUpper(CultureInfo.InvariantCulture) != "DC")
                    {
                        builder.Append('/');
                        builder.Append(
                            _components[i].Value
                                .Replace("\\", string.Empty)
                                .Replace("/", "\\/"));
                    }
                }

                return builder.ToString();
            }
        }

        /// <summary>
        /// Gets the common name of the Distinguished Name, if present.
        /// </summary>
        /// <value>The common name of the Distinguished Name.</value>
        public string CommonName
        {
            get
            {
                return (from rdn in _components
                        where rdn.Name.ToUpper(CultureInfo.InvariantCulture) == "CN"
                        select rdn.Value.Replace("\\", string.Empty)).FirstOrDefault();
            }
        }

        /// <summary>
        /// Gets the DNS domain equivalent based on the DC components of the DN.
        /// </summary>
        /// <value>A string representing the DNS domain.</value>
        [SuppressMessage(
            "Globalization",
            "CA1308:Normalize strings to uppercase",
            Justification = "DNS is typically lowercase")]
        public string DnsDomain
        {
            get
            {
                var sb = new StringBuilder();

                foreach (var rdn in _components.Where(rdn => rdn.Name.ToUpper(CultureInfo.InvariantCulture) == "DC"))
                {
                    sb.Append(rdn.Value).Append('.');
                }

                return sb.ToString().TrimEnd('.').ToLower(CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// Gets the domain root of the Distinguished Name.
        /// </summary>
        /// <value>The domain root of the Distinguished Name.</value>
        public string DomainRoot
        {
            get
            {
                var sb = new StringBuilder();

                foreach (var rdn in _components)
                {
                    if (rdn.Name.ToUpper(CultureInfo.InvariantCulture) == "DC")
                    {
                        sb.Append(rdn.Name).Append('=').Append(rdn.Value).Append(',');
                    }
                }

                return sb.ToString().TrimEnd(',');
            }
        }

        /// <summary>
        /// Gets the Global Catalog path in the
        /// form: LDAP://HostName[:PortNumber]/DistinguishedName.
        /// </summary>
        /// <value>The Global Catalog path of the DistinguishedName.</value>
        public string GcPath
        {
            get
            {
                if (_components.Count == 0)
                {
                    return "GC://";
                }
                else
                {
                    return string.Format(
                        CultureInfo.InvariantCulture,
                        "GC://{0}{1}{2}{3}{4}",
                        LdapServer.Length > 0 ? LdapServer : DnsDomain,
                        ServerPort > 0 ? ":" : string.Empty,
                        ServerPort > 0 ? Convert.ToString(ServerPort, CultureInfo.InvariantCulture) : string.Empty,
                        LdapServer.Length > 0 || DnsDomain.Length > 0 ? "/" : string.Empty,
                        ToString());
                }
            }
        }

        /// <summary>
        /// Gets the LDAP path in the form: LDAP://HostName[:PortNumber]/DistinguishedName.
        /// </summary>
        /// <value>The LDAP path of the DistinguishedName.</value>
        public string LdapPath
        {
            get
            {
                if (_components.Count == 0)
                {
                    return "LDAP://";
                }
                else
                {
                    return string.Format(
                        CultureInfo.InvariantCulture,
                        "LDAP://{0}{1}{2}{3}{4}",
                        LdapServer.Length > 0 ? LdapServer : DnsDomain,
                        ServerPort > 0 ? ":" : string.Empty,
                        ServerPort > 0 ? Convert.ToString(ServerPort, CultureInfo.InvariantCulture) : string.Empty,
                        LdapServer.Length > 0 || DnsDomain.Length > 0 ? "/" : string.Empty,
                        ToString());
                }
            }
        }

        /// <summary>
        /// Gets or sets the LDAP server. The "HostName" can be a computer name, an IP address, or a
        /// domain name. A server name can also be specified in the binding string. If an LDAP
        /// server is not specified, one is deduced by the presence of DC values in the
        /// distinguished name. Most LDAP providers follow a model that requires a server name to be specified.
        /// </summary>
        /// <value>The LDAP server.</value>
        public string LdapServer
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the Parent Distinguished Name of this Distinguished Name.
        /// </summary>
        /// <value>The Parent Distinguished Name.</value>
        public DistinguishedName Parent
        {
            get
            {
                if (_components.Count > 0)
                {
                    var sb = new StringBuilder();

                    // Skip first component, then return the rest.
                    for (var i = 1; i < _components.Count; i++)
                    {
                        sb.Append(_components[i].Name).Append('=').Append(_components[i].Value).Append(',');
                    }

                    return new DistinguishedName(sb.ToString().TrimEnd(','));
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Gets or sets the server port number. The "PortNumber" specifies the port to be used for
        /// the connection. If no port number is specified, the LDAP provider uses the default port
        /// number. The default port number is 389 if not using an SSL connection or 636 if using an
        /// SSL connection. Unless a port number is specified, the port number is not used.
        /// </summary>
        /// <value>The server port number. Returns 0 is the port is not specified.</value>
        public int ServerPort
        {
            get;
            set;
        }

        /// <summary>
        /// Checks to see whether two DN objects are not equal.
        /// </summary>
        /// <param name="dn1">The first DistinguishedName instance..</param>
        /// <param name="dn2">The second DistinguishedName instance.</param>
        /// <returns><c>true</c> if the two instance are equal; otherwise, <c>false</c>.</returns>
        /// <returns>true if the two objects are not equal; false otherwise.</returns>
        public static bool operator !=(DistinguishedName dn1, DistinguishedName dn2)
        {
            return !(dn1 == dn2);
        }

        /// <summary>
        /// Checks to see whether two DN objects are equal.
        /// </summary>
        /// <param name="dn1">The first DistinguishedName instance.</param>
        /// <param name="dn2">The second DistinguishedName instance.</param>
        /// <returns><c>true</c> if the two instance are equal; otherwise, <c>false</c>.</returns>
        /// <returns>true if the two objects are equal; false otherwise.</returns>
        [SuppressMessage(
            "Blocker Code Smell",
            "S3875:\"operator==\" should not be overloaded on reference types",
            Justification = "Whitespace in DN are ignored for equality, so override object reference equality.")]
        public static bool operator ==(DistinguishedName dn1, DistinguishedName dn2)
        {
            return dn1 is null ? dn2 is null : dn1.Equals(dn2);
        }

        /// <summary>
        /// Parses the specified distinguished name and returns a distinguished name instance.
        /// </summary>
        /// <param name="distinguishedName">The distinguished name.</param>
        /// <returns>A distinguished name instance.</returns>
        public static DistinguishedName Parse(string distinguishedName)
        {
            return new DistinguishedName(distinguishedName);
        }

        /// <summary>
        /// Returns a Child distinguished name based on the child's relative distinguished name.
        /// </summary>
        /// <param name="childDistinguishedName">The child's relative distinguished nameValue.</param>
        /// <returns>A Child distinguished name instance.</returns>
        public DistinguishedName Child(string childDistinguishedName)
        {
            return new DistinguishedName($"{childDistinguishedName},{ToString()}");
        }

        /// <summary>
        /// Returns a Distinguished Name that represents the container of the object. If the object
        /// is a container, then the entire Distinguished Name is returned...
        /// </summary>
        /// <returns>A DistinguishedName object.</returns>
        public DistinguishedName Container()
        {
            return new DistinguishedName(ToString().Replace($"CN={CommonName},", string.Empty));
        }

        /// <summary>
        /// Determines whether the child distinguished name is part of this distinguished name.
        /// </summary>
        /// <param name="childDistinguishedName">The child distinguished name instance.</param>
        /// <returns>
        /// <c>true</c> if the child distinguished name is part of this distinguished name;
        /// otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(DistinguishedName childDistinguishedName)
        {
            Check.NotNull(childDistinguishedName, nameof(childDistinguishedName));

            if (childDistinguishedName._components.Count >= _components.Count)
            {
                var startNode = childDistinguishedName._components.Count - _components.Count;
                for (var i = startNode; i < _components.Count; i++)
                {
                    var childName = childDistinguishedName._components[i].Name.ToUpperInvariant();
                    var childValue = childDistinguishedName._components[i].Value.ToUpperInvariant();

                    var thisName = _components[i - startNode].Name.ToUpperInvariant();
                    var thisValue = _components[i - startNode].Value.ToUpperInvariant();

                    if (!((childName == thisName) && (childValue == thisValue)))
                    {
                        return false;
                    }
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Determines whether the child distinguished name is part of this distinguished name.
        /// </summary>
        /// <param name="childDistinguishedName">The child distinguished name string.</param>
        /// <returns>
        /// <c>true</c> if the child distinguished name is part of this distinguished name;
        /// otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(string childDistinguishedName)
        {
            return Contains(new DistinguishedName(childDistinguishedName));
        }

        /// <summary>
        /// Determines whether the specified <see cref="object" /> is equal to the current instance.
        /// </summary>
        /// <param name="obj">The <see cref="object" /> to compare.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="object" /> is equal to the current instance;
        /// otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="NullReferenceException">
        /// The <paramref name="obj" /> parameter is null.
        /// </exception>
        public override bool Equals(object obj) =>
            obj is DistinguishedName distinguishedName && Contains(distinguishedName);

        /// <summary>
        /// Serves as a hash function for this instance.
        /// </summary>
        /// <returns>A hash code for the current instance.</returns>
        public override int GetHashCode() => ToString().ToUpperInvariant().GetHashCode();

        /// <summary>
        /// Returns a <see cref="string" /> that represents the current Distinguished Name instance.
        /// </summary>
        /// <returns>A <see cref="string" /> that represents the current Distinguished Name instance.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            foreach (var rdn in _components)
            {
                sb.Append(rdn.Name).Append('=').Append(rdn.Value).Append(',');
            }

            return sb.ToString().TrimEnd(',').Replace("+,", "+");
        }

        private static bool IsAlpha(char c)
        {
            return (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z');
        }

        private static bool IsHex(char c)
        {
            return IsNumber(c) || (c >= 'A' && c <= 'F') || (c >= 'a' && c <= 'f');
        }

        private static bool IsLdapDnSpecial(char c)
        {
            return c == ',' || c == '=' || c == '+' || c == '<' || c == '>' || c == '#' || c == ';' || c == '"' || c == '\\';
        }

        private static bool IsNumber(char c)
        {
            return c >= '0' && c <= '9';
        }

        private void Process(string distinguishedName)
        {
            // Any of the attributes defined in the directory schema may be used to make up a DN.
            // The order of the component attribute value pairs is important. The DN contains one
            // component for each level of the directory hierarchy from the root down to the level
            // where the entry resides.

            // LDAP DNs begin with the most specific attribute, and continue with progressively
            // broader attributes, often ending with a country attribute or domain components.

            // Each component of the DN is referred to as the Relative Distinguished Name (RDN). An
            // RDN consist of an attribute=value pair

            // Some characters have special meaning in a DN. For example, = (equals) separates an
            // attribute name and value, and , (comma) separates attribute=value pairs. The special
            // characters are , (comma), = (equals), + (plus), < (less than), > (greater than), #
            // (number sign), ; (semicolon), \ (backslash), and " (quotation mark, ASCII 34).

            // A special character can be escaped (by a backslash: '\' ASCII 92) in an attribute
            // value to remove the special meaning. or by replacing the character to be escaped by a
            // backslash and two hex digits, which form a single byte in the code of the character.

            // When the entire attribute value is surrounded by "" (quotation marks) (ASCII 34), all
            // characters are taken as is, except for the \ (backslash). The \ (backslash) can be
            // used to escape a backslash (ASCII 92) or quotation marks (ASCII 34), any of the
            // special characters previously mentioned, or hex pairs.

            // To escape a single backslash, use \\

            // The formal syntax for a Distinguished Name (DN) is based on RFC 2253. The Backus Naur
            // Form (BNF) syntax (http://en.wikipedia.org/wiki/Backus-Naur_form):

            // <name> ::= <name-component> ( <spaced-separator> ) | <name-component>
            // <spaced-separator> <name> <spaced-separator> ::= <optional-space> <separator>
            // <optional-space> <separator> ::= "," | ";" <optional-space> ::= *( " " )
            // <name-component> ::= <attribute> | <attribute> <optional-space> "+" <optional-space>
            // <name-component> <attribute> ::= <string> | <key> <optional-space> "="
            // <optional-space> <string> <key> ::= 1*( <keychar> ) | "OID." <oid> | "oid." <oid>
            // <keychar> ::= letters and numbers <oid> ::= <digitstring> | <digitstring> "." <oid>
            // <digitstring> ::= 1*<digit> <digit> ::= digits 0-9 <string> ::= *( <stringchar> |
            // <pair> ) | '"' *( <stringchar> | <special> | <pair> ) '"' | "#" <hex> <special> ::=
            // "," | "=" | "+" | "<" | ">" | "#" | ";" <pair> ::= "\" ( <special> | "\" | '"')
            // <stringchar> ::= any character except <special> or "\" or '"' <hex> ::= 2*<hexchar>
            // <hexchar> ::= 0-9, a-f, A-F

            // A semicolon (;) character can be used to separate RDNs in a distinguished name,
            // although the comma (,) character is the typical notation. White-space characters
            // might be present on either side of the comma or semicolon. These white-space
            // characters are ignored, and the semicolon is replaced with a comma.

            // In addition, space (' ' ASCII 32) characters may be present either before or after a
            // '+' or '='. These space characters are ignored when parsing.

            // Based on: http://publib.boulder.ibm.com/infocenter/iseries/v5r3/topic/rzahy/rzahyunderdn.htm

            // Now, Let's process the DistinguishedName and extract the components...
            _log.Debug("DN: " + distinguishedName);

            if (distinguishedName.Length == 0)
            {
                // Empty DNs are okay too.
                return;
            }

            var relativeDnList = SplitToRelativeParts(distinguishedName);

            _log.Debug($"Parsed Distinguished Name into {relativeDnList.Count} Relative Distinguished Names...");

            foreach (var rdn in relativeDnList)
            {
                ProcessRelativeDn(distinguishedName, rdn);
            }
        }

        private void ProcessRelativeDn(string distinguishedName, string rdn)
        {
            var parseState = string.Empty;
            var attributeName = new StringBuilder();
            var attributeValue = new StringBuilder();
            var position = 0;

            while (position < rdn.Length)
            {
                switch (parseState)
                {
                    default:

                        // Ignore any spaces at the beginning of the string
                        try
                        {
                            while (rdn[position] == ' ')
                            {
                                position++;
                            }
                        }
                        catch (IndexOutOfRangeException)
                        {
                            throw new InvalidDistinguishedNameException("A Relative DN is just spaces!");
                        }

                        // Ok, at this point, we should be looking at the first non-space character.
                        if (IsAlpha(rdn[position]))
                        {
                            var workingValue = distinguishedName.Substring(position);

                            // Check to see if the attributeName is an OID
                            if (workingValue.StartsWith("OID.", StringComparison.InvariantCultureIgnoreCase))
                            {
                                // However only the exact strings OID and oid are allowed
                                if (workingValue.StartsWith("OID", StringComparison.InvariantCulture)
                                    || workingValue.StartsWith("oid", StringComparison.InvariantCulture))
                                {
                                    position += 4;
                                    parseState = "AttributeOID";
                                }
                                else
                                {
                                    throw new InvalidDistinguishedNameException("OID mixed-case is not allowed!");
                                }
                            }
                            else
                            {
                                // No, we must be looking at an attribute name
                                parseState = "AttributeName";
                            }
                        }
                        else
                        {
                            // Is this a digit? "OID." is optional so if this is a number, then it
                            // must be an OID
                            if (IsNumber(rdn[position]))
                            {
                                parseState = "AttributeOID";
                            }
                            else
                            {
                                // If it is not a letter or number, then it's invalid...
                                throw new InvalidDistinguishedNameException("Invalid character in attribute!");
                            }
                        }

                        break;

                    case "AttributeOID":
                        try
                        {
                            // Double-check that the character is a number
                            if (!IsNumber(rdn[position]))
                            {
                                throw new InvalidDistinguishedNameException("OID must start with a number!");
                            }

                            // Let's continue processing.
                            while (IsNumber(rdn[position]) || rdn[position] == '.')
                            {
                                attributeName.Append(rdn[position]);
                                position++;
                            }

                            // The OID can be followed by any number of blank spaces
                            while (rdn[position] == ' ')
                            {
                                position++;
                            }

                            if (rdn[position] == '=')
                            {
                                // The AttributeName is complete, lets validate OID.
                                var name = attributeName.ToString();

                                // OID are not allowed to end with a period
                                if (name.EndsWith(".", StringComparison.InvariantCulture))
                                {
                                    throw new InvalidDistinguishedNameException("OID cannot end with a period!");
                                }

                                // OID are not allowed to have two periods together
                                if (name.IndexOf("..", StringComparison.Ordinal) > -1)
                                {
                                    throw new InvalidDistinguishedNameException("OID cannot two periods together.");
                                }

                                // OID numbers are not allowed to have leading zeros
                                var parts = name.Split('.');
                                if (parts.Any(part => (part.Length > 1) && (part[0] == '0')))
                                {
                                    throw new InvalidDistinguishedNameException("OID cannot have a leading zero.");
                                }

                                // This is a valid OID, Let's get the value.
                                position++;
                                parseState = "GetValue";
                            }
                            else
                            {
                                throw new InvalidDistinguishedNameException("Attribute name is unterminated.");
                            }
                        }
                        catch (IndexOutOfRangeException)
                        {
                            throw new InvalidDistinguishedNameException("Attribute name is unterminated.");
                        }

                        break;

                    case "AttributeName":
                        try
                        {
                            // Double-check that the character is letter
                            if (!IsAlpha(rdn[position]))
                            {
                                throw new InvalidDistinguishedNameException("Attribute name must start with a letter.");
                            }

                            // Let's continue processing.
                            while (IsAlpha(rdn[position]) || IsNumber(rdn[position]))
                            {
                                attributeName.Append(rdn[position]);
                                position++;
                            }

                            // The name can be followed by any number of blank spaces
                            while (rdn[position] == ' ')
                            {
                                position++;
                            }

                            if (rdn[position] == '=')
                            {
                                // The AttributeName is complete, Let's get the value.
                                position++;
                                parseState = "GetValue";
                            }
                            else
                            {
                                throw new InvalidDistinguishedNameException("Attribute name is unterminated.");
                            }
                        }
                        catch (IndexOutOfRangeException)
                        {
                            throw new InvalidDistinguishedNameException("Attribute name is unterminated.");
                        }

                        break;

                    case "GetValue":
                        try
                        {
                            // Get rid of any leading spaces
                            while (rdn[position] == ' ')
                            {
                                position++;
                            }
                        }
                        catch (IndexOutOfRangeException)
                        {
                            // It is okay to have an empty value, so catch the exception and store
                            // an empty value.
                        }

                        // Find out what type of value this is
                        switch (rdn[position])
                        {
                            case '"': // this is a quoted string
                                position++; // Ignore the start quote
                                try
                                {
                                    while (rdn[position] != '"')
                                    {
                                        if (rdn[position] == '\\')
                                        {
                                            try
                                            {
                                                if (IsHex(rdn[position + 1]) && IsHex(rdn[position + 2]))
                                                {
                                                    // Let's convert the hexadecimal to it's
                                                    // character and store
                                                    position++; // Discard Escape character
                                                    var ch = Convert.ToByte(rdn.Substring(position, 2), 16);
                                                    var value = Convert.ToString(ch, CultureInfo.InvariantCulture);
                                                    attributeValue.Append(value);
                                                }
                                                else
                                                {
                                                    if (rdn[position + 1] == ' ')
                                                    {
                                                        // Covert escaped spaces to regular spaces
                                                        attributeValue.Append(' ');
                                                    }
                                                    else
                                                    {
                                                        if (IsLdapDnSpecial(rdn[position + 1]) || rdn[position + 1] == ' ')
                                                        {
                                                            attributeValue.Append(rdn, position, 2);
                                                        }
                                                        else
                                                        {
                                                            throw new InvalidDistinguishedNameException("Escape sequence \\" + rdn[position] + " is invalid.");
                                                        }
                                                    }
                                                }

                                                position += 2;
                                            }
                                            catch (IndexOutOfRangeException)
                                            {
                                                throw new InvalidDistinguishedNameException("Invalid escape sequence.");
                                            }
                                        }
                                        else
                                        {
                                            if (IsLdapDnSpecial(rdn[position]))
                                            {
                                                attributeValue.Append('\\');
                                            }

                                            attributeValue.Append(rdn[position]);
                                            position++;
                                        }
                                    }
                                }
                                catch (IndexOutOfRangeException)
                                {
                                    throw new InvalidDistinguishedNameException("Quoted value was not terminated!");
                                }

                                position++; // Ignore the closing quote

                                // Remove any trailing spaces
                                while (position < rdn.Length && rdn[position] == ' ')
                                {
                                    position++;
                                }

                                break;

                            case '#': // this is a hexadecimal string
                                position++;

                                // hexadecimal values consist of two characters each.
                                while (position + 1 < rdn.Length && IsHex(rdn[position]) && IsHex(rdn[position + 1]))
                                {
                                    // Let's convert the hexadecimal to it's character and store
                                    var ch = Convert.ToChar(Convert.ToByte(rdn.Substring(position, 2), 16));
                                    attributeValue.Append(ch);
                                    position += 2;
                                }

                                break;

                            default: // this is a regular (un-quoted) string
                                while (position < rdn.Length && rdn[position] != '+')
                                {
                                    if (rdn[position] == '\\')
                                    {
                                        try
                                        {
                                            // Check to see if this is a hexadecimal escape sequence
                                            // or a regular escape sequence.
                                            if (!(IsHex(rdn[position + 1]) && IsHex(rdn[position + 2]))
                                                && !(IsLdapDnSpecial(rdn[position]) || rdn[position] == ' '))
                                            {
                                                throw new InvalidDistinguishedNameException("Escape sequence \\" + rdn[position] + " is invalid.");
                                            }

                                            attributeValue.Append(rdn, position, 2);
                                            position += 2;
                                        }
                                        catch (IndexOutOfRangeException)
                                        {
                                            throw new InvalidDistinguishedNameException("Invalid escape sequence!");
                                        }
                                    }
                                    else
                                    {
                                        if (IsLdapDnSpecial(rdn[position]))
                                        {
                                            throw new InvalidDistinguishedNameException("Unquoted special character '" + rdn[position] + "'");
                                        }
                                        else
                                        {
                                            attributeValue.Append(rdn[position]);
                                            position++;
                                        }
                                    }
                                }

                                break;
                        }

                        // Check for end-of-string or + sign (which indicates a multi-valued RDN)
                        if (position >= rdn.Length)
                        {
                            // We are at the end of the string
                            break;
                        }
                        else
                        {
                            if (rdn[position] == '+')
                            {
                                // if we've found a plus sign, that means that there's another
                                // name/value pair after it. We'll store what we've found, advance
                                // to the next character, and let the loop cycle again...
                                var value = attributeValue.ToString().TrimEnd() + "+";
                                position++;

                                _components.Add(new NameValue
                                {
                                    Name = attributeName.ToString().TrimEnd(),
                                    Value = value
                                });

                                attributeName.Clear();
                                attributeValue.Clear();

                                parseState = string.Empty;
                            }
                            else
                            {
                                throw new ArgumentException("Invalid Distinguished Name! Invalid characters at end of value.", distinguishedName);
                            }
                        }

                        break;
                }
            }

            // We are finished with the RDN, check the ending state...
            if (parseState != "GetValue")
            {
                throw new InvalidDistinguishedNameException();
            }

            _components.Add(new NameValue
            {
                Name = attributeName.ToString().TrimEnd(),
                Value = attributeValue.ToString().TrimEnd()
            });
        }

        private List<string> SplitToRelativeParts(string distinguishedName)
        {
            var relativeDnList = new List<string>();
            var relativeDn = new StringBuilder();
            var lookForSeparator = true;
            var startPosition = 0;

            // Ignore any spaces at the beginning of the string
            try
            {
                while (distinguishedName[startPosition] == ' ')
                {
                    startPosition++;
                }
            }
            catch (IndexOutOfRangeException)
            {
                throw new ArgumentException("Invalid Distinguished Name, It's just spaces!", distinguishedName);
            }

            // Ignore LDAP:// or GC:// in the front of string
            if (distinguishedName.IndexOf("LDAP://", startPosition, StringComparison.Ordinal) > -1)
            {
                startPosition += 7;
            }

            if (distinguishedName.IndexOf("GC://", startPosition, StringComparison.Ordinal) > -1)
            {
                startPosition += 5;
            }

            // Is the DN just the protocol part?
            if (startPosition == distinguishedName.Length)
            {
                throw new ArgumentException("Invalid Distinguished Name! Needs more than just the protocol part.", distinguishedName);
            }

            // Is the LDAP server already in the DN?
            var slash = distinguishedName.IndexOf("/", StringComparison.Ordinal);
            var equal = distinguishedName.IndexOf("=", StringComparison.Ordinal);

            if ((slash > -1) && (slash < equal))
            {
                var serverPart = distinguishedName.Substring(startPosition, distinguishedName.IndexOf("/", startPosition, StringComparison.Ordinal) - startPosition);
                var colon = serverPart.IndexOf(":", StringComparison.Ordinal);

                if (colon == 0)
                {
                    throw new ArgumentException("Invalid Distinguished Name! Can't have Server Port without Server Name.", distinguishedName);
                }
                else
                {
                    var portIndex = colon > 0 ? colon : serverPart.Length;

                    LdapServer = serverPart.Substring(0, portIndex);
                    if (portIndex != serverPart.Length)
                    {
                        ServerPort = Convert.ToInt32(serverPart.Substring(portIndex + 1), CultureInfo.InvariantCulture);
                    }
                }

                startPosition += serverPart.Length + 1;
            }

            for (var i = startPosition; i < distinguishedName.Length; i++)
            {
                var current = distinguishedName[i];
                var previous = default(char);

                if (i > 0)
                {
                    previous = distinguishedName[i - 1];
                }

                if (lookForSeparator)
                {
                    if ((current == ',' || current == ';') && (previous != '\\'))
                    {
                        // We found a separator, store the RDN.
                        relativeDnList.Add(relativeDn.ToString());
                        relativeDn.Length = 0;
                    }
                    else
                    {
                        relativeDn.Append(current);
                        if (current == '"')
                        {
                            lookForSeparator = false;
                        }
                    }
                }
                else
                {
                    relativeDn.Append(current);

                    // Check for the ending quote; however, escaped quotes don't change the state.
                    if (current == '"' && previous != '\\')
                    {
                        lookForSeparator = true;
                    }
                }
            }

            // Add last relative part
            relativeDnList.Add(relativeDn.ToString());

            return relativeDnList;
        }
    }
}
