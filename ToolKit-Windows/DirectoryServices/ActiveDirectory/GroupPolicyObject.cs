using System;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text.RegularExpressions;
using Common.Logging;

namespace ToolKit.DirectoryServices.ActiveDirectory
{
    /// <summary>
    /// A Group Policy Object (GPO) is a collection of settings that define what a system will look
    /// like and how it will behave for a defined group of users.A GPO is associated with selected
    /// Active Directory containers, such as sites, domains, or organizational units (OUs). GPO
    /// settings define registry-based polices, security options, software installation and
    /// maintenance options, scripts options, and folder redirection options.
    /// </summary>
    public class GroupPolicyObject
    {
        private static ILog _log = LogManager.GetLogger<GroupPolicyObject>();

        /// <summary>
        /// Initializes a new instance of the GroupPolicyObject class
        /// </summary>
        /// <param name="domainName">The Name of the Domain where the GPO resides</param>
        /// <param name="nameOfGpo">The name of the GPO</param>
        public GroupPolicyObject(string domainName, string nameOfGpo)
        {
            if (string.IsNullOrEmpty(domainName))
            {
                throw new ArgumentNullException(nameof(domainName));
            }

            if (string.IsNullOrEmpty(nameOfGpo))
            {
                throw new ArgumentNullException(nameof(nameOfGpo));
            }

            DomainName = domainName;
            Name = nameOfGpo;
            string dn;

            try
            {
                var context = new DirectoryContext(DirectoryContextType.Domain, domainName);

                var domainObject = Domain.GetDomain(context).GetDirectoryEntry();

                dn = (string)domainObject.Properties["distinguishedName"][0];
                _log.Debug($"_DomainDN: {dn}");
            }
            catch (Exception ex)
            {
                _log.Error($"Could not find domain: {domainName}", ex);
                throw;
            }

            // Let look up the GPO and put the information into the properties
            _log.Debug($"Looking up GPO: {nameOfGpo}");

            try
            {
                var name = $"CN=Policies,CN=System,{dn}";
                var path = DirectoryServices.DistinguishedName.Parse(name);
                var filter = new LdapFilter("displayName", "=", nameOfGpo);

                using (var query = new LdapQuery(path, filter, false))
                {
                    var result = query.FindOne();

                    DistinguishedName = result.Properties["distinguishedName"][0] as string;
                    _log.Debug($"GPO DistinguishedName: {DistinguishedName}");
                }
            }
            catch (Exception ex)
            {
                _log.Error($"Could not find GPO: {nameOfGpo}. Exception: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Gets the Distinguished Name of the Group Policy Object
        /// </summary>
        /// <returns>A string representing the Distinguished Name</returns>
        public string DistinguishedName { get; private set; }

        /// <summary>
        /// Gets the name of the domain where the GPO resides
        /// </summary>
        /// <returns>The domain name</returns>
        public string DomainName { get; private set; }

        /// <summary>
        /// Gets the friendly name of the Group Policy Object
        /// </summary>
        /// <returns>The Name of the GPO</returns>
        public string Name { get; private set; }

        /// <summary>
        /// Append the GPO at the end of the list. This GPO will be applied first.
        /// </summary>
        /// <param name="distinguishedNameOfOu">The Distinguished Name of the Organizational Unit</param>
        public void Append(string distinguishedNameOfOu)
        {
            InsertAt(distinguishedNameOfOu, -1);
        }

        /// <summary>
        /// Insert the GPO as the first entry. This GPO will be applied last.
        /// </summary>
        /// <param name="distinguishedNameOfOu">The Distinguished Name of the Organizational Unit</param>
        public void Insert(string distinguishedNameOfOu)
        {
            InsertAt(distinguishedNameOfOu, 0);
        }

        /// <summary>
        /// Insert the GPO at a specific place. GPO are applied from the last entry to the first
        /// entry. This is reverse of the way they are listed in this attribute.
        /// </summary>
        /// <param name="distinguishedNameOfOu">The Distinguished Name of the Organizational Unit</param>
        /// <param name="place">The place(index) to insert the GPO at</param>
        public void InsertAt(string distinguishedNameOfOu, int place)
        {
            var thisLink = $"[LDAP://{DistinguishedName};0]";
            string oldGpLink;
            var newGpLink = string.Empty;

            // Let's get the existing gpLink attribute
            var path = DirectoryServices.DistinguishedName.Parse(distinguishedNameOfOu);
            using (var entry = new DirectoryEntry(path.LdapPath))
            {
                var pathOfOu = entry.Path;
                _log.Debug($"OU Distinguished Name: {pathOfOu}");

                var link = (entry.Properties["GPLink"].Value as string) ?? string.Empty;

                _log.Debug($"oldGPLink: {link}");

                oldGpLink = link;
            }

            if (oldGpLink.Contains(DistinguishedName))
            {
                _log.Debug("GPO already Linked on this OU. Removing old one.");
                oldGpLink = oldGpLink.Replace(thisLink, string.Empty);
            }

            var re = new Regex(@"(\[[^\[]*\])");

            var links = (from Match item in re.Matches(oldGpLink) select item.Value).ToList();

            // Now let's insert the new GPO dn in the gpLink
            if (links.Count > 0)
            {
                var tmpGpLink = links.ToArray();

                for (var i = 0; i < links.Count; i++)
                {
                    if (i == place)
                    {
                        newGpLink += thisLink;
                    }

                    newGpLink += tmpGpLink[i];

                    if ((place == -1) && (i == links.Count - 1))
                    {
                        // Append the GPO
                        newGpLink += thisLink;
                    }
                }
            }
            else
            {
                newGpLink = thisLink;
            }

            _log.Debug($"newGPLink: {newGpLink}");

            // Commit the change back to to the OU
            using (var entry = new DirectoryEntry(path.LdapPath))
            {
                if (entry.Properties.Contains("GPLink"))
                {
                    entry.Properties["GPLink"].Value = newGpLink;
                }
                else
                {
                    entry.Properties["GPLink"].Add(newGpLink);
                    entry.Properties["gPOptions"].Add("0");
                }

                entry.CommitChanges();
            }
        }
    }
}
