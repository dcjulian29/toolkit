using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.Globalization;
using Common.Logging;
using Toolkit;
using ToolKit.DirectoryServices.ServiceInterfaces;
using DN = ToolKit.DirectoryServices.DistinguishedName;

namespace ToolKit.DirectoryServices.ActiveDirectory
{
    /// <summary>
    /// This class represents an ActiveDirectory Computer
    /// </summary>
    public class Computer : DirectoryObject, IComputer
    {
        private static ILog _log = LogManager.GetLogger<Computer>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Computer"/> class.
        /// </summary>
        /// <param name="distinguishedName">string representation of the distinguished name.</param>
        public Computer(string distinguishedName)
            : base(distinguishedName)
        {
            CheckType();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Computer"/> class.
        /// </summary>
        /// <param name="distinguishedName">DistinguishedName object of the distinguished name.</param>
        public Computer(DistinguishedName distinguishedName)
            : base(distinguishedName)
        {
            CheckType();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Computer"/> class.
        /// </summary>
        /// <param name="result">A SearchResult Object.</param>
        public Computer(SearchResult result)
            : base(result)
        {
            CheckType();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Computer"/> class.
        /// </summary>
        /// <param name="entry">A DirectoryEntry Object.</param>
        public Computer(DirectoryEntry entry)
            : base(entry)
        {
            CheckType();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Computer"/> class.
        /// </summary>
        /// <param name="domain">The domain of the computer.</param>
        /// <param name="computerName">SAM Account Name of the computer.</param>
        public Computer(string domain, string computerName)
        {
            var context = new DirectoryContext(DirectoryContextType.Domain, domain);

            var domainObject = Domain.GetDomain(context);

            using (var search = new DirectorySearcher())
            {
                search.SearchRoot = domainObject.GetDirectoryEntry();
                search.Filter = "(samAccountName=" + computerName + "$)";

                var result = search.FindOne();

                if (result == null)
                {
                    throw new ArgumentException("Computer does not exist in this domain!");
                }

                Initialize(DN.Parse(result.Path));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Computer"/> class.
        /// </summary>
        /// <param name="properties">A Dictionary of properties.</param>
        /// <remarks>This constructor is primarily used for unit tests.</remarks>
        internal Computer(Dictionary<string, object> properties) : base(properties)
        {
            CheckType();
        }

        /// <summary>
        /// Gets the date and time that this computer account expires.
        /// </summary>
        /// <value>The date and time that this computer account expires.</value>
        public DateTime AccountExpires
        {
            get
            {
                try
                {
                    string expiration = GetNodeValue("//DirectoryObject/accountexpires");
                    if (String.IsNullOrEmpty(expiration))
                    {
                        return DateTime.MaxValue;
                    }

                    return DateTime.FromFileTime(Convert.ToInt64(expiration));
                }
                catch (ArgumentOutOfRangeException)
                {
                    // Means that the account never expires.
                    return DateTime.MaxValue;
                }
            }
        }

        /// <summary>
        /// Gets the bad password count of the computer.
        /// </summary>
        /// <value>The bad password count of the computer.</value>
        public int BadPasswordCount
        {
            get
            {
                return Convert.ToInt32(GetNodeValue("//DirectoryObject/badpwdcount"));
            }
        }

        /// <summary>
        /// Gets the bad password time of the computer.
        /// </summary>
        /// <value>The bad password time.</value>
        public DateTime BadPasswordTime
        {
            get
            {
                var nodeValue = GetNodeValue("//DirectoryObject/badpasswordtime");
                return DateTime.FromFileTimeUtc(Convert.ToInt64(nodeValue));
            }
        }

        /// <summary>
        /// Gets the category of the computer.
        /// </summary>
        /// <value>The category of the computer.</value>
        public string Category
        {
            get
            {
                return GetNodeValue("//DirectoryObject/objectcategory");
            }
        }

        /// <summary>
        /// Gets the date and time that computer account was last changed.
        /// </summary>
        /// <value>The date and time that the computer account was last changed.</value>
        public DateTime Changed
        {
            get
            {
                return Modified;
            }
        }

        /// <summary>
        /// Gets the common name of the computer.
        /// </summary>
        /// <value>The common name of the computer.</value>
        public string CommonName
        {
            get
            {
                return GetNodeValue("//DirectoryObject/cn");
            }
        }

        /// <summary>
        /// Gets the computer account control value of the computer account.
        /// </summary>
        /// <value>The computer account control value of the computer account.</value>
        public int ComputerAccountControl
        {
            get
            {
                return Convert.ToInt32(GetNodeValue("//DirectoryObject/useraccountcontrol"));
            }
        }

        /// <summary>
        /// Gets the country code for the computer account
        /// </summary>
        /// <value>The country code for the computer account.</value>
        public int CountryCode
        {
            get
            {
                return Convert.ToInt32(GetNodeValue("//DirectoryObject/countrycode"));
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
                    DateTimeStyles.AssumeUniversal);
            }
        }

        /// <summary>
        /// Gets the description of the computer account.
        /// </summary>
        /// <value>The description of the computer account.</value>
        public string Description
        {
            get
            {
                return GetNodeValue("//DirectoryObject/description");
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Computer"/> is disabled.
        /// </summary>
        /// <value><c>true</c> if disabled; otherwise, <c>false</c>.</value>
        public bool Disabled
        {
            get
            {
                return Convert.ToBoolean(ComputerAccountControl
                                         & (int)ADS_USER_FLAG.ACCOUNTDISABLE);
            }
        }

        /// <summary>
        /// Gets the distinguished name of the computer account.
        /// </summary>
        /// <value>The distinguished name of the computer account.</value>
        public string DistinguishedName
        {
            get
            {
                return GetNodeValue("//DirectoryObject/distinguishedname");
            }
        }

        /// <summary>
        /// Gets the DNS address of the computer account.
        /// </summary>
        /// <value>The DNS address of the computer account.</value>
        public string DnsHostName
        {
            get
            {
                return GetNodeValue("//DirectoryObject/dnshostname");
            }
        }

        /// <summary>
        /// Gets the NetBIOS Domain name of the computer.
        /// </summary>
        /// <value>The domain.</value>
        public string DomainName
        {
            get
            {
                return DN.Parse(DistinguishedName).NetBiosDomain;
            }
        }

        /// <summary>
        /// Gets the distinguished name list of groups.
        /// </summary>
        /// <value>The distinguished name list of groups.</value>
        public List<string> Groups
        {
            get
            {
                return GetNodeListValues("//DirectoryObject/memberof");
            }
        }

        /// <summary>
        /// Gets the GUID of the computer object.
        /// </summary>
        /// <value>The GUID of the computer object.</value>
        public Guid Guid
        {
            get
            {
                return new Guid(HexEncoding.ToBytes(GetNodeValue("//DirectoryObject/objectguid")));
            }
        }

        /// <summary>
        /// Gets the last logoff date and time of the computer account.
        /// </summary>
        /// <value>The last logoff date and time of the computer account.</value>
        public DateTime LastLogoff
        {
            get
            {
                string nodeValue = GetNodeValue("//DirectoryObject/lastlogoff");
                return DateTime.FromFileTimeUtc(Convert.ToInt64(nodeValue));
            }
        }

        /// <summary>
        /// Gets the last logon date and time of the computer account.
        /// </summary>
        /// <value>The last logon date and time of the computer account.</value>
        public DateTime LastLogon
        {
            get
            {
                string nodeValue = GetNodeValue("//DirectoryObject/lastlogontimestamp");
                return DateTime.FromFileTimeUtc(Convert.ToInt64(nodeValue));
            }
        }

        /// <summary>
        /// Gets the location of the computer account.
        /// </summary>
        /// <value>The location of the computer account.</value>
        public string Location
        {
            get
            {
                return GetNodeValue("//DirectoryObject/location");
            }
        }

        /// <summary>
        /// Gets the logon count of the computer account.
        /// </summary>
        /// <value>The logon count of the computer account.</value>
        public int LogonCount
        {
            get
            {
                return Convert.ToInt32(GetNodeValue("//DirectoryObject/logoncount"));
            }
        }

        /// <summary>
        /// Gets the manager's DistinguishedName for the computer account.
        /// </summary>
        /// <value>The manager's DistinguishedName for the computer account.</value>
        public string ManagedBy
        {
            get
            {
                return GetNodeValue("//DirectoryObject/managedby");
            }
        }

        /// <summary>
        /// Gets the modified date and time for the computer account.
        /// </summary>
        /// <value>The modified date and time for the computer account.</value>
        public DateTime Modified
        {
            get
            {
                return DateTime.Parse(
                    GetNodeValue("//DirectoryObject/whenchanged"),
                    null,
                    DateTimeStyles.AssumeUniversal);
            }
        }

        /// <summary>
        /// Gets the name of the computer account.
        /// </summary>
        /// <value>The name of the computer account.</value>
        public string Name
        {
            get
            {
                return GetNodeValue("//DirectoryObject/name");
            }
        }

        /// <summary>
        /// Gets the operating system of the computer.
        /// </summary>
        /// <value>The operating system of the computer.</value>
        public string OperatingSystem
        {
            get
            {
                return GetNodeValue("//DirectoryObject/operatingsystem");
            }
        }

        /// <summary>
        /// Gets the operating system service pack of the computer.
        /// </summary>
        /// <value>The operating system service pack of the computer.</value>
        public string OperatingSystemServicePack
        {
            get
            {
                return GetNodeValue("//DirectoryObject/operatingsystemservicepack");
            }
        }

        /// <summary>
        /// Gets the operating system version of the computer.
        /// </summary>
        /// <value>The operating system version of the computer.</value>
        public string OperatingSystemVersion
        {
            get
            {
                return GetNodeValue("//DirectoryObject/operatingsystemversion");
            }
        }

        /// <summary>
        /// Gets the password last set date and time of the computer account.
        /// </summary>
        /// <value>The password last set date and time of the computer account.</value>
        public DateTime PasswordLastSet
        {
            get
            {
                string nodeValue = GetNodeValue("//DirectoryObject/pwdlastset");
                return DateTime.FromFileTimeUtc(Convert.ToInt64(nodeValue));
            }
        }

        /// <summary>
        /// Gets the primary group id of the computer account.
        /// </summary>
        /// <value>The primary group id of the computer account.</value>
        public int PrimaryGroupId
        {
            get
            {
                return Convert.ToInt32(GetNodeValue("//DirectoryObject/primarygroupid"));
            }
        }

        /// <summary>
        /// Gets the SAM Account name of the computer account.
        /// </summary>
        /// <value>The SAM Account name of the computer account.</value>
        public string SamAccountName
        {
            get
            {
                return GetNodeValue("//DirectoryObject/samaccountname");
            }
        }

        /// <summary>
        /// Gets a list of service principal names for this computer.
        /// </summary>
        /// <value>A list of service principal names.</value>
        public List<string> ServicePrincipalNames
        {
            get
            {
                return GetNodeListValues("//DirectoryObject/serviceprincipalname");
            }
        }

        /// <summary>
        /// Gets the Security ID of the computer account.
        /// </summary>
        /// <value>The Security ID of the computer account.</value>
        public string Sid
        {
            get
            {
                string nodeValue = GetNodeValue("//DirectoryObject/objectsid");
                return Security.Sid.ToString(HexEncoding.ToBytes(nodeValue));
            }
        }

        /// <summary>
        /// Gets the update sequence number (USN) of the computer account when it was created.
        /// </summary>
        /// <value>The update sequence number (USN) of the computer account when it was created.</value>
        public long UpdateSequenceNumberCreated
        {
            get
            {
                return Convert.ToInt64(GetNodeValue("//DirectoryObject/usncreated"));
            }
        }

        /// <summary>
        /// Gets the current update sequence number (USN) of the computer account.
        /// </summary>
        /// <value>The current update sequence number (USN) of the computer account.</value>
        public long UpdateSequenceNumberCurrent
        {
            get
            {
                return Convert.ToInt64(GetNodeValue("//DirectoryObject/usnchanged"));
            }
        }

        /// <summary>
        /// Disables this computer account in Active Directory.
        /// </summary>
        public void Disable()
        {
            using (var entry = ToDirectoryEntry())
            {
                var uac = (int)entry.Properties["userAccountControl"].Value;
                entry.Properties["userAccountControl"].Value = uac | (int)ADS_USER_FLAG.ACCOUNTDISABLE;
                entry.CommitChanges();

                // Now this object is "out-of-date", so reload.
                Initialize(DN.Parse(entry.Path));
            }
        }

        /// <summary>
        /// Determines whether this computer is a domain controller.
        /// </summary>
        /// <returns><c>true</c> if this computer is a domain controller; otherwise, <c>false</c>.</returns>
        public bool IsDomainController()
        {
            return Convert.ToBoolean(ComputerAccountControl & (int)ADS_USER_FLAG.SERVER_TRUST_ACCOUNT);
        }

        /// <summary>
        /// Determines whether this computer is a server as determined by the OS.
        /// </summary>
        /// <returns><c>true</c> if this computer is a server; otherwise, <c>false</c>.</returns>
        public bool IsServer()
        {
            if (IsDomainController())
            {
                return true;
            }

            // Samba servers do not report their OS, so it is customary to put the server in a
            // special OU.
            if (DistinguishedName.ToUpper(CultureInfo.InvariantCulture).Contains(",OU=SAMBA"))
            {
                return true;
            }

            if (OperatingSystem.ToLower(CultureInfo.InvariantCulture).Contains("server"))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Determines whether this computer is a workstation as determined by the OS.
        /// </summary>
        /// <returns><c>true</c> if this computer is a workstation; otherwise, <c>false</c>.</returns>
        public bool IsWorkstation()
        {
            return !IsServer();
        }

        /// <summary>
        /// Converts this object into a Domain Controller instance.
        /// </summary>
        /// <returns>A Domain Controller instance.</returns>
        public DomainController ToDomainController()
        {
            if (!IsDomainController())
            {
                throw new InvalidCastException("This object is not a Domain Controller!");
            }

            var context = new DirectoryContext(DirectoryContextType.DirectoryServer, DnsHostName);

            return DomainController.GetDomainController(context);
        }

        private void CheckType()
        {
            // Check to see if this is indeed a Computer object
            if (!GetObjectClass().Contains("computer"))
            {
                throw new ArgumentException("The directory object does not represent a computer!");
            }
        }
    }
}
