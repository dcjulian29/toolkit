using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Common.Logging;

namespace ToolKit.Cryptography
{
    /// <summary>
    /// This class allows a program to accept self-signed SSL certificate using the built-in .Net
    /// WebClient classes.
    /// </summary>
    public static class SslAcceptPolicy
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(SslAcceptPolicy));

        /// <summary>
        /// Gets a value indicating whether <see cref="SslAcceptPolicy"/> is enabled.
        /// </summary>
        public static bool Enabled { get; private set; }

        /// <summary>Gets the original SSL Acceptance Policy.</summary>
        internal static RemoteCertificateValidationCallback OriginalPolicy { get; private set; }

        /// <summary>
        /// Set the SSL Acceptance Policy to accept all certificates even self-signed certificates.
        /// </summary>
        [SuppressMessage(
            "Security",
            "CA5359:Do Not Disable Certificate Validation",
            Justification = "Sometimes you just need to accept self-signed certificates.")]
        public static void AcceptAll()
        {
            if (Enabled)
            {
                throw new InvalidOperationException("SSL Accept Policy is already active. Reset First.");
            }

            OriginalPolicy = ServicePointManager.ServerCertificateValidationCallback;

            ServicePointManager.ServerCertificateValidationCallback = AcceptCertificatePolicy.Validate;

            Enabled = true;
        }

        /// <summary>
        /// Reset the SSL Acceptance Policy to the default policy or the previous policy if one was set..
        /// </summary>
        public static void Reset()
        {
            if (!Enabled)
            {
                return;
            }

            ServicePointManager.ServerCertificateValidationCallback = OriginalPolicy;

            OriginalPolicy = null;
            Enabled = false;
        }

        /// <summary>
        /// Implements an SSL Certificate Policy that considers all certificates valid, no matter what.
        /// </summary>
        internal static class AcceptCertificatePolicy
        {
            /// <summary>Validates the SSL Certificate.</summary>
            /// <param name="sender">The sender.</param>
            /// <param name="certificate">The certificate.</param>
            /// <param name="chain">The chain.</param>
            /// <param name="sslPolicyErrors">The SSL policy errors.</param>
            /// <returns>
            ///   <c>true</c> telling the sender to always accept the certificate.
            /// </returns>
            internal static bool Validate(
                object sender,
                X509Certificate certificate,
                X509Chain chain,
                SslPolicyErrors sslPolicyErrors)
            {
                _log.Debug($"Accepting Certificate: {certificate.Subject}\nFrom: {certificate.Issuer}");
                _log.Debug($"Sender: {sender}");
                _log.Debug($"Chain: {chain}");
                _log.Debug($"SSL Policy Errors: {sslPolicyErrors}");

                return true;
            }
        }
    }
}
