using System;
using System.Diagnostics.CodeAnalysis;
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
    [SuppressMessage(
        "Design",
        "RCS1187:Use constant instead of field.",
        Justification = "This time the analysis rule don't make sense for this class")]
    [SuppressMessage(
        "Performance",
        "CA1802:Use literals where appropriate",
        Justification = "This time the analysis rule don't make sense for this class")]
    public class RsaPrivateKey
    {
        private static readonly string _elementCoefficient = "InverseQ";

        private static readonly string _elementExponent = "Exponent";

        private static readonly string _elementModulus = "Modulus";

        private static readonly string _elementParent = "RSAKeyValue";

        private static readonly string _elementPrimeExponentP = "DP";

        private static readonly string _elementPrimeExponentQ = "DQ";

        private static readonly string _elementPrimeP = "P";

        private static readonly string _elementPrimeQ = "Q";

        private static readonly string _elementPrivateExponent = "D";

        private static readonly string _keyCoefficient = "PrivateKey.InverseQ";

        private static readonly string _keyExponent = "PublicKey.Exponent";

        private static readonly string _keyModulus = "PublicKey.Modulus";

        private static readonly string _keyPrimeExponentP = "PrivateKey.DP";

        private static readonly string _keyPrimeExponentQ = "PrivateKey.DQ";

        private static readonly string _keyPrimeP = "PrivateKey.P";

        private static readonly string _keyPrimeQ = "PrivateKey.Q";

        private static readonly string _keyPrivateExponent = "PrivateKey.D";

        /// <summary>
        /// Initializes a new instance of the <see cref="RsaPrivateKey" /> class.
        /// </summary>
        /// <param name="keyXml">The private key represented as a XML string.</param>
        public RsaPrivateKey(string keyXml)
        {
            Coefficient = ReadXmlElement(keyXml, "InverseQ");
            Exponent = ReadXmlElement(keyXml, _elementExponent);
            Modulus = ReadXmlElement(keyXml, _elementModulus);
            PrimeExponentP = ReadXmlElement(keyXml, "DP");
            PrimeExponentQ = ReadXmlElement(keyXml, "DQ");
            PrimeP = ReadXmlElement(keyXml, "P");
            PrimeQ = ReadXmlElement(keyXml, "Q");
            PrivateExponent = ReadXmlElement(keyXml, "D");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RsaPrivateKey" /> class.
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

            if (string.IsNullOrEmpty(privateKeyPassword))
            {
                throw new ArgumentNullException(nameof(privateKeyPassword));
            }

            var rawCert = File.ReadAllBytes(certificateFileName);

            using (var cert = new X509Certificate2())
            {
                cert.Import(rawCert, privateKeyPassword, X509KeyStorageFlags.Exportable);

                return new RsaPrivateKey(cert.PrivateKey.ToXmlString(true));
            }
        }

        /// <summary>
        /// Load private key from app.config or web.config file.
        /// </summary>
        /// <returns>an AsymmetricPrivateKey instance containing the private key, or null.</returns>
        public static RsaPrivateKey LoadFromEnvironment()
        {
            var key = new RsaPrivateKey
            {
                // Public Key parts
                Modulus = ReadKeyFromEnvironment(_keyModulus),
                Exponent = ReadKeyFromEnvironment(_keyExponent),

                // Private Key parts
                PrimeP = ReadKeyFromEnvironment(_keyPrimeP),
                PrimeQ = ReadKeyFromEnvironment(_keyPrimeQ),
                PrimeExponentP = ReadKeyFromEnvironment(_keyPrimeExponentP),
                PrimeExponentQ = ReadKeyFromEnvironment(_keyPrimeExponentQ),
                Coefficient = ReadKeyFromEnvironment(_keyCoefficient),
                PrivateExponent = ReadKeyFromEnvironment(_keyPrivateExponent)
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

            FileStream stream = null;

            try
            {
                stream = new FileStream(filePath, mode);
                using (var writer = new StreamWriter(stream))
                {
                    stream = null;

                    writer.Write(ToXml());
                    writer.Close();
                }
            }
            finally
            {
                stream?.Dispose();
            }
        }

        /// <summary>
        /// Converts this private key to an RSAParameters object.
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

            sb.Append(WriteXmlNode(_elementParent));
            sb.Append(WriteXmlElement(_elementCoefficient, Coefficient));
            sb.Append(WriteXmlElement(_elementExponent, Exponent));
            sb.Append(WriteXmlElement(_elementModulus, Modulus));
            sb.Append(WriteXmlElement(_elementPrimeExponentP, PrimeExponentP));
            sb.Append(WriteXmlElement(_elementPrimeExponentQ, PrimeExponentQ));
            sb.Append(WriteXmlElement(_elementPrimeP, PrimeP));
            sb.Append(WriteXmlElement(_elementPrimeQ, PrimeQ));
            sb.Append(WriteXmlElement(_elementPrivateExponent, PrivateExponent));
            sb.Append(WriteXmlNode(_elementParent, true));

            return sb.ToString();
        }

        private static string ReadKeyFromEnvironment(string key, bool required = true)
        {
            var s = Environment.GetEnvironmentVariable(key);

            if (string.IsNullOrEmpty(s) && required)
            {
                throw new ArgumentException($"key <{key}> is missing from Missing from the Environment.");
            }

            return s;
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
