using System;
using System.IO;
using System.Reflection;
using System.Text;
using ToolKit.Cryptography;
using Xunit;

namespace UnitTests.Cryptography
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "StyleCop.CSharp.DocumentationRules",
        "SA1600:ElementsMustBeDocumented",
        Justification = "Test Suites do not need XML Documentation.")]
    public class HashTests
    {
        public readonly string TargetString;

        private static readonly string _assemblyPath =
            Path.GetDirectoryName(Assembly.GetAssembly(typeof(HashTests)).Location)
            + Path.DirectorySeparatorChar;

        public HashTests()
        {
            TargetString = "The instinct of nearly all societies is to lock up anybody who is truly free. "
                           + "First, society begins by trying to beat you up. If this fails, they try to poison you. "
                           + "If this fails too, they finish by loading honors on your head."
                           + " - Jean Cocteau (1889-1963)";
        }

        [Fact]
        public void Constructor_Should_ThrowExceptionWhenInvalidProvider()
        {
            // Arrange
            var provider = (Hash.Provider)8;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new Hash(provider));
        }

        [Fact]
        public void CRC32_Should_CalculateCorrectHash()
        {
            // Arrange
            var expected = "AA692113";
            var hash = new Hash(Hash.Provider.CRC32);

            // Act
            var data = hash.Calculate(new EncryptionData(TargetString));
            var actual = data.Hex;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CRC32_Should_CalculateCorrectHash_When_ProvidedFile()
        {
            // Arrange
            var hash = new Hash(Hash.Provider.CRC32);
            var expected = "8893EF97";
            var actual = String.Empty;

            // Act
            using (var sr = new StreamReader($"{_assemblyPath}gettysburg.txt"))
            {
                actual = hash.Calculate(sr.BaseStream).Hex;
            }

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CRC32_Should_CalculateCorrectHash_When_Salted()
        {
            // Arrange
            var hash = new Hash(Hash.Provider.CRC32);
            var expected = "2A873DD7";

            // Act
            var data = hash.Calculate(new EncryptionData(TargetString), new EncryptionData("Salty!"));
            var actual = data.Hex;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void MD5_Should_CalculateCorrectHash()
        {
            // Arrange
            var hash = new Hash(Hash.Provider.MD5);
            var expected = "44D36517B0CCE797FF57118ABE264FD9";

            // Act
            var data = hash.Calculate(new EncryptionData(TargetString));
            var actual = data.Hex;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void MD5_Should_CalculateCorrectHash_When_ProvidedFileContainingBinaryData()
        {
            // Arrange
            var hash = new Hash(Hash.Provider.MD5);
            var expected = "4F32AB797F0FCC782AAC0B4F4E5B1693";
            var actual = String.Empty;

            // Act
            using (var sr = new StreamReader($"{_assemblyPath}sample.doc"))
            {
                actual = hash.Calculate(sr.BaseStream).Hex;
            }

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void MD5_Should_CalculateCorrectHash_When_ProvidedFileContainingTextData()
        {
            // Arrange
            var hash = new Hash(Hash.Provider.MD5);
            var expected = "CD4123AC876D1F206E710E25EA33C723";
            var actual = String.Empty;

            // Act
            using (var sr = new StreamReader($"{_assemblyPath}gettysburg.txt"))
            {
                actual = hash.Calculate(sr.BaseStream).Hex;
            }

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void MD5_Should_CalculateCorrectHash_When_Salted()
        {
            // Arrange
            var hash = new Hash(Hash.Provider.MD5);
            var expected = "ADEFEA4AD9CF8F584DC3A6CD006C0782";

            // Act
            var data = hash.Calculate(new EncryptionData(TargetString), new EncryptionData("Salty!"));
            var actual = data.Hex;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SHA1_Should_CalculateCorrectHash()
        {
            // Arrange
            var hash = new Hash(Hash.Provider.SHA1);
            var expected = "9E93AB42BCC8F738C7FBB6CCA27A902DC663DBE1";

            // Act
            var data = hash.Calculate(new EncryptionData(TargetString));
            var actual = data.Hex;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SHA1_Should_CalculateCorrectHash_When_EncodingProvided()
        {
            // Arrange
            var hash = new Hash(Hash.Provider.SHA1);
            var expected = "5EE81DA7C13B58069B090AFF467E0243265875D7";

            // Act
            var data = hash.Calculate(new EncryptionData("全球最大的華文新聞網站", Encoding.UTF32));
            var actual = data.Hex;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SHA1_Should_CalculateCorrectHash_When_Salted()
        {
            // Arrange
            var hash = new Hash(Hash.Provider.SHA1);
            var expected = "43608DA36175EB1995897396503CEF68C80D1EB3";

            // Act
            var data = hash.Calculate(new EncryptionData(TargetString), new EncryptionData("Salty!"));
            var actual = data.Hex;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SHA256_Should_CalculateCorrectHash()
        {
            // Arrange
            var hash = new Hash(Hash.Provider.SHA256);
            var expected = "40AF07ABFE970590B2C313619983651B1E7B2F8C2D855C6FD4266DAFD7A5E670";

            // Act
            var data = hash.Calculate(new EncryptionData(TargetString));
            var actual = data.Hex;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SHA256_Should_CalculateCorrectHash_When_Salted()
        {
            // Arrange
            var hash = new Hash(Hash.Provider.SHA256);
            var expected = "C86220F692D1E4AC7FD86FB00B2AAAD1766D22FF7D2451D2682D564B952B5F29";

            // Act
            var data = hash.Calculate(new EncryptionData(TargetString), new EncryptionData("Salty!"));
            var actual = data.Hex;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SHA384_Should_CalculateCorrectHash()
        {
            // Arrange
            var hash = new Hash(Hash.Provider.SHA384);
            var expected = "9FC0AFB3DA61201937C95B133AB397FE62C329D6061A8768" +
                           "DA2B9D09923F07624869D01CD76826E1152DAB7BFAA30915";

            // Act
            var data = hash.Calculate(new EncryptionData(TargetString));
            var actual = data.Hex;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SHA384_Should_CalculateCorrectHash_When_Salted()
        {
            // Arrange
            var hash = new Hash(Hash.Provider.SHA384);
            var expected = "72558A538E25857207BCC2E334630EE7B7E55D84B96B04FB" +
                           "324CF36E5A0A72793D0C58315B29DF6C71B74DBEC073319C";

            // Act
            var data = hash.Calculate(new EncryptionData(TargetString), new EncryptionData("Salty!"));
            var actual = data.Hex;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SHA512_Should_CalculateCorrectHash()
        {
            // Arrange
            var hash = new Hash(Hash.Provider.SHA512);
            var expected = "2E7D4B051DD528F3E9339E0927930007426F4968B5A4A08349472784272F17DA" +
                           "5C532EDCFFE14934988503F77DEF4AB58EB05394838C825632D04A10F42A753B";

            // Act
            var data = hash.Calculate(new EncryptionData(TargetString));
            var actual = data.Hex;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SHA512_Should_CalculateCorrectHash_When_Salted()
        {
            // Arrange
            var hash = new Hash(Hash.Provider.SHA512);
            var expected = "4CE75E7E10523243AFD29FB7E15D15BF0DB1DBCC42194089214E1C2661AA6EA9" +
                           "67DE2D496EA17395FC17C7C63540B136D6292811020C7F48D48905E0F6030CCC";

            // Act
            var data = hash.Calculate(new EncryptionData(TargetString), new EncryptionData("Salty!"));
            var actual = data.Hex;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Value_Should_ReturnSameResultAsComputerHash()
        {
            // Arrange
            var hash = new Hash(Hash.Provider.MD5);

            // Act
            var data = hash.Calculate(new EncryptionData(TargetString), new EncryptionData("Salty!"));
            var actual = hash.Value.Hex;
            var expected = data.Hex;

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
