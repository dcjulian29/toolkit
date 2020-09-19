using System;
using System.Diagnostics;
using System.Net;
using System.Net.Security;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using ToolKit.Cryptography;
using Xunit;

namespace UnitTests.Cryptography
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
         "StyleCop.CSharp.DocumentationRules",
         "SA1600:ElementsMustBeDocumented",
         Justification = "Test Suites do not need XML Documentation.")]
    public class SslAcceptPolicyTests
    {
        [Fact]
        public void AcceptAll_Should_AllowSelfSignedCertificate()
        {
            // Arrange
            SslAcceptPolicy.Reset();
            var cert = LoadCertificate();
            var chain = new X509Chain();

            SslAcceptPolicy.AcceptAll();

            // Act
            var actual = ServicePointManager.ServerCertificateValidationCallback.Invoke(
                this,
                cert,
                chain,
                SslPolicyErrors.None);

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void AcceptAll_Should_AllowSelfSignedCertificate_When_PriorPolicyIsPresent()
        {
            // Arrange
            SslAcceptPolicy.Reset();
            var cert = LoadCertificate();
            var chain = new X509Chain();
            ServicePointManager.ServerCertificateValidationCallback = AnotherCertificatePolicy.Validate;

            SslAcceptPolicy.AcceptAll();

            // Act
            var actual = ServicePointManager.ServerCertificateValidationCallback.Invoke(
                this,
                cert,
                chain,
                SslPolicyErrors.None);

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void AcceptAll_Should_EnableAllCertificatedToBeAccepted()
        {
            // Arrange
            SslAcceptPolicy.Reset();
            var expected = typeof(SslAcceptPolicy.AcceptCertificatePolicy).Name;

            // Act
            SslAcceptPolicy.AcceptAll();
            var actual = ServicePointManager.ServerCertificateValidationCallback.Method.DeclaringType.Name;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AcceptAll_Should_EnablePolicy()
        {
            // Arrange
            SslAcceptPolicy.Reset();

            // Act
            SslAcceptPolicy.AcceptAll();

            // Assert
            Assert.True(SslAcceptPolicy.Enabled);
        }

        [Fact]
        public void AcceptAll_Should_ThrowException_When_AlreadyEnabled()
        {
            // Arrange
            SslAcceptPolicy.Reset();
            SslAcceptPolicy.AcceptAll();

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => SslAcceptPolicy.AcceptAll());
        }

        [Fact]
        public void OriginalPolicy_Should_ContainOriginalPolicy_When_AcceptAllIsEnabled()
        {
            // Arrange
            SslAcceptPolicy.Reset();
            var expected = ServicePointManager.ServerCertificateValidationCallback;

            // Act
            SslAcceptPolicy.AcceptAll();

            // Assert
            Assert.Equal(expected, SslAcceptPolicy.OriginalPolicy);
        }

        [Fact]
        public void Reset_Should_DisablePolicy()
        {
            // Act
            SslAcceptPolicy.Reset();

            // Assert
            Assert.False(SslAcceptPolicy.Enabled);
        }

        [Fact]
        public void Reset_Should_DisablePolicyAfterBeingEnabled()
        {
            // Arrange
            SslAcceptPolicy.Reset();
            SslAcceptPolicy.AcceptAll();

            // Act
            SslAcceptPolicy.Reset();

            // Assert
            Assert.False(SslAcceptPolicy.Enabled);
        }

        [Fact]
        public void Reset_Should_PutOriginalPolicy_When_OneExisted()
        {
            // Arrange
            SslAcceptPolicy.Reset();
            ServicePointManager.ServerCertificateValidationCallback = AnotherCertificatePolicy.Validate;
            var expected = typeof(AnotherCertificatePolicy).Name;

            // Act
            SslAcceptPolicy.AcceptAll();
            SslAcceptPolicy.Reset();
            var actual = ServicePointManager.ServerCertificateValidationCallback.Method.DeclaringType.Name;

            // Assert
            Assert.Equal(expected, actual);
        }

        private X509Certificate2 LoadCertificate()
        {
            var pfx = $"{Assembly.GetExecutingAssembly().GetName().Name}.SelfSign.pfx";
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(pfx);

            var bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);

            return new X509Certificate2(bytes, string.Empty);
        }

        private static class AnotherCertificatePolicy
        {
            internal static bool Validate(
                object sender,
                X509Certificate certificate,
                X509Chain chain,
                SslPolicyErrors sslPolicyErrors)
            {
                Debug.Print(sender.ToString());
                Debug.Print(certificate.ToString());
                Debug.Print(chain.ToString());
                Debug.Print(sslPolicyErrors.ToString());

                return false;
            }
        }
    }
}
