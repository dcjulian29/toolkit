using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using ToolKit.Cryptography;
using Xunit;

namespace UnitTests.Cryptography
{
    [SuppressMessage(
         "StyleCop.CSharp.DocumentationRules",
         "SA1600:ElementsMustBeDocumented",
         Justification = "Test Suites do not need XML Documentation.")]
    public class RsaPrivateKeyTests
    {
        private static readonly string _assemblyPath =
            Path.GetDirectoryName(Assembly.GetAssembly(typeof(RsaPrivateKeyTests)).Location)
            + Path.DirectorySeparatorChar;

        [Fact]
        public void ExportToXmlFile_Should_OverwriteThePrivateKey()
        {
            // Arrange
            var file = $"{_assemblyPath}{Guid.NewGuid()}.xml";
            var key = RsaPrivateKey.LoadFromXmlFile($"{_assemblyPath}privateKey.xml");
            key.ExportToXmlFile(file);

            // Act
            key.ExportToXmlFile(file, true);

            // Assert
            Assert.True(File.Exists(file));
        }

        [Fact]
        public void ExportToXmlFile_Should_SaveThePrivateKey()
        {
            // Arrange
            var file = $"{_assemblyPath}{Guid.NewGuid()}.xml";
            var key = RsaPrivateKey.LoadFromXmlFile($"{_assemblyPath}privateKey.xml");

            // Act
            key.ExportToXmlFile(file);

            // Assert
            Assert.True(File.Exists(file));
        }

        [Fact]
        public void ExportToXmlFile_Should_ThrowException_IfPrivateKeyFileExist()
        {
            // Arrange
            var file = $"{_assemblyPath}{Guid.NewGuid()}.xml";
            var key = RsaPrivateKey.LoadFromXmlFile($"{_assemblyPath}privateKey.xml");
            key.ExportToXmlFile(file);

            // Act & Assert
            Assert.Throws<IOException>(() => key.ExportToXmlFile(file));
        }

        [Fact]
        public void LoadFromCertificateFile_Should_LoadCertificate_When_FileIsPasswordProtected()
        {
            // Arrange
            var cert = $"{_assemblyPath}RsaEncrypt.pfx";

            // Act
            var privateKey = RsaPrivateKey.LoadFromCertificateFile(cert, "password");

            // Assert
            Assert.NotNull(privateKey);
        }

        [Fact]
        public void LoadFromCertificateFile_Should_ThrowException_When_PrivateKeyFileDoesNotExists()
        {
            // Assert
            Assert.Throws<ArgumentException>(()
                => RsaPrivateKey.LoadFromCertificateFile($"{_assemblyPath}nonexist.pfx", "password"));
        }

        [Fact]
        public void LoadFromCertificateFile_Should_ThrowException_When_PrivateKeyPasswordNotProvided()
        {
            // Arrange
            var cert = $"{_assemblyPath}RsaEncrypt.pfx";

            // Assert
            Assert.Throws<ArgumentNullException>(() => RsaPrivateKey.LoadFromCertificateFile(cert, ""));
        }

        [Fact]
        public void LoadFromEnvironment_Should_ThrowExceptionWhenEnvironmentDoesNotContainValues()
            => Assert.Throws<ArgumentException>(() => RsaPrivateKey.LoadFromEnvironment());

        [Fact]
        public void LoadFromString_Should_ThrowException_When_XmlDoesntContainElements()
        {
            // Arrange
            const string xml = "<RSAKeyValue><Exponent>AQAB</Exponent></RSAKeyValue>";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new RsaPrivateKey(xml));
        }

        [Fact]
        public void LoadFromXmlFile_Should_LoadThePrivateKeyFromFile()
        {
            // Arrange
            const string expected = "71bwZomEwGq5FFx+43FIFngA6uZEZqPTMcTfc250F8WH"
                         + "7AFE94ucRpQR6JOKt6POZj/2NtY499YIKlJIjWM4Qw==";
            var file = $"{_assemblyPath}privateKey.xml";

            // Act
            var key = RsaPrivateKey.LoadFromXmlFile(file);

            // Assert
            Assert.Equal(expected, key.PrimeP);
        }

        [Fact]
        public void LoadFromXmlFile_Should_ThrowException_When_FileDoesNotExists() =>
            Assert.Throws<ArgumentException>(()
                => RsaPrivateKey.LoadFromXmlFile($"{_assemblyPath}nonexist.xml"));

        [Fact]
        public void ToPublicKey_Should_ReturnThePublicKeyPortionOfAPrivateKey()
        {
            // Arrange
            var key = RsaPrivateKey.LoadFromXmlFile($"{_assemblyPath}privateKey.xml");

            // Act
            var publicKey = key.ToPublicKey();

            // Assert
            Assert.Equal("AQAB", publicKey.Exponent);
        }
    }
}
