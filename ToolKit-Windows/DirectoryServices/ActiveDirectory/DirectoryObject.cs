using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Common.Logging;
using ToolKit.Validation;
using ToolKit.Xml;

namespace ToolKit.DirectoryServices.ActiveDirectory
{
    /// <summary>
    /// The class encapsulates a node or object in the Active Directory hierarchy and is extended by
    /// more specific class to represent the node or object.
    /// </summary>
    public class DirectoryObject
    {
        // This class can do a lot a debug logging. No need to do most of it if running in release mode.
        private static readonly bool _debugMode =
            AssemblyProperties.IsDebugMode(typeof(DirectoryObject).Assembly.Location);

        private static readonly ILog _log = LogManager.GetLogger<DirectoryObject>();

        private string _distinguishedName;

        private XmlDocument _properties;

        private int _propertiesCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectoryObject"/> class.
        /// </summary>
        /// <param name="properties">A Dictionary of properties.</param>
        /// <remarks>This constructor is primarily used for unit tests.</remarks>
        internal DirectoryObject(Dictionary<string, object> properties)
        {
            _distinguishedName = "cn=testObject,dc=company,dc=local";

            Initialize(properties);
        }

        /// <summary>
        /// Initializes a new instance of the <see
        /// cref="ToolKit.DirectoryServices.ActiveDirectory.DirectoryObject"/> class. This
        /// constructor is not public and should only be used when the derived class wants to add an
        /// additional constructor. At some point though, the derived classes' constructor needs to
        /// call the "Initialize" method to load up the internal data structures.
        /// </summary>
        protected DirectoryObject()
        {
            _distinguishedName = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see
        /// cref="ToolKit.DirectoryServices.ActiveDirectory.DirectoryObject"/> class.
        /// </summary>
        /// <param name="distinguishedName">string representation of the distinguished name.</param>
        protected DirectoryObject(string distinguishedName)
        {
            _distinguishedName = string.Empty;
            if (string.IsNullOrEmpty(distinguishedName))
            {
                throw new ArgumentNullException(nameof(distinguishedName));
            }

            Initialize(DistinguishedName.Parse(distinguishedName));
        }

        /// <summary>
        /// Initializes a new instance of the <see
        /// cref="ToolKit.DirectoryServices.ActiveDirectory.DirectoryObject"/> class.
        /// </summary>
        /// <param name="distinguishedName">DistinguishedName object of the distinguished name.</param>
        protected DirectoryObject(DistinguishedName distinguishedName)
        {
            _distinguishedName = string.Empty;
            if (distinguishedName == null)
            {
                throw new ArgumentNullException(nameof(distinguishedName));
            }

            Initialize(distinguishedName);
        }

        /// <summary>
        /// Initializes a new instance of the <see
        /// cref="ToolKit.DirectoryServices.ActiveDirectory.DirectoryObject"/> class.
        /// </summary>
        /// <param name="result">A SearchResult Object.</param>
        protected DirectoryObject(SearchResult result)
        {
            _distinguishedName = string.Empty;
            if (result == null)
            {
                throw new ArgumentNullException(nameof(result));
            }

            Initialize(DistinguishedName.Parse(result.Path));
        }

        /// <summary>
        /// Initializes a new instance of the <see
        /// cref="ToolKit.DirectoryServices.ActiveDirectory.DirectoryObject"/> class.
        /// </summary>
        /// <param name="entry">A DirectoryEntry Object.</param>
        protected DirectoryObject(DirectoryEntry entry)
        {
            _distinguishedName = string.Empty;
            if (entry == null)
            {
                throw new ArgumentNullException(nameof(entry));
            }

            Initialize(DistinguishedName.Parse(entry.Path));
        }

        /// <summary>
        /// Gets the NetBIOS Domain of an Active Directory Distinguished Name.
        /// </summary>
        /// <value>The NetBIOS Domain.</value>
        public string NetBiosDomain
        {
            get
            {
                var domainRoot = DistinguishedName.Parse(_distinguishedName).DomainRoot;

                return !string.IsNullOrEmpty(domainRoot)
                    ? ResolveNetBios().ToUpper(CultureInfo.InvariantCulture) : null;
            }
        }

        /// <summary>
        /// Gets The Number of Properties
        /// </summary>
        /// <value>The number of properties in this object.</value>
        public int NumberOfProperties => _propertiesCount;

        /// <summary>
        /// Determines if the specified distinguished name exists.
        /// </summary>
        /// <param name="distinguishedName">The distinguished Name of the object.</param>
        /// <returns><c>True</c> if the object exists, otherwise <c>False</c></returns>
        public static bool Exists(string distinguishedName)
        {
            return DirectoryEntry.Exists(DistinguishedName.Parse(distinguishedName).LdapPath);
        }

        /// <summary>
        /// Determines whether the specified object is a computer object.
        /// </summary>
        /// <param name="entry">The directory entry representing the ADSI object.</param>
        /// <returns><c>true</c> if the specified entry is a computer; otherwise, <c>false</c>.</returns>
        public static bool IsComputer(DirectoryEntry entry)
        {
            return IsType(entry, "computer");
        }

        /// <summary>
        /// Determines whether the specified distinguished name is a computer object.
        /// </summary>
        /// <param name="distinguishedName">The distinguished name representing the ADSI object</param>
        /// <returns>
        /// <c>true</c> if the specified distinguished name is a computer; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsComputer(DistinguishedName distinguishedName)
        {
            using (var de = new DirectoryEntry(distinguishedName?.GcPath))
            {
                return Exists(distinguishedName?.GcPath) && IsComputer(de);
            }
        }

        /// <summary>
        /// Determines whether the specified search result object is a computer.
        /// </summary>
        /// <param name="result">The search result representing the ADSI object.</param>
        /// <returns><c>true</c> if the specified result is a computer; otherwise, <c>false</c>.</returns>
        public static bool IsComputer(SearchResult result)
        {
            return IsComputer(result?.GetDirectoryEntry());
        }

        /// <summary>
        /// Determines whether the specified distinguished name is a computer object.
        /// </summary>
        /// <param name="distinguishedName">The distinguished name representing the ADSI object</param>
        /// <returns>
        /// <c>true</c> if the specified distinguished name is a computer; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsComputer(string distinguishedName)
        {
            return IsComputer(DistinguishedName.Parse(distinguishedName));
        }

        /// <summary>
        /// Determines whether the specified object is a contact object.
        /// </summary>
        /// <param name="entry">The directory entry representing the ADSI object.</param>
        /// <returns><c>true</c> if the specified entry is a contact; otherwise, <c>false</c>.</returns>
        public static bool IsContact(DirectoryEntry entry)
        {
            return IsType(entry, "contact");
        }

        /// <summary>
        /// Determines whether the specified distinguished name is a contact object.
        /// </summary>
        /// <param name="distinguishedName">The distinguished name representing the ADSI object</param>
        /// <returns>
        /// <c>true</c> if the specified distinguished name is a contact; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsContact(DistinguishedName distinguishedName)
        {
            using (var de = new DirectoryEntry(distinguishedName?.GcPath))
            {
                return Exists(distinguishedName?.GcPath) && IsContact(de);
            }
        }

        /// <summary>
        /// Determines whether the specified search result object is a contact.
        /// </summary>
        /// <param name="result">The search result representing the ADSI object.</param>
        /// <returns><c>true</c> if the specified result is a contact; otherwise, <c>false</c>.</returns>
        public static bool IsContact(SearchResult result)
        {
            return IsContact(result?.GetDirectoryEntry());
        }

        /// <summary>
        /// Determines whether the specified distinguished name is a contact object.
        /// </summary>
        /// <param name="distinguishedName">The distinguished name representing the ADSI object</param>
        /// <returns>
        /// <c>true</c> if the specified distinguished name is a contact; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsContact(string distinguishedName)
        {
            return IsContact(DistinguishedName.Parse(distinguishedName));
        }

        /// <summary>
        /// Determines whether the specified object is a group object.
        /// </summary>
        /// <param name="entry">The directory entry representing the ADSI object.</param>
        /// <returns><c>true</c> if the specified entry is a group; otherwise, <c>false</c>.</returns>
        public static bool IsGroup(DirectoryEntry entry)
        {
            return IsType(entry, "group");
        }

        /// <summary>
        /// Determines whether the specified distinguished name is a group object.
        /// </summary>
        /// <param name="distinguishedName">The distinguished name representing the ADSI object</param>
        /// <returns><c>true</c> if the specified distinguished name is a group; otherwise, <c>false</c>.</returns>
        public static bool IsGroup(DistinguishedName distinguishedName)
        {
            using (var de = new DirectoryEntry(distinguishedName?.GcPath))
            {
                return Exists(distinguishedName?.GcPath) && IsGroup(de);
            }
        }

        /// <summary>
        /// Determines whether the specified search result object is a group.
        /// </summary>
        /// <param name="result">The search result representing the ADSI object.</param>
        /// <returns><c>true</c> if the specified result is a group; otherwise, <c>false</c>.</returns>
        public static bool IsGroup(SearchResult result)
        {
            return IsGroup(result?.GetDirectoryEntry());
        }

        /// <summary>
        /// Determines whether the specified distinguished name is a group object.
        /// </summary>
        /// <param name="distinguishedName">The distinguished name representing the ADSI object</param>
        /// <returns><c>true</c> if the specified distinguished name is a group; otherwise, <c>false</c>.</returns>
        public static bool IsGroup(string distinguishedName)
        {
            return IsGroup(DistinguishedName.Parse(distinguishedName));
        }

        /// <summary>
        /// Determines whether the specified object is a user object.
        /// </summary>
        /// <param name="entry">The directory entry representing the ADSI object.</param>
        /// <returns><c>true</c> if the specified entry is a user; otherwise, <c>false</c>.</returns>
        public static bool IsUser(DirectoryEntry entry)
        {
            return IsType(entry, "user") && !IsType(entry, "computer");
        }

        /// <summary>
        /// Determines whether the specified distinguished name is a user object.
        /// </summary>
        /// <param name="distinguishedName">The distinguished name representing the ADSI object</param>
        /// <returns><c>true</c> if the specified distinguished name is a user; otherwise, <c>false</c>.</returns>
        public static bool IsUser(DistinguishedName distinguishedName)
        {
            using (var de = new DirectoryEntry(distinguishedName?.GcPath))
            {
                return Exists(distinguishedName?.GcPath) && IsUser(de);
            }
        }

        /// <summary>
        /// Determines whether the specified search result object is a user.
        /// </summary>
        /// <param name="result">The search result representing the ADSI object.</param>
        /// <returns><c>true</c> if the specified result is a user; otherwise, <c>false</c>.</returns>
        public static bool IsUser(SearchResult result)
        {
            return IsUser(result?.GetDirectoryEntry());
        }

        /// <summary>
        /// Determines whether the specified distinguished name is a user object.
        /// </summary>
        /// <param name="distinguishedName">The distinguished name representing the ADSI object</param>
        /// <returns><c>true</c> if the specified distinguished name is a user; otherwise, <c>false</c>.</returns>
        public static bool IsUser(string distinguishedName)
        {
            return IsUser(DistinguishedName.Parse(distinguishedName));
        }

        /// <summary>
        /// Moves this object to the distinguished name of the new parent container.
        /// </summary>
        /// <param name="newLocation">The distinguished name of the new parent container.</param>
        public void Move(string newLocation)
        {
            Move(DistinguishedName.Parse(newLocation));
        }

        /// <summary>
        /// Moves this object to the distinguished name of the new parent container.
        /// </summary>
        /// <param name="newLocation">The distinguished name of the new parent container.</param>
        public void Move(DistinguishedName newLocation)
        {
            newLocation = Check.NotNull(newLocation, nameof(newLocation));

            if (!Exists(newLocation.GcPath))
            {
                throw new ArgumentException("Destination does not exists!");
            }

            using (var o = ToDirectoryEntry())
            {
                using (var n = new DirectoryEntry(newLocation.LdapPath))
                {
                    o.MoveTo(n, n.Name);
                }
            }
        }

        /// <summary>
        /// Converts this object to a DirectoryEntry.
        /// </summary>
        /// <returns>This object as a DirectoryEntry</returns>
        public DirectoryEntry ToDirectoryEntry()
        {
            return new DirectoryEntry(DistinguishedName.Parse(_distinguishedName).LdapPath);
        }

        internal void Initialize(ResultPropertyCollection collection)
        {
            var extracted = new Dictionary<string, object>();

            if ((collection == null) || (collection.Count == 0))
            {
                return;
            }

            if (collection.PropertyNames == null)
            {
                return;
            }

            foreach (string key in collection.PropertyNames)
            {
                extracted.Add(
                    key,
                    collection[key].Count == 1 ? collection[key][0] : collection[key].Cast<object>().ToArray());
            }

            Initialize(extracted);
        }

        internal void Initialize(Dictionary<string, object> collection)
        {
            _properties = new XmlDocument()
            {
                XmlResolver = null
            };

            using (var reader = XmlReader.Create(
                new StringReader("<?xml version=\"1.0\" encoding=\"utf-8\" ?><DirectoryObject></DirectoryObject>"),
                new XmlReaderSettings() { XmlResolver = null }))
            {
                _properties.Load(reader);
            }

            var topNode = _properties.SelectSingleNode("//DirectoryObject");

            if ((collection == null) || (collection.Count == 0))
            {
                return;
            }

            foreach (var item in collection)
            {
                if (item.Value is object[] objects)
                {
                    foreach (var property in objects)
                    {
                        var element = CreateXmlElement(item.Key, property);

                        topNode?.AppendChild(element);
                    }
                }
                else
                {
                    var element = CreateXmlElement(item.Key, item.Value);

                    topNode?.AppendChild(element);
                }
            }

            _propertiesCount = collection.Count;
        }

        /// <summary>
        /// Returns the values contained in the _properties using an XPath query
        /// </summary>
        /// <param name="xpathQuery">an XPath Query</param>
        /// <returns>List of string values of the nodes</returns>
        protected List<string> GetNodeListValues(string xpathQuery)
        {
            var nodeListValues = new List<string>();

            try
            {
                var list = _properties.SelectNodes(xpathQuery);

                if (list == null)
                {
                    return nodeListValues;
                }

                foreach (XmlNode node in list)
                {
                    string value;

                    if (node.Attributes?[0].InnerText == "System.String")
                    {
                        value = XmlEncoder.Decode(node.InnerText);
                    }
                    else
                    {
                        value = node.InnerText;
                    }

                    if (_debugMode)
                    {
                        _log.Debug($"{xpathQuery} --> {value}");
                    }

                    nodeListValues.Add(value);
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message, ex);
                throw;
            }

            return nodeListValues;
        }

        /// <summary>
        /// Returns the value in the node using an XPath query
        /// </summary>
        /// <param name="xpathQuery">an XPath Query</param>
        /// <returns>value of the node</returns>
        protected string GetNodeValue(string xpathQuery)
        {
            string value = null;

            try
            {
                var node = _properties.SelectSingleNode(xpathQuery);

                if (node == null)
                {
                    _log.Debug($"{xpathQuery} --> null");
                }
                else
                {
                    if (node.Attributes?[0].InnerText == "System.String")
                    {
                        if (!string.IsNullOrEmpty(node.InnerText))
                        {
                            value = XmlEncoder.Decode(node.InnerText);
                        }

                        _log.Debug($"{xpathQuery} --> \"{value}\"");
                    }
                    else
                    {
                        value = node.InnerText;
                        _log.Debug($"{xpathQuery} --> \"{value}\"");
                    }
                }
            }
            catch (ArgumentNullException ex)
            {
                _log.Warn($"{xpathQuery} --> null", ex);
            }

            return value;
        }

        /// <summary>
        /// Gets the object class or classes of this object.
        /// </summary>
        /// <returns>List of classes separated by a space</returns>
        protected string GetObjectClass()
        {
            var objectClass = string.Empty;

            try
            {
                var list = _properties.SelectNodes("//DirectoryObject/objectclass");

                if (list != null)
                {
                    objectClass = list.Cast<XmlNode>()
                        .Aggregate(objectClass, (current, node) => current + $"{node.InnerText} ");
                }
            }
            catch (ArgumentNullException anex)
            {
                _log.Error(anex.Message, anex);
            }
            catch (InvalidCastException icex)
            {
                _log.Error(icex.Message, icex);
            }

            if (_debugMode)
            {
                _log.Debug($"objectClass: {objectClass}");
            }

            return objectClass.Trim();
        }

        /// <summary>
        /// Initializes the internal data structure using the specified distinguished name of the object.
        /// </summary>
        /// <param name="distinguishedName">The distinguished name of the object.</param>
        protected void Initialize(DistinguishedName distinguishedName)
        {
            if (_debugMode)
            {
                _log.Debug($"ldapPath: {distinguishedName?.LdapPath}");
            }

            // Create a Search Result object to take advantaged of the COM marshaling of properties.
            using (var entry = new DirectoryEntry(distinguishedName?.LdapPath))
            {
                using (var search = new DirectorySearcher(entry, "cn=*"))
                {
                    search.Filter = "(cn=*)";
                    search.SearchScope = SearchScope.Base;

                    var result = search.FindOne();

                    _distinguishedName = result.Path;

                    Initialize(result.Properties);
                }
            }
        }

        private static bool IsType(DirectoryEntry entry, string objectType)
        {
            if (entry == null)
            {
                return false;
            }

            var propertyCollection = entry.Properties;

            if (propertyCollection.Count == 0)
            {
                return false;
            }

            if (!entry.Properties.Contains("objectclass"))
            {
                return false;
            }

            var objectClass = propertyCollection["objectclass"].Cast<object>()
                .Aggregate(string.Empty, (current, property) => current + $"{property} ");

            return objectClass.Contains(objectType);
        }

        private XmlElement CreateXmlElement(string key, object value)
        {
            var element = _properties.CreateElement(key);
            var attribute = _properties.CreateAttribute("type");

            attribute.InnerText = value.GetType().ToString();
            element.Attributes.Append(attribute);

            if (value.GetType() == typeof(byte[]))
            {
                element.InnerText = HexEncoding.ToString((byte[])value);
            }
            else
            {
                if (key == "userparameters")
                {
                    // This is a weird value in that it contains non-printable characters which are
                    // invalid for the underlying XML storage... so we'll just convert the value to
                    // Hexadecimal for storage and then convert out a string if needed later.
                    var valueBytes = new UnicodeEncoding().GetBytes((string)value);
                    element.InnerText = HexEncoding.ToString(valueBytes);
                }
                else
                {
                    element.InnerText = value is string ? XmlEncoder.Encode(value.ToString()) : value.ToString();
                }
            }

            if (_debugMode)
            {
                _log.Debug($"{key} > {attribute.InnerText} : {element.InnerText}");
            }

            return element;
        }

        private string ResolveNetBios()
        {
            // This Value would only exist for an Active Directory Distinguished Name.
            string netBiosName = null;
            var dn = DistinguishedName.Parse(_distinguishedName);

            var context = new DirectoryContext(DirectoryContextType.Domain, dn.DnsDomain);
            var forest = Domain.GetDomain(context).Forest;

            using (var root = forest.RootDomain.GetDirectoryEntry())
            {
                var path = root.Path.Substring(7);
                var slash = path.IndexOf("/", StringComparison.Ordinal);
                path = $"LDAP://{path.Insert(slash + 1, "CN=Partitions, CN=Configuration,")}";

                using (var entry = new DirectoryEntry(path))
                {
                    SearchResultCollection results;
                    using (var search = new DirectorySearcher(entry))
                    {
                        search.Filter = "(&(objectClass=top)(nETBIOSName=*))";
                        search.PropertiesToLoad.Add("nETBIOSName");
                        search.PropertiesToLoad.Add("nCName");

                        results = search.FindAll();
                    }

                    foreach (SearchResult result in results)
                    {
                        var contextName = (string)result.Properties["nCName"][0];
                        if (string.Equals(contextName, dn.DomainRoot, StringComparison.OrdinalIgnoreCase))
                        {
                            netBiosName = (string)result.Properties["netBIOSName"][0];
                        }
                    }
                }
            }

            return netBiosName;
        }
    }
}
