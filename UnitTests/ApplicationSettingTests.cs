using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using ToolKit;
using Xunit;

namespace UnitTests
{
    [SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1600:ElementsMustBeDocumented",
        Justification = "Test Suites do not need XML Documentation.")]
    public class ApplicationSettingTests
    {
        [Fact]
        public void Load_Should_Returne_ExpectedResult_When_CurrentDirectoryIsNotAssemblyPath()
        {
            // Arrange
            var parameters = new MyParameters();
            parameters.MyProperty1 = 10;
            var filename = Guid.NewGuid().ToString() + ".xml";
            parameters.Save(filename);

            // Act
            Directory.SetCurrentDirectory(Environment.GetEnvironmentVariable("TEMP"));
            var loadParameters = new MyParameters().Load(filename);

            // Assert
            Assert.Equal(loadParameters.MyProperty1, parameters.MyProperty1);
        }

        [Fact]
        public void MyProperty1_Should_Returne_ExpectedResult()
        {
            // Arrange
            var parameters = new MyParameters();
            parameters.MyProperty1 = 10;
            var filename = Guid.NewGuid().ToString() + ".xml";

            // Act
            parameters.Save(filename);

            var loadParameters = new MyParameters().Load(filename);

            // Assert
            Assert.Equal(loadParameters.MyProperty1, parameters.MyProperty1);
        }

        [Fact]
        public void MyProperty2_Should_Return_ExpectedResult()
        {
            // Arrange
            var parameters = new MyParameters();
            parameters.MyProperty2 = "Another Test";
            var filename = Guid.NewGuid().ToString() + ".xml";

            // Act
            parameters.Save(filename);

            var loadParameters = new MyParameters().Load(filename);

            // Assert
            Assert.Equal(loadParameters.MyProperty2, parameters.MyProperty2);
        }

        [Fact]
        public void Should_Return_Null_When_File_Does_Not_Exists()
        {
            // Arrange

            // Act
            var loadParmeters = new MyParameters().Load("NonExists.xml");

            // Assert
            Assert.Null(loadParmeters);
        }

        public class MyParameters : ApplicationSettings<MyParameters>
        {
            public int MyProperty1 { get; set; }

            public string MyProperty2 { get; set; }
        }
    }
}
