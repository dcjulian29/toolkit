using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using ToolKit.DirectoryServices.ActiveDirectory;
using Xunit;

namespace UnitTests.DirectoryServices.ActiveDirectory
{
    [SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1600:ElementsMustBeDocumented",
        Justification = "Test Suites do not need XML Documentation.")]
    public class UserTests
    {
        [Fact]
        public void AccountExpires_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = DateTime.Parse("1893-04-11T23:47:16.8775807", null, DateTimeStyles.AdjustToUniversal);
            var user = new User(InitializeProperties());

            // Act
            var actual = user.AccountExpires;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AccountExpires_Should_ReturnsMaxDateTime_When_EmptyString()
        {
            // Arrange
            var expected = DateTime.MaxValue;
            var user = new User(UserAccountExpireEmpty());

            // Act
            var actual = user.AccountExpires;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AccountExpires_Should_ReturnsMaxDateTime_When_InvalidNumber()
        {
            // Arrange
            var expected = DateTime.MaxValue;
            var user = new User(UserAccountExpireInvalid());

            // Act
            var actual = user.AccountExpires;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void BadPasswordCount_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = 2;
            var user = new User(InitializeProperties());

            // Act
            var actual = user.BadPasswordCount;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void BadPasswordTime_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = DateTime.Parse("2015-10-05T11:00:51.0451845", null, DateTimeStyles.AdjustToUniversal);
            var user = new User(InitializeProperties());

            // Act
            var actual = user.BadPasswordTime;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Category_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "CN=Person,CN=Schema,CN=Configuration,DC=company,DC=local";
            var user = new User(InitializeProperties());

            // Act
            var actual = user.Category;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Changed_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = new DateTime(2015, 10, 28, 3, 39, 34, DateTimeKind.Utc);
            var user = new User(InitializeProperties());

            // Act
            var actual = user.Changed;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void City_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "Bethesda";
            var user = new User(InitializeProperties());

            // Act
            var actual = user.City;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CommonName_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "USER01";
            var user = new User(InitializeProperties());

            // Act
            var actual = user.CommonName;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Company_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "Company";
            var user = new User(InitializeProperties());

            // Act
            var actual = user.Company;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Country_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "Aruba";
            var user = new User(InitializeProperties());

            // Act
            var actual = user.Country;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CountryCode_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = 553;
            var user = new User(InitializeProperties());

            // Act
            var actual = user.CountryCode;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Created_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = new DateTime(2015, 10, 5, 11, 0, 50, DateTimeKind.Utc);
            var user = new User(InitializeProperties());

            // Act
            var actual = user.Created;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Ctor_Should_ReturnExpectedResult()
        {
            // Arrange
            var obj = InitializeProperties();

            // Act
            var user = new User(obj);

            // Assert
            Assert.NotNull(user);
        }

        [Fact]
        public void Ctor_Should_ThrowException_When_NonUserObject()
        {
            // Arrange
            var obj = NotAUser();

            // Act/Assert
            Assert.Throws<ArgumentException>(() =>
            {
                var user = new User(obj);
            });
        }

        [Fact]
        public void Department_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "Accounting";
            var user = new User(InitializeProperties());

            // Act
            var actual = user.Department;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Description_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "User For Stuff";
            var user = new User(InitializeProperties());

            // Act
            var actual = user.Description;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Disabled_Should_ReturnExpectedResult()
        {
            // Arrange
            var user = new User(InitializeProperties());

            // Act
            var actual = user.Disabled;

            // Assert
            Assert.False(actual);
        }

        [Fact]
        public void Disabled_Should_ReturnTrue_When_UserIsDisabled()
        {
            // Arrange
            var user = new User(DisabledUser());

            // Act
            var actual = user.Disabled;

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void DisplayName_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "USER01";
            var user = new User(InitializeProperties());

            // Act
            var actual = user.DisplayName;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DistinguishedName_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "CN=USER01,CN=Users,DC=company,DC=local";
            var user = new User(InitializeProperties());

            // Act
            var actual = user.DistinguishedName;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void EmailAddress_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "user@company.com";
            var user = new User(InitializeProperties());

            // Act
            var actual = user.EmailAddress;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Fax_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "202-555-1212";
            var user = new User(InitializeProperties());

            // Act
            var actual = user.Fax;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FirstName_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "First";
            var user = new User(InitializeProperties());

            // Act
            var actual = user.FirstName;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Groups_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = new List<string>()
            {
                "CN=group1,CN=Users,DC=company,DC=local"
            };

            var user = new User(InitializeProperties());

            // Act
            var actual = user.Groups;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Guid_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = Guid.Parse("cd29418b-45d7-4d55-952e-e4da717172af");
            var user = new User(InitializeProperties());

            // Act
            var actual = user.Guid;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void HomeDirectory_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = @"\\FS01\USER01";
            var user = new User(InitializeProperties());

            // Act
            var actual = user.HomeDirectory;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void HomeDrive_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "H:";
            var user = new User(InitializeProperties());

            // Act
            var actual = user.HomeDrive;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void HomePhone_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "202-555-1212";
            var user = new User(InitializeProperties());

            // Act
            var actual = user.HomePhone;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void IpPhone_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "202-555-1212";
            var user = new User(InitializeProperties());

            // Act
            var actual = user.IpPhone;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void IsAdministrativeAccount_Should_ReturnFalse_When_NotAdministrativeUser()
        {
            // Arrange
            var user = new User(InitializeProperties());

            // Act
            var actual = user.IsAdministrativeAccount();

            // Assert
            Assert.False(actual);
        }

        [Fact]
        public void IsAdministrativeAccount_Should_ReturnTrue_When_AdministrativeUser()
        {
            // Arrange
            var user = new User(AdministrativeAccount());

            // Act
            var actual = user.IsAdministrativeAccount();

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void IsApplicationAccount_Should_ReturnTrue_When_ApplicationAccount()
        {
            // Arrange
            var user = new User(ApplicationAccount());

            // Act
            var actual = user.IsApplicationAccount();

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void IsRegularAccount_Should_ReturnTrue_When_RegularAccount()
        {
            // Arrange
            var user = new User(InitializeProperties());

            // Act
            var actual = user.IsRegularAccount();

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void IsServiceAccount_Should_ReturnTrue_When_ServiceAccount()
        {
            // Arrange
            var user = new User(ServiceAccount());

            // Act
            var actual = user.IsServiceAccount();

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void LastLogoff_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = DateTime.Parse("2015-10-29T00:24:10.5124008", null, DateTimeStyles.AdjustToUniversal);
            var user = new User(InitializeProperties());

            // Act
            var actual = user.LastLogoff;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void LastLogon_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = DateTime.Parse("2015-10-17T21:28:33.7228038", null, DateTimeStyles.AdjustToUniversal);
            var user = new User(InitializeProperties());

            // Act
            var actual = user.LastLogon;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void LastLogonTimestamp_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = DateTime.Parse("2015-10-27T22:28:54.7417734", null, DateTimeStyles.AdjustToUniversal);
            var user = new User(InitializeProperties());

            // Act
            var actual = user.LastLogonTimestamp;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void LastName_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "Last";
            var user = new User(InitializeProperties());

            // Act
            var actual = user.LastName;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void LogonCount_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = 60;
            var user = new User(InitializeProperties());

            // Act
            var actual = user.LogonCount;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void LogonScript_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = @"\\FS01\Scripts\logon.bat";
            var user = new User(InitializeProperties());

            // Act
            var actual = user.LogonScript;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Manager_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "CN=manager1,CN=Users,DC=company,DC=local";
            var user = new User(InitializeProperties());

            // Act
            var actual = user.Manager;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void MiddleInitial_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "C";
            var user = new User(InitializeProperties());

            // Act
            var actual = user.MiddleInitial;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void MobilePhone_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "202-555-1212";
            var user = new User(InitializeProperties());

            // Act
            var actual = user.MobilePhone;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Modified_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = new DateTime(2015, 10, 28, 3, 39, 34, DateTimeKind.Utc);
            var user = new User(InitializeProperties());

            // Act
            var actual = user.Modified;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Name_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "USER01";
            var user = new User(InitializeProperties());

            // Act
            var actual = user.Name;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Notes_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "A note about the user.";
            var user = new User(InitializeProperties());

            // Act
            var actual = user.Notes;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Office_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "Ashburn";
            var user = new User(InitializeProperties());

            // Act
            var actual = user.Office;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Pager_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "202-555-1212";
            var user = new User(InitializeProperties());

            // Act
            var actual = user.Pager;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void PasswordLastSet_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = DateTime.Parse("2015-10-05T11:00:51.0459354", null, DateTimeStyles.AdjustToUniversal);
            var user = new User(InitializeProperties());

            // Act
            var actual = user.PasswordLastSet;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void PoBox_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "151515";
            var user = new User(InitializeProperties());

            // Act
            var actual = user.PoBox;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void PostalCode_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "20016";
            var user = new User(InitializeProperties());

            // Act
            var actual = user.PostalCode;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void PrimaryGroupId_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = 515;
            var user = new User(InitializeProperties());

            // Act
            var actual = user.PrimaryGroupId;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ProfilePath_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = @"\\FS01\Profiles\USER01";
            var user = new User(InitializeProperties());

            // Act
            var actual = user.ProfilePath;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Province_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "Maine";
            var user = new User(InitializeProperties());

            // Act
            var actual = user.Province;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Region_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "Aruba";
            var user = new User(InitializeProperties());

            // Act
            var actual = user.Region;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SamAccountName_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "USER01";
            var user = new User(InitializeProperties());

            // Act
            var actual = user.SamAccountName;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Sid_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "S-1-5-21-1501611499-78517565-1004253924-2105";
            var user = new User(InitializeProperties());

            // Act
            var actual = user.Sid;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void State_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "Maine";
            var user = new User(InitializeProperties());

            // Act
            var actual = user.State;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void StreetAddress_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "123 Any Street";
            var user = new User(InitializeProperties());

            // Act
            var actual = user.StreetAddress;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TelephoneNumber_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "202-555-1212";
            var user = new User(InitializeProperties());

            // Act
            var actual = user.TelephoneNumber;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Title_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "Software Engineer";
            var user = new User(InitializeProperties());

            // Act
            var actual = user.Title;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void UpdateSequenceNumberCreated_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = 43640;
            var user = new User(InitializeProperties());

            // Act
            var actual = user.UpdateSequenceNumberCreated;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void UpdateSequenceNumberCurrent_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = 129204;
            var user = new User(InitializeProperties());

            // Act
            var actual = user.UpdateSequenceNumberCurrent;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void UserAccountControl_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = 4096;
            var user = new User(InitializeProperties());

            // Act
            var actual = user.UserAccountControl;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void UserPrincipalName_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "user01@company.local";
            var user = new User(InitializeProperties());

            // Act
            var actual = user.UserPrincipalName;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void WebSite_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "http://company.local/user01";
            var user = new User(InitializeProperties());

            // Act
            var actual = user.WebSite;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Zip_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "20016";
            var user = new User(InitializeProperties());

            // Act
            var actual = user.Zip;

            // Assert
            Assert.Equal(expected, actual);
        }

        private Dictionary<string, object> AdministrativeAccount()
        {
            var properties = InitializeProperties();

            properties["samaccountname"] = "account-adm";

            return properties;
        }

        private Dictionary<string, object> ApplicationAccount()
        {
            var properties = InitializeProperties();

            properties["samaccountname"] = "account-app";

            return properties;
        }

        private Dictionary<string, object> DisabledUser()
        {
            var properties = InitializeProperties();

            properties["useraccountcontrol"] = 66050;

            return properties;
        }

        private Dictionary<string, object> InitializeProperties()
        {
            return new Dictionary<string, object>()
            {
                { "accountexpires", 92233720368775807 },
                { "badpwdcount", 2 },
                { "badpasswordtime", 130885164510451845 },
                { "objectcategory", "CN=Person,CN=Schema,CN=Configuration,DC=company,DC=local" },
                { "cn", "USER01" },
                { "useraccountcontrol", 4096 },
                { "c", "Aruba" },
                { "countrycode", 553 },
                { "whencreated", DateTime.Parse("10/5/2015 11:00:50", null, DateTimeStyles.AdjustToUniversal) },
                { "description", "User For Stuff" },
                { "displayname", "USER01" },
                { "distinguishedname", "CN=USER01,CN=Users,DC=company,DC=local" },
                { "userprincipalname", "user01@company.local" },
                { "memberof", "CN=group1,CN=Users,DC=company,DC=local" },
                {
                    "objectguid", new Byte[]
                    {
                        0x8B, 0x41, 0x29, 0xCD, 0xD7, 0x45, 0x55, 0x4D,
                        0x95, 0x2E, 0xE4, 0xDA, 0x71, 0x71, 0x72, 0xAF
                    }
                },
                { "lastlogoff", 130905518505124008 },
                { "location", "Ashburn" },
                { "logoncount", 60 },
                { "manager", "CN=manager1,CN=Users,DC=company,DC=local" },
                { "whenchanged", DateTime.Parse("10/28/2015 3:39:34", null, DateTimeStyles.AdjustToUniversal) },
                { "name", "USER01" },
                { "department", "Accounting" },
                { "l", "Bethesda" },
                { "company", "Company" },
                { "pwdlastset", 130885164510459354 },
                { "primarygroupid", 515 },
                { "samaccountname", "USER01" },
                {
                    "objectsid", new Byte[]
                    {
                        0x01, 0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x05,
                        0x15, 0x00, 0x00, 0x00, 0xEB, 0xC5, 0x80, 0x59,
                        0x3D, 0x15, 0xAE, 0x04, 0xE4, 0xB2, 0xDB, 0x3B,
                        0x39, 0x08, 0x00, 0x00
                    }
                },
                { "usncreated", (Int64)43640 },
                { "usnchanged", (Int64)129204 },
                { "objectclass", new[] { "top", "person", "organizationalPerson", "user" } },
                { "mail", "user@company.com" },
                { "facsimiletelephonenumber", "202-555-1212" },
                { "givenname", "First" },
                { "homedirectory", @"\\FS01\USER01" },
                { "homedrive", "H:" },
                { "homephone", "202-555-1212" },
                { "ipphone", "202-555-1212" },
                { "lastlogon", 130895909137228038 },
                { "lastlogontimestamp", 130904585347417734 },
                { "sn", "Last" },
                { "scriptpath", @"\\FS01\Scripts\logon.bat" },
                { "initials", "C" },
                { "mobile", "202-555-1212" },
                { "info", "A note about the user." },
                { "physicaldeliveryofficename", "Ashburn" },
                { "pager", "202-555-1212" },
                { "postofficebox", 151515 },
                { "postalcode", 20016 },
                { "profilepath", @"\\FS01\Profiles\USER01" },
                { "st", "Maine" },
                { "streetaddress", "123 Any Street" },
                { "telephonenumber", "202-555-1212" },
                { "title", "Software Engineer" },
                { "wwwhomepage", "http://company.local/user01" }
            };
        }

        private Dictionary<string, object> NotAUser()
        {
            var properties = InitializeProperties();

            properties["objectclass"] = new[] { "top", "group" };

            return properties;
        }

        private Dictionary<string, object> ServiceAccount()
        {
            var properties = InitializeProperties();

            properties["samaccountname"] = "account-svc";

            return properties;
        }

        private Dictionary<string, object> UserAccountExpireEmpty()
        {
            var properties = InitializeProperties();

            properties["accountexpires"] = String.Empty;

            return properties;
        }

        private Dictionary<string, object> UserAccountExpireInvalid()
        {
            var properties = InitializeProperties();

            properties["accountexpires"] = -1;

            return properties;
        }
    }
}
