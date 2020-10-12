using ToolKit.Cryptography;
using Xunit;

namespace UnitTests
{
    public class DiffieHellmanTests
    {
        [Fact]
        public void Decrypt_Should_EncryptTheExpectedResult()
        {
            // Arrange
            var expected = new EncryptionData("The Secret Key");

            var customerA = new DiffieHellman();
            var customerB = new DiffieHellman();

            // Customer B wants to talk to Customer A
            var secret = customerB.Encrypt(customerA.PublicKey, expected);
            var publicKey = customerB.PublicKey;
            var iv = customerB.IV;

            // Act
            var actual = customerA.Decrypt(publicKey, secret, iv);

            // Assert
            Assert.Equal(expected.Text, actual.Text);
        }
    }
}
