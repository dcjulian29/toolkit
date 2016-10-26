using System.Diagnostics.CodeAnalysis;
using System.Text;
using ToolKit.Cryptography;
using Xunit;

namespace UnitTests.Cryptography
{
    [SuppressMessage(
         "StyleCop.CSharp.DocumentationRules",
         "SA1600:ElementsMustBeDocumented",
         Justification = "Test Suites do not need XML Documentation.")]
    public class DataProtectionApiTests
    {
        private string _original = "This is the text to encrypt";

        [Fact]
        public void DecryptWithBytes_Should_ReturnCorrectDecryptedResult()
        {
            // Arrange
            var originalBytes = Encoding.Unicode.GetBytes(_original);

            var dataProtectionApi = new DataProtectionApi();
            var encrypted = dataProtectionApi.Encrypt(originalBytes);

            // Act
            var decrypted = dataProtectionApi.Decrypt(encrypted);

            // Assert
            Assert.Equal(originalBytes, decrypted);
        }

        [Fact]
        public void DecryptWithBytes_Should_ReturnCorrectDecryptedResult_When_UsingMachineScope()
        {
            // Arrange
            var originalBytes = Encoding.Unicode.GetBytes(_original);

            var data = new DataProtectionApi
            {
                KeyType = DataProtectionKeyType.MachineKey
            };

            var encrypted = data.Encrypt(originalBytes);

            // Act
            var decrypted = data.Decrypt(encrypted);

            // Assert
            Assert.Equal(originalBytes, decrypted);
        }

        [Fact]
        public void DecryptWithBytes_Should_ReturnCorrectDecryptedResult_WhenUsingUserScope()
        {
            // Arrange
            var originalBytes = Encoding.Unicode.GetBytes(_original);

            var data = new DataProtectionApi
            {
                KeyType = DataProtectionKeyType.UserKey
            };

            var encrypted = data.Encrypt(originalBytes);

            // Act
            var decrypted = data.Decrypt(encrypted);

            // Assert
            Assert.Equal(originalBytes, decrypted);
        }

        [Fact]
        public void DecryptWithStrings_Should_ReturnCorrectDecryptedResult()
        {
            // Arrange
            var data = new DataProtectionApi();
            var encrypted = data.Encrypt(_original);

            // Act
            var decrypted = data.Decrypt(encrypted);

            // Assert
            Assert.Equal(_original, decrypted);
        }

        [Fact]
        public void DecryptWithStrings_Should_ReturnCorrectDecryptedResult_WhenUsingMachineScope()
        {
            // Arrange
            var data = new DataProtectionApi
            {
                KeyType = DataProtectionKeyType.MachineKey
            };

            var encrypted = data.Encrypt(_original);

            // Act
            var decrypted = data.Decrypt(encrypted);

            // Assert
            Assert.Equal(_original, decrypted);
        }

        [Fact]
        public void DecryptWithStrings_Should_ReturnCorrectDecryptedResult_WhenUsingUserScope()
        {
            // Arrange
            var data = new DataProtectionApi
            {
                KeyType = DataProtectionKeyType.UserKey
            };

            var encrypted = data.Encrypt(_original);

            // Act
            var decrypted = data.Decrypt(encrypted);

            // Assert
            Assert.Equal(_original, decrypted);
        }

        [Fact]
        public void Key_Should_AllowADifferentKeyToBeUsed()
        {
            // Arrange
            var data = new DataProtectionApi()
            {
                Key = new EncryptionData("UnitT3st")
            };

            var encrypted = data.Encrypt(_original);

            // Act
            var decrypted = data.Decrypt(encrypted);

            // Assert
            Assert.Equal(_original, decrypted);
        }
    }
}
