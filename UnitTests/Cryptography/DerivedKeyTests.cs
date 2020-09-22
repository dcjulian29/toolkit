using System.Diagnostics.CodeAnalysis;
using ToolKit.Cryptography;
using Xunit;

namespace UnitTests.Cryptography
{
    [SuppressMessage(
         "StyleCop.CSharp.DocumentationRules",
         "SA1600:ElementsMustBeDocumented",
         Justification = "Test Suites do not need XML Documentation.")]
    public class DerivedKeysTest
    {
        private readonly string _password = "asdfasdfasdf1324jlkjh";
        private readonly string _plainText = "Now is the time for all good men to come to the aid of their country.";

        [Fact]
        public void DerivedKeys_Should_ProduceTh_SamrKeyProvidedWithSamePasswordAndSalt()
        {
            // Arrange
            var salt = "98765poiuy";
            var vector = new EncryptionData("sdfasdfasdfasdf");

            var k1 = new DerivedKey(_password, salt);
            var ek1 = new EncryptionData(k1.GetBytes(25));

            var k2 = new DerivedKey(_password, salt);
            var ek2 = new EncryptionData(k2.GetBytes(25));

            var e1 = new SymmetricEncryption(SymmetricEncryption.Provider.TripleDES)
            {
                InitializationVector = vector
            };

            var e2 = new SymmetricEncryption(SymmetricEncryption.Provider.TripleDES)
            {
                InitializationVector = vector
            };

            // Act
            var encrypted = e1.Encrypt(new EncryptionData(_plainText), ek1);
            var decrypted = e2.Decrypt(encrypted, ek2);

            // Assert
            Assert.Equal(_plainText, decrypted.Text);
        }

        [Fact]
        public void DerivedKeys_Should_ProduceTheSameKeyProvidedWithSamePassword()
        {
            // Arrange
            var vector = new EncryptionData("sdfasdfasdfasdf");
            var k1 = new DerivedKey(_password);
            var ek1 = new EncryptionData(k1.GetBytes(25));

            var k2 = new DerivedKey(_password);
            var ek2 = new EncryptionData(k2.GetBytes(25));

            var e1 = new SymmetricEncryption(SymmetricEncryption.Provider.TripleDES)
            {
                InitializationVector = vector
            };

            var e2 = new SymmetricEncryption(SymmetricEncryption.Provider.TripleDES)
            {
                InitializationVector = vector
            };

            // Act
            var encrypted = e1.Encrypt(new EncryptionData(_plainText), ek1);
            var decrypted = e2.Decrypt(encrypted, ek2);

            // Assert
            Assert.Equal(_plainText, decrypted.Text);
        }
    }
}
