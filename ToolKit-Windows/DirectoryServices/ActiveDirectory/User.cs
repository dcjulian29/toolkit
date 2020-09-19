using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.Globalization;
using Common.Logging;
using ToolKit.DirectoryServices.ServiceInterfaces;

namespace ToolKit.DirectoryServices.ActiveDirectory
{
    /// <summary>
    /// This class represents an ActiveDirectory User.
    /// </summary>
    public class User : DirectoryObject, IUser
    {
        private static ILog _log = LogManager.GetLogger<User>();

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        /// <param name="distinguishedName">string representation of the distinguished name.</param>
        public User(string distinguishedName)
            : base(distinguishedName)
        {
            CheckType();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        /// <param name="distinguishedName">DistinguishedName object of the distinguished name.</param>
        public User(DistinguishedName distinguishedName)
            : base(distinguishedName)
        {
            CheckType();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        /// <param name="result">A SearchResult Object.</param>
        public User(SearchResult result)
            : base(result)
        {
            CheckType();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        /// <param name="entry">A DirectoryEntry Object.</param>
        public User(DirectoryEntry entry)
            : base(entry)
        {
            CheckType();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        /// <param name="domain">The domain of the user.</param>
        /// <param name="userName">SAM Account Name of the user.</param>
        public User(string domain, string userName)
        {
            var context = new DirectoryContext(DirectoryContextType.Domain, domain);

            var domainObject = Domain.GetDomain(context);

            using (var search = new DirectorySearcher())
            {
                search.SearchRoot = domainObject.GetDirectoryEntry();
                search.Filter = "(samAccountName=" + userName + ")";

                var result = search.FindOne();

                if (result == null)
                {
                    throw new ArgumentException("User does not exist in this domain!");
                }

                Initialize(DirectoryServices.DistinguishedName.Parse(result.Path));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        /// <param name="properties">A Dictionary of properties.</param>
        /// <remarks>This constructor is primarily used for unit tests.</remarks>
        internal User(Dictionary<string, object> properties)
            : base(properties)
        {
            CheckType();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        protected User()
        {
        }

        /// <summary>
        /// Gets the date and time that this user account expires.
        /// </summary>
        /// <value>The date and time that this user account expires.</value>
        public DateTime AccountExpires
        {
            get
            {
                try
                {
                    var expiration = GetNodeValue("//DirectoryObject/accountexpires");
                    return string.IsNullOrEmpty(expiration)
                        ? DateTime.MaxValue
                        : DateTime.FromFileTimeUtc(Convert.ToInt64(expiration, CultureInfo.InvariantCulture));
                }
                catch (ArgumentOutOfRangeException)
                {
                    // Means that the account never expires.
                    return DateTime.MaxValue;
                }
            }
        }

        /// <summary>
        /// Gets the bad password count of the user.
        /// </summary>
        /// <value>The bad password count of the user.</value>
        public int BadPasswordCount
        {
            get
            {
                return Convert.ToInt32(GetNodeValue("//DirectoryObject/badpwdcount"), CultureInfo.CurrentCulture);
            }
        }

        /// <summary>
        /// Gets the bad password time of the user.
        /// </summary>
        /// <value>The bad password time.</value>
        public DateTime BadPasswordTime
        {
            get
            {
                var nodeValue = GetNodeValue("//DirectoryObject/badpasswordtime");
                return DateTime.FromFileTimeUtc(Convert.ToInt64(nodeValue, CultureInfo.InvariantCulture));
            }
        }

        /// <summary>
        /// Gets the category of the user.
        /// </summary>
        /// <value>The category of the user.</value>
        public string Category
        {
            get
            {
                return GetNodeValue("//DirectoryObject/objectcategory");
            }
        }

        /// <summary>
        /// Gets the date and time that user account was last changed.
        /// </summary>
        /// <value>The date and time that the user account was last changed.</value>
        public DateTime Changed
        {
            get
            {
                return Modified;
            }
        }

        /// <summary>
        /// Gets the city that the user account is in.
        /// </summary>
        /// <value>The city that the user account is in.</value>
        public string City
        {
            get
            {
                return GetNodeValue("//DirectoryObject/l");
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
        /// Gets the company that the user account belongs to.
        /// </summary>
        /// <value>The company that the user account belongs to.</value>
        public string Company
        {
            get
            {
                return GetNodeValue("//DirectoryObject/company");
            }
        }

        /// <summary>
        /// Gets the country that the user account is in.
        /// </summary>
        /// <value>The country that the user account is in.</value>
        public string Country
        {
            get
            {
                return GetNodeValue("//DirectoryObject/c");
            }
        }

        /// <summary>
        /// Gets the country code for the user account.
        /// </summary>
        /// <value>The country code for the user account.</value>
        public int CountryCode
        {
            get
            {
                return Convert.ToInt32(GetNodeValue("//DirectoryObject/countrycode"), CultureInfo.CurrentCulture);
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
        /// Gets the department of the user account.
        /// </summary>
        /// <value>The department of the user account.</value>
        public string Department
        {
            get
            {
                return GetNodeValue("//DirectoryObject/department");
            }
        }

        /// <summary>
        /// Gets the description of the user account.
        /// </summary>
        /// <value>The description of the user account.</value>
        public string Description
        {
            get
            {
                return GetNodeValue("//DirectoryObject/description");
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="User"/> is disabled.
        /// </summary>
        /// <value><c>true</c> if disabled; otherwise, <c>false</c>.</value>
        public bool Disabled
        {
            get
            {
                return Convert.ToBoolean(UserAccountControl
                                         & (int)ADS_USER_FLAG.ACCOUNTDISABLE);
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
        /// Gets the distinguished name of the user account.
        /// </summary>
        /// <value>The distinguished name of the user account.</value>
        public string DistinguishedName
        {
            get
            {
                return GetNodeValue("//DirectoryObject/distinguishedname");
            }
        }

        /// <summary>
        /// Gets the email address of the user account.
        /// </summary>
        /// <value>The email address of the user account.</value>
        public string EmailAddress
        {
            get
            {
                return GetNodeValue("//DirectoryObject/mail");
            }
        }

        /// <summary>
        /// Gets the fax number for the user account.
        /// </summary>
        /// <value>The fax number for the user account.</value>
        public string Fax
        {
            get
            {
                return GetNodeValue("//DirectoryObject/facsimiletelephonenumber");
            }
        }

        /// <summary>
        /// Gets the first name of the user account.
        /// </summary>
        /// <value>The first name of the user account.</value>
        public string FirstName
        {
            get
            {
                return GetNodeValue("//DirectoryObject/givenname");
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
        /// Gets the GUID of the user object.
        /// </summary>
        /// <value>The GUID of the user object.</value>
        [SuppressMessage(
            "Naming",
            "CA1720:Identifier contains type name",
            Justification = "Too Bad. It is what it is.")]
        public Guid Guid
        {
            get
            {
                return new Guid(HexEncoding.ToBytes(GetNodeValue("//DirectoryObject/objectguid")));
            }
        }

        /// <summary>
        /// Gets the home directory of the user account.
        /// </summary>
        /// <value>The home directory of the user account.</value>
        public string HomeDirectory
        {
            get
            {
                return GetNodeValue("//DirectoryObject/homedirectory");
            }
        }

        /// <summary>
        /// Gets the home drive of the user account.
        /// </summary>
        /// <value>The home drive of the user account.</value>
        public string HomeDrive
        {
            get
            {
                return GetNodeValue("//DirectoryObject/homedrive");
            }
        }

        /// <summary>
        /// Gets the home phone of the user account.
        /// </summary>
        /// <value>The home phone of the user account.</value>
        public string HomePhone
        {
            get
            {
                return GetNodeValue("//DirectoryObject/homephone");
            }
        }

        /// <summary>
        /// Gets the IP phone of the user account.
        /// </summary>
        /// <value>The IP phone of the user account.</value>
        public string IpPhone
        {
            get
            {
                return GetNodeValue("//DirectoryObject/ipphone");
            }
        }

        /// <summary>
        /// Gets the last logoff date and time of the user account.
        /// </summary>
        /// <value>The last logoff date and time of the user account.</value>
        public DateTime LastLogoff
        {
            get
            {
                var nodeValue = GetNodeValue("//DirectoryObject/lastlogoff");
                return DateTime.FromFileTimeUtc(Convert.ToInt64(nodeValue, CultureInfo.InvariantCulture));
            }
        }

        /// <summary>
        /// Gets the last logon date and time of the user account.
        /// </summary>
        /// <value>The last logon date and time of the user account.</value>
        public DateTime LastLogon
        {
            get
            {
                var nodeValue = GetNodeValue("//DirectoryObject/lastlogon");
                return DateTime.FromFileTimeUtc(Convert.ToInt64(nodeValue, CultureInfo.InvariantCulture));
            }
        }

        /// <summary>
        /// Gets the last logon timestamp of the user account.
        /// </summary>
        /// <value>The last logon timestamp of the user account.</value>
        public DateTime LastLogonTimestamp
        {
            get
            {
                var nodeValue = GetNodeValue("//DirectoryObject/lastlogontimestamp");
                return DateTime.FromFileTimeUtc(Convert.ToInt64(nodeValue, CultureInfo.InvariantCulture));
            }
        }

        /// <summary>
        /// Gets the last name of the user account.
        /// </summary>
        /// <value>The last name of the user account.</value>
        public string LastName
        {
            get
            {
                return GetNodeValue("//DirectoryObject/sn");
            }
        }

        /// <summary>
        /// Gets the logon count of the user account.
        /// </summary>
        /// <value>The logon count of the user account.</value>
        public int LogonCount
        {
            get
            {
                return Convert.ToInt32(GetNodeValue("//DirectoryObject/logoncount"), CultureInfo.CurrentCulture);
            }
        }

        /// <summary>
        /// Gets the logon script of the user account.
        /// </summary>
        /// <value>The logon script of the user account.</value>
        public string LogonScript
        {
            get
            {
                return GetNodeValue("//DirectoryObject/scriptpath");
            }
        }

        /// <summary>
        /// Gets the manager's DistinguishedName for the user account.
        /// </summary>
        /// <value>The manager's DistinguishedName for the user account.</value>
        public string Manager
        {
            get
            {
                return GetNodeValue("//DirectoryObject/manager");
            }
        }

        /// <summary>
        /// Gets the middle initial of the user account.
        /// </summary>
        /// <value>The middle initial of the user account.</value>
        public string MiddleInitial
        {
            get
            {
                return GetNodeValue("//DirectoryObject/initials");
            }
        }

        /// <summary>
        /// Gets the mobile phone number of the user account.
        /// </summary>
        /// <value>The mobile phone number of the user account.</value>
        public string MobilePhone
        {
            get
            {
                return GetNodeValue("//DirectoryObject/mobile");
            }
        }

        /// <summary>
        /// Gets the modified date and time for the user account.
        /// </summary>
        /// <value>The modified date and time for the user account.</value>
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
        /// Gets the name of the user account.
        /// </summary>
        /// <value>The name of the user account.</value>
        public string Name
        {
            get
            {
                return GetNodeValue("//DirectoryObject/name");
            }
        }

        /// <summary>
        /// Gets the notes of the user account.
        /// </summary>
        /// <value>The notes of the user account.</value>
        public string Notes
        {
            get
            {
                return GetNodeValue("//DirectoryObject/info");
            }
        }

        /// <summary>
        /// Gets the office of the user account.
        /// </summary>
        /// <value>The office of the user account.</value>
        public string Office
        {
            get
            {
                return GetNodeValue("//DirectoryObject/physicaldeliveryofficename");
            }
        }

        /// <summary>
        /// Gets the pager number of the user account.
        /// </summary>
        /// <value>The pager number of the user account.</value>
        public string Pager
        {
            get
            {
                return GetNodeValue("//DirectoryObject/pager");
            }
        }

        /// <summary>
        /// Gets the password last set date and time of the user account.
        /// </summary>
        /// <value>The password last set date and time of the user account.</value>
        public DateTime PasswordLastSet
        {
            get
            {
                var nodeValue = GetNodeValue("//DirectoryObject/pwdlastset");
                return DateTime.FromFileTimeUtc(Convert.ToInt64(nodeValue, CultureInfo.InvariantCulture));
            }
        }

        /// <summary>
        /// Gets the PO box of the user account.
        /// </summary>
        /// <value>The PO box of the user account.</value>
        public string PoBox
        {
            get
            {
                return GetNodeValue("//DirectoryObject/postofficebox");
            }
        }

        /// <summary>
        /// Gets the postal code of the user account.
        /// </summary>
        /// <value>The postal code of the user account.</value>
        public string PostalCode
        {
            get
            {
                return GetNodeValue("//DirectoryObject/postalcode");
            }
        }

        /// <summary>
        /// Gets the primary group id of the user account.
        /// </summary>
        /// <value>The primary group id of the user account.</value>
        public int PrimaryGroupId
        {
            get
            {
                return Convert.ToInt32(GetNodeValue("//DirectoryObject/primarygroupid"), CultureInfo.CurrentCulture);
            }
        }

        /// <summary>
        /// Gets the profile path of the user account.
        /// </summary>
        /// <value>The profile path of the user account.</value>
        public string ProfilePath
        {
            get
            {
                return GetNodeValue("//DirectoryObject/profilepath");
            }
        }

        /// <summary>
        /// Gets the province of the user account.
        /// </summary>
        /// <value>The province of the user account.</value>
        public string Province
        {
            get
            {
                return State;
            }
        }

        /// <summary>
        /// Gets the region of the user account.
        /// </summary>
        /// <value>The region of the user account.</value>
        public string Region
        {
            get
            {
                return Country;
            }
        }

        /// <summary>
        /// Gets the SAM Account name of the user account.
        /// </summary>
        /// <value>The SAM Account name of the user account.</value>
        public string SamAccountName
        {
            get
            {
                return GetNodeValue("//DirectoryObject/samaccountname");
            }
        }

        /// <summary>
        /// Gets the Security ID of the user account.
        /// </summary>
        /// <value>The Security ID of the user account.</value>
        public string Sid
        {
            get
            {
                var nodeValue = GetNodeValue("//DirectoryObject/objectsid");
                return Security.Sid.ToString(HexEncoding.ToBytes(nodeValue));
            }
        }

        /// <summary>
        /// Gets the state of the user account.
        /// </summary>
        /// <value>The state of the user account.</value>
        public string State
        {
            get
            {
                return GetNodeValue("//DirectoryObject/st");
            }
        }

        /// <summary>
        /// Gets the street address of the user account.
        /// </summary>
        /// <value>The street address of the user account.</value>
        public string StreetAddress
        {
            get
            {
                return GetNodeValue("//DirectoryObject/streetaddress");
            }
        }

        /// <summary>
        /// Gets the telephone number of the user account.
        /// </summary>
        /// <value>The telephone number of the user account.</value>
        public string TelephoneNumber
        {
            get
            {
                return GetNodeValue("//DirectoryObject/telephonenumber");
            }
        }

        /// <summary>
        /// Gets the title of the user account.
        /// </summary>
        /// <value>The title of the user account.</value>
        public string Title
        {
            get
            {
                return GetNodeValue("//DirectoryObject/title");
            }
        }

        /// <summary>
        /// Gets the update sequence number (USN) of the user account when it was created.
        /// </summary>
        /// <value>The update sequence number (USN) of the user account when it was created.</value>
        public long UpdateSequenceNumberCreated
        {
            get
            {
                return Convert.ToInt64(GetNodeValue("//DirectoryObject/usncreated"), CultureInfo.CurrentCulture);
            }
        }

        /// <summary>
        /// Gets the current update sequence number (USN) of the user account.
        /// </summary>
        /// <value>The current update sequence number (USN) of the user account.</value>
        public long UpdateSequenceNumberCurrent
        {
            get
            {
                return Convert.ToInt64(GetNodeValue("//DirectoryObject/usnchanged"), CultureInfo.CurrentCulture);
            }
        }

        /// <summary>
        /// Gets the user account control value of the user account.
        /// </summary>
        /// <value>The user account control value of the user account.</value>
        public int UserAccountControl
        {
            get
            {
                return Convert.ToInt32(GetNodeValue("//DirectoryObject/useraccountcontrol"), CultureInfo.CurrentCulture);
            }
        }

        /// <summary>
        /// Gets the user principal name of the user account.
        /// </summary>
        /// <value>The user principal name of the user account.</value>
        public string UserPrincipalName
        {
            get
            {
                return GetNodeValue("//DirectoryObject/userprincipalname");
            }
        }

        /// <summary>
        /// Gets the web site of the user account.
        /// </summary>
        /// <value>The web site of the user account.</value>
        public string WebSite
        {
            get
            {
                return GetNodeValue("//DirectoryObject/wwwhomepage");
            }
        }

        /// <summary>
        /// Gets the zip code of the user account.
        /// </summary>
        /// <value>The zip code of the user account.</value>
        public string Zip
        {
            get
            {
                return PostalCode;
            }
        }

        /// <summary>
        /// Disables this account in Active Directory.
        /// </summary>
        public void Disable()
        {
            using (var de = ToDirectoryEntry())
            {
                try
                {
                    var uac = (int)de.Properties["userAccountControl"].Value;

                    de.Properties["userAccountControl"].Value = uac | (int)ADS_USER_FLAG.ACCOUNTDISABLE;

                    de.CommitChanges();
                }
                catch (DirectoryServicesCOMException ex)
                {
                    _log.Error($"Unable to Disable Account! {DistinguishedName}", ex);
                    throw;
                }
                catch (UnauthorizedAccessException uaex)
                {
                    _log.Error($"Access Denied trying to disable {DistinguishedName}", uaex);
                    throw;
                }
            }
        }

        /// <summary>
        /// Determines whether this user is an administrative account.
        /// </summary>
        /// <returns><c>true</c> if this user is an administrative account; otherwise, <c>false</c>.</returns>
        public bool IsAdministrativeAccount()
        {
            return SamAccountName.EndsWith("-ADM", StringComparison.InvariantCultureIgnoreCase)
                || DistinguishedName.Contains(",OU=Administrative Accounts,");
        }

        /// <summary>
        /// Determines whether this user is an application account.
        /// </summary>
        /// <returns><c>true</c> if this user is an application account; otherwise, <c>false</c>.</returns>
        public bool IsApplicationAccount()
        {
            return SamAccountName.EndsWith("-APP", StringComparison.InvariantCultureIgnoreCase)
                || DistinguishedName.Contains(",OU=Application Accounts,");
        }

        /// <summary>
        /// Determines whether this user is an regular account.
        /// </summary>
        /// <returns><c>true</c> if this user is an regular account; otherwise, <c>false</c>.</returns>
        public bool IsRegularAccount()
        {
            return !(IsAdministrativeAccount() || IsApplicationAccount() || IsServiceAccount());
        }

        /// <summary>
        /// Determines whether this user is an service account.
        /// </summary>
        /// <returns><c>true</c> if this user is an service account; otherwise, <c>false</c>.</returns>
        public bool IsServiceAccount()
        {
            return SamAccountName.EndsWith("-SVC", StringComparison.InvariantCultureIgnoreCase)
                || DistinguishedName.Contains(",OU=Service Accounts,");
        }

        private void CheckType()
        {
            // Check to see if this is indeed a User object Computer objects also have "user" in
            // it's objectClass attribute...
            if ((!GetObjectClass().Contains("user")) || GetObjectClass().Contains("computer"))
            {
                throw new ArgumentException("The directory object does not represent a user!");
            }
        }
    }
}
