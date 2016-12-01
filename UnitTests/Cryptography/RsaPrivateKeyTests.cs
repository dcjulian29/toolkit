using System;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.IO;
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
        [Fact]
        public void ExportToXmlFile_Should_OverwriteThePrivateKey()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var file = $"{Directory.GetCurrentDirectory()}\\{guid}.xml";
            var keyFile = Directory.GetCurrentDirectory() + @"\privateKey.xml";
            var key = RsaPrivateKey.LoadFromXmlFile(keyFile);
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
            var guid = Guid.NewGuid();
            var file = $"{Directory.GetCurrentDirectory()}\\{guid}.xml";
            var keyFile = Directory.GetCurrentDirectory() + @"\privateKey.xml";
            var key = RsaPrivateKey.LoadFromXmlFile(keyFile);

            // Act
            key.ExportToXmlFile(file);

            // Assert
            Assert.True(File.Exists(file));
        }

        [Fact]
        public void ExportToXmlFile_Should_ThrowException_IfPrivateKeyFileExist()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var file = $"{Directory.GetCurrentDirectory()}\\{guid}.xml";
            var keyFile = Directory.GetCurrentDirectory() + @"\privateKey.xml";
            var key = RsaPrivateKey.LoadFromXmlFile(keyFile);
            key.ExportToXmlFile(file);

            // Act & Assert
            Assert.Throws<IOException>(() =>
            {
                key.ExportToXmlFile(file);
            });
        }

        [Fact]
        public void LoadFromCertificateFile_Should_LoadCertificate_When_FileIsPasswordProtected()
        {
            // Arrage
            var cert = Directory.GetCurrentDirectory() + @"\RsaEncrypt.pfx";

            // Act
            var privateKey = RsaPrivateKey.LoadFromCertificateFile(cert, "password");

            // Assert
            Assert.NotNull(privateKey);
        }

        [Fact]
        public void LoadFromCertificateFile_Should_ThrowException_When_PrivateKeyFileDoesNotExists()
        {
            // Assert
            Assert.Throws<ArgumentException>(() =>
            {
                var privateKey = RsaPrivateKey.LoadFromCertificateFile("nonexist.pfx", "password");
            });
        }

        [Fact]
        public void LoadFromCertificateFile_Should_ThrowException_When_PrivateKeyPasswordNotProvided()
        {
            // Arrage
            var cert = Directory.GetCurrentDirectory() + @"\RsaEncrypt.pfx";

            // Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                var privateKey = RsaPrivateKey.LoadFromCertificateFile(cert, "");
            });
        }

        [Fact]
        public void LoadFromConfig_Should_ThrowExceptionWhenConfigAppSettingsEmpty()
        {
            // Arrange
            var guid = $"{Directory.GetCurrentDirectory()}\\{Guid.NewGuid().ToString()}";
            var dll = $"{Directory.GetCurrentDirectory()}\\Unittests.dll";
            File.Delete(guid);
            File.Move($"{dll}.config", guid);
            ConfigurationManager.RefreshSection("appSettings");

            // Act & Assert
            Assert.Throws<ConfigurationErrorsException>(() =>
            {
                var key = RsaPrivateKey.LoadFromConfig();
            });

            File.Move(guid, $"{dll}.config");
            ConfigurationManager.RefreshSection("appSettings");
        }

        [Fact]
        public void LoadFromString_Should_ThrowException_When_XmlDoesntContainElements()
        {
            // Arrange
            var xml = "<RSAKeyValue><Exponent>AQAB</Exponent></RSAKeyValue>";

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
            {
                var key = new RsaPrivateKey(xml);
            });
        }

        [Fact]
        public void LoadFromXmlFile_Should_LoadThePrivateKeyFromFile()
        {
            // Arrange
            var expected = "71bwZomEwGq5FFx+43FIFngA6uZEZqPTMcTfc250F8WH"
                         + "7AFE94ucRpQR6JOKt6POZj/2NtY499YIKlJIjWM4Qw==";
            var file = Directory.GetCurrentDirectory() + @"\privateKey.xml";

            // Act
            var key = RsaPrivateKey.LoadFromXmlFile(file);

            // Assert
            Assert.Equal(expected, key.PrimeP);
        }

        [Fact]
        public void LoadFromXmlFile_Should_ThrowException_When_FileDoesNotExists()
        {
            // Assert
            Assert.Throws<ArgumentException>(() =>
            {
                var key = RsaPrivateKey.LoadFromXmlFile("nonexist.xml");
            });
        }

        [Fact]
        public void ToPublicKey_Should_ReturnThePublicKeyPortionOfAPrivateKey()
        {
            // Arrange
            var keyFile = Directory.GetCurrentDirectory() + @"\privateKey.xml";
            var key = RsaPrivateKey.LoadFromXmlFile(keyFile);

            // Act
            var publicKey = key.ToPublicKey();

            // Assert
            Assert.Equal("AQAB", publicKey.Exponent);
        }
    }
}
