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
    public class ComputerTests
    {
        [Fact]
        public void AccountExpires_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = new DateTime(2015, 12, 2, 5, 0, 0, DateTimeKind.Utc);
            var computer = new Computer(InitializeProperties());

            // Act
            var actual = computer.AccountExpires;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AccountExpires_Should_ReturnsMaxDateTime_When_EmptyString()
        {
            // Arrange
            var expected = DateTime.MaxValue;
            var computer = new Computer(ComputerAccountExpireEmpty());

            // Act
            var actual = computer.AccountExpires;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void BadPasswordCount_Should_ReturnExpectedResult()
        {
            // Arrange
            const int expected = 2;
            var computer = new Computer(InitializeProperties());

            // Act
            var actual = computer.BadPasswordCount;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void BadPasswordTime_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = DateTime.Parse("2015-10-05T11:00:51.0451845", null, DateTimeStyles.AdjustToUniversal);
            var computer = new Computer(InitializeProperties());

            // Act
            var actual = computer.BadPasswordTime;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Category_Should_ReturnExpectedResult()
        {
            // Arrange
            const string expected = "CN=Computer,CN=Schema,CN=Configuration,DC=company,DC=local";
            var computer = new Computer(InitializeProperties());

            // Act
            var actual = computer.Category;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Changed_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = new DateTime(2015, 10, 28, 15, 39, 34, DateTimeKind.Utc);
            var computer = new Computer(InitializeProperties());

            // Act
            var actual = computer.Changed;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CommonName_Should_ReturnExpectedResult()
        {
            // Arrange
            const string expected = "COMPUTER01";
            var computer = new Computer(InitializeProperties());

            // Act
            var actual = computer.CommonName;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ComputerAccountControl_Should_ReturnExpectedResult()
        {
            // Arrange
            const int expected = 4096;
            var computer = new Computer(InitializeProperties());

            // Act
            var actual = computer.ComputerAccountControl;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CountryCode_Should_ReturnExpectedResult()
        {
            // Arrange
            const int expected = 42;
            var computer = new Computer(InitializeProperties());

            // Act
            var actual = computer.CountryCode;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Created_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = new DateTime(2015, 10, 5, 11, 0, 50);
            var computer = new Computer(InitializeProperties());

            // Act
            var actual = computer.Created;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Ctor_Should_ReturnExpectedResult()
        {
            // Arrange
            var obj = InitializeProperties();

            // Act
            var computer = new Computer(obj);

            // Assert
            Assert.NotNull(computer);
        }

        [Fact]
        public void Ctor_Should_ThrowException_When_NonComputerObject()
        {
            // Arrange
            var obj = NotAComputer();

            // Act/Assert
            Assert.Throws<ArgumentException>(() => _ = new Computer(obj));
        }

        [Fact]
        public void Description_Should_ReturnExpectedResult()
        {
            // Arrange
            const string expected = "Computer For Stuff";
            var computer = new Computer(InitializeProperties());

            // Act
            var actual = computer.Description;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Disabled_Should_ReturnExpectedResult()
        {
            // Arrange
            const bool expected = false;
            var computer = new Computer(InitializeProperties());

            // Act
            var actual = computer.Disabled;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DistinguishedName_Should_ReturnExpectedResult()
        {
            // Arrange
            const string expected = "CN=COMPUTER01,CN=Computers,DC=company,DC=local";
            var computer = new Computer(InitializeProperties());

            // Act
            var actual = computer.DistinguishedName;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DnsHostName_Should_ReturnExpectedResult()
        {
            // Arrange
            const string expected = "computer01.company.local";
            var computer = new Computer(InitializeProperties());

            // Act
            var actual = computer.DnsHostName;

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

            var computer = new Computer(InitializeProperties());

            // Act
            var actual = computer.Groups;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Guid_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = Guid.Parse("cd29418b-45d7-4d55-952e-e4da717172af");
            var computer = new Computer(InitializeProperties());

            // Act
            var actual = computer.Guid;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void IsDomainController_Should_ReturnFalse_When_NotDomainController()
        {
            // Arrange
            var computer = new Computer(InitializeProperties());

            // Act
            var actual = computer.IsDomainController();

            // Assert
            Assert.False(actual);
        }

        [Fact]
        public void IsDomainController_Should_ReturnTrue_When_DomainController()
        {
            // Arrange
            var computer = new Computer(DomainController());

            // Act
            var actual = computer.IsDomainController();

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void IsServer_Should_ReturnTrue_When_DomainContoller()
        {
            // Arrange
            var computer = new Computer(DomainController());

            // Act
            var actual = computer.IsServer();

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void IsServer_Should_ReturnTrue_When_ServerOs()
        {
            // Arrange
            var computer = new Computer(ComputerServer());

            // Act
            var actual = computer.IsServer();

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void IsWorkstation_Should_ReturnExpectedResult()
        {
            // Arrange
            var computer = new Computer(InitializeProperties());

            // Act
            var actual = computer.IsWorkstation();

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void LastLogoff_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = DateTime.Parse("2015-10-29T00:24:10.5124008", null, DateTimeStyles.AdjustToUniversal);
            var computer = new Computer(InitializeProperties());

            // Act
            var actual = computer.LastLogoff;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void LastLogon_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = DateTime.Parse("2015-10-27T00:24:04.3524715", null, DateTimeStyles.AdjustToUniversal);
            var computer = new Computer(InitializeProperties());

            // Act
            var actual = computer.LastLogon;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Location_Should_ReturnExpectedResult()
        {
            // Arrange
            const string expected = "Ashburn";
            var computer = new Computer(InitializeProperties());

            // Act
            var actual = computer.Location;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void LogonCount_Should_ReturnExpectedResult()
        {
            // Arrange
            const int expected = 60;
            var computer = new Computer(InitializeProperties());

            // Act
            var actual = computer.LogonCount;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ManagedBy_Should_ReturnExpectedResult()
        {
            // Arrange
            const string expected = "CN=user1,CN=Users,DC=company,DC=local";
            var computer = new Computer(InitializeProperties());

            // Act
            var actual = computer.ManagedBy;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Modified_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = new DateTime(2015, 10, 28, 15, 39, 34);
            var computer = new Computer(InitializeProperties());

            // Act
            var actual = computer.Modified;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Name_Should_ReturnExpectedResult()
        {
            // Arrange

            const string expected = "COMPUTER01";
            var computer = new Computer(InitializeProperties());

            // Act
            var actual = computer.Name;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void OperatingSystem_Should_ReturnExpectedResult()
        {
            // Arrange
            const string expected = "Windows 10 Pro";
            var computer = new Computer(InitializeProperties());

            // Act
            var actual = computer.OperatingSystem;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void OperatingSystemServicePack_Should_ReturnExpectedResult()
        {
            // Arrange
            const string expected = "1";
            var computer = new Computer(InitializeProperties());

            // Act
            var actual = computer.OperatingSystemServicePack;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void OperatingSystemVersion_Should_ReturnExpectedResult()
        {
            // Arrange
            const string expected = "10.0 (10240)";
            var computer = new Computer(InitializeProperties());

            // Act
            var actual = computer.OperatingSystemVersion;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void PasswordLastSet_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = DateTime.Parse("2015-10-05T11:00:51.0459354", null, DateTimeStyles.AdjustToUniversal);
            var computer = new Computer(InitializeProperties());

            // Act
            var actual = computer.PasswordLastSet;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void PrimaryGroupId_Should_ReturnExpectedResult()
        {
            // Arrange
            const int expected = 515;
            var computer = new Computer(InitializeProperties());

            // Act
            var actual = computer.PrimaryGroupId;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SamAccountName_Should_ReturnExpectedResult()
        {
            // Arrange
            const string expected = "COMPUTER01$";
            var computer = new Computer(InitializeProperties());

            // Act
            var actual = computer.SamAccountName;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ServicePrincipalNames_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = new List<string>()
            {
                "WSMAN/COMPUTER01",
                "WSMAN/COMPUTER01.company.local",
                "TERMSRV/COMPUTER01",
                "TERMSRV/COMPUTER01.company.local",
                "HOST/COMPUTER01",
                "HOST/COMPUTER01.company.local"
            };

            var computer = new Computer(InitializeProperties());

            // Act
            var actual = computer.ServicePrincipalNames;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Sid_Should_ReturnExpectedResult()
        {
            // Arrange
            const string expected = "S-1-5-21-1501611499-78517565-1004253924-2105";
            var computer = new Computer(InitializeProperties());

            // Act
            var actual = computer.Sid;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void UpdateSequenceNumberCreated_Should_ReturnExpectedResult()
        {
            // Arrange
            const int expected = 43640;
            var computer = new Computer(InitializeProperties());

            // Act
            var actual = computer.UpdateSequenceNumberCreated;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void UpdateSequenceNumberCurrent_Should_ReturnExpectedResult()
        {
            // Arrange
            const int expected = 129204;
            var computer = new Computer(InitializeProperties());

            // Act
            var actual = computer.UpdateSequenceNumberCurrent;

            // Assert
            Assert.Equal(expected, actual);
        }

        private Dictionary<string, object> ComputerAccountExpireEmpty()
        {
            var properties = InitializeProperties();

            properties["accountexpires"] = string.Empty;

            return properties;
        }

        private Dictionary<string, object> ComputerServer()
        {
            var properties = InitializeProperties();

            properties["operatingsystem"] = "Windows Server 2012 R2 Standard";
            properties["operatingsystemversion"] = "6.3 (9600)";

            return properties;
        }

        private Dictionary<string, object> DomainController()
        {
            var properties = InitializeProperties();

            properties["useraccountcontrol"] = 532480;
            properties["operatingsystem"] = "Some Computer OS";

            return properties;
        }

        private Dictionary<string, object> InitializeProperties()
        {
            return new Dictionary<string, object>()
            {
                { "accountexpires", 130935060000000000 },
                { "badpwdcount", 2 },
                { "badpasswordtime", 130885164510451845 },
                { "objectcategory", "CN=Computer,CN=Schema,CN=Configuration,DC=company,DC=local" },
                { "cn", "COMPUTER01" },
                { "useraccountcontrol", 4096 },
                { "countrycode", 42 },
                { "whencreated", DateTime.Parse("10/5/2015 11:00:50", null, DateTimeStyles.AdjustToUniversal) },
                { "description", "Computer For Stuff" },
                { "distinguishedname", "CN=COMPUTER01,CN=Computers,DC=company,DC=local" },
                { "dnshostname", "computer01.company.local" },
                { "memberof", "CN=group1,CN=Users,DC=company,DC=local" },
                { "objectguid", new byte[]
                    {
                        0x8B, 0x41, 0x29, 0xCD, 0xD7, 0x45, 0x55, 0x4D,
                        0x95, 0x2E, 0xE4, 0xDA, 0x71, 0x71, 0x72, 0xAF
                    }
                },
                { "lastlogoff", 130905518505124008 },
                { "lastlogontimestamp", 130903790443524715 },
                { "location", "Ashburn" },
                { "logoncount", 60 },
                { "managedby", "CN=user1,CN=Users,DC=company,DC=local" },
                { "whenchanged", DateTime.Parse("10/28/2015 15:39:34", null, DateTimeStyles.AdjustToUniversal) },
                { "name", "COMPUTER01" },
                { "operatingsystem", "Windows 10 Pro" },
                { "operatingsystemservicepack", "1" },
                { "operatingsystemversion", "10.0 (10240)" },
                { "pwdlastset", 130885164510459354 },
                { "primarygroupid", 515 },
                { "samaccountname", "COMPUTER01$" },
                { "serviceprincipalname", new[]
                    {
                        "WSMAN/COMPUTER01",
                        "WSMAN/COMPUTER01.company.local",
                        "TERMSRV/COMPUTER01",
                        "TERMSRV/COMPUTER01.company.local",
                        "HOST/COMPUTER01",
                        "HOST/COMPUTER01.company.local"
                    }
                },
                { "objectsid", new byte[]
                    {
                        0x01, 0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x05,
                        0x15, 0x00, 0x00, 0x00, 0xEB, 0xC5, 0x80, 0x59,
                        0x3D, 0x15, 0xAE, 0x04, 0xE4, 0xB2, 0xDB, 0x3B,
                        0x39, 0x08, 0x00, 0x00
                    }
                },
                { "usncreated", (long)43640 },
                { "usnchanged", (long)129204 },
                { "objectclass", new[] { "top", "person", "user", "computer" } }
            };
        }

        private Dictionary<string, object> NotAComputer()
        {
            var properties = InitializeProperties();

            properties["objectclass"] = new[] { "top", "person", "user" };

            return properties;
        }
    }
}
