using System;
using System.Diagnostics.CodeAnalysis;
using Common.Logging;
using ToolKit.DirectoryServices.ServiceInterfaces;

namespace ToolKit.DirectoryServices.ActiveDirectory
{
    /// <summary>
    /// This class provide Common LDAP Queries for Active Directory. It is based on a document that
    /// was created by a previous co-worker.
    /// </summary>
    [SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1650:ElementDocumentationMustBeSpelledCorrectly",
        Justification = "This class contains acronyms or abbreviations.")]
    [SuppressMessage("ReSharper", "StringLiteralTypo", Justification = "Contains Many Acroymns")]
    [SuppressMessage("ReSharper", "IdentifierTypo", Justification = "Contains Many Acroymns")]
    [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Contains Many Acroymns")]
    public static class ActiveDirectoryCommonFilters
    {
        /// <summary>
        /// Gets an LdapFilter that represents administrative users.
        /// </summary>
        /// <value>LdapFilter that represents users.</value>
        public static LdapFilter AdministrativeUsers => new LdapFilter("sAMAccountName=*-adm").And(Users);

        /// <summary>
        /// Gets an LdapFilter that represents computers.
        /// </summary>
        /// <value>LdapFilter that represents computers.</value>
        public static LdapFilter Computers => new LdapFilter("objectCategory=computer");

        /// <summary>
        /// Gets an LdapFilter that represents contacts.
        /// </summary>
        /// <value>LdapFilter that represents contacts.</value>
        public static LdapFilter Contacts => new LdapFilter("objectClass=contact").And("objectCategory=person");

        /// <summary>
        /// Gets an LdapFilter that represents domain controllers.
        /// </summary>
        /// <value>LdapFilter that represents domain controllers.</value>
        public static LdapFilter DomainControllers => UserAccessControl(ADS_USER_FLAG.SERVER_TRUST_ACCOUNT).And(Computers);

        /// <summary>
        /// Gets an LdapFilter that represents domain local groups.
        /// </summary>
        /// <value>LdapFilter that represents domain local groups.</value>
        public static LdapFilter DomainLocalGroups => Groups.And("groupType:1.2.840.113556.1.4.803:=2147483652", true);

        /// <summary>
        /// Gets an LdapFilter that represents empty groups.
        /// </summary>
        /// <value>LdapFilter that represents empty groups.</value>
        public static LdapFilter EmptyGroups => Groups.And(new LdapFilter("member=*").Not(), true);

        /// <summary>
        /// Gets an LdapFilter that represents global groups.
        /// </summary>
        /// <value>LdapFilter that represents global groups.</value>
        public static LdapFilter GlobalGroups => Groups.And("groupType:1.2.840.113556.1.4.803:=2147483650", true);

        /// <summary>
        /// Gets an LdapFilter that represents groups.
        /// </summary>
        /// <value>LdapFilter that represents groups.</value>
        public static LdapFilter Groups => new LdapFilter("objectCategory=group");

        /// <summary>
        /// Gets an LdapFilter that represents groups with email addresses.
        /// </summary>
        /// <value>LdapFilter that represents groups with email addresses.</value>
        public static LdapFilter GroupsWithEmail => Groups.And("mail=*", true);

        /// <summary>
        /// Gets an LdapFilter that represents normal users.
        /// </summary>
        /// <value>LdapFilter that represents users.</value>
        public static LdapFilter NormalAccounts =>
            LdapFilter.And(
                ActiveDirectoryCommonFilters.Users,
                new LdapFilter("sAMAccountName", LdapFilter.Equal, "*-adm").Not(),
                new LdapFilter("sAMAccountName", LdapFilter.Equal, "*svc").Not());

        /// <summary>
        /// Gets a sAMAccountType LdapFilter object that represents a certain type of object in
        /// Active Directory. <seealso href="http://msdn.microsoft.com/en-us/library/cc228417(PROT.13).aspx"/>
        /// </summary>
        /// <value>sAMAccountType value describing an account type object.</value>
        public static LdapFilter SAM_ALIAS_OBJECT => new LdapFilter("sAMAcountType=536870912");

        /// <summary>
        /// Gets a sAMAccountType LdapFilter object that represents a certain type of object in
        /// Active Directory. <seealso href="http://msdn.microsoft.com/en-us/library/cc228417(PROT.13).aspx"/>
        /// </summary>
        /// <value>sAMAccountType value describing an account type object.</value>
        public static LdapFilter SAM_GROUP_OBJECT => new LdapFilter("sAMAcountType=268435456");

        /// <summary>
        /// Gets a sAMAccountType LdapFilter object that represents a certain type of object in
        /// Active Directory. <seealso href="http://msdn.microsoft.com/en-us/library/cc228417(PROT.13).aspx"/>
        /// </summary>
        /// <value>sAMAccountType value describing an account type object.</value>
        public static LdapFilter SAM_MACHINE_ACCOUNT => new LdapFilter("sAMAcountType=805306369");

        /// <summary>
        /// Gets a sAMAccountType LdapFilter object that represents a certain type of object in
        /// Active Directory. <seealso href="http://msdn.microsoft.com/en-us/library/cc228417(PROT.13).aspx"/>
        /// </summary>
        /// <value>sAMAccountType value describing an account type object.</value>
        public static LdapFilter SAM_NON_SECURITY_ALIAS_OBJECT => new LdapFilter("sAMAcountType=536870913");

        /// <summary>
        /// Gets a sAMAccountType LdapFilter object that represents a certain type of object in
        /// Active Directory. <seealso href="http://msdn.microsoft.com/en-us/library/cc228417(PROT.13).aspx"/>
        /// </summary>
        /// <value>sAMAccountType value describing an account type object.</value>
        public static LdapFilter SAM_NON_SECURITY_GROUP_OBJECT => new LdapFilter("sAMAcountType=268435457");

        /// <summary>
        /// Gets a sAMAccountType LdapFilter object that represents a certain type of object in
        /// Active Directory. <seealso href="http://msdn.microsoft.com/en-us/library/cc228417(PROT.13).aspx"/>
        /// </summary>
        /// <value>sAMAccountType value describing an account type object.</value>
        public static LdapFilter SAM_NORMAL_USER_ACCOUNT => new LdapFilter("sAMAcountType=805306368");

        /// <summary>
        /// Gets a sAMAccountType LdapFilter object that represents a certain type of object in
        /// Active Directory. <seealso href="http://msdn.microsoft.com/en-us/library/cc228417(PROT.13).aspx"/>
        /// </summary>
        /// <value>sAMAccountType value describing an account type object.</value>
        public static LdapFilter SAM_TRUST_ACCOUNT => new LdapFilter("sAMAcountType=805306370");

        /// <summary>
        /// Gets a sAMAccountType LdapFilter object that represents a certain type of object in
        /// Active Directory. <seealso href="http://msdn.microsoft.com/en-us/library/cc228417(PROT.13).aspx"/>
        /// </summary>
        /// <value>sAMAccountType value describing an account type object.</value>
        public static LdapFilter SAM_USER_OBJECT => new LdapFilter("sAMAcountType=805306368");

        /// <summary>
        /// Gets an LdapFilter that represents service accounts.
        /// </summary>
        /// <value>LdapFilter that represents service accounts.</value>
        public static LdapFilter ServiceAccounts => new LdapFilter("sAMAccountName=*svc").And(Users);

        /// <summary>
        /// Gets an LdapFilter that represents universal groups.
        /// </summary>
        /// <value>LdapFilter that represents universal groups.</value>
        public static LdapFilter UniversalGroups => Groups.And("groupType:1.2.840.113556.1.4.803:=2147483656", true);

        /// <summary>
        /// Gets an LdapFilter that represents user accounts.
        /// </summary>
        /// <value>LdapFilter that represents users.</value>
        public static LdapFilter Users => new LdapFilter("objectClass=user").And("objectCategory=person");

        /// <summary>
        /// Gets an LdapFilter that represents users that are not disabled and must change their
        /// password at their next logon.
        /// </summary>
        /// <value>LdapFilter that represents users.</value>
        public static LdapFilter UsersNotDisabledMustChangePassword =>
            LdapFilter.And(
                Users,
                new LdapFilter("pwdLastSet=0"),
                UserAccessControl(ADS_USER_FLAG.ACCOUNTDISABLE).Not());

        /// <summary>
        /// Gets an LdapFilter that represents users with email addresses.
        /// </summary>
        /// <value>LdapFilter that represents users.</value>
        public static LdapFilter UsersWithEmail => Users.And("mail=*", true);

        /// <summary>
        /// Gets an LdapFilter that represents users without email addresses.
        /// </summary>
        /// <value>LdapFilter that represents users.</value>
        public static LdapFilter UsersWithoutEmail => Users.And(new LdapFilter("mail=*").Not(), true);

        /// <summary>
        /// Gets an LdapFilter that represents users without logon scripts.
        /// </summary>
        /// <value>LdapFilter that represents users.</value>
        public static LdapFilter UsersWithoutLogonScript => Users.And(new LdapFilter("scriptPath=*").Not(), true);

        /// <summary>
        /// Gets an LdapFilter that represents users without a profile path set.
        /// </summary>
        /// <value>LdapFilter that represents users.</value>
        public static LdapFilter UsersWithoutProfilePath => Users.And(new LdapFilter("profilePath=*").Not(), true);

        /// <summary>
        /// Returns an LdapFilter for computers that match the partial or complete name provided.
        /// </summary>
        /// <param name="name">The partial name of the computer.</param>
        /// <returns>LdapFilter for computers that match the partial or complete name provided.</returns>
        public static LdapFilter ComputersNameContaining(string name) => Computers.And(String.Format("name=*{0}*", name), true);

        /// <summary>
        /// Returns an LdapFilter for computers that match the partial or complete operating system
        /// name provided.
        /// </summary>
        /// <param name="operatingSystem">The operating system.</param>
        /// <returns>LdapFilter for computer that match the partial or complete OS provided</returns>
        public static LdapFilter ComputersWithOS(string operatingSystem) => Computers.And(String.Format("operatingSystem=*{0}*", operatingSystem), true);

        /// <summary>
        /// Returns an LdapFilter for computers that match the partial or complete operating system
        /// name and service pack provided.
        /// </summary>
        /// <param name="operatingSystem">The operating system.</param>
        /// <param name="servicePack">The service pack.</param>
        /// <returns>LdapFilter for computer that match the partial or complete OS and SP provided</returns>
        public static LdapFilter ComputersWithOsAndSp(string operatingSystem, int servicePack) =>
            ComputersWithOS(operatingSystem)
                .And($"operatingSystemServicePack=*{servicePack}*", true);

        /// <summary>
        /// Returns an LdapFilter for contacts who are a member of a group (typically a distribution list)
        /// </summary>
        /// <param name="group">The group's DistinguishedName.</param>
        /// <returns>LdapFilter for contacts</returns>
        public static LdapFilter ContactsInGroup(DistinguishedName group) =>
            Contacts.And(new LdapFilter("memberOf", "=", @group.ToString()), true);

        /// <summary>
        /// Returns an LdapFilter for Group that matches the name provided
        /// </summary>
        /// <param name="nameOfGroup">The name of the group.</param>
        /// <returns>LdapFilter for group</returns>
        public static LdapFilter Group(string nameOfGroup) => Group(nameOfGroup, false);

        /// <summary>
        /// Returns an LdapFilter for Group that matches the name provided
        /// </summary>
        /// <param name="nameOfGroup">The name of group.</param>
        /// <param name="fuzzySearch">if set to <c>true</c>, use wild cards.</param>
        /// <returns>LdapFilter for group</returns>
        public static LdapFilter Group(string nameOfGroup, bool fuzzySearch) =>
            Groups.And(fuzzySearch ? $"sAMAccountName=*{nameOfGroup}*" : $"sAMAccountName={nameOfGroup}", true);

        /// <summary>
        /// Returns an LdapFilter for use with userAccountControl searches.
        /// </summary>
        /// <param name="uac">An ADS_USER_Flag enumeration</param>
        /// <returns>LdapFilter for User Account Control Search</returns>
        public static LdapFilter UserAccessControl(ADS_USER_FLAG uac)
        {
            switch (uac)
            {
                case ADS_USER_FLAG.SCRIPT:
                    return new LdapFilter("userAccountControl:1.2.840.113556.1.4.803:=1");

                case ADS_USER_FLAG.ACCOUNTDISABLE:
                    return new LdapFilter("userAccountControl:1.2.840.113556.1.4.803:=2");

                case ADS_USER_FLAG.HOMEDIR_REQUIRED:
                    return new LdapFilter("userAccountControl:1.2.840.113556.1.4.803:=8");

                case ADS_USER_FLAG.LOCKOUT:
                    return new LdapFilter("userAccountControl:1.2.840.113556.1.4.803:=16");

                case ADS_USER_FLAG.PASSWD_NOTREQD:
                    return new LdapFilter("userAccountControl:1.2.840.113556.1.4.803:=32");

                case ADS_USER_FLAG.PASSWD_CANT_CHANGE:
                    return new LdapFilter("userAccountControl:1.2.840.113556.1.4.803:=64");

                case ADS_USER_FLAG.ENCRYPTED_TEXT_PASSWORD_ALLOWED:
                    return new LdapFilter("userAccountControl:1.2.840.113556.1.4.803:=128");

                case ADS_USER_FLAG.TEMP_DUPLICATE_ACCOUNT:
                    return new LdapFilter("userAccountControl:1.2.840.113556.1.4.803:=256");

                case ADS_USER_FLAG.NORMAL_ACCOUNT:
                    return new LdapFilter("userAccountControl:1.2.840.113556.1.4.803:=512");

                case ADS_USER_FLAG.INTERDOMAIN_TRUST_ACCOUNT:
                    return new LdapFilter("userAccountControl:1.2.840.113556.1.4.803:=2048");

                case ADS_USER_FLAG.WORKSTATION_TRUST_ACCOUNT:
                    return new LdapFilter("userAccountControl:1.2.840.113556.1.4.803:=4096");

                case ADS_USER_FLAG.SERVER_TRUST_ACCOUNT:
                    return new LdapFilter("userAccountControl:1.2.840.113556.1.4.803:=8192");

                case ADS_USER_FLAG.DONT_EXPIRE_PASSWD:
                    return new LdapFilter("userAccountControl:1.2.840.113556.1.4.803:=65536");

                case ADS_USER_FLAG.MNS_LOGON_ACCOUNT:
                    return new LdapFilter("userAccountControl:1.2.840.113556.1.4.803:=131072");

                case ADS_USER_FLAG.SMARTCARD_REQUIRED:
                    return new LdapFilter("userAccountControl:1.2.840.113556.1.4.803:=262144");

                case ADS_USER_FLAG.TRUSTED_FOR_DELEGATION:
                    return new LdapFilter("userAccountControl:1.2.840.113556.1.4.803:=524288");

                case ADS_USER_FLAG.NOT_DELEGATED:
                    return new LdapFilter("userAccountControl:1.2.840.113556.1.4.803:=1048576");

                case ADS_USER_FLAG.USE_DES_KEY_ONLY:
                    return new LdapFilter("userAccountControl:1.2.840.113556.1.4.803:=2097152");

                case ADS_USER_FLAG.DONT_REQUIRE_PREAUTH:
                    return new LdapFilter("userAccountControl:1.2.840.113556.1.4.803:=4194304");

                case ADS_USER_FLAG.PASSWORD_EXPIRED:
                    return new LdapFilter("userAccountControl:1.2.840.113556.1.4.803:=8388608");

                case ADS_USER_FLAG.TRUSTED_TO_AUTHENTICATE_FOR_DELEGATION:
                    return new LdapFilter("userAccountControl:1.2.840.113556.1.4.803:=16777216");

                default:
                    throw new InvalidOperationException("Invalid User Flag!");
            }
        }

        /// <summary>
        /// Returns an LdapFilter for Users and contacts in a group.
        /// </summary>
        /// <param name="group">The group's Distinguished Name.</param>
        /// <returns>LdapFilter for the Users and Contacts</returns>
        public static LdapFilter UsersContactsInGroup(DistinguishedName group) =>
            LdapFilter.And(
                new LdapFilter("objectCategory=person"),
                LdapFilter.Or(
                    new LdapFilter("objectClass=contact"),
                    new LdapFilter("objectClass=user")),
                new LdapFilter("memberOf", "=", @group.ToString()));

        /// <summary>
        /// Returns an LdapFilter for Users that were created after the specified date.
        /// </summary>
        /// <param name="theDate">The date that will be converted to an Ldap format.</param>
        /// <returns>LdapFilter for the Users</returns>
        public static LdapFilter UsersCreatedAfterDate(DateTime theDate) =>
            Users.And(
                new LdapFilter("whenCreated", ">=", AdDateTime.ToLdapDateTime(theDate)), true);

        /// <summary>
        /// Returns an LdapFilter for Users that were created before the specified date.
        /// </summary>
        /// <param name="theDate">The date that will be converted to an Ldap format.</param>
        /// <returns>LdapFilter for the Users</returns>
        public static LdapFilter UsersCreatedBeforeDate(DateTime theDate) =>
            Users.And(
                new LdapFilter("whenCreated", "<=", AdDateTime.ToLdapDateTime(theDate)), true);

        /// <summary>
        /// Returns an LdapFilter for Users that were created before the specified date.
        /// </summary>
        /// <param name="firstDate">The first date of the range.</param>
        /// <param name="secondDate">The second date of the range.</param>
        /// <returns>LdapFilter for the Users</returns>
        public static LdapFilter UsersCreatedBetweenDates(DateTime firstDate, DateTime secondDate) =>
            Users.And(
                LdapFilter.And(
                    new LdapFilter("whenCreated", ">=", AdDateTime.ToLdapDateTime(firstDate)),
                    new LdapFilter("whenCreated", "<=", AdDateTime.ToLdapDateTime(secondDate))),
                true);
    }
}
