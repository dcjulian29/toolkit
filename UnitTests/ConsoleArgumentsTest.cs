using System.Diagnostics.CodeAnalysis;
using ToolKit;
using Xunit;

namespace UnitTests
{
    [SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1600:ElementsMustBeDocumented",
        Justification = "Test Suites do not need XML Documentation.")]
    public class ConsoleArgumentsTest
    {
        [Fact]
        public void Argument_With_Enclosed_Value_Should_Parse_Correctly()
        {
            // Arrange
            string[] args =
            {
                "/Load:\"FileName.ext\""
            };

            // Act
            var param = new ConsoleArguments(args);

            // Assert
            Assert.Equal("FileName.ext", param["Load"]);
        }

        [Fact]
        public void Argument_With_Separator_Embeded_In_Value_Correctly_Parses()
        {
            // Arrange
            string[] args =
            {
                "/Display:\"Test-:-User\""
            };

            // Act
            var param = new ConsoleArguments(args);

            // Assert
            Assert.Equal("Test-:-User", param["Display"]);
        }

        [Fact]
        public void Argument_With_Unquoted_Value_Should_Parse_Correctly()
        {
            // Arrange
            string[] args =
            {
                "/User=TestUser",
            };

            // Act
            var param = new ConsoleArguments(args);

            // Assert
            Assert.Equal("TestUser", param["User"]);
        }

        [Fact]
        public void Arguments_Should_Parse_Regardlese_Of_Case_Sensitivity()
        {
            // Arrange
            string[] args =
            {
                "/C"
            };

            var param = new ConsoleArguments();

            // Act
            param.Parse(args);

            // Assert
            Assert.True(param.IsPresent("c"));
            Assert.True(param.IsPresent("C"));
        }

        [Fact]
        public void Bug141()
        {
            // Arrange
            string[] args =
            {
                "/arg1",
                "c:\\temp\\myfile.txt"
            };

            // Act
            var param = new ConsoleArguments(args);

            // Assert
            Assert.Equal(@"c:\temp\myfile.txt", param["arg1"]);
        }

        [Fact]
        public void Complex_Argument_Should_Parse_Correctly()
        {
            // Arrange
            string[] args =
            {
                "/Display:\"Test-:-User\"",
                "/User=TestUser",
                "/Load:\"FileName.ext\"",
                "--Sign",
                "-Description",
                "'--=nice=--'"
            };

            // Act
            var param = new ConsoleArguments(args);

            // Assert
            Assert.Equal("Test-:-User", param["Display"]);
            Assert.Equal("TestUser", param["User"]);
            Assert.Equal("FileName.ext", param["Load"]);
            Assert.True(param.ToBoolean("Sign"));
            Assert.Equal("--=nice=--", param["Description"]);
        }

        [Fact]
        public void Double_Hyphen_Argument_Should_Parse_Correctly()
        {
            // Arrange
            string[] args =
            {
                "-Load",
                "FileName.ext"
            };

            // Act
            var param = new ConsoleArguments(args);

            // Assert
            Assert.Equal("FileName.ext", param["Load"]);
        }

        [Fact]
        public void Invalid_Arguments_Should_Return_Zero_Count()
        {
            // Arrange
            string[] args =
            {
                "Load",
                "asdfd"
            };

            // Act
            var param = new ConsoleArguments(args);

            // Assert
            Assert.Equal(0, param.Count);
        }

        [Fact]
        public void IsPresent_Should_Return_False_When_Non_Existant_Argument()
        {
            // Arrange
            string[] args =
            {
                "--Sign"
            };

            // Act
            var param = new ConsoleArguments(args);

            // Assert
            Assert.False(param.IsPresent("NotExist"));
        }

        [Fact]
        public void Non_Existant_Argument_Should_Return_Null()
        {
            // Arrange
            string[] args =
            {
                "--Sign"
            };

            // Act
            var param = new ConsoleArguments(args);

            // Assert
            Assert.Null(param["NotExist"]);
        }

        [Fact]
        public void Same_Argument_Twice_Should_Only_Record_First_Occurrence()
        {
            // Arrange
            string[] args =
            {
                "/Load",
                "FileName.ext",
                "/Load",
                "SomeOtherName.ext"
            };

            // Act
            var param = new ConsoleArguments(args);

            // Assert
            Assert.Equal("FileName.ext", param["Load"]);
        }

        [Fact]
        public void Single_Hyphen_Argument_Should_Correctly_Parse()
        {
            // Arrange
            string[] args =
            {
                "-Load",
                "FileName.ext"
            };

            // Act
            var param = new ConsoleArguments(args);

            // Assert
            Assert.Equal("FileName.ext", param["Load"]);
        }

        [Fact]
        public void Slash_Argument_Should_Correctly_Parse()
        {
            // Arrange
            string[] args =
            {
                "/Load",
                "FileName.ext"
            };

            // Act
            var param = new ConsoleArguments(args);

            // Assert
            Assert.Equal("FileName.ext", param["Load"]);
        }

        [Fact]
        public void ToBoolean_Argument_By_Itself_Should_Be_True()
        {
            // Arrange
            string[] args =
            {
                "--Sign"
            };

            var param = new ConsoleArguments();

            // Act
            param.Parse(args);

            // Assert
            Assert.True(param.ToBoolean("Sign"));
        }
    }
}
