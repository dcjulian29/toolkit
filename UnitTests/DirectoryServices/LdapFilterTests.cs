using System.Diagnostics.CodeAnalysis;
using ToolKit.DirectoryServices;
using Xunit;

namespace UnitTests.DirectoryServices
{
    [SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1600:ElementsMustBeDocumented",
        Justification = "Test Suites do not need XML Documentation.")]
    public class LdapFilterTests
    {
        [Fact]
        public void Bitwise_Filter_Must_Be_A_Embedded_Filter_When_Not_Is_Used()
        {
            // Arrange
            var expected = "(!(UserAccountControl:1.2.840.113556.1.4.803:=65536))";

            // Act
            var filter = new LdapFilter("UserAccountControl:1.2.840.113556.1.4.803:=65536").Not();

            // Assert
            Assert.Equal(expected, filter.ToString());
        }

        [Fact]
        public void Complex_Filter_1()
        {
            // Arrange
            var expected = "(&(objectClass=Person)(|(sn=Jensen)(cn=Babs J*)))";

            // Act
            var filter = new LdapFilter("cn=Babs J*")
              .Or("sn=Jensen")
              .And("objectClass=Person");

            // Assert
            Assert.Equal(expected, filter.ToString());
        }

        [Fact]
        public void Compound_Element_And_Append_Using_Individual_Strings()
        {
            // Arrange
            var expected = "(&(cn=Babs J*)(sn=Jensen))";

            // Act
            var filter = new LdapFilter("cn=Babs J*").And("sn", "=", "Jensen", true);

            // Assert
            Assert.Equal(expected, filter.ToString());
        }

        [Fact]
        public void Compound_Element_And_Append_Using_LdapFilter()
        {
            // Arrange
            var expected = "(&(cn=Babs J*)(sn=Jensen))";

            // Act
            var filter1 = new LdapFilter("sn=Jensen");
            var filter = new LdapFilter("cn=Babs J*").And(filter1, true);

            // Assert
            Assert.Equal(expected, filter.ToString());
        }

        [Fact]
        public void Compound_Element_And_Append_Using_Single_String()
        {
            // Arrange
            var expected = "(&(cn=Babs J*)(sn=Jensen))";

            // Act
            var filter = new LdapFilter("cn=Babs J*").And("sn=Jensen", true);

            // Assert
            Assert.Equal(expected, filter.ToString());
        }

        [Fact]
        public void Compound_Element_And_Multiple_Filters()
        {
            // Arrange
            var expected = "(&(objectCategory=person)(objectClass=contact)(sn=Easterling))";
            var filter1 = new LdapFilter("objectCategory=person");
            var filter2 = new LdapFilter("objectClass=contact");
            var filter3 = new LdapFilter("sn=Easterling");

            // Act
            var filter = LdapFilter.And(filter1, filter2, filter3);

            // Assert
            Assert.Equal(expected, filter.ToString());
        }

        [Fact]
        public void Compound_Element_And_Using_Individual_Strings()
        {
            // Arrange
            var expected = "(&(sn=Jensen)(cn=Babs J*))";

            // Act
            var filter = new LdapFilter("cn=Babs J*").And("sn", "=", "Jensen");

            // Assert
            Assert.Equal(expected, filter.ToString());
        }

        [Fact]
        public void Compound_Element_And_Using_LdapFilter()
        {
            // Arrange
            var expected = "(&(sn=Jensen)(cn=Babs J*))";

            var filter1 = new LdapFilter("sn=Jensen");

            // Act
            var filter = new LdapFilter("cn=Babs J*").And(filter1);

            // Assert
            Assert.Equal(expected, filter.ToString());
        }

        [Fact]
        public void Compound_Element_And_Using_Single_String()
        {
            // Arrange

            // Act

            // Assert
            var expected = "(&(sn=Jensen)(cn=Babs J*))";

            var filter = new LdapFilter("cn=Babs J*").And("sn=Jensen");

            Assert.Equal(expected, filter.ToString());
        }

        [Fact]
        public void Compound_Element_Not()
        {
            // Arrange
            var expected = "(!(|(cn=Tim Howes)(cn=Julian Easterling)))";

            // Act
            var filter = new LdapFilter("cn=Julian Easterling")
              .Or("cn=Tim Howes")
              .Not();

            // Assert
            Assert.Equal(expected, filter.ToString());
        }

        [Fact]
        public void Compound_Element_Or_Append_Using_Individual_Strings()
        {
            // Arrange
            var expected = "(|(cn=Babs J*)(sn=Jensen))";

            // Act
            var filter = new LdapFilter("cn=Babs J*").Or("sn", "=", "Jensen", true);

            // Assert
            Assert.Equal(expected, filter.ToString());
        }

        [Fact]
        public void Compound_Element_Or_Append_Using_LdapFilter()
        {
            // Arrange
            var expected = "(|(cn=Babs J*)(sn=Jensen))";

            var filter1 = new LdapFilter("sn=Jensen");

            // Act
            var filter = new LdapFilter("cn=Babs J*").Or(filter1, true);

            // Assert
            Assert.Equal(expected, filter.ToString());
        }

        [Fact]
        public void Compound_Element_Or_Append_Using_Single_String()
        {
            // Arrange
            var expected = "(|(cn=Babs J*)(sn=Jensen))";

            // Act
            var filter = new LdapFilter("cn=Babs J*").Or("sn=Jensen", true);

            // Assert
            Assert.Equal(expected, filter.ToString());
        }

        [Fact]
        public void Compound_Element_Or_Multiple_Filters()
        {
            // Arrange
            var expected = "(|(objectCategory=person)(objectClass=contact)(sn=Easterling))";

            var filter1 = new LdapFilter("objectCategory=person");
            var filter2 = new LdapFilter("objectClass=contact");
            var filter3 = new LdapFilter("sn=Easterling");

            // Act
            var filter = LdapFilter.Or(filter1, filter2, filter3);

            // Assert
            Assert.Equal(expected, filter.ToString());
        }

        [Fact]
        public void Compound_Element_Or_Using_Individual_Strings()
        {
            // Arrange
            var expected = "(|(sn=Jensen)(cn=Babs J*))";

            // Act
            var filter = new LdapFilter("cn=Babs J*").Or("sn", "=", "Jensen");

            // Assert
            Assert.Equal(expected, filter.ToString());
        }

        [Fact]
        public void Compound_Element_Or_Using_LdapFilter()
        {
            // Arrange
            var expected = "(|(sn=Jensen)(cn=Babs J*))";

            var filter1 = new LdapFilter("sn=Jensen");

            // Act
            var filter = new LdapFilter("cn=Babs J*").Or(filter1);

            // Assert
            Assert.Equal(expected, filter.ToString());
        }

        [Fact]
        public void Compound_Element_Or_Using_Single_String()
        {
            // Arrange
            var expected = "(|(sn=Jensen)(cn=Babs J*))";

            // Act
            var filter = new LdapFilter("cn=Babs J*").Or("sn=Jensen");

            // Assert
            Assert.Equal(expected, filter.ToString());
        }

        [Fact]
        public void Single_Element_Filter_Not()
        {
            // Arrange
            var expected = "(!samaccountname=*$)";

            // Act
            var filter = new LdapFilter("samaccountname=*$").Not();

            // Assert
            Assert.Equal(expected, filter.ToString());
        }

        [Fact]
        public void Single_Element_Filter_Using_Approx_Filter_Type()
        {
            // Arrange
            var expected = "(cn~=Babs Jensen)";

            // Act
            var filter = new LdapFilter("cn", LdapFilter.Approx, "Babs Jensen");

            // Assert
            Assert.Equal(expected, filter.ToString());
        }

        [Fact]
        public void Single_Element_Filter_Using_Equal_Filter_Type()
        {
            // Arrange
            var expected = "(cn=Babs Jensen)";

            // Act
            var filter = new LdapFilter("cn", LdapFilter.Equal, "Babs Jensen");

            // Assert
            Assert.Equal(expected, filter.ToString());
        }

        [Fact]
        public void Single_Element_Filter_Using_Greater_Filter_Type()
        {
            // Arrange
            var expected = "(cn>=Babs Jensen)";

            // Act
            var filter = new LdapFilter("cn", LdapFilter.Greater, "Babs Jensen");

            // Assert
            Assert.Equal(expected, filter.ToString());
        }

        [Fact]
        public void Single_Element_Filter_Using_Individual_Strings()
        {
            // Arrange
            var expected = "(cn=Babs Jensen)";

            // Act
            var filter = new LdapFilter("cn", "=", "Babs Jensen");

            // Assert
            Assert.Equal(expected, filter.ToString());
        }

        [Fact]
        public void Single_Element_Filter_Using_Less_Filter_Type()
        {
            // Arrange
            var expected = "(cn<=Babs Jensen)";

            // Act
            var filter = new LdapFilter("cn", LdapFilter.Less, "Babs Jensen");

            // Assert
            Assert.Equal(expected, filter.ToString());
        }

        [Fact]
        public void Single_Element_Filter_Using_Single_String()
        {
            // Arrange
            var expected = "(cn=Babs Jensen)";

            // Act
            var filter = new LdapFilter("cn=Babs Jensen");

            // Assert
            Assert.Equal(expected, filter.ToString());
        }

        [Fact]
        public void Single_Element_Filter_Using_Wildcard()
        {
            // Arrange
            var expected = "(cn=*)";

            // Act
            var filter = new LdapFilter("cn", "=", LdapFilter.Any);

            // Assert
            Assert.Equal(expected, filter.ToString());
        }

        [Fact]
        public void Single_Element_Filter_With_Parenthesis()
        {
            // Arrange
            var expected = "(cn=Babs Jensen)";

            // Act
            var filter = new LdapFilter("(cn=Babs Jensen)");

            // Assert
            Assert.Equal(expected, filter.ToString());
        }

        [Fact]
        public void Z_Complex_1()
        {
            // Arrange
            var expected = "(&(objectCategory=person)(objectClass=user)"
                        + "(!(userAccountControl:1.2.840.113556.1.4.803:=65536))"
                        + "(!(userAccountControl:1.2.840.113556.1.4.803:=2))"
                        + "(!samaccountname=gen-*)(!samaccountname=tst-*)"
                        + "(!samaccountname=adm-*)(!samaccountname=res-*)"
                        + "(!samaccountname=tmp-*)(!samaccountname=trn-*)"
                        + "(!samaccountname=app-*)(!samaccountname=svc-*)"
                        + "(!samaccountname=*test*)(!samaccountname=*$))";

            // Act
            var filter =
              LdapFilter.And(
                new LdapFilter("objectCategory=person"),
                new LdapFilter("objectClass=user"),
                new LdapFilter("userAccountControl:1.2.840.113556.1.4.803:=65536").Not(),
                new LdapFilter("userAccountControl:1.2.840.113556.1.4.803:=2").Not(),
                new LdapFilter("samaccountname=gen-*").Not(),
                new LdapFilter("samaccountname=tst-*").Not(),
                new LdapFilter("samaccountname=adm-*").Not(),
                new LdapFilter("samaccountname=res-*").Not(),
                new LdapFilter("samaccountname=tmp-*").Not(),
                new LdapFilter("samaccountname=trn-*").Not(),
                new LdapFilter("samaccountname=app-*").Not(),
                new LdapFilter("samaccountname=svc-*").Not(),
                new LdapFilter("samaccountname=*test*").Not(),
                new LdapFilter("samaccountname=*$").Not());

            // Assert
            Assert.Equal(expected, filter.ToString());
        }

        [Fact]
        public void Z_Complex_2()
        {
            // Arrange
            var expected = "(&(objectCategory=person)(objectClass=contact)(|(sn=Easterling)(sn=Zivuku)))";

            // Act
            var filter =
              LdapFilter.And(
                new LdapFilter("objectCategory=person"),
                new LdapFilter("objectClass=contact"),
                new LdapFilter("sn=Zivuku").Or("sn=Easterling"));

            // Assert
            Assert.Equal(expected, filter.ToString());
        }

        [Fact]
        public void Z_Complex_3()
        {
            // Arrange
            var expected = "(&(objectClass=organizationalUnit)(|(ou:dn:=Domain Controllers)(ou:dn:=Computers)))";

            // Act
            var filter = new LdapFilter("ou:dn:=Computers")
              .Or("ou:dn:=Domain Controllers")
              .And("objectClass=organizationalUnit");

            // Assert
            Assert.Equal(expected, filter.ToString());
        }
    }
}
