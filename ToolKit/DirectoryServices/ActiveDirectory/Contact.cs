using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.Globalization;
using Common.Logging;
using Toolkit;

namespace ToolKit.DirectoryServices.ActiveDirectory
{
    /// <summary>
    /// This class represents an Active Directory Contact
    /// </summary>
    public class Contact : DirectoryObject
    {
        private static ILog _log = LogManager.GetLogger<Contact>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Contact"/> class.
        /// </summary>
        /// <param name="distinguishedName">DistinguishedName object of the distinguished name.</param>
        public Contact(DistinguishedName distinguishedName)
            : base(distinguishedName)
        {
            CheckType();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Contact"/> class.
        /// </summary>
        /// <param name="result">A SearchResult Object.</param>
        public Contact(SearchResult result)
            : base(result)
        {
            CheckType();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Contact"/> class.
        /// </summary>
        /// <param name="entry">A DirectoryEntry Object.</param>
        public Contact(DirectoryEntry entry)
            : base(entry)
        {
            CheckType();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Contact"/> class.
        /// </summary>
        /// <param name="domain">The domain of the contact.</param>
        /// <param name="contactName">The Name attribute of the contact.</param>
        public Contact(string domain, string contactName)
        {
            var context = new DirectoryContext(DirectoryContextType.Domain, domain);

            var domainObject = Domain.GetDomain(context);

            using (var search = new DirectorySearcher())
            {
                search.SearchRoot = domainObject.GetDirectoryEntry();
                search.Filter = "(name=" + contactName + ")";

                var result = search.FindOne();

                if (result == null)
                {
                    throw new ArgumentException("Contact does not exist in this domain!");
                }

                Initialize(DirectoryServices.DistinguishedName.Parse(result.Path));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Contact"/> class.
        /// </summary>
        /// <param name="distinguishedName">string representation of the distinguished name.</param>
        public Contact(string distinguishedName)
            : base(distinguishedName)
        {
            CheckType();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Contact"/> class.
        /// </summary>
        /// <param name="properties">A Dictionary of properties.</param>
        /// <remarks>This constructor is primarily used for unit tests.</remarks>
        internal Contact(Dictionary<string, object> properties)
            : base(properties)
        {
            CheckType();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Contact"></see> class. This constructor is
        /// not public and should only be used when the derived class wants to add an additional
        /// constructor. At some point though, the derived classes' constructor needs to call the
        /// "Initialize" method to load up the internal data structures.
        /// </summary>
        protected Contact()
        {
        }

        /// <summary>
        /// Gets the category of the contact.
        /// </summary>
        /// <value>The category of the contact.</value>
        public string Category
        {
            get
            {
                return GetNodeValue("//DirectoryObject/objectcategory");
            }
        }

        /// <summary>
        /// Gets the date and time that contact was last changed.
        /// </summary>
        /// <value>The date and time that the contact was last changed.</value>
        public DateTime Changed
        {
            get
            {
                return Modified;
            }
        }

        /// <summary>
        /// Gets the city that the contact is in.
        /// </summary>
        /// <value>The city that the contact is in.</value>
        public string City
        {
            get
            {
                return GetNodeValue("//DirectoryObject/l");
            }
        }

        /// <summary>
        /// Gets the common name of the contact.
        /// </summary>
        /// <value>The common name of the contact.</value>
        public string CommonName
        {
            get
            {
                return GetNodeValue("//DirectoryObject/cn");
            }
        }

        /// <summary>
        /// Gets the company that the contact belongs to
        /// </summary>
        /// <value>The company that the contact belongs to.</value>
        public string Company
        {
            get
            {
                return GetNodeValue("//DirectoryObject/company");
            }
        }

        /// <summary>
        /// Gets the country that the contact is in.
        /// </summary>
        /// <value>The country that the contact is in.</value>
        public string Country
        {
            get
            {
                return GetNodeValue("//DirectoryObject/co");
            }
        }

        /// <summary>
        /// Gets the country code for the contact
        /// </summary>
        /// <value>The country code for the contact.</value>
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
                    DateTimeStyles.AdjustToUniversal);
            }
        }

        /// <summary>
        /// Gets the department of the contact.
        /// </summary>
        /// <value>The department of the contact.</value>
        public string Department
        {
            get
            {
                return GetNodeValue("//DirectoryObject/department");
            }
        }

        /// <summary>
        /// Gets the description of the contact.
        /// </summary>
        /// <value>The description of the contact.</value>
        public string Description
        {
            get
            {
                return GetNodeValue("//DirectoryObject/description");
            }
        }

        /// <summary>
        /// Gets the display name of the contact.
        /// </summary>
        /// <value>The display name of the contact.</value>
        public string DisplayName
        {
            get
            {
                return GetNodeValue("//DirectoryObject/displayname");
            }
        }

        /// <summary>
        /// Gets the the distinguished name of the contact.
        /// </summary>
        /// <value>The distinguished name of the contact.</value>
        public string DistinguishedName
        {
            get
            {
                return GetNodeValue("//DirectoryObject/distinguishedname");
            }
        }

        /// <summary>
        /// Gets the email address of the contact.
        /// </summary>
        /// <value>The email address of the contact.</value>
        public string EmailAddress
        {
            get
            {
                return GetNodeValue("//DirectoryObject/mail");
            }
        }

        /// <summary>
        /// Gets the fax number for the contact.
        /// </summary>
        /// <value>The fax number for the contact.</value>
        public string Fax
        {
            get
            {
                return GetNodeValue("//DirectoryObject/facsimiletelephonenumber");
            }
        }

        /// <summary>
        /// Gets the first name of the contact.
        /// </summary>
        /// <value>The first name of the contact.</value>
        public string FirstName
        {
            get
            {
                return GetNodeValue("//DirectoryObject/givenname");
            }
        }

        /// <summary>
        /// Gets the GUID of the contact object.
        /// </summary>
        /// <value>The GUID of the contact object.</value>
        public Guid Guid
        {
            get
            {
                return new Guid(HexEncoding.ToBytes(GetNodeValue("//DirectoryObject/objectguid")));
            } // get
        }

        /// <summary>
        /// Gets the home phone of the contact.
        /// </summary>
        /// <value>The home phone of the contact.</value>
        public string HomePhone
        {
            get
            {
                return GetNodeValue("//DirectoryObject/homephone");
            }
        }

        /// <summary>
        /// Gets the IP phone of the contact.
        /// </summary>
        /// <value>The IP phone of the contact.</value>
        public string IpPhone
        {
            get
            {
                return GetNodeValue("//DirectoryObject/ipphone");
            }
        }

        /// <summary>
        /// Gets the last name of the contact.
        /// </summary>
        /// <value>The last name of the contact.</value>
        public string LastName
        {
            get
            {
                return GetNodeValue("//DirectoryObject/sn");
            }
        }

        /// <summary>
        /// Gets the manager's DistinguishedName for the contact.
        /// </summary>
        /// <value>The manager's DistinguishedName for the contact.</value>
        public string Manager
        {
            get
            {
                return GetNodeValue("//DirectoryObject/manager");
            }
        }

        /// <summary>
        /// Gets the middle initial of the contact.
        /// </summary>
        /// <value>The middle initial of the contact.</value>
        public string MiddleInitial
        {
            get
            {
                return GetNodeValue("//DirectoryObject/initials");
            }
        }

        /// <summary>
        /// Gets the mobile phone number of the contact.
        /// </summary>
        /// <value>The mobile phone number of the contact.</value>
        public string MobilePhone
        {
            get
            {
                return GetNodeValue("//DirectoryObject/mobile");
            }
        }

        /// <summary>
        /// Gets the modified date and time for the contact.
        /// </summary>
        /// <value>The modified date and time for the contact.</value>
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
        /// Gets the name of the contact.
        /// </summary>
        /// <value>The name of the contact.</value>
        public string Name
        {
            get
            {
                return GetNodeValue("//DirectoryObject/name");
            }
        }

        /// <summary>
        /// Gets the notes of the contact.
        /// </summary>
        /// <value>The notes of the contact.</value>
        public string Notes
        {
            get
            {
                return GetNodeValue("//DirectoryObject/info");
            }
        }

        /// <summary>
        /// Gets the office of the contact.
        /// </summary>
        /// <value>The office of the contact.</value>
        public string Office
        {
            get
            {
                return GetNodeValue("//DirectoryObject/physicaldeliveryofficename");
            }
        }

        /// <summary>
        /// Gets the pager number of the contact.
        /// </summary>
        /// <value>The pager number of the contact.</value>
        public string Pager
        {
            get
            {
                return GetNodeValue("//DirectoryObject/pager");
            }
        }

        /// <summary>
        /// Gets the PO box of the contact.
        /// </summary>
        /// <value>The PO box of the contact.</value>
        public string PoBox
        {
            get
            {
                return GetNodeValue("//DirectoryObject/postofficebox");
            }
        }

        /// <summary>
        /// Gets the postal code of the contact.
        /// </summary>
        /// <value>The postal code of the contact.</value>
        public string PostalCode
        {
            get
            {
                return GetNodeValue("//DirectoryObject/postalcode");
            }
        }

        /// <summary>
        /// Gets the province of the contact.
        /// </summary>
        /// <value>The province of the contact.</value>
        public string Province
        {
            get
            {
                return State;
            }
        }

        /// <summary>
        /// Gets the region of the contact.
        /// </summary>
        /// <value>The region of the contact.</value>
        public string Region
        {
            get
            {
                return Country;
            }
        }

        /// <summary>
        /// Gets the state of the contact.
        /// </summary>
        /// <value>The state of the contact.</value>
        public string State
        {
            get
            {
                return GetNodeValue("//DirectoryObject/st");
            }
        }

        /// <summary>
        /// Gets the street address of the contact.
        /// </summary>
        /// <value>The street address of the contact.</value>
        public string StreetAddress
        {
            get
            {
                return GetNodeValue("//DirectoryObject/streetaddress");
            }
        }

        /// <summary>
        /// Gets the telephone number of the contact.
        /// </summary>
        /// <value>The telephone number of the contact.</value>
        public string TelephoneNumber
        {
            get
            {
                return GetNodeValue("//DirectoryObject/telephonenumber");
            }
        }

        /// <summary>
        /// Gets the title of the contact.
        /// </summary>
        /// <value>The title of the contact.</value>
        public string Title
        {
            get
            {
                return GetNodeValue("//DirectoryObject/title");
            }
        }

        /// <summary>
        /// Gets the update sequence number (USN) of the contact when it was created.
        /// </summary>
        /// <value>The update sequence number (USN) of the contact when it was created.</value>
        public long UpdateSequenceNumberCreated
        {
            get
            {
                return Convert.ToInt64(GetNodeValue("//DirectoryObject/usncreated"));
            }
        }

        /// <summary>
        /// Gets the current update sequence number (USN) of the contact.
        /// </summary>
        /// <value>The current update sequence number (USN) of the contact.</value>
        public long UpdateSequenceNumberCurrent
        {
            get
            {
                return Convert.ToInt64(GetNodeValue("//DirectoryObject/usnchanged"));
            }
        }

        /// <summary>
        /// Gets the web site of the contact.
        /// </summary>
        /// <value>The web site of the contact.</value>
        public string WebSite
        {
            get
            {
                return GetNodeValue("//DirectoryObject/wwwhomepage");
            }
        }

        /// <summary>
        /// Gets the zip code of the contact.
        /// </summary>
        /// <value>The zip code of the contact.</value>
        public string Zip
        {
            get
            {
                return PostalCode;
            }
        }

        private void CheckType()
        {
            // Check to see if this is indeed a Contact object
            if (!GetObjectClass().Contains("contact"))
            {
                throw new ArgumentException("The directory object does not represent a contact!");
            }
        }
    }
}
