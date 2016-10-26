using System;
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
        private static ILog _log = LogManager.GetLogger(typeof(SslAcceptPolicy));

        /// <summary>
        /// Gets a value indicating whether <see cref="SslAcceptPolicy"/> is enabled.
        /// </summary>
        public static bool Enabled { get; private set; }

        internal static RemoteCertificateValidationCallback OriginalPolicy { get; private set; }

        /// <summary>
        /// Set the SSL Acceptance Policy to accept all certificates even self-signed certificates.
        /// </summary>
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
            if (Enabled)
            {
                ServicePointManager.ServerCertificateValidationCallback = OriginalPolicy;
                OriginalPolicy = null;
                Enabled = false;
            }
        }

        /// <summary>
        /// Implements an SSL Certificate Policy that considers all certificates valid, no matter what.
        /// </summary>
        internal static class AcceptCertificatePolicy
        {
            internal static bool Validate(
                object sender,
                X509Certificate certificate,
                X509Chain chain,
                SslPolicyErrors sslPolicyErrors)
            {
                _log.Debug(m =>
                        m("Accepting Certificate: {0}\nFrom: {2}", certificate.Subject, certificate.Issuer));

                return true;
            }
        }
    }
}
