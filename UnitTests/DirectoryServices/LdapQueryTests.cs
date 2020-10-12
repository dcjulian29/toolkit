using System.Diagnostics.CodeAnalysis;
using ToolKit.DirectoryServices;
using Xunit;

namespace UnitTests.DirectoryServices
{
    [SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1600:ElementsMustBeDocumented",
        Justification = "Test Suites do not need XML Documentation.")]
    public class LdapQueryTests
    {
        [Fact]
        public void Filter_Should_ReturnExpectedResult_When_ConstructorProvidedWithNoFilter()
        {
            // Arrange
            var expected = "(objectClass=*)";

            // Act
            var query = new LdapQuery("cn=Users,dc=company,dc=com");

            // Assert
            Assert.Equal(expected, query.Filter);
        }

        [Fact]
        public void Filter_Should_ReturnExpectedResult_When_ConstructorProvidedWithStringFilter()
        {
            // Arrange
            var expected = "(sn=Jackson*)";

            // Act
            var query = new LdapQuery("cn=Users,dc=company,dc=com", "(sn=Jackson*)");

            // Assert
            Assert.Equal(expected, query.Filter);
        }

        [Fact]
        public void Path_Should_ReturnExpectedResult_When_ConstructorProvidedWithDistinguishedNameGlobalCatalog()
        {
            // Arrange
            var expected = "GC://company.com/cn=Users,dc=company,dc=com";
            var dn = new DistinguishedName("cn=Users,dc=company,dc=com");

            // Act
            var query = new LdapQuery(dn, true);

            // Assert
            Assert.Equal(expected, query.Path);
        }

        [Fact]
        public void Path_Should_ReturnExpectedResult_When_ConstructorProvidedWithDistinguishedNameNoGlobalCatalog()
        {
            // Arrange
            var expected = "LDAP://company.com/cn=Users,dc=company,dc=com";
            var dn = new DistinguishedName("cn=Users,dc=company,dc=com");

            // Act
            var query = new LdapQuery(dn, false);

            // Assert
            Assert.Equal(expected, query.Path);
        }

        [Fact]
        public void Path_Should_ReturnExpectedResult_When_ConstructorProvidedWithStringPath()
        {
            // Arrange
            var expected = "cn=Users,dc=company,dc=com";

            // Act
            var query = new LdapQuery("cn=Users,dc=company,dc=com");

            // Assert
            Assert.Equal(expected, query.Path);
        }
    }
}
