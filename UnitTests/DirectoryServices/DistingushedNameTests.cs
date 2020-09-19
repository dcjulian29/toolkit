using System;
using ToolKit.DirectoryServices;
using Xunit;

namespace UnitTests.DirectoryServices
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1600:ElementsMustBeDocumented",
        Justification = "Test Suites do not need XML Documentation.")]
    public class DistingushedNameTests
    {
        [Fact]
        public void Child_Should_BuildCorrectDistinguishedName()
        {
            // Arrange
            var fullDN = new DistinguishedName("OU=Users,OU=HDQRK,DC=example,DC=com");
            var baseDN = new DistinguishedName("DC=example,DC=com");

            // Act
            var childDN = baseDN.Child("OU=Users,OU=HDQRK");

            // Assert
            Assert.Equal(fullDN.ToString(), childDN.ToString());
        }

        [Fact]
        public void CommonName_Should_ReturnNull_When_CommonNameIsNotPresent()
        {
            // Arrange
            var dn = new DistinguishedName("OU=tstou,DC=corp,DC=example,DC=com");

            // Act

            // Assert
            Assert.Null(dn.CommonName);
        }

        [Fact]
        public void CommonName_Should_ReturnValue_When_DistinguishedNameHasOne()
        {
            // Arrange
            var dn = new DistinguishedName("CN=cpcar834,OU=users,OU=tstou,DC=corp,DC=example,DC=com");

            // Act

            // Assert
            Assert.Equal("cpcar834", dn.CommonName);
        }

        [Fact]
        public void Constructor_Should_ParseCorrectly_When_DistinguishedNameContainsEscapedSpace()
        {
            // Arrange
            var dn = new DistinguishedName("CN=\"Julian\\ Easterling\"");

            // Act

            // Assert
            Assert.Equal("CN=Julian Easterling", dn.ToString());
        }

        [Fact]
        public void Constructor_Should_ParseCorrectly_When_DistinguishedNameContainsEscapedSpecialCharacters()
        {
            // Arrange
            var dn = new DistinguishedName(@"OU=""\,\=\+\<\>\#\;\\\ \\""");

            // Act

            // Assert
            Assert.Equal(@"OU=\,\=\+\<\>\#\;\\ \\", dn.ToString());
        }

        [Fact]
        public void Constructor_Should_ParseCorrectly_When_DistinguishedNameContainsHexEncodedBinaryValue()
        {
            // Arrange
            var dn = new DistinguishedName("CN=#4A756C69616E");

            // Act

            // Assert
            Assert.Equal("CN=Julian", dn.ToString());
        }

        [Fact]
        public void Constructor_Should_ParseCorrectly_When_DistinguishedNameContainsHexEncodedBinaryValueAtBeginning()
        {
            // Arrange
            var dn = new DistinguishedName("CN=#4A756C69616E,CN=Users,DC=example,DC=com");

            // Act

            // Assert
            Assert.Equal("CN=Julian,CN=Users,DC=example,DC=com", dn.ToString());
        }

        [Fact]
        public void Constructor_Should_ParseCorrectly_When_DistinguishedNameContainsHexEncodedBinaryValueAtEnd()
        {
            // Arrange
            var dn = new DistinguishedName("CN=Julian,CN=Users,DC=example,DC=#636f6d");

            // Act

            // Assert
            Assert.Equal("CN=Julian,CN=Users,DC=example,DC=com", dn.ToString());
        }

        [Fact]
        public void Constructor_Should_ParseCorrectly_When_DistinguishedNameContainsHexEncodedBinaryValueInMiddle()
        {
            // Arrange
            var dn = new DistinguishedName("CN=Julian,CN=Users,DC=#6578616D706C65,DC=com");

            // Act

            // Assert
            Assert.Equal("CN=Julian,CN=Users,DC=example,DC=com", dn.ToString());
        }

        [Fact]
        public void Constructor_Should_ParseCorrectly_When_DistinguishedNameContainsMultvaluedRelativeDN()
        {
            // Arrange
            var dn = new DistinguishedName("CN=Dzingai + SN=Smith,CN=Users,DC=example,DC=com");

            // Act

            // Assert
            Assert.Equal("CN=Dzingai+SN=Smith,CN=Users,DC=example,DC=com", dn.ToString());
        }

        [Fact]
        public void Constructor_Should_ParseCorrectly_When_DistinguishedNameContainsOID()
        {
            // Arrange
            var dn1 = new DistinguishedName("OID.3.43.128=Keith");
            var dn2 = new DistinguishedName("3.43.128=Keith");
            var dn3 = new DistinguishedName("oid.3.43.128=Keith");

            // Act

            // Assert
            Assert.Equal(dn1, dn2);
            Assert.Equal(dn2, dn3);
        }

        [Fact]
        public void Constructor_Should_ParseCorrectly_When_DistinguishedNameContainsOIDWithSpaces()
        {
            // Arrange
            var dn1 = new DistinguishedName("   32.21.0   =    Jay");
            var dn2 = new DistinguishedName("OID.32.21.0=Jay");
            var dn3 = new DistinguishedName("oid.32.21.0   =  Jay     ");

            // Act

            // Assert
            Assert.Equal(dn1, dn2);
            Assert.Equal(dn2, dn3);
        }

        [Fact]
        public void Constructor_Should_ParseCorrectly_When_DistinguishedNameContainsOIDWithZeroPart()
        {
            // Arrange
            var dn = new DistinguishedName("34.0.21=Jay");

            // Act

            // Assert
            Assert.Equal("34.0.21=Jay", dn.ToString());
        }

        [Fact]
        public void Constructor_Should_ParseCorrectly_When_DistinguishedNameContainsQuotedString()
        {
            // Arrange
            var dn = new DistinguishedName("CN=Julian,OU=\"Users And Computers\",OU=HDQRK");

            // Act

            // Assert
            Assert.Equal("CN=Julian,OU=Users And Computers,OU=HDQRK", dn.ToString());
        }

        [Fact]
        public void Constructor_Should_ParseCorrectly_When_DistinguishedNameContainsUnescapedSpecialCharacters()
        {
            // Arrange
            var dn = new DistinguishedName(@"OU="",=+<>#;""");

            // Act

            // Assert
            Assert.Equal(@"OU=\,\=\+\<\>\#\;", dn.ToString());
        }

        [Fact]
        public void Constructor_Should_ParseCorrectly_When_DistinguishedNameDoesNotReallyContainsAnOID()
        {
            // Arrange
            var dn = new DistinguishedName("OID33A2=Jay");

            // Act

            // Assert
            Assert.Equal("OID33A2=Jay", dn.ToString());
        }

        [Fact]
        public void Constructor_Should_ParseCorrectly_When_DistinguishedNameHasBlankValue()
        {
            // Arrange
            var dn = new DistinguishedName("CN=,OU=People,DC=corp,DC=example,DC=com");

            // Act

            // Assert
            Assert.Equal("CN=,OU=People,DC=corp,DC=example,DC=com", dn.ToString());
        }

        [Fact]
        public void Constructor_Should_ParseCorrectly_When_DistinguishedNameHasExtraSpaces()
        {
            // Arrange
            var dn = new DistinguishedName("    CN    =   TestUser  , OU =  People,DC   =   example,DC = com");

            // Act

            // Assert
            Assert.Equal("CN=TestUser,OU=People,DC=example,DC=com", dn.ToString());
        }

        [Fact]
        public void Constructor_Should_ParseCorrectly_When_DistinguishedNameHasSemicolons()
        {
            // Arrange
            var dn = new DistinguishedName("CN=TestUser;OU=People,DC=hdq,DC=corp;DC=example;DC=com");

            // Act

            // Assert
            Assert.Equal("CN=TestUser,OU=People,DC=hdq,DC=corp,DC=example,DC=com", dn.ToString());
        }

        [Fact]
        public void Constructor_Should_ParseCorrectly_When_DistinguishedNameIsAnEmptyString()
        {
            // Arrange
            var dn = new DistinguishedName(string.Empty);

            // Act

            // Assert
            Assert.Equal(string.Empty, dn.ToString());
        }

        [Fact]
        public void Constructor_Should_ParseCorrectly_When_ProvidedWithDCComponents()
        {
            // Arrange
            var dn = new DistinguishedName("CN=TestUser,OU=People,DC=hdq,DC=corp,DC=example,DC=com");

            // Act

            // Assert
            Assert.Equal("CN=TestUser,OU=People,DC=hdq,DC=corp,DC=example,DC=com", dn.ToString());
        }

        [Fact]
        public void Constructor_Should_ParseCorrectly_When_ProvidedWithoutDCComponents()
        {
            // Arrange
            var dn = new DistinguishedName("CN=TestUser,OU=People,O=Example Inc.,C=US");

            // Act

            // Assert
            Assert.Equal("CN=TestUser,OU=People,O=Example Inc.,C=US", dn.ToString());
        }

        [Fact]
        public void Constructor_Should_ParseCorrectly_When_VariousDNCausedParsingIssues()
        {
            // Arrange
            var listOfDistinguishedNames = new string[]
            {
                "CN=gen-boslwdf&b,OU=Users,OU=BOSLW,DC=fs,DC=corp,DC=example,DC=com",
                "CN=Exchange Administrators,OU=MVCI,OU=Exchange 5.5 Directory Objects,DC=mv,DC=corp,DC=example,DC=com",
                "CN=GPO-SUBJW-Restricted Local Drives,OU=Groups,OU=SUBJW,DC=int,DC=corp,DC=example,DC=com",
                "CN=DF&B - International - RHRS - Managed?Franchise,OU=Exchange Distribution Groups,DC=hdq,DC=corp,DC=example,DC=com",
                "CN=EBC\\, VA Northern,OU=Exchange Distribution Groups,DC=hdq,DC=corp,DC=example,DC=com",
                "CN=vquga140,OU=Users,OU=HDQRK,DC=hdq,DC=corp,DC=example,DC=com",
                "CN=jlmu1007,OU=Users,OU=SXFCZ,DC=int,DC=corp,DC=example,DC=com",
                "CN=AGRP-AD-West-RSM,OU=Administrative Global Groups,OU=Administrative Groups,DC=corp,DC=example,DC=com",
                "CN=DL - BackUp Exec (CTDCA),OU=Exchange Distribution Groups,DC=hdq,DC=corp,DC=example,DC=com",
                "CN=dl-wst26,OU=Exchange Distribution Groups,DC=hdq,DC=corp,DC=example,DC=com",
                "CN=UAGRP-AD-Rit-RSM,OU=Administrative Universal Groups,OU=Administrative Groups,DC=corp,DC=example,DC=com",
                "OU=Administrative Universal Groups,OU=Administrative Groups,DC=corp,DC=example,DC=com",
                "OU=TSTOU,DC=corp,DC=example,DC=COM",
                "OU=ENTLOG,DC=MICROSOFT,DC=COM",
                "DC=corp,DC=example,DC=COM",
                "DC=example,DC=COM",
                "DC=CORP,DC=LOCAL",
                "CN=Xerox Document Imaging (WERCC),OU=Users,OU=TSTOU,DC=hdq,DC=corp,DC=example,DC=com"
            };

            // Act

            // Assert
            foreach (var dn in listOfDistinguishedNames)
            {
                Assert.Equal(dn, DistinguishedName.Parse(dn).ToString());
            }
        }

        [Fact]
        public void Constructor_Should_ThrowException_When_AttributeBeginsWithNumber()
        {
            // Arrange

            // Act

            // Assert
            Assert.Throws<InvalidDistinguishedNameException>(() => _ = new DistinguishedName("3N=TestUser"));
        }

        [Fact]
        public void Constructor_Should_ThrowException_When_DistinguishedNameIsMalformed()
        {
            // Arrange

            // Act

            // Assert
            Assert.Throws<InvalidDistinguishedNameException>(
                () => _ = new DistinguishedName("CN=TestUser,People,DC=example,DC=com"));
        }

        [Fact]
        public void Constructor_Should_ThrowException_When_InvalidCharsInHexEncodedBinaryValue()
        {
            // Arrange

            // Act

            // Assert
            Assert.Throws<ArgumentException>(
                () => _ = new DistinguishedName("CN=#34fer4,CN=Users,DC=example,DC=com"));
        }

        [Fact]
        public void Constructor_Should_ThrowException_When_InvalidHexEncodedBinaryValueLength()
        {
            // Arrange

            // Act

            // Assert
            Assert.Throws<ArgumentException>(() => _ = new DistinguishedName("CN=#35fe1,CN=Users,DC=example,DC=com"));
        }

        [Fact]
        public void Constructor_Should_ThrowException_When_OIDHasAdjacentPeriods()
        {
            // Arrange

            // Act

            // Assert
            Assert.Throws<InvalidDistinguishedNameException>(() => _ = new DistinguishedName("34..32.15=Jay"));
        }

        [Fact]
        public void Constructor_Should_ThrowException_When_OIDHasLeadingZero()
        {
            // Arrange

            // Act

             // Assert
            Assert.Throws<InvalidDistinguishedNameException>(() => _ = new DistinguishedName("03.23.1=Jay"));
        }

        [Fact]
        public void Constructor_Should_ThrowException_When_OIDHasTrailingPeriod()
        {
            // Arrange

            // Act

            // Assert
            Assert.Throws<InvalidDistinguishedNameException>(() => _ = new DistinguishedName("OID.34.54.15.=Jay"));
        }

        [Fact]
        public void Constructor_Should_ThrowException_When_OIDIsMixedCase()
        {
            // Arrange

            // Act

            // Assert
            Assert.Throws<InvalidDistinguishedNameException>(() => _ = new DistinguishedName("oId.3.23.1=Jay"));
        }

        [Fact]
        public void Constructor_Should_ThrowException_When_QuotedStringIsUnterminated()
        {
            // Arrange

            // Act

            // Assert
            Assert.Throws<InvalidDistinguishedNameException>(
                () => _ = new DistinguishedName("CN=\"Julian\\ Easterling"));
        }

        [Fact]
        public void Contains_Should_ReturnFalse_WhenOneDistingusihedNameIsNotContainedWithinAnother()
        {
            // Arrange
            var dn1 = new DistinguishedName("CN=TestUser,OU=People,DC=example,DC=com");
            var dn2 = new DistinguishedName("OU=People,DC=example,DC=com");
            var dn3 = new DistinguishedName("DC=example,DC=com");

            // Act

            // Assert
            Assert.False(dn1.Contains(dn2));
            Assert.False(dn1.Contains(dn3));
            Assert.False(dn2.Contains(dn3));
        }

        [Fact]
        public void Contains_Should_ReturnTrue_WhenOneDistingusihedNameIsContainedWithinAnother()
        {
            // Arrange
            var dn1 = new DistinguishedName("CN=TestUser,OU=People,DC=example,DC=com");
            var dn2 = new DistinguishedName("OU=People,DC=example,DC=com");
            var dn3 = new DistinguishedName("DC=example,DC=com");

            // Act

            // Assert
            Assert.True(dn2.Contains(dn1));
            Assert.True(dn3.Contains(dn1));
            Assert.True(dn3.Contains(dn2));
        }

        [Fact]
        public void DnsName_Should_ReturnCorrectlyParsedValue()
        {
            // Arrange
            var dn = new DistinguishedName("OU=TSTOU,DC=corp,DC=example,DC=COM");

            // Act

            // Assert
            Assert.Equal("corp.example.com", dn.DnsDomain);
        }

        [Fact]
        public void DomainRoot_Should_ReturnCorrectlyParsedValue()
        {
            // Arrange
            var dn = new DistinguishedName("OU=TSTOU,DC=corp,DC=example,DC=COM");

            // Act

            // Assert
            Assert.Equal("DC=corp,DC=example,DC=COM", dn.DomainRoot);
        }

        [Fact]
        public void EqualsMethod_Should_ReturnFalseWithNonEqualDistinguishedNames()
        {
            // Arrange
            var dn1 = new DistinguishedName("Cn=Dave,oU=pEoPle,DC=example,Dc=cOM");
            var dn2 = new DistinguishedName("CN=david,OU=people,DC=example,DC=com");

            // Act
            var result = dn1.Equals(dn2);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void EqualsMethod_Should_ReturnTrueWithEqualDistinguishedNames()
        {
            // Arrange
            var dn1 = new DistinguishedName("Cn=Dave,oU=pEoPle,DC=example,Dc=cOM");
            var dn2 = new DistinguishedName("CN=dave,OU=people,DC=example,DC=com");

            // Act
            var result = dn1.Equals(dn2);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void EqualsOperator_Should_ReturnFalseWithNonEqualDistinguishedNames()
        {
            // Arrange
            var dn1 = new DistinguishedName("Cn=Dave,oU=pEoPle,DC=example,Dc=cOM");
            var dn2 = new DistinguishedName("CN=david,OU=people,DC=example,DC=com");

            // Act
            var result = dn1 == dn2;

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void EqualsOperator_Should_ReturnTrueWithEqualDistinguishedNames()
        {
            // Arrange
            var dn1 = new DistinguishedName("Cn=Dave,oU=pEoPle,DC=example,Dc=cOM");
            var dn2 = new DistinguishedName("CN=dave,OU=people,DC=example,DC=com");

            // Act
            var result = dn1 == dn2;

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void GcPath_Should_ReturnCorrectlyParsedValue()
        {
            // Arrange
            var dn = new DistinguishedName("OU=TSTOU,DC=corp,DC=example,DC=COM");

            // Act

            // Assert
            Assert.Equal("GC://corp.example.com/OU=TSTOU,DC=corp,DC=example,DC=COM", dn.GcPath);
        }

        [Fact]
        public void HashCode_Should_BeEqualWithEqualDistinguishedNames()
        {
            // Arrange
            var dn1 = new DistinguishedName("Cn=Dave,oU=pEoPle,DC=example,Dc=cOM");
            var dn2 = new DistinguishedName("CN=dave,OU=people,DC=example,DC=com");

            // Act

            // Assert
            Assert.Equal(dn1.GetHashCode(), dn2.GetHashCode());
        }

        [Fact]
        public void HashCode_Should_BeNotEqualWithNonEqualDistinguishedNames()
        {
            // Arrange
            var dn1 = new DistinguishedName("Cn=Dave,oU=pEoPle,DC=example,Dc=cOM");
            var dn2 = new DistinguishedName("CN=david,OU=people,DC=example,DC=com");

            // Act

            // Assert
            Assert.NotEqual(dn1.GetHashCode(), dn2.GetHashCode());
        }

        [Fact]
        public void LdapPath_Should_ReturnCorrectlyParsedValue()
        {
            // Arrange
            var dn = new DistinguishedName("OU=TSTOU,DC=corp,DC=example,DC=COM");

            // Act

            // Assert
            Assert.Equal("LDAP://corp.example.com/OU=TSTOU,DC=corp,DC=example,DC=COM", dn.LdapPath);
        }

        [Fact]
        public void LdapPath_Should_ReturnCorrectlyParsedValue_With_ExplicitLdapServer()
        {
            // Arrange & Act
            var dn = new DistinguishedName("OU=TSTOU,DC=corp,DC=example,DC=COM")
            {
                LdapServer = "hdqrkhdqdc2"
            };

            // Assert
            Assert.Equal("LDAP://hdqrkhdqdc2/OU=TSTOU,DC=corp,DC=example,DC=COM", dn.LdapPath);
        }

        [Fact]
        public void LdapPath_Should_ReturnCorrectlyParsedValue_With_ExplicitServerPort()
        {
            // Arrange & Act
            var dn = new DistinguishedName("DC=corp,DC=example,DC=COM")
            {
                ServerPort = 5555
            };

            // Assert
            Assert.Equal("LDAP://corp.example.com:5555/DC=corp,DC=example,DC=COM", dn.LdapPath);
        }

        [Fact]
        public void NotEqualsOperator_Should_ReturnFalseWithEqualDistinguishedNames()
        {
            // Arrange
            var dn1 = new DistinguishedName("Cn=Dave,oU=pEoPle,  DC=example,Dc=cOM");
            var dn2 = new DistinguishedName("CN=dave,  OU=people,DC=example,DC=com");

            // Act
            var result = dn1 != dn2;

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void NotEqualsOperator_Should_ReturnTrueWithNonEqualDistinguishedNames()
        {
            // Arrange
            var dn1 = new DistinguishedName("Cn=Dave,  oU=pEoPle,DC=example,Dc=cOM");
            var dn2 = new DistinguishedName("CN=david,OU=people,  DC=example,DC=com");

            // Act
            var result = dn1 != dn2;

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Parent_Should_ReturnTheParentDistinguishedName()
        {
            // Arrange
            var dn1 = new DistinguishedName("CN=TestUser,OU=People,DC=example,DC=com");

            // Act
            var parent = dn1.Parent.ToString();

            // Assert
            Assert.Equal("OU=People,DC=example,DC=com", parent);
        }

        [Fact]
        public void ParentOfNullDistinguishedName_Should_Throw_NullReferneceExeception()
        {
            // Arrange
            var dn = new DistinguishedName("CN=TestUser,OU=People,DC=example,DC=com");

            // Act
            for (var i = 0; i <= 4; i++)
            {
                dn = dn.Parent;
            }

            // Assert
            Assert.Throws<NullReferenceException>(() => _ = dn.Parent);
        }

        [Fact]
        public void ParentOfTopMostRelativeDistinguishedName_Should_ReturnNull()
        {
            // Arrange
            var dn = new DistinguishedName("CN=TestUser,OU=People,DC=example,DC=com");

            // Act
            for (var i = 0; i <= 3; i++)
            {
                dn = dn.Parent;
            }

            // Assert
            Assert.Null(dn.Parent);
        }
    }
}
