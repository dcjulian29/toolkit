using System;
using System.Collections.Generic;
using System.DirectoryServices;

namespace ToolKit.DirectoryServices
{
    /// <summary>
    /// This interface represents an generic user account.
    /// </summary>
    public interface IUser
    {
        /// <summary>
        /// Gets the date and time that this user account expires.
        /// </summary>
        /// <value>The date and time that this user account expires.</value>
        DateTime AccountExpires { get; }

        /// <summary>
        /// Gets the bad password count of the user.
        /// </summary>
        /// <value>The bad password count of the user.</value>
        int BadPasswordCount { get; }

        /// <summary>
        /// Gets the bad password time of the user.
        /// </summary>
        /// <value>The bad password time.</value>
        DateTime BadPasswordTime { get; }

        /// <summary>
        /// Gets the category of the user.
        /// </summary>
        /// <value>The category of the user.</value>
        string Category { get; }

        /// <summary>
        /// Gets the date and time that user account was last changed.
        /// </summary>
        /// <value>The date and time that the user account was last changed.</value>
        DateTime Changed { get; }

        /// <summary>
        /// Gets the city that the user account is in.
        /// </summary>
        /// <value>The city that the user account is in.</value>
        string City { get; }

        /// <summary>
        /// Gets the company that the user account belongs to
        /// </summary>
        /// <value>The company that the user account belongs to.</value>
        string Company { get; }

        /// <summary>
        /// Gets the country that the user account is in.
        /// </summary>
        /// <value>The country that the user account is in.</value>
        string Country { get; }

        /// <summary>
        /// Gets the country code for the user account
        /// </summary>
        /// <value>The country code for the user account.</value>
        int CountryCode { get; }

        /// <summary>
        /// Gets when the account was created.
        /// </summary>
        /// <value>When the account was created.</value>
        DateTime Created { get; }

        /// <summary>
        /// Gets the department of the user account.
        /// </summary>
        /// <value>The department of the user account.</value>
        string Department { get; }

        /// <summary>
        /// Gets the description of the user account.
        /// </summary>
        /// <value>The description of the user account.</value>
        string Description { get; }

        /// <summary>
        /// Gets the display name of the user account.
        /// </summary>
        /// <value>The display name of the user account.</value>
        string DisplayName { get; }

        /// <summary>
        /// Gets the the distinguished name of the user account.
        /// </summary>
        /// <value>The distinguished name of the user account.</value>
        string DistinguishedName { get; }

        /// <summary>
        /// Gets the email address of the user account.
        /// </summary>
        /// <value>The email address of the user account.</value>
        string EmailAddress { get; }

        /// <summary>
        /// Gets the fax number for the user account.
        /// </summary>
        /// <value>The fax number for the user account.</value>
        string Fax { get; }

        /// <summary>
        /// Gets the first name of the user account.
        /// </summary>
        /// <value>The first name of the user account.</value>
        string FirstName { get; }

        /// <summary>
        /// Gets the distinguished name list of groups.
        /// </summary>
        /// <value>The distinguished name list of groups.</value>
        List<string> Groups { get; }

        /// <summary>
        /// Gets the GUID of the user object.
        /// </summary>
        /// <value>The GUID of the user object.</value>
        Guid Guid { get; }

        /// <summary>
        /// Gets the home directory of the user account.
        /// </summary>
        /// <value>The home directory of the user account.</value>
        string HomeDirectory { get; }

        /// <summary>
        /// Gets the home drive of the user account.
        /// </summary>
        /// <value>The home drive of the user account.</value>
        string HomeDrive { get; }

        /// <summary>
        /// Gets the home phone of the user account.
        /// </summary>
        /// <value>The home phone of the user account.</value>
        string HomePhone { get; }

        /// <summary>
        /// Gets the IP phone of the user account.
        /// </summary>
        /// <value>The IP phone of the user account.</value>
        string IpPhone { get; }

        /// <summary>
        /// Gets the last logoff date and time of the user account.
        /// </summary>
        /// <value>The last logoff date and time of the user account.</value>
        DateTime LastLogoff { get; }

        /// <summary>
        /// Gets the last logon date and time of the user account.
        /// </summary>
        /// <value>The last logon date and time of the user account.</value>
        DateTime LastLogon { get; }

        /// <summary>
        /// Gets the last logon timestamp of the user account.
        /// </summary>
        /// <value>The last logon timestamp of the user account.</value>
        DateTime LastLogonTimestamp { get; }

        /// <summary>
        /// Gets the last name of the user account.
        /// </summary>
        /// <value>The last name of the user account.</value>
        string LastName { get; }

        /// <summary>
        /// Gets the logon count of the user account.
        /// </summary>
        /// <value>The logon count of the user account.</value>
        int LogonCount { get; }

        /// <summary>
        /// Gets the logon script of the user account.
        /// </summary>
        /// <value>The logon script of the user account.</value>
        string LogonScript { get; }

        /// <summary>
        /// Gets the manager's DistinguishedName for the user account.
        /// </summary>
        /// <value>The manager's DistinguishedName for the user account.</value>
        string Manager { get; }

        /// <summary>
        /// Gets the middle initial of the user account.
        /// </summary>
        /// <value>The middle initial of the user account.</value>
        string MiddleInitial { get; }

        /// <summary>
        /// Gets the mobile phone number of the user account.
        /// </summary>
        /// <value>The mobile phone number of the user account.</value>
        string MobilePhone { get; }

        /// <summary>
        /// Gets the modified date and time for the user account.
        /// </summary>
        /// <value>The modified date and time for the user account.</value>
        DateTime Modified { get; }

        /// <summary>
        /// Gets the name of the user account.
        /// </summary>
        /// <value>The name of the user account.</value>
        string Name { get; }

        /// <summary>
        /// Gets the notes of the user account.
        /// </summary>
        /// <value>The notes of the user account.</value>
        string Notes { get; }

        /// <summary>
        /// Gets the office of the user account.
        /// </summary>
        /// <value>The office of the user account.</value>
        string Office { get; }

        /// <summary>
        /// Gets the pager number of the user account.
        /// </summary>
        /// <value>The pager number of the user account.</value>
        string Pager { get; }

        /// <summary>
        /// Gets the password last set date and time of the user account.
        /// </summary>
        /// <value>The password last set date and time of the user account.</value>
        DateTime PasswordLastSet { get; }

        /// <summary>
        /// Gets the PO box of the user account.
        /// </summary>
        /// <value>The PO box of the user account.</value>
        string PoBox { get; }

        /// <summary>
        /// Gets the postal code of the user account.
        /// </summary>
        /// <value>The postal code of the user account.</value>
        string PostalCode { get; }

        /// <summary>
        /// Gets the primary group id of the user account.
        /// </summary>
        /// <value>The primary group id of the user account.</value>
        int PrimaryGroupId { get; }

        /// <summary>
        /// Gets the profile path of the user account.
        /// </summary>
        /// <value>The profile path of the user account.</value>
        string ProfilePath { get; }

        /// <summary>
        /// Gets the province of the user account.
        /// </summary>
        /// <value>The province of the user account.</value>
        string Province { get; }

        /// <summary>
        /// Gets the region of the user account.
        /// </summary>
        /// <value>The region of the user account.</value>
        string Region { get; }

        /// <summary>
        /// Gets the SAM Account name of the user account.
        /// </summary>
        /// <value>The SAM Account name of the user account.</value>
        string SamAccountName { get; }

        /// <summary>
        /// Gets the Security ID of the user account.
        /// </summary>
        /// <value>The Security ID of the user account.</value>
        string Sid { get; }

        /// <summary>
        /// Gets the state of the user account.
        /// </summary>
        /// <value>The state of the user account.</value>
        string State { get; }

        /// <summary>
        /// Gets the street address of the user account.
        /// </summary>
        /// <value>The street address of the user account.</value>
        string StreetAddress { get; }

        /// <summary>
        /// Gets the telephone number of the user account.
        /// </summary>
        /// <value>The telephone number of the user account.</value>
        string TelephoneNumber { get; }

        /// <summary>
        /// Gets the title of the user account.
        /// </summary>
        /// <value>The title of the user account.</value>
        string Title { get; }

        /// <summary>
        /// Gets the user account control value of the user account.
        /// </summary>
        /// <value>The user account control value of the user account.</value>
        int UserAccountControl { get; }

        /// <summary>
        /// Gets the user principal name of the user account.
        /// </summary>
        /// <value>The user principal name of the user account.</value>
        string UserPrincipalName { get; }

        /// <summary>
        /// Gets the web site of the user account.
        /// </summary>
        /// <value>The web site of the user account.</value>
        string WebSite { get; }

        /// <summary>
        /// Gets the zip code of the user account.
        /// </summary>
        /// <value>The zip code of the user account.</value>
        string Zip { get; }

        /// <summary>
        /// Determines whether this user is an administrative account.
        /// </summary>
        /// <returns><c>true</c> if this user is an administrative account; otherwise, <c>false</c>.</returns>
        bool IsAdministrativeAccount();

        /// <summary>
        /// Determines whether this user is an application account.
        /// </summary>
        /// <returns><c>true</c> if this user is an application account; otherwise, <c>false</c>.</returns>
        bool IsApplicationAccount();

        /// <summary>
        /// Determines whether this user is a generic account. Generic accounts are used to hold a
        /// mailbox which is assigned to other user object during the generic accounts lifespan.
        /// </summary>
        /// <returns><c>true</c> if this user is an generic account; otherwise, <c>false</c>.</returns>
        bool IsGenericAccount();

        /// <summary>
        /// Determines whether this user is an regular account.
        /// </summary>
        /// <returns><c>true</c> if this user is an regular account; otherwise, <c>false</c>.</returns>
        bool IsRegularAccount();

        /// <summary>
        /// Determines whether this user is an service account.
        /// </summary>
        /// <returns><c>true</c> if this user is an service account; otherwise, <c>false</c>.</returns>
        bool IsServiceAccount();

        /// <summary>
        /// Converts this User instance to a DirectoryEntry.
        /// </summary>
        /// <returns>This user as a DirectoryEntry</returns>
        DirectoryEntry ToDirectoryEntry();
    }
}
