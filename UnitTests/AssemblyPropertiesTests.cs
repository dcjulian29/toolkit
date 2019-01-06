using System;
using ToolKit;
using Xunit;

namespace UnitTests
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1600:ElementsMustBeDocumented",
        Justification = "Test Suites do not need XML Documentation.")]
    public class AssemblyPropertiesTests
    {
        private string _toolkitLibraryPath = AppDomain.CurrentDomain.BaseDirectory + "\\ToolKit-Windows.dll";

        [Fact]
        public void BuildVersion_Should_ReturnTheVersionOfTheSpecifiedAssembly_When_AnAssemblyIsProvided()
        {
            // Arrange

            // Act
            var actual = AssemblyProperties.BuildVersion(_toolkitLibraryPath);

            // Assert
            Assert.True(actual >= 0);
        }

        [Fact]
        public void BuildVersion_Should_ReturnTheVersionOfTheTestsAssembly_When_AnAssemblyIsNotProvided()
        {
            // Arrange

            // Act
            var actual = AssemblyProperties.BuildVersion(typeof(AssemblyPropertiesTests).Assembly.Location);

            // Assert
            Assert.True(actual >= 0);
        }

        [Fact]
        public void Configuration_Should_ReturnTheConfigurationOfTheSpecifiedAssembly_When_AnAssemblyIsProvided()
        {
            // Arrange

            // Act
            var actual = AssemblyProperties.Configuration(_toolkitLibraryPath);

            // Assert
            Assert.NotEqual(String.Empty, actual);
        }

        [Fact]
        public void Configuration_Should_ReturnTheConfigurationOfTheTestsAssembly_When_AnAssemblyIsNotProvided()
        {
            // Arrange

            // Act
            var actual = AssemblyProperties.Configuration(typeof(AssemblyPropertiesTests).Assembly.Location);

            // Assert
            Assert.NotEqual(String.Empty, actual);
        }

        [Fact]
        public void Guid_Should_ReturnTheGuidOfTheSpecifiedAssembly_When_AnAssemblyIsProvided()
        {
            // Arrange

            // Act
            var actual = AssemblyProperties.Guid(_toolkitLibraryPath);

            // Assert
            Assert.NotEqual(String.Empty, actual);
        }

        [Fact]
        public void Guid_Should_ReturnTheGuidOfTheTestsAssembly_When_AnAssemblyIsNotProvided()
        {
            // Arrange

            // Act
            var actual = AssemblyProperties.Guid(typeof(AssemblyPropertiesTests).Assembly.Location);

            // Assert
            Assert.NotEqual(String.Empty, actual);
        }

        [Fact]
        public void IsDebugMode_Should_NotEqualIsReleaseMode_When_AnAssemblyIsNotProvided()
        {
            // Arrange

            // Act
            var releaseMode = AssemblyProperties.IsReleaseMode(typeof(AssemblyPropertiesTests).Assembly.Location);
            var debugMode = AssemblyProperties.IsDebugMode(typeof(AssemblyPropertiesTests).Assembly.Location);

            // Assert
            Assert.NotEqual(releaseMode, debugMode);
        }

        [Fact]
        public void IsDebugMode_Should_NotEqualIsReleaseMode_When_AnAssemblyIsProvided()
        {
            // Arrange

            // Act
            var releaseMode = AssemblyProperties.IsReleaseMode(_toolkitLibraryPath);
            var debugMode = AssemblyProperties.IsDebugMode(_toolkitLibraryPath);

            // Assert
            Assert.NotEqual(releaseMode, debugMode);
        }

        [Fact]
        public void MajorVersion_Should_ReturnTheVersionOfTheSpecifiedAssembly_When_AnAssemblyIsProvided()
        {
            // Arrange

            // Act
            var actual = AssemblyProperties.MajorVersion(_toolkitLibraryPath);

            // Assert
            Assert.True(actual >= 0);
        }

        [Fact]
        public void MajorVersion_Should_ReturnTheVersionOfTheTestsAssembly_When_AnAssemblyIsNotProvided()
        {
            // Arrange

            // Act
            var actual = AssemblyProperties.MajorVersion(typeof(AssemblyPropertiesTests).Assembly.Location);

            // Assert
            Assert.True(actual >= 0);
        }

        [Fact]
        public void MinorVersion_Should_ReturnTheVersionOfTheSpecifiedAssembly_When_AnAssemblyIsProvided()
        {
            // Arrange

            // Act
            var actual = AssemblyProperties.MinorVersion(_toolkitLibraryPath);

            // Assert
            Assert.True(actual >= 0);
        }

        [Fact]
        public void MinorVersion_Should_ReturnTheVersionOfTheTestsAssembly_When_AnAssemblyIsNotProvided()
        {
            // Arrange

            // Act
            var actual = AssemblyProperties.MinorVersion(typeof(AssemblyPropertiesTests).Assembly.Location);

            // Assert
            Assert.True(actual >= 0);
        }

        [Fact]
        public void Name_Should_ReturnTheNameOfTheUnittestAssembly_When_AnAssemblyIsProvided()
        {
            // Arrange

            // Act
            var actual = AssemblyProperties.Name(_toolkitLibraryPath);

            // Assert
            Assert.Equal("ToolKit-Windows", actual);
        }

        [Fact]
        public void Name_Should_ReturnTheTheNameOfTheUnittestAssembly_When_AnAssemblyIsNotProvided()
        {
            // Arrange

            // Act
            var actual = AssemblyProperties.Name(typeof(AssemblyPropertiesTests).Assembly.Location);

            // Assert
            Assert.Equal("UnitTests", actual);
        }

        [Fact]
        public void RevisionVersion_Should_ReturnTheVersionOfTheSpecifiedAssembly_When_AnAssemblyIsProvided()
        {
            // Arrange

            // Act
            var actual = AssemblyProperties.RevisionVersion(_toolkitLibraryPath);

            // Assert
            Assert.True(actual >= 0);
        }

        [Fact]
        public void RevisionVersion_Should_ReturnTheVersionOfTheTestsAssembly_When_AnAssemblyIsNotProvided()
        {
            // Arrange

            // Act
            var actual = AssemblyProperties.RevisionVersion(typeof(AssemblyPropertiesTests).Assembly.Location);

            // Assert
            Assert.True(actual >= 0);
        }

        [Fact]
        public void Version_Should_ReturnTheVersionOfTheSpecifiedAssembly_When_AnAssemblyIsProvided()
        {
            // Arrange

            // Act
            var actual = AssemblyProperties.Configuration(_toolkitLibraryPath);

            // Assert
            Assert.NotEqual(String.Empty, actual);
        }

        [Fact]
        public void Version_Should_ReturnTheVersionOfTheTestsAssembly_When_AnAssemblyIsNotProvided()
        {
            // Arrange

            // Act
            var actual = AssemblyProperties.Version(typeof(AssemblyPropertiesTests).Assembly.Location);

            // Assert
            Assert.NotEqual(String.Empty, actual);
        }
    }
}
