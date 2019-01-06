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
    /// Represents a private encryption key. Not intended to be shared, as it contains all the
    /// elements that make up the key.
    /// </summary>
    public class RsaPrivateKey
    {
        private const string ElementCoefficient = "InverseQ";
        private const string ElementExponent = "Exponent";
        private const string ElementModulus = "Modulus";
        private const string ElementParent = "RSAKeyValue";
        private const string ElementPrimeExponentP = "DP";
        private const string ElementPrimeExponentQ = "DQ";
        private const string ElementPrimeP = "P";
        private const string ElementPrimeQ = "Q";
        private const string ElementPrivateExponent = "D";
        private const string KeyCoefficient = "PrivateKey.InverseQ";
        private const string KeyExponent = "PublicKey.Exponent";
        private const string KeyModulus = "PublicKey.Modulus";
        private const string KeyPrimeExponentP = "PrivateKey.DP";
        private const string KeyPrimeExponentQ = "PrivateKey.DQ";
        private const string KeyPrimeP = "PrivateKey.P";
        private const string KeyPrimeQ = "PrivateKey.Q";
        private const string KeyPrivateExponent = "PrivateKey.D";

        /// <summary>
        /// Initializes a new instance of the <see cref="RsaPrivateKey"/> class.
        /// </summary>
        /// <param name="keyXml">The private key represented as a XML string.</param>
        public RsaPrivateKey(string keyXml)
        {
            Coefficient = ReadXmlElement(keyXml, "InverseQ");
            Exponent = ReadXmlElement(keyXml, ElementExponent);
            Modulus = ReadXmlElement(keyXml, ElementModulus);
            PrimeExponentP = ReadXmlElement(keyXml, "DP");
            PrimeExponentQ = ReadXmlElement(keyXml, "DQ");
            PrimeP = ReadXmlElement(keyXml, "P");
            PrimeQ = ReadXmlElement(keyXml, "Q");
            PrivateExponent = ReadXmlElement(keyXml, "D");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RsaPrivateKey"/> class.
        /// </summary>
        public RsaPrivateKey()
        {
        }

        /// <summary>
        /// Gets or sets the "Coefficient" parameter for the asymmetric algorithm.
        /// </summary>
        public string Coefficient { get; set; }

        /// <summary>
        /// Gets or sets the "Exponent" parameter for the asymmetric algorithm.
        /// </summary>
        public string Exponent { get; set; }

        /// <summary>
        /// Gets or sets the "Modulus" parameter for the asymmetric algorithm.
        /// </summary>
        public string Modulus { get; set; }

        /// <summary>
        /// Gets or sets the "PrimeExponentP" parameter for the asymmetric algorithm.
        /// </summary>
        public string PrimeExponentP { get; set; }

        /// <summary>
        /// Gets or sets the "PrimeExponentQ" parameter for the asymmetric algorithm.
        /// </summary>
        public string PrimeExponentQ { get; set; }

        /// <summary>
        /// Gets or sets the "PrimeP" parameter for the asymmetric algorithm.
        /// </summary>
        public string PrimeP { get; set; }

        /// <summary>
        /// Gets or sets the "PrimeQ" parameter for the asymmetric algorithm.
        /// </summary>
        public string PrimeQ { get; set; }

        /// <summary>
        /// Gets or sets the "PrivateExponent" parameter for the asymmetric algorithm.
        /// </summary>
        public string PrivateExponent { get; set; }

        /// <summary>
        /// Loads private key from an X509 Certificate if the private key is marked as exportable.
        /// </summary>
        /// <param name="certificateFileName">Name of the certificate file.</param>
        /// <param name="privateKeyPassword">The private key password.</param>
        /// <returns>an AsymmetricPrivateKey instance containing the private key, or null.</returns>
        public static RsaPrivateKey LoadFromCertificateFile(string certificateFileName, string privateKeyPassword)
        {
            if (!File.Exists(certificateFileName))
            {
                throw new ArgumentException("Certificate File does not exist!", nameof(certificateFileName));
            }

            if (String.IsNullOrEmpty(privateKeyPassword))
            {
                throw new ArgumentNullException(nameof(privateKeyPassword));
            }

            var rawCert = File.ReadAllBytes(certificateFileName);

            var cert = new X509Certificate2();

            cert.Import(rawCert, privateKeyPassword, X509KeyStorageFlags.Exportable);

            return new RsaPrivateKey(cert.PrivateKey.ToXmlString(true));
        }

        /// <summary>
        /// Load private key from app.config or web.config file
        /// </summary>
        /// <returns>an AsymmetricPrivateKey instance containing the private key, or null.</returns>
        public static RsaPrivateKey LoadFromEnvironment()
        {
            var key = new RsaPrivateKey
            {
                // Public Key parts
                Modulus = ReadKeyFromEnvironment(KeyModulus),
                Exponent = ReadKeyFromEnvironment(KeyExponent),

                // Private Key parts
                PrimeP = ReadKeyFromEnvironment(KeyPrimeP),
                PrimeQ = ReadKeyFromEnvironment(KeyPrimeQ),
                PrimeExponentP = ReadKeyFromEnvironment(KeyPrimeExponentP),
                PrimeExponentQ = ReadKeyFromEnvironment(KeyPrimeExponentQ),
                Coefficient = ReadKeyFromEnvironment(KeyCoefficient),
                PrivateExponent = ReadKeyFromEnvironment(KeyPrivateExponent)
            };

            return key;
        }

        /// <summary>
        /// Load private key from XML represented as a string.
        /// </summary>
        /// <param name="keyXml">The key represented as a XML String.</param>
        /// <returns>an AsymmetricPrivateKey instance containing the private key, or null.</returns>
        public static RsaPrivateKey LoadFromXml(string keyXml)
        {
            return new RsaPrivateKey(keyXml);
        }

        /// <summary>
        /// Load private key from a Stream containing XML represented as a string.
        /// </summary>
        /// <param name="filePath">The name of the file to load the XML from.</param>
        /// <returns>an AsymmetricPrivateKey instance containing the private key, or null.</returns>
        public static RsaPrivateKey LoadFromXmlFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new ArgumentException("Private key file does not exist!", nameof(filePath));
            }

            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = new StreamReader(stream))
                {
                    return new RsaPrivateKey(reader.ReadToEnd());
                }
            }
        }

        /// <summary>
        /// Writes the XML representation of this private key to a file.
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
        /// Converts this private key to an RSAParameters object
        /// </summary>
        /// <returns>A RSAParameters instance containing the parameters from this key.</returns>
        public RSAParameters ToParameters()
        {
            var parameters = new RSAParameters
            {
                InverseQ = Base64Encoding.ToBytes(Coefficient),
                Exponent = Base64Encoding.ToBytes(Exponent),
                Modulus = Base64Encoding.ToBytes(Modulus),
                DP = Base64Encoding.ToBytes(PrimeExponentP),
                DQ = Base64Encoding.ToBytes(PrimeExponentQ),
                P = Base64Encoding.ToBytes(PrimeP),
                Q = Base64Encoding.ToBytes(PrimeQ),
                D = Base64Encoding.ToBytes(PrivateExponent)
            };

            return parameters;
        }

        /// <summary>
        /// Creates a public key based on this private key.
        /// </summary>
        /// <returns>a public key based on this private key.</returns>
        public RsaPublicKey ToPublicKey()
        {
            var publicKey = new RsaPublicKey
            {
                Exponent = Exponent,
                Modulus = Modulus
            };

            return publicKey;
        }

        /// <summary>
        /// Converts this private key to its XML string representation.
        /// </summary>
        /// <returns>this private key represented as a XML string.</returns>
        public string ToXml()
        {
            var sb = new StringBuilder();

            sb.Append(WriteXmlNode(ElementParent));
            sb.Append(WriteXmlElement(ElementCoefficient, Coefficient));
            sb.Append(WriteXmlElement(ElementExponent, Exponent));
            sb.Append(WriteXmlElement(ElementModulus, Modulus));
            sb.Append(WriteXmlElement(ElementPrimeExponentP, PrimeExponentP));
            sb.Append(WriteXmlElement(ElementPrimeExponentQ, PrimeExponentQ));
            sb.Append(WriteXmlElement(ElementPrimeP, PrimeP));
            sb.Append(WriteXmlElement(ElementPrimeQ, PrimeQ));
            sb.Append(WriteXmlElement(ElementPrivateExponent, PrivateExponent));
            sb.Append(WriteXmlNode(ElementParent, true));

            return sb.ToString();
        }

        private static string ReadKeyFromEnvironment(string key, bool required = true)
        {
            var s = Environment.GetEnvironmentVariable(key);

            if (!String.IsNullOrEmpty(s))
            {
                return s;
            }

            throw new ApplicationException($"key <{key}> is missing from Missing from the Environment.");
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
