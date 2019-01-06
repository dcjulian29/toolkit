using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.Globalization;
using ToolKit;
using ToolKit.DirectoryServices.ServiceInterfaces;

namespace ToolKit.DirectoryServices.ActiveDirectory
{
    /// <summary>
    /// This class represents an ActiveDirectory Group
    /// </summary>
    public class Group : DirectoryObject, IGroup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Group"/> class.
        /// </summary>
        /// <param name="distinguishedName">string representation of the distinguished name.</param>
        public Group(string distinguishedName)
            : base(distinguishedName)
        {
            CheckType();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Group"/> class.
        /// </summary>
        /// <param name="distinguishedName">DistinguishedName object of the distinguished name.</param>
        public Group(DistinguishedName distinguishedName)
            : base(distinguishedName)
        {
            CheckType();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Group"/> class.
        /// </summary>
        /// <param name="result">A SearchResult Object.</param>
        public Group(SearchResult result)
            : base(result)
        {
            CheckType();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Group"/> class.
        /// </summary>
        /// <param name="entry">A DirectoryEntry Object.</param>
        public Group(DirectoryEntry entry)
            : base(entry)
        {
            CheckType();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Group"/> class.
        /// </summary>
        /// <param name="domain">The domain of the group.</param>
        /// <param name="groupName">SAM Account Name of the group.</param>
        public Group(string domain, string groupName)
        {
            var context = new DirectoryContext(DirectoryContextType.Domain, domain);

            var domainObject = Domain.GetDomain(context);

            using (var search = new DirectorySearcher())
            {
                search.SearchRoot = domainObject.GetDirectoryEntry();
                search.Filter = "(samAccountName=" + groupName + ")";

                var result = search.FindOne();

                if (result == null)
                {
                    throw new ArgumentException("Group does not exist in this domain!");
                }

                Initialize(DirectoryServices.DistinguishedName.Parse(result.Path));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Group"/> class.
        /// </summary>
        /// <param name="properties">A Dictionary of properties.</param>
        /// <remarks>This constructor is primarily used for unit tests.</remarks>
        internal Group(Dictionary<string, object> properties)
            : base(properties)
        {
            CheckType();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Group"></see> class. This constructor is
        /// not public and should only be used when the derived class wants to add an additional
        /// constructor. At some point though, the derived classes' constructor needs to call the
        /// "Initialize" method to load up the internal data structures.
        /// </summary>
        protected Group()
        {
        }

        /// <summary>
        /// Gets the category of the group.
        /// </summary>
        /// <value>The category of the group.</value>
        public string Category
        {
            get
            {
                return GetNodeValue("//DirectoryObject/objectcategory");
            }
        }

        /// <summary>
        /// Gets the date and time that group was last changed.
        /// </summary>
        /// <value>The date and time that the group was last changed.</value>
        public DateTime Changed
        {
            get
            {
                return Modified;
            }
        }

        /// <summary>
        /// Gets the common name of the group.
        /// </summary>
        /// <value>The common name of the group.</value>
        public string CommonName
        {
            get
            {
                return GetNodeValue("//DirectoryObject/cn");
            }
        }

        /// <summary>
        /// Gets when the account was created.
        /// </summary>
        /// <value>When the account was created.</value>
        public DateTime Created
        {
            get
            {
                return DateTime.Parse(
                    GetNodeValue("//DirectoryObject/whencreated"),
                    null,
                    DateTimeStyles.AdjustToUniversal);
            }
        }

        /// <summary>
        /// Gets the description of the group.
        /// </summary>
        /// <value>The description of the group.</value>
        public string Description
        {
            get
            {
                return GetNodeValue("//DirectoryObject/description");
            }
        }

        /// <summary>
        /// Gets the display name of the user account.
        /// </summary>
        /// <value>The display name of the user account.</value>
        public string DisplayName
        {
            get
            {
                return GetNodeValue("//DirectoryObject/displayname");
            }
        }

        /// <summary>
        /// Gets the the distinguished name of the group.
        /// </summary>
        /// <value>The distinguished name of the group.</value>
        public string DistinguishedName
        {
            get
            {
                return GetNodeValue("//DirectoryObject/distinguishedname");
            }
        }

        /// <summary>
        /// Gets the email address of the group.
        /// </summary>
        /// <value>The email address of the group.</value>
        public string EmailAddress
        {
            get
            {
                return GetNodeValue("//DirectoryObject/mail");
            }
        }

        /// <summary>
        /// Gets the GUID of the group object.
        /// </summary>
        /// <value>The GUID of the group object.</value>
        public Guid Guid
        {
            get
            {
                return new Guid(HexEncoding.ToBytes(GetNodeValue("//DirectoryObject/objectguid")));
            }
        }

        /// <summary>
        /// Gets the DistinguishedName for the group's manager.
        /// </summary>
        /// <value>The DistinguishedName for the group's manager.</value>
        public string ManagedBy
        {
            get
            {
                return GetNodeValue("//DirectoryObject/managedby");
            }
        }

        /// <summary>
        /// Gets the distinguished name list of members.
        /// </summary>
        /// <value>The distinguished name list of members.</value>
        public List<string> Members
        {
            get
            {
                return GetNodeListValues("//DirectoryObject/member");
            }
        }

        /// <summary>
        /// Gets the modified date and time for the group.
        /// </summary>
        /// <value>The modified date and time for the group.</value>
        public DateTime Modified
        {
            get
            {
                return DateTime.Parse(
                    GetNodeValue("//DirectoryObject/whenchanged"),
                    null,
                    DateTimeStyles.AdjustToUniversal);
            }
        }

        /// <summary>
        /// Gets the name of the group.
        /// </summary>
        /// <value>The name of the group.</value>
        public string Name
        {
            get
            {
                return GetNodeValue("//DirectoryObject/name");
            }
        }

        /// <summary>
        /// Gets the notes of the group.
        /// </summary>
        /// <value>The notes of the group.</value>
        public string Notes
        {
            get
            {
                return GetNodeValue("//DirectoryObject/info");
            }
        }

        /// <summary>
        /// Gets the name of the sam account name of the group
        /// </summary>
        /// <value>The sam account name of the group.</value>
        public string SamAccountName
        {
            get
            {
                return GetNodeValue("//DirectoryObject/samaccountname");
            }
        }

        /// <summary>
        /// Gets the Security ID of the group.
        /// </summary>
        /// <value>The Security ID of the group.</value>
        public string Sid
        {
            get
            {
                var nodeValue = GetNodeValue("//DirectoryObject/objectsid");
                return Security.Sid.ToString(HexEncoding.ToBytes(nodeValue));
            }
        }

        /// <summary>
        /// Gets the update sequence number (USN) of the group when it was created.
        /// </summary>
        /// <value>The update sequence number (USN) of the group when it was created.</value>
        public long UpdateSequenceNumberCreated
        {
            get
            {
                return Convert.ToInt64(GetNodeValue("//DirectoryObject/usncreated"));
            }
        }

        /// <summary>
        /// Gets the current update sequence number (USN) of the group.
        /// </summary>
        /// <value>The current update sequence number (USN) of the group.</value>
        public long UpdateSequenceNumberCurrent
        {
            get
            {
                return Convert.ToInt64(GetNodeValue("//DirectoryObject/usnchanged"));
            }
        }

        /// <summary>
        /// Determines whether this group instance is a distribution list.
        /// </summary>
        /// <returns><c>true</c> if this group instance is a distribution list; otherwise, <c>false</c>.</returns>
        public bool IsDistributionList()
        {
            return !IsFlagSet(
                ADS_GROUP_TYPE.SECURITY_ENABLED,
                Convert.ToInt32(GetNodeValue("//DirectoryObject/grouptype")));
        }

        /// <summary>
        /// Determines whether this group instance is a domain local group.
        /// </summary>
        /// <returns>
        /// <c>true</c> if this group instance is a domain local group; otherwise, <c>false</c>.
        /// </returns>
        public bool IsDomainLocal()
        {
            return IsFlagSet(
                ADS_GROUP_TYPE.DOMAIN_LOCAL_GROUP,
                Convert.ToInt32(GetNodeValue("//DirectoryObject/grouptype")));
        }

        /// <summary>
        /// Determines whether this group instance is global.
        /// </summary>
        /// <returns><c>true</c> if this group instance is global; otherwise, <c>false</c>.</returns>
        public bool IsGlobal()
        {
            return IsFlagSet(
                ADS_GROUP_TYPE.GLOBAL_GROUP,
                Convert.ToInt32(GetNodeValue("//DirectoryObject/grouptype")));
        }

        /// <summary>
        /// Determines whether this group instance is security enabled.
        /// </summary>
        /// <returns><c>true</c> if this group instance is security enabled; otherwise, <c>false</c>.</returns>
        public bool IsSecurity()
        {
            return IsFlagSet(
                ADS_GROUP_TYPE.SECURITY_ENABLED,
                Convert.ToInt32(GetNodeValue("//DirectoryObject/grouptype")));
        }

        /// <summary>
        /// Determines whether this group instance is a universal group.
        /// </summary>
        /// <returns><c>true</c> if this group instance is a universal group; otherwise, <c>false</c>.</returns>
        public bool IsUniversal()
        {
            return IsFlagSet(
                ADS_GROUP_TYPE.UNIVERSAL_GROUP,
                Convert.ToInt32(GetNodeValue("//DirectoryObject/grouptype")));
        }

        private void CheckType()
        {
            // Check to see if this is indeed a Group object
            if (!GetObjectClass().Contains("group"))
            {
                throw new ArgumentException("The directory object does not represent a group!");
            }
        }

        private bool IsFlagSet(ADS_GROUP_TYPE flag, int flagsToCheck)
        {
            return (flagsToCheck & (int)flag) == (int)flag;
        }
    }
}
