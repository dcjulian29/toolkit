using System;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;

namespace ToolKit.Cryptography
{
    /// <summary>
    /// Represents a public encryption key. Intended to be shared, it contains only the Modulus and Exponent.
    /// </summary>
    public class RsaPublicKey
    {
        private const string ElementExponent = "Exponent";
        private const string ElementModulus = "Modulus";
        private const string ElementParent = "RSAKeyValue";
        private const string KeyExponent = "PublicKey.Exponent";
        private const string KeyModulus = "PublicKey.Modulus";

        /// <summary>
        /// Initializes a new instance of the <see cref="RsaPublicKey"/> class.
        /// </summary>
        /// <param name="keyXml">The public key represented as a XML string.</param>
        public RsaPublicKey(string keyXml)
        {
            Modulus = ReadXmlElement(keyXml, ElementModulus);
            Exponent = ReadXmlElement(keyXml, ElementExponent);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RsaPublicKey"/> class.
        /// </summary>
        /// <param name="exponent">The exponent.</param>
        /// <param name="modulus">The modulus.</param>
        public RsaPublicKey(string exponent, string modulus)
        {
            Exponent = exponent;
            Modulus = modulus;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RsaPublicKey"/> class.
        /// </summary>
        public RsaPublicKey()
        {
        }

        /// <summary>
        /// Gets or sets the "Exponent" parameter for the asymmetric algorithm.
        /// </summary>
        /// <value>The "Exponent" parameter for the asymmetric algorithm.</value>
        public string Exponent { get; set; }

        /// <summary>
        /// Gets or sets the "Modulus" parameter for the asymmetric algorithm.
        /// </summary>
        /// <value>The "Modulus" parameter for the asymmetric algorithm.</value>
        public string Modulus { get; set; }

        /// <summary>
        /// Loads public key from an X509 Certificate.
        /// </summary>
        /// <param name="certificateFileName">Name of the certificate file.</param>
        /// <returns>an RSA Public Key instance containing the public key, or null.</returns>
        public static RsaPublicKey LoadFromCertificateFile(string certificateFileName)
        {
            if (!File.Exists(certificateFileName))
            {
                throw new ArgumentException("Certificate File does not exist!", nameof(certificateFileName));
            }

            var rawCert = File.ReadAllBytes(certificateFileName);

            var cert = new X509Certificate2();

            cert.Import(rawCert);

            return new RsaPublicKey(cert.PublicKey.Key.ToXmlString(false));
        }

        /// <summary>
        /// Loads public key from an X509 Certificate.
        /// </summary>
        /// <param name="certificateFileName">Name of the certificate file.</param>
        /// <param name="filePassword">Passwrod of the certificate file.</param>
        /// <returns>an RSA Public Key instance containing the public key, or null.</returns>
        public static RsaPublicKey LoadFromCertificateFile(string certificateFileName, string filePassword)
        {
            if (!File.Exists(certificateFileName))
            {
                throw new ArgumentException("Certificate File does not exist!", nameof(certificateFileName));
            }

            var rawCert = File.ReadAllBytes(certificateFileName);

            var cert = new X509Certificate2();

            cert.Import(rawCert, filePassword, X509KeyStorageFlags.DefaultKeySet);

            return new RsaPublicKey(cert.PublicKey.Key.ToXmlString(false));
        }

        /// <summary>
        /// Load public key from app.config or web.config file
        /// </summary>
        /// <returns>an RSA Public Key instance containing the public key, or null.</returns>
        public static RsaPublicKey LoadFromConfig()
        {
            var key = new RsaPublicKey
            {
                Modulus = ReadConfigKey(KeyModulus),
                Exponent = ReadConfigKey(KeyExponent)
            };

            return key;
        }

        /// <summary>
        /// Load public key from XML represented as a string.
        /// </summary>
        /// <param name="keyXml">The key represented as a XML String.</param>
        /// <returns>an RSA Public Key instance containing the public key, or null.</returns>
        public static RsaPublicKey LoadFromXml(string keyXml)
        {
            return new RsaPublicKey(keyXml);
        }

        /// <summary>
        /// Load public key from a File containing XML represented as a string.
        /// </summary>
        /// <param name="filePath">The name of the file to load the XML from.</param>
        /// <returns>an RSA Public Key instance containing the public key, or null.</returns>
        public static RsaPublicKey LoadFromXmlFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new ArgumentException("Public key file does not exist!", nameof(filePath));
            }

            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = new StreamReader(stream))
                {
                    return new RsaPublicKey(reader.ReadToEnd());
                }
            }
        }

        /// <summary>
        /// Writes the XML representation of this public key to a file.
        /// </summary>
        /// <param name="filePath">The file path to export the XML to.</param>
        /// <param name="overwrite">if set to <c>true</c> and the file exists, it will be overwritten.</param>
        public void ExportToXmlFile(string filePath, bool overwrite = false)
        {
            var mode = FileMode.CreateNew;

            if (overwrite)
            {
                mode = FileMode.Create;
            }

            using (var stream = new FileStream(filePath, mode))
            {
                using (var sw = new StreamWriter(stream))
                {
                    sw.Write(ToXml());
                    sw.Close();
                }
            }
        }

        /// <summary>
        /// Converts this public key to an RSAParameters object.
        /// </summary>
        /// <returns>A RSAParameters instance containing the parameters from this key.</returns>
        public RSAParameters ToParameters()
        {
            var r = new RSAParameters
            {
                Modulus = Convert.FromBase64String(Modulus),
                Exponent = Convert.FromBase64String(Exponent)
            };

            return r;
        }

        /// <summary>
        /// Converts this public key to its XML string representation.
        /// </summary>
        /// <returns>this public key represented as a XML string.</returns>
        public string ToXml()
        {
            var sb = new StringBuilder();

            sb.Append(WriteXmlNode(ElementParent));
            sb.Append(WriteXmlElement(ElementModulus, Modulus));
            sb.Append(WriteXmlElement(ElementExponent, Exponent));
            sb.Append(WriteXmlNode(ElementParent, true));

            return sb.ToString();
        }

        private static string ReadConfigKey(string key)
        {
            var s = Convert.ToString(ConfigurationManager.AppSettings.Get(key));

            if (!String.IsNullOrEmpty(s))
            {
                return s;
            }

            throw new ConfigurationErrorsException($"key <{key}> is missing from .config file");
        }

        private static string ReadXmlElement(string xml, string element)
        {
            var m = Regex.Match(xml, $"<{element}>(?<Element>[^>]*)</{element}>", RegexOptions.IgnoreCase);

            if (m.Captures.Count == 0)
            {
                throw new ArgumentException($"Could not find <{element}></{element}> in provided Public Key XML.");
            }

            return m.Groups["Element"].ToString();
        }

        private static string WriteXmlElement(string element, string value)
        {
            return $"<{element}>{value}</{element}>{Environment.NewLine}";
        }

        private static string WriteXmlNode(string element, bool closing = false)
        {
            return closing ? $"/<{element}>{Environment.NewLine}" : $"<{element}>{Environment.NewLine}";
        }
    }
}
