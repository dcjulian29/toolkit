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
    public class ContactTests
    {
        [Fact]
        public void Category_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "CN=Person,CN=Schema,CN=Configuration,DC=company,DC=local";
            var contact = new Contact(InitializeProperties());

            // Act
            var actual = contact.Category;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Changed_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = new DateTime(2015, 10, 28, 3, 39, 34, DateTimeKind.Utc);
            var contact = new Contact(InitializeProperties());

            // Act
            var actual = contact.Changed;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void City_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "Bethesda";
            var contact = new Contact(InitializeProperties());

            // Act
            var actual = contact.City;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CommonName_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "CONTACT01";
            var contact = new Contact(InitializeProperties());

            // Act
            var actual = contact.CommonName;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Company_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "Company";
            var contact = new Contact(InitializeProperties());

            // Act
            var actual = contact.Company;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Country_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "Aruba";
            var contact = new Contact(InitializeProperties());

            // Act
            var actual = contact.Country;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CountryCode_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = 553;
            var contact = new Contact(InitializeProperties());

            // Act
            var actual = contact.CountryCode;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Created_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = new DateTime(2015, 10, 5, 11, 0, 50, DateTimeKind.Utc);
            var contact = new Contact(InitializeProperties());

            // Act
            var actual = contact.Created;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Ctor_Should_ReturnExpectedResult()
        {
            // Arrange
            var obj = InitializeProperties();

            // Act
            var contact = new Contact(obj);

            // Assert
            Assert.NotNull(contact);
        }

        [Fact]
        public void Ctor_Should_ThrowException_When_NonContactObject()
        {
            // Arrange
            var obj = NotAContact();

            // Act/Assert
            Assert.Throws<ArgumentException>(() =>
            {
                var contact = new Contact(obj);
            });
        }

        [Fact]
        public void Department_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "Accounting";
            var contact = new Contact(InitializeProperties());

            // Act
            var actual = contact.Department;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Description_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "Contact For Stuff";
            var contact = new Contact(InitializeProperties());

            // Act
            var actual = contact.Description;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DisplayName_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "CONTACT01";
            var contact = new Contact(InitializeProperties());

            // Act
            var actual = contact.DisplayName;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DistinguishedName_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "CN=CONTACT01,CN=Users,DC=company,DC=local";
            var contact = new Contact(InitializeProperties());

            // Act
            var actual = contact.DistinguishedName;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void EmailAddress_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "contact@company.com";
            var contact = new Contact(InitializeProperties());

            // Act
            var actual = contact.EmailAddress;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Fax_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "202-555-1212";
            var contact = new Contact(InitializeProperties());

            // Act
            var actual = contact.Fax;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FirstName_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "First";
            var contact = new Contact(InitializeProperties());

            // Act
            var actual = contact.FirstName;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Guid_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = Guid.Parse("cd29418b-45d7-4d55-952e-e4da717172af");
            var contact = new Contact(InitializeProperties());

            // Act
            var actual = contact.Guid;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void HomePhone_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "202-555-1212";
            var contact = new Contact(InitializeProperties());

            // Act
            var actual = contact.HomePhone;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void IpPhone_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "202-555-1212";
            var contact = new Contact(InitializeProperties());

            // Act
            var actual = contact.IpPhone;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void LastName_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "Last";
            var contact = new Contact(InitializeProperties());

            // Act
            var actual = contact.LastName;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Manager_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "CN=manager1,CN=Users,DC=company,DC=local";
            var contact = new Contact(InitializeProperties());

            // Act
            var actual = contact.Manager;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void MiddleInitial_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "C";
            var contact = new Contact(InitializeProperties());

            // Act
            var actual = contact.MiddleInitial;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void MobilePhone_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "202-555-1212";
            var contact = new Contact(InitializeProperties());

            // Act
            var actual = contact.MobilePhone;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Modified_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = new DateTime(2015, 10, 28, 3, 39, 34, DateTimeKind.Utc);
            var contact = new Contact(InitializeProperties());

            // Act
            var actual = contact.Modified;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Name_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "CONTACT01";
            var contact = new Contact(InitializeProperties());

            // Act
            var actual = contact.Name;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Notes_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "A note about the contact.";
            var contact = new Contact(InitializeProperties());

            // Act
            var actual = contact.Notes;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Office_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "Ashburn";
            var contact = new Contact(InitializeProperties());

            // Act
            var actual = contact.Office;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Pager_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "202-555-1212";
            var contact = new Contact(InitializeProperties());

            // Act
            var actual = contact.Pager;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void PoBox_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "151515";
            var contact = new Contact(InitializeProperties());

            // Act
            var actual = contact.PoBox;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void PostalCode_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "20016";
            var contact = new Contact(InitializeProperties());

            // Act
            var actual = contact.PostalCode;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Province_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "Maine";
            var contact = new Contact(InitializeProperties());

            // Act
            var actual = contact.Province;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Region_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "Aruba";
            var contact = new Contact(InitializeProperties());

            // Act
            var actual = contact.Region;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void State_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "Maine";
            var contact = new Contact(InitializeProperties());

            // Act
            var actual = contact.State;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void StreetAddress_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "123 Any Street";
            var contact = new Contact(InitializeProperties());

            // Act
            var actual = contact.StreetAddress;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TelephoneNumber_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "202-555-1212";
            var contact = new Contact(InitializeProperties());

            // Act
            var actual = contact.TelephoneNumber;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Title_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "Software Engineer";
            var contact = new Contact(InitializeProperties());

            // Act
            var actual = contact.Title;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void UpdateSequenceNumberCreated_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = 43640;
            var contact = new Contact(InitializeProperties());

            // Act
            var actual = contact.UpdateSequenceNumberCreated;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void UpdateSequenceNumberCurrent_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = 129204;
            var contact = new Contact(InitializeProperties());

            // Act
            var actual = contact.UpdateSequenceNumberCurrent;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void WebSite_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "http://company.local/contact01";
            var contact = new Contact(InitializeProperties());

            // Act
            var actual = contact.WebSite;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Zip_Should_ReturnExpectedResult()
        {
            // Arrange
            var expected = "20016";
            var contact = new Contact(InitializeProperties());

            // Act
            var actual = contact.Zip;

            // Assert
            Assert.Equal(expected, actual);
        }

        private Dictionary<string, object> InitializeProperties()
        {
            return new Dictionary<string, object>()
            {
                { "objectcategory", "CN=Person,CN=Schema,CN=Configuration,DC=company,DC=local" },
                { "l", "Bethesda" },
                { "cn", "CONTACT01" },
                { "company", "Company" },
                { "co", "Aruba" },
                { "countrycode", 553 },
                { "whencreated", DateTime.Parse("10/5/2015 11:00:50", null, DateTimeStyles.AdjustToUniversal) },
                { "department", "Accounting" },
                { "description", "Contact For Stuff" },
                { "displayname", "CONTACT01" },
                { "distinguishedname", "CN=CONTACT01,CN=Users,DC=company,DC=local" },
                { "mail", "contact@company.com" },
                { "facsimiletelephonenumber", "202-555-1212" },
                { "givenname", "First" },
                {
                    "objectguid", new Byte[]
                    {
                        0x8B, 0x41, 0x29, 0xCD, 0xD7, 0x45, 0x55, 0x4D,
                        0x95, 0x2E, 0xE4, 0xDA, 0x71, 0x71, 0x72, 0xAF
                    }
                },
                { "homephone", "202-555-1212" },
                { "ipphone", "202-555-1212" },
                { "sn", "Last" },
                { "manager", "CN=manager1,CN=Users,DC=company,DC=local" },
                { "initials", "C" },
                { "mobile", "202-555-1212" },
                { "whenchanged", DateTime.Parse("10/28/2015 3:39:34", null, DateTimeStyles.AdjustToUniversal) },
                { "name", "CONTACT01" },
                { "info", "A note about the contact." },
                { "physicaldeliveryofficename", "Ashburn" },
                { "pager", "202-555-1212" },
                { "postofficebox", 151515 },
                { "postalcode", 20016 },
                { "st", "Maine" },
                { "streetaddress", "123 Any Street" },
                { "telephonenumber", "202-555-1212" },
                { "title", "Software Engineer" },
                { "usncreated", (Int64)43640 },
                { "usnchanged", (Int64)129204 },
                { "wwwhomepage", "http://company.local/contact01" },
                { "objectclass", new[] { "top", "person", "organizationalPerson", "contact" } }
            };
        }

        private Dictionary<string, object> NotAContact()
        {
            var properties = InitializeProperties();

            properties["objectclass"] = new[] { "top", "group" };

            return properties;
        }
    }
}
