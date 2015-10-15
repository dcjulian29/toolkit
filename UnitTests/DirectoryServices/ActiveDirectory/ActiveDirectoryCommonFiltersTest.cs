using System;
using System.Diagnostics.CodeAnalysis;
using ToolKit.DirectoryServices;
using ToolKit.DirectoryServices.ActiveDirectory;
using ToolKit.DirectoryServices.ServiceInterfaces;
using Xunit;

namespace UnitTests.DirectoryServices.ActiveDirectory
{
    [SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1600:ElementsMustBeDocumented",
        Justification = "Test Suites do not need XML Documentation.")]
    [SuppressMessage("ReSharper", "StringLiteralTypo")]
    [SuppressMessage("ReSharper", "IdentifierTypo")]
    public class ActiveDirectoryCommonFiltersTest
    {
        [Fact]
        public void Computers()
        {
            // Arrange
            var expected = "(objectCategory=computer)";

            // Act
            var filter = ActiveDirectoryCommonFilters.Computers;

            // Assert
            Assert.Equal(expected, filter.ToString());
        }

        [Fact]
        public void Computers_DomainControllers()
        {
            // Arrange
            var expected = "(&(objectCategory=computer)"
                           + "(userAccountControl:1.2.840.113556.1.4.803:=8192))";

            // Act
            var filter = ActiveDirectoryCommonFilters.DomainControllers;

            // Assert
            Assert.Equal(expected, filter.ToString());
        }

        [Fact]
        public void Computers_Name_Containing()
        {
            // Arrange
            var expected = "(&(objectCategory=computer)(name=*ex*))";

            // Act
            var filter = ActiveDirectoryCommonFilters.ComputersNameContaining("ex");

            // Assert
            Assert.Equal(expected, filter.ToString());
        }

        [Fact]
        public void Computers_With_Operating_System()
        {
            // Arrange
            var expected = "(&(objectCategory=computer)(operatingSystem=*vista*))";

            // Act
            var filter = ActiveDirectoryCommonFilters.ComputersWithOS("vista");

            // Assert
            Assert.Equal(expected, filter.ToString());
        }

        [Fact]
        public void Computers_With_Operating_System_And_Service_Pack()
        {
            // Arrange
            var expected = "(&(&(objectCategory=computer)"
                           + "(operatingSystem=*server*))"
                           + "(operatingSystemServicePack=*1*))";

            // Act
            var filter = ActiveDirectoryCommonFilters.ComputersWithOsAndSp("server", 1);

            // Assert
            Assert.Equal(expected, filter.ToString());
        }

        [Fact]
        public void Contacts()
        {
            // Arrange
            var expected = "(&(objectCategory=person)(objectClass=contact))";

            // Act
            var filter = ActiveDirectoryCommonFilters.Contacts;

            // Assert
            Assert.Equal(expected, filter.ToString());
        }

        [Fact]
        public void Contacts_In_Group()
        {
            // Arrange
            var expected = "(&(&(objectCategory=person)(objectClass=contact))"
                           + "(memberOf=CN=TEST,OU=Groups,OU=HDQRK,DC=COMPANY,DC=COM))";

            // Act
            var filter = ActiveDirectoryCommonFilters.ContactsInGroup(
                DistinguishedName.Parse(
                    "CN=TEST,OU=Groups,OU=HDQRK,DC=COMPANY,DC=COM"));

            // Assert
            Assert.Equal(expected, filter.ToString());
        }

        [Fact]
        public void Group_With_Fuzzy_Search()
        {
            // Arrange
            var expected = "(&(objectCategory=group)(sAMAccountName=*domain*))";

            // Act
            var filter = ActiveDirectoryCommonFilters.Group("domain", true);

            // Assert
            Assert.Equal(expected, filter.ToString());
        }

        [Fact]
        public void Group_With_Strict_Search()
        {
            // Arrange
            var expected = "(&(objectCategory=group)(sAMAccountName=domain))";

            // Act
            var filter = ActiveDirectoryCommonFilters.Group("domain");

            // Assert
            Assert.Equal(expected, filter.ToString());
        }

        [Fact]
        public void Groups()
        {
            // Arrange
            var expected = "(objectCategory=group)";

            // Act
            var filter = ActiveDirectoryCommonFilters.Groups;

            // Assert
            Assert.Equal(expected, filter.ToString());
        }

        [Fact]
        public void Groups_Domain_Local()
        {
            // Arrange
            var expected = "(&(objectCategory=group)(groupType:1.2.840.113556.1.4.803:=2147483652))";

            // Act
            var filter = ActiveDirectoryCommonFilters.DomainLocalGroups;

            // Assert
            Assert.Equal(expected, filter.ToString());
        }

        [Fact]
        public void Groups_Global()
        {
            // Arrange
            var expected = "(&(objectCategory=group)(groupType:1.2.840.113556.1.4.803:=2147483650))";

            // Act
            var filter = ActiveDirectoryCommonFilters.GlobalGroups;

            // Assert
            Assert.Equal(expected, filter.ToString());
        }

        [Fact]
        public void Groups_Universal()
        {
            // Arrange
            var expected = "(&(objectCategory=group)(groupType:1.2.840.113556.1.4.803:=2147483656))";

            // Act
            var filter = ActiveDirectoryCommonFilters.UniversalGroups;

            // Assert
            Assert.Equal(expected, filter.ToString());
        }

        [Fact]
        public void Groups_With_Email_Address()
        {
            // Arrange
            var expected = "(&(objectCategory=group)(mail=*))";

            // Act
            var filter = ActiveDirectoryCommonFilters.GroupsWithEmail;

            // Assert
            Assert.Equal(expected, filter.ToString());
        }

        [Fact]
        public void Groups_With_No_Members()
        {
            // Arrange
            var expected = "(&(objectCategory=group)(!member=*))";

            // Act
            var filter = ActiveDirectoryCommonFilters.EmptyGroups;

            // Assert
            Assert.Equal(expected, filter.ToString());
        }

        [Fact]
        public void SAM_ALIAS_OBJECT()
        {
            // Arrange
            var expected = "(sAMAcountType=536870912)";

            // Act
            var filter = ActiveDirectoryCommonFilters.SAM_ALIAS_OBJECT;

            // Assert
            Assert.Equal(expected, filter.ToString());
        }

        [Fact]
        public void SAM_GROUP_OBJECT()
        {
            // Arrange
            var expected = "(sAMAcountType=268435456)";

            // Act
            var filter = ActiveDirectoryCommonFilters.SAM_GROUP_OBJECT;

            // Assert
            Assert.Equal(expected, filter.ToString());
        }

        [Fact]
        public void SAM_MACHINE_ACCOUNT()
        {
            // Arrange
            var expected = "(sAMAcountType=805306369)";

            // Act
            var filter = ActiveDirectoryCommonFilters.SAM_MACHINE_ACCOUNT;

            // Assert
            Assert.Equal(expected, filter.ToString());
        }

        [Fact]
        public void SAM_NON_SECURITY_ALIAS_OBJECT()
        {
            // Arrange
            var expected = "(sAMAcountType=536870913)";

            // Act
            var filter = ActiveDirectoryCommonFilters.SAM_NON_SECURITY_ALIAS_OBJECT;

            // Assert
            Assert.Equal(expected, filter.ToString());
        }

        [Fact]
        public void SAM_NON_SECURITY_GROUP_OBJECT()
        {
            // Arrange
            var expected = "(sAMAcountType=268435457)";

            // Act
            var filter = ActiveDirectoryCommonFilters.SAM_NON_SECURITY_GROUP_OBJECT;

            // Assert
            Assert.Equal(expected, filter.ToString());
        }

        [Fact]
        public void SAM_NORMAL_USER_ACCOUNT()
        {
            // Arrange
            var expected = "(sAMAcountType=805306368)";

            // Act
            var filter = ActiveDirectoryCommonFilters.SAM_NORMAL_USER_ACCOUNT;

            // Assert
            Assert.Equal(expected, filter.ToString());
        }

        [Fact]
        public void SAM_TRUST_ACCOUNT()
        {
            // Arrange
            var expected = "(sAMAcountType=805306370)";

            // Act
            var filter = ActiveDirectoryCommonFilters.SAM_TRUST_ACCOUNT;

            // Assert
            Assert.Equal(expected, filter.ToString());
        }

        [Fact]
        public void SAM_USER_OBJECT()
        {
            // Arrange
            var expected = "(sAMAcountType=805306368)";

            // Act
            var filter = ActiveDirectoryCommonFilters.SAM_USER_OBJECT;

            // Assert
            Assert.Equal(expected, filter.ToString());
        }

        [Fact]
        public void UAC_ACCOUNTDISABLE()
        {
            // Arrange
            var expected = "(userAccountControl:1.2.840.113556.1.4.803:=2)";

            // Act
            var filter = ActiveDirectoryCommonFilters.UserAccessControl(
                ADS_USER_FLAG.ACCOUNTDISABLE);

            // Assert
            Assert.Equal(expected, filter.ToString());
        }

        [Fact]
        public void UAC_DONT_EXPIRE_PASSWORD()
        {
            // Arrange
            var expected = "(userAccountControl:1.2.840.113556.1.4.803:=65536)";

            // Act
            var filter = ActiveDirectoryCommonFilters.UserAccessControl(
                ADS_USER_FLAG.DONT_EXPIRE_PASSWD);

            // Assert
            Assert.Equal(expected, filter.ToString());
        }

        [Fact]
        public void UAC_DONT_REQ_PREAUTH()
        {
            // Arrange
            var expected = "(userAccountControl:1.2.840.113556.1.4.803:=4194304)";

            // Act
            var filter = ActiveDirectoryCommonFilters.UserAccessControl(
                ADS_USER_FLAG.DONT_REQUIRE_PREAUTH);

            // Assert
            Assert.Equal(expected, filter.ToString());
        }

        [Fact]
        public void UAC_ENCRYPTED_TEXT_PWD_ALLOWED()
        {
            // Arrange
            var expected = "(userAccountControl:1.2.840.113556.1.4.803:=128)";

            // Act
            var filter = ActiveDirectoryCommonFilters.UserAccessControl(
                ADS_USER_FLAG.ENCRYPTED_TEXT_PASSWORD_ALLOWED);

            // Assert
            Assert.Equal(expected, filter.ToString());
        }

        [Fact]
        public void UAC_HOMEDIR_REQUIRED()
        {
            // Arrange
            var expected = "(userAccountControl:1.2.840.113556.1.4.803:=8)";

            // Act
            var filter = ActiveDirectoryCommonFilters.UserAccessControl(
                ADS_USER_FLAG.HOMEDIR_REQUIRED);

            // Assert
            Assert.Equal(expected, filter.ToString());
        }

        [Fact]
        public void UAC_INTERDOMAIN_TRUST_ACCOUNT()
        {
            // Arrange
            var expected = "(userAccountControl:1.2.840.113556.1.4.803:=2048)";

            // Act
            var filter = ActiveDirectoryCommonFilters.UserAccessControl(
                ADS_USER_FLAG.INTERDOMAIN_TRUST_ACCOUNT);

            // Assert
            Assert.Equal(expected, filter.ToString());
        }

        [Fact]
        public void UAC_LOCKOUT()
        {
            // Arrange
            var expected = "(userAccountControl:1.2.840.113556.1.4.803:=16)";

            // Act
            var filter = ActiveDirectoryCommonFilters.UserAccessControl(
                ADS_USER_FLAG.LOCKOUT);

            // Assert
            Assert.Equal(expected, filter.ToString());
        }

        [Fact]
        public void UAC_MNS_LOGON_ACCOUNT()
        {
            // Arrange
            var expected = "(userAccountControl:1.2.840.113556.1.4.803:=131072)";

            // Act
            var filter = ActiveDirectoryCommonFilters.UserAccessControl(
                ADS_USER_FLAG.MNS_LOGON_ACCOUNT);

            // Assert
            Assert.Equal(expected, filter.ToString());
        }

        [Fact]
        public void UAC_NORMAL_ACCOUNT()
        {
            // Arrange
            var expected = "(userAccountControl:1.2.840.113556.1.4.803:=512)";

            // Act
            var filter = ActiveDirectoryCommonFilters.UserAccessControl(
                ADS_USER_FLAG.NORMAL_ACCOUNT);

            // Assert
            Assert.Equal(expected, filter.ToString());
        }

        [Fact]
        public void UAC_NOT_DELEGATED()
        {
            // Arrange
            var expected = "(userAccountControl:1.2.840.113556.1.4.803:=1048576)";

            // Act
            var filter = ActiveDirectoryCommonFilters.UserAccessControl(
                ADS_USER_FLAG.NOT_DELEGATED);

            // Assert
            Assert.Equal(expected, filter.ToString());
        }

        [Fact]
        public void UAC_PASSWD_CANT_CHANGE()
        {
            // Arrange
            var expected = "(userAccountControl:1.2.840.113556.1.4.803:=64)";

            // Act
            var filter = ActiveDirectoryCommonFilters.UserAccessControl(
                ADS_USER_FLAG.PASSWD_CANT_CHANGE);

            // Assert
            Assert.Equal(expected, filter.ToString());
        }

        [Fact]
        public void UAC_PASSWD_NOTREQD()
        {
            // Arrange
            var expected = "(userAccountControl:1.2.840.113556.1.4.803:=32)";

            // Act
            var filter = ActiveDirectoryCommonFilters.UserAccessControl(
                ADS_USER_FLAG.PASSWD_NOTREQD);

            // Assert
            Assert.Equal(expected, filter.ToString());
        }

        [Fact]
        public void UAC_PASSWORD_EXPIRED()
        {
            // Arrange
            var expected = "(userAccountControl:1.2.840.113556.1.4.803:=8388608)";

            // Act
            var filter = ActiveDirectoryCommonFilters.UserAccessControl(
                ADS_USER_FLAG.PASSWORD_EXPIRED);

            // Assert
            Assert.Equal(expected, filter.ToString());
        }

        [Fact]
        public void UAC_SCRIPT()
        {
            // Arrange
            var expected = "(userAccountControl:1.2.840.113556.1.4.803:=1)";

            // Act
            var filter = ActiveDirectoryCommonFilters.UserAccessControl(
                ADS_USER_FLAG.SCRIPT);

            // Assert
            Assert.Equal(expected, filter.ToString());
        }

        [Fact]
        public void UAC_SERVER_TRUST_ACCOUNT()
        {
            // Arrange
            var expected = "(userAccountControl:1.2.840.113556.1.4.803:=8192)";

            // Act
            var filter = ActiveDirectoryCommonFilters.UserAccessControl(
                ADS_USER_FLAG.SERVER_TRUST_ACCOUNT);

            // Assert
            Assert.Equal(expected, filter.ToString());
        }

        [Fact]
        public void UAC_SMARTCARD_REQUIRED()
        {
            // Arrange
            var expected = "(userAccountControl:1.2.840.113556.1.4.803:=262144)";

            // Act
            var filter = ActiveDirectoryCommonFilters.UserAccessControl(
                ADS_USER_FLAG.SMARTCARD_REQUIRED);

            // Assert
            Assert.Equal(expected, filter.ToString());
        }

        [Fact]
        public void UAC_TEMP_DUPLICATE_ACCOUNT()
        {
            // Arrange
            var expected = "(userAccountControl:1.2.840.113556.1.4.803:=256)";

            // Act
            var filter = ActiveDirectoryCommonFilters.UserAccessControl(
                ADS_USER_FLAG.TEMP_DUPLICATE_ACCOUNT);

            // Assert
            Assert.Equal(expected, filter.ToString());
        }

        [Fact]
        public void UAC_TRUSTED_FOR_DELEGATION()
        {
            // Arrange
            var expected = "(userAccountControl:1.2.840.113556.1.4.803:=524288)";

            // Act
            var filter = ActiveDirectoryCommonFilters.UserAccessControl(
                ADS_USER_FLAG.TRUSTED_FOR_DELEGATION);

            // Assert
            Assert.Equal(expected, filter.ToString());
        }

        [Fact]
        public void UAC_TRUSTED_TO_AUTH_FOR_DELEGATION()
        {
            // Arrange
            var expected = "(userAccountControl:1.2.840.113556.1.4.803:=16777216)";

            // Act
            var filter = ActiveDirectoryCommonFilters.UserAccessControl(
                ADS_USER_FLAG.TRUSTED_TO_AUTHENTICATE_FOR_DELEGATION);

            // Assert
            Assert.Equal(expected, filter.ToString());
        }

        [Fact]
        public void UAC_USE_DES_KEY_ONLY()
        {
            // Arrange
            var expected = "(userAccountControl:1.2.840.113556.1.4.803:=2097152)";

            // Act
            var filter = ActiveDirectoryCommonFilters.UserAccessControl(
                ADS_USER_FLAG.USE_DES_KEY_ONLY);

            // Assert
            Assert.Equal(expected, filter.ToString());
        }

        [Fact]
        public void UAC_WORKSTATION_TRUST_ACCOUNT()
        {
            // Arrange
            var expected = "(userAccountControl:1.2.840.113556.1.4.803:=4096)";

            // Act
            var filter = ActiveDirectoryCommonFilters.UserAccessControl(
                ADS_USER_FLAG.WORKSTATION_TRUST_ACCOUNT);

            // Assert
            Assert.Equal(expected, filter.ToString());
        }

        [Fact]
        public void Users()
        {
            // Arrange
            var expected = "(&(objectCategory=person)(objectClass=user))";

            // Act
            var filter = ActiveDirectoryCommonFilters.Users;

            // Assert
            Assert.Equal(expected, filter.ToString());
        }

        [Fact]
        public void Users_Administrative_Users()
        {
            // Arrange
            var expected = "(&(&(objectCategory=person)(objectClass=user))(sAMAccountName=*-adm))";

            // Act
            var filter = ActiveDirectoryCommonFilters.AdministrativeUsers;

            // Assert
            Assert.Equal(expected, filter.ToString());
        }

        [Fact]
        public void Users_And_Contacts_Who_Are_Members_Of_A_Group()
        {
            // Arrange
            var expected = "(&(objectCategory=person)"
                           + "(|(objectClass=contact)(objectClass=user))"
                           + "(memberOf=CN=TEST,OU=Groups,OU=HDQRK,DC=COMPANY,DC=COM))";

            // Act
            var filter =
                ActiveDirectoryCommonFilters.UsersContactsInGroup(
                    DistinguishedName.Parse(
                        "CN=TEST,OU=Groups,OU=HDQRK,DC=COMPANY,DC=COM"));

            // Assert
            Assert.Equal(expected, filter.ToString());
        }

        [Fact]
        public void Users_Created_After_Date()
        {
            // Arrange
            var expected = "(&(&(objectCategory=person)(objectClass=user))"
                           + "(whenCreated>=20090501000000.0Z))";
            var theDate = new DateTime(2009, 5, 1);

            // Act
            var filter = ActiveDirectoryCommonFilters.UsersCreatedAfterDate(theDate);

            // Assert
            Assert.Equal(expected, filter.ToString());
        }

        [Fact]
        public void Users_Created_Before_Date()
        {
            // Arrange
            var expected = "(&(&(objectCategory=person)(objectClass=user))"
                           + "(whenCreated<=20090501000000.0Z))";
            var theDate = new DateTime(2009, 5, 1);

            // Act
            var filter = ActiveDirectoryCommonFilters.UsersCreatedBeforeDate(theDate);

            // Assert
            Assert.Equal(expected, filter.ToString());
        }

        [Fact]
        public void Users_Created_Between_Dates()
        {
            // Arrange
            var expected = "(&(&(objectCategory=person)(objectClass=user))"
                           + "(&(whenCreated>=20090501000000.0Z)(whenCreated<=20090501000000.0Z)))";
            var firstDate = new DateTime(2009, 5, 1);
            var secondDate = new DateTime(2009, 5, 1);

            // Act
            var filter =
                ActiveDirectoryCommonFilters.UsersCreatedBetweenDates(firstDate, secondDate);

            // Assert
            Assert.Equal(expected, filter.ToString());
        }

        [Fact]
        public void Users_Normal_Account()
        {
            // Arrange
            var expected = "(&(&(objectCategory=person)(objectClass=user))"
                           + "(!sAMAccountName=*-adm)(!sAMAccountName=*svc))";

            // Act
            var filter = ActiveDirectoryCommonFilters.NormalAccounts;

            // Assert
            Assert.Equal(expected, filter.ToString());
        }

        [Fact]
        public void Users_Service_Accounts()
        {
            // Arrange
            var expected = "(&(&(objectCategory=person)(objectClass=user))(sAMAccountName=*svc))";

            // Act
            var filter = ActiveDirectoryCommonFilters.ServiceAccounts;

            // Assert
            Assert.Equal(expected, filter.ToString());
        }

        [Fact]
        public void Users_Who_Are_Disabled()
        {
            // Arrange
            var expected = "(&(&(objectCategory=person)(objectClass=user))"
                           + "(userAccountControl:1.2.840.113556.1.4.803:=2))";

            // Act
            var filter = LdapFilter.And(
                ActiveDirectoryCommonFilters.Users,
                ActiveDirectoryCommonFilters.UserAccessControl(ADS_USER_FLAG.ACCOUNTDISABLE));

            // Assert
            Assert.Equal(expected, filter.ToString());
        }

        [Fact]
        public void Users_Who_Are_Locked_Out()
        {
            // Arrange
            var expected = "(&(&(objectCategory=person)(objectClass=user))"
                           + "(userAccountControl:1.2.840.113556.1.4.803:=16))";

            // Act
            var filter = LdapFilter.And(
                ActiveDirectoryCommonFilters.Users,
                ActiveDirectoryCommonFilters.UserAccessControl(ADS_USER_FLAG.LOCKOUT));

            // Assert
            Assert.Equal(expected, filter.ToString());
        }

        [Fact]
        public void Users_Who_Are_Not_Disabled_And_Must_Change_Password()
        {
            // Arrange
            var expected = "(&(&(objectCategory=person)(objectClass=user))"
                           + "(pwdLastSet=0)(!(userAccountControl:1.2.840.113556.1.4.803:=2)))";

            // Act
            var filter = ActiveDirectoryCommonFilters.UsersNotDisabledMustChangePassword;

            // Assert
            Assert.Equal(expected, filter.ToString());
        }

        [Fact]
        public void Users_Whose_Passwords_Do_Not_Expire()
        {
            // Arrange
            var expected = "(&(&(objectCategory=person)(objectClass=user))"
                           + "(userAccountControl:1.2.840.113556.1.4.803:=65536))";

            // Act
            var filter = LdapFilter.And(
                ActiveDirectoryCommonFilters.Users,
                ActiveDirectoryCommonFilters.UserAccessControl(ADS_USER_FLAG.DONT_EXPIRE_PASSWD));

            // Assert
            Assert.Equal(expected, filter.ToString());
        }

        [Fact]
        public void Users_With_Email_Address()
        {
            // Arrange
            var expected = "(&(&(objectCategory=person)(objectClass=user))(mail=*))";

            // Act
            var filter = ActiveDirectoryCommonFilters.UsersWithEmail;

            // Assert
            Assert.Equal(expected, filter.ToString());
        }

        [Fact]
        public void Users_With_No_Email_Address()
        {
            // Arrange
            var expected = "(&(&(objectCategory=person)(objectClass=user))(!mail=*))";

            // Act
            var filter = ActiveDirectoryCommonFilters.UsersWithoutEmail;

            // Assert
            Assert.Equal(expected, filter.ToString());
        }

        [Fact]
        public void Users_With_No_Logon_Script()
        {
            // Arrange
            var expected = "(&(&(objectCategory=person)(objectClass=user))(!scriptPath=*))";

            // Act
            var filter = ActiveDirectoryCommonFilters.UsersWithoutLogonScript;

            // Assert
            Assert.Equal(expected, filter.ToString());
        }

        [Fact]
        public void Users_With_No_Profile_Path()
        {
            // Arrange
            var expected = "(&(&(objectCategory=person)(objectClass=user))(!profilePath=*))";

            // Act
            var filter = ActiveDirectoryCommonFilters.UsersWithoutProfilePath;

            // Assert
            Assert.Equal(expected, filter.ToString());
        }
    }
}
