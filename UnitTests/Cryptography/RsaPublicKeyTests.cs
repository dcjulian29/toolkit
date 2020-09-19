using System;
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
    public class RsaPublicKeyTests
    {
        [Fact]
        public void Exponent_Should_ReturnExpectedResult()
        {
            // Arrange
            const string e = "AQAB";
            const string m = "z8FBNlJQrn4rEKhGlNvQDKgKDlHHOVu2hlWkP0pRrxTyp1/h/dJsBN4dfASY2A1"
                    + "r5KBCg1dySSNVFB5bJP9o9ob2GEL0dlbZtg0CiiWCwOFWBgakav3Va1+CUF6DbN"
                    + "g3gw2c/1WQaq73xB/WbKuLp5yk22HP/kVOUaG6H33Muv3s2/GxXClgnw8tOqLYbO"
                    + "A/tb9G9d0MIyhhEG/vOR8kkKrwZTeIECbT8vZl0B952leMKmoBN3AJVdnzmP43H"
                    + "Jvx28N0VJ0teV0emgOGnyhpNMB01KlTqd2Kc6Hls7AQbBJ/bkgmlnSdQA7vipig"
                    + "FKU19rhgMd4/95gBmTJOb71wfQ==";

            // Act
            var key = new RsaPublicKey(e, m);

            // Assert
            Assert.Equal(e, key.Exponent);
        }

        [Fact]
        public void ExportToXmlFile_Should_OverwriteThePublicKeyIfToldTo()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var file = $"{Directory.GetCurrentDirectory()}\\{guid}.xml";
            const string e = "AQAB";
            const string m = "z8FBNlJQrn4rEKhGlNvQDKgKDlHHOVu2hlWkP0pRrxTyp1/h/dJsBN4dfASY2A1"
                    + "r5KBCg1dySSNVFB5bJP9o9ob2GEL0dlbZtg0CiiWCwOFWBgakav3Va1+CUF6DbN"
                    + "g3gw2c/1WQaq73xB/WbKuLp5yk22HP/kVOUaG6H33Muv3s2/GxXClgnw8tOqLYbO"
                    + "A/tb9G9d0MIyhhEG/vOR8kkKrwZTeIECbT8vZl0B952leMKmoBN3AJVdnzmP43H"
                    + "Jvx28N0VJ0teV0emgOGnyhpNMB01KlTqd2Kc6Hls7AQbBJ/bkgmlnSdQA7vipig"
                    + "FKU19rhgMd4/95gBmTJOb71wfQ==";
            var key = new RsaPublicKey(e, m);
            key.ExportToXmlFile(file, true);

            // Act
            key.ExportToXmlFile(file, true);

            // Assert
            Assert.True(File.Exists(file));
        }

        [Fact]
        public void ExportToXmlFile_Should_SaveThePublicKey()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var file = $"{Directory.GetCurrentDirectory()}\\{guid}.xml";
            const string e = "AQAB";
            const string m = "z8FBNlJQrn4rEKhGlNvQDKgKDlHHOVu2hlWkP0pRrxTyp1/h/dJsBN4dfASY2A1"
                    + "r5KBCg1dySSNVFB5bJP9o9ob2GEL0dlbZtg0CiiWCwOFWBgakav3Va1+CUF6DbN"
                    + "g3gw2c/1WQaq73xB/WbKuLp5yk22HP/kVOUaG6H33Muv3s2/GxXClgnw8tOqLYbO"
                    + "A/tb9G9d0MIyhhEG/vOR8kkKrwZTeIECbT8vZl0B952leMKmoBN3AJVdnzmP43H"
                    + "Jvx28N0VJ0teV0emgOGnyhpNMB01KlTqd2Kc6Hls7AQbBJ/bkgmlnSdQA7vipig"
                    + "FKU19rhgMd4/95gBmTJOb71wfQ==";
            var key = new RsaPublicKey(e, m);

            // Act
            key.ExportToXmlFile(file);

            // Assert
            Assert.True(File.Exists(file));
        }

        [Fact]
        public void ExportToXmlFile_Should_ThrowException_IfPublicKeyFileExist()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var file = $"{Directory.GetCurrentDirectory()}\\{guid}.xml";
            const string e = "AQAB";
            const string m = "z8FBNlJQrn4rEKhGlNvQDKgKDlHHOVu2hlWkP0pRrxTyp1/h/dJsBN4dfASY2A1"
                    + "r5KBCg1dySSNVFB5bJP9o9ob2GEL0dlbZtg0CiiWCwOFWBgakav3Va1+CUF6DbN"
                    + "g3gw2c/1WQaq73xB/WbKuLp5yk22HP/kVOUaG6H33Muv3s2/GxXClgnw8tOqLYbO"
                    + "A/tb9G9d0MIyhhEG/vOR8kkKrwZTeIECbT8vZl0B952leMKmoBN3AJVdnzmP43H"
                    + "Jvx28N0VJ0teV0emgOGnyhpNMB01KlTqd2Kc6Hls7AQbBJ/bkgmlnSdQA7vipig"
                    + "FKU19rhgMd4/95gBmTJOb71wfQ==";
            var key = new RsaPublicKey(e, m);
            key.ExportToXmlFile(file);

            // Act & Assert
            Assert.Throws<IOException>(() => key.ExportToXmlFile(file));
        }

        [Fact]
        public void LoadFromCertificateFile_Should_LoadCertificate_When_FileIsNotPasswordProtected()
        {
            // Arrage
            var cert = Directory.GetCurrentDirectory() + @"\RsaEncrypt.cer";

            // Act
            var publicKey = RsaPublicKey.LoadFromCertificateFile(cert);

            // Assert
            Assert.NotNull(publicKey);
        }

        [Fact]
        public void LoadFromCertificateFile_Should_LoadCertificate_When_FileIsPasswordProtected()
        {
            // Arrage
            var cert = Directory.GetCurrentDirectory() + @"\RsaEncrypt.pfx";

            // Act
            var publicKey = RsaPublicKey.LoadFromCertificateFile(cert, "password");

            // Assert
            Assert.NotNull(publicKey);
        }

        [Fact]
        public void LoadFromCertificateFile_Should_ThrowException_When_PublicKeyFileDoesNotExists()
        {
            // Assert
            Assert.Throws<ArgumentException>(() => _ = RsaPublicKey.LoadFromCertificateFile("nonexist.cer"));
        }

        [Fact]
        public void LoadFromCertificateFile_Should_ThrowException_When_PublicKeyFileWithPasswordDoesNotExists()
        {
            // Assert
            Assert.Throws<ArgumentException>(() => _ = RsaPublicKey.LoadFromCertificateFile("nonexist.cer", "password"));
        }

        [Fact]
        public void LoadFromConfig_Should_ThrowExceptionWhenConfigAppSettingsEmpty()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentException>(() => _ = RsaPublicKey.LoadFromEnvironment());
        }

        [Fact]
        public void LoadFromString_Should_ThrowException_When_XmlDoesntContainElements()
        {
            // Arrange
            const string xml = "<RSAKeyValue>" +
                      "<SomeOtherValue>0D59Km2Eo9oopcm7Y2wOXx0TRRXQFybl9HHe/ve47Qcf2EoKbs9nkuMmhCJlJ" +
                      "zrq6ZJzgQSEbpVyaWn8OHq0I50rQ13dJsALEquhlfwVWw6Hit7qRvveKlOAGfj8xdkaXJ" +
                      "LYS1tA06tKHfYxgt6ysMBZd0DIedYoE1fe3VlLZyE=</SomeOtherValue>" +
                      "<Exponent>AQAB</Exponent>" +
                      "</RSAKeyValue>";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _ = new RsaPublicKey(xml));
        }

        [Fact]
        public void LoadFromXml_Should_ReturnExpectedResult()
        {
            // Arrange
            const string xml = "<RSAKeyValue>" +
                      "<Modulus>0D59Km2Eo9oopcm7Y2wOXx0TRRXQFybl9HHe/ve47Qcf2EoKbs9nkuMmhCJlJ" +
                      "zrq6ZJzgQSEbpVyaWn8OHq0I50rQ13dJsALEquhlfwVWw6Hit7qRvveKlOAGfj8xdkaXJ" +
                      "LYS1tA06tKHfYxgt6ysMBZd0DIedYoE1fe3VlLZyE=</Modulus>" +
                      "<Exponent>AQAB</Exponent>" +
                      "</RSAKeyValue>";

            // Act
            var rsa = RsaPublicKey.LoadFromXml(xml);

            // Assert
            Assert.Equal("AQAB", rsa.Exponent);
        }

        [Fact]
        public void LoadFromXmlFile_Should_LoadThePublicKeyFromFile()
        {
            // Arrange
            var file = Directory.GetCurrentDirectory() + @"\publicKey.xml";

            // Act
            var key = RsaPublicKey.LoadFromXmlFile(file);

            // Assert
            Assert.Equal("AQAB", key.Exponent);
        }

        [Fact]
        public void LoadFromXmlFile_Should_ThrowException_When_FileDoesNotExists()
        {
            // Assert
            Assert.Throws<ArgumentException>(() => _ = RsaPublicKey.LoadFromXmlFile("nonexist.xml"));
        }

        [Fact]
        public void Modulus_Should_ReturnExpectedResult()
        {
            // Arrange
            const string e = "AQAB";
            const string m = "z8FBNlJQrn4rEKhGlNvQDKgKDlHHOVu2hlWkP0pRrxTyp1/h/dJsBN4dfASY2A1"
                    + "r5KBCg1dySSNVFB5bJP9o9ob2GEL0dlbZtg0CiiWCwOFWBgakav3Va1+CUF6DbN"
                    + "g3gw2c/1WQaq73xB/WbKuLp5yk22HP/kVOUaG6H33Muv3s2/GxXClgnw8tOqLYbO"
                    + "A/tb9G9d0MIyhhEG/vOR8kkKrwZTeIECbT8vZl0B952leMKmoBN3AJVdnzmP43H"
                    + "Jvx28N0VJ0teV0emgOGnyhpNMB01KlTqd2Kc6Hls7AQbBJ/bkgmlnSdQA7vipig"
                    + "FKU19rhgMd4/95gBmTJOb71wfQ==";

            // Act
            var key = new RsaPublicKey(e, m);

            // Assert
            Assert.Equal(m, key.Modulus);
        }
    }
}
