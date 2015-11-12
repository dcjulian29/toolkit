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
    public class GroupTests
    {
        [Fact]
        public void Category_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "CN=Group,CN=Schema,CN=Configuration,DC=company,DC=local";
            var group = new Group(InitializeProperties());

            // Act
            var actual = group.Category;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Changed_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = new DateTime(2015, 10, 28, 3, 39, 34, DateTimeKind.Utc);
            var group = new Group(InitializeProperties());

            // Act
            var actual = group.Changed;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CommonName_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "GROUP01";
            var group = new Group(InitializeProperties());

            // Act
            var actual = group.CommonName;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Created_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = new DateTime(2015, 10, 5, 11, 0, 50, DateTimeKind.Utc);
            var group = new Group(InitializeProperties());

            // Act
            var actual = group.Created;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Ctor_Should_ReturnExpectedResult()
        {
            // Arrange
            var obj = InitializeProperties();

            // Act
            var group = new Group(obj);

            // Assert
            Assert.NotNull(group);
        }

        [Fact]
        public void Ctor_Should_ThrowException_When_NonGroupObject()
        {
            // Arrange
            var obj = NotAGroup();

            // Act/Assert
            Assert.Throws<ArgumentException>(() =>
            {
                var group = new Group(obj);
            });
        }

        [Fact]
        public void Description_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "Group For Stuff";
            var group = new Group(InitializeProperties());

            // Act
            var actual = group.Description;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DisplayName_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "GROUP01";
            var group = new Group(InitializeProperties());

            // Act
            var actual = group.DisplayName;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DistinguishedName_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "CN=GROUP01,CN=Users,DC=company,DC=local";
            var group = new Group(InitializeProperties());

            // Act
            var actual = group.DistinguishedName;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void EmailAddress_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "group@company.com";
            var group = new Group(InitializeProperties());

            // Act
            var actual = group.EmailAddress;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Guid_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = Guid.Parse("cd29418b-45d7-4d55-952e-e4da717172af");
            var group = new Group(InitializeProperties());

            // Act
            var actual = group.Guid;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void IsDistributionList_Should_ReturnTrue()
        {
            // Arrange
            var group = new Group(DistributionList());

            // Act
            var actual = group.IsDistributionList();

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void IsDomainLocal_Should_ReturnTrue()
        {
            // Arrange
            var group = new Group(DomainLocalGroup());

            // Act
            var actual = group.IsDomainLocal();

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void IsGlobal_Should_ReturnTrue()
        {
            // Arrange
            var group = new Group(InitializeProperties());

            // Act
            var actual = group.IsGlobal();

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void IsSecurity_Should_ReturnTrue()
        {
            // Arrange
            var group = new Group(InitializeProperties());

            // Act
            var actual = group.IsSecurity();

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void IsUniversal_Should_ReturnTrue()
        {
            // Arrange
            var group = new Group(UniversalGroup());

            // Act
            var actual = group.IsUniversal();

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void ManagedBy_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "CN=manager1,CN=Users,DC=company,DC=local";
            var group = new Group(InitializeProperties());

            // Act
            var actual = group.ManagedBy;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Members_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = new List<string>()
            {
                "CN=user1,CN=Users,DC=company,DC=local"
            };

            var group = new Group(InitializeProperties());

            // Act
            var actual = group.Members;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Modified_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = new DateTime(2015, 10, 28, 3, 39, 34, DateTimeKind.Utc);
            var group = new Group(InitializeProperties());

            // Act
            var actual = group.Modified;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Name_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "GROUP01";
            var group = new Group(InitializeProperties());

            // Act
            var actual = group.Name;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Notes_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "A note about the group.";
            var group = new Group(InitializeProperties());

            // Act
            var actual = group.Notes;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SamAccountName_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "GROUP01";
            var group = new Group(InitializeProperties());

            // Act
            var actual = group.SamAccountName;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Sid_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "S-1-5-21-1501611499-78517565-1004253924-2105";
            var group = new Group(InitializeProperties());

            // Act
            var actual = group.Sid;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void UpdateSequenceNumberCreated_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = 43640;
            var group = new Group(InitializeProperties());

            // Act
            var actual = group.UpdateSequenceNumberCreated;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void UpdateSequenceNumberCurrent_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = 129204;
            var group = new Group(InitializeProperties());

            // Act
            var actual = group.UpdateSequenceNumberCurrent;

            // Assert
            Assert.Equal(expected, actual);
        }

        private Dictionary<string, object> DistributionList()
        {
            var properties = InitializeProperties();

            properties["grouptype"] = 2;

            return properties;
        }

        private Dictionary<string, object> DomainLocalGroup()
        {
            var properties = InitializeProperties();

            properties["grouptype"] = -2147483644;

            return properties;
        }

        private Dictionary<string, object> InitializeProperties()
        {
            return new Dictionary<string, object>()
            {
                { "objectcategory", "CN=Group,CN=Schema,CN=Configuration,DC=company,DC=local" },
                { "cn", "GROUP01" },
                { "whencreated", DateTime.Parse("10/5/2015 11:00:50", null, DateTimeStyles.AdjustToUniversal) },
                { "description", "Group For Stuff" },
                { "displayname", "GROUP01" },
                { "distinguishedname", "CN=GROUP01,CN=Users,DC=company,DC=local" },
                { "mail", "group@company.com" },
                { "member", "CN=user1,CN=Users,DC=company,DC=local" },
                {
                    "objectguid", new Byte[]
                    {
                        0x8B, 0x41, 0x29, 0xCD, 0xD7, 0x45, 0x55, 0x4D,
                        0x95, 0x2E, 0xE4, 0xDA, 0x71, 0x71, 0x72, 0xAF
                    }
                },
                { "samaccountname", "GROUP01" },
                {
                    "objectsid", new Byte[]
                    {
                        0x01, 0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x05,
                        0x15, 0x00, 0x00, 0x00, 0xEB, 0xC5, 0x80, 0x59,
                        0x3D, 0x15, 0xAE, 0x04, 0xE4, 0xB2, 0xDB, 0x3B,
                        0x39, 0x08, 0x00, 0x00
                    }
                },
                { "managedby", "CN=manager1,CN=Users,DC=company,DC=local" },
                { "whenchanged", DateTime.Parse("10/28/2015 3:39:34", null, DateTimeStyles.AdjustToUniversal) },
                { "name", "GROUP01" },
                { "info", "A note about the group." },
                { "usncreated", (Int64)43640 },
                { "usnchanged", (Int64)129204 },
                { "grouptype", -2147483646 },
                { "objectclass", new[] { "top", "group" } }
            };
        }

        private Dictionary<string, object> NotAGroup()
        {
            var properties = InitializeProperties();

            properties["objectclass"] = new[] { "top", "user" };

            return properties;
        }

        private Dictionary<string, object> UniversalGroup()
        {
            var properties = InitializeProperties();

            properties["grouptype"] = -2147483640;

            return properties;
        }
    }
}
