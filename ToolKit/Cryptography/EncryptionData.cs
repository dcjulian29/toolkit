using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace ToolKit.Cryptography
{
    /// <summary>
    /// Represents Hex, Byte, Base64, or String data to encrypt/decrypt; use the .Text property to
    /// set/get a string representation use the .Hex property to set/get a string-based Hexadecimal
    /// representation use the .Base64 to set/get a string-based Base64 representation
    /// </summary>
    /// <remarks>
    /// Adapted from code originally written by Jeff Atwood. The original code had no explicit
    /// license attached to it. If licensing is a concern, you should contact the original author.
    /// </remarks>
    public class EncryptionData : IEquatable<EncryptionData>
    {
        private byte[] _byteData;

        /// <summary>
        /// Initializes a new instance of the <see cref="EncryptionData"/> class.
        /// </summary>
        public EncryptionData()
        {
            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EncryptionData"/> class.
        /// </summary>
        /// <param name="data">A byte array containing the data.</param>
        public EncryptionData(byte[] data)
        {
            Initialize();

            _byteData = data;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EncryptionData"/> class.
        /// </summary>
        /// <param name="data">A string containing the data.</param>
        public EncryptionData(string data)
        {
            Initialize();

            Text = data;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EncryptionData"/> class.
        /// </summary>
        /// <param name="data">A string containing the data.</param>
        /// <param name="encoding">The encoding to use to convert the string to a byte array.</param>
        public EncryptionData(string data, Encoding encoding)
        {
            Initialize();

            EncodingToUse = encoding;
            Text = data;
        }

        /// <summary>
        /// Gets or sets the Base64 string representation of this data.
        /// </summary>
        /// <value>The Base64 string representation of this data.</value>
        public string Base64
        {
            get => Base64Encoding.ToString(Bytes);

            set => Bytes = Base64Encoding.ToBytes(value);
        }

        /// <summary>
        /// Gets or sets the byte representation of the data;
        /// </summary>
        /// <value>
        /// The byte representation of the data; This will be padded to MinBytes and trimmed to
        /// MaxBytes as necessary.
        /// </value>
        public byte[] Bytes
        {
            get
            {
                if ((MaximumBytes > 0) && (_byteData.Length > MaximumBytes))
                {
                    var b = new byte[MaximumBytes];
                    Array.Copy(_byteData, b, b.Length);
                    _byteData = b;
                }

                if ((MinimumBytes > 0) && (_byteData.Length < MinimumBytes))
                {
                    var b = new byte[MinimumBytes];
                    Array.Copy(_byteData, b, _byteData.Length);
                    _byteData = b;
                }

                return _byteData;
            }

            set => _byteData = value;
        }

        /// <summary>
        /// Gets or sets the text encoding for this instance
        /// </summary>
        public Encoding EncodingToUse { get; set; }

        /// <summary>
        /// Gets or sets the Hex string representation of this data.
        /// </summary>
        /// <value>The Hex string representation of this data.</value>
        public string Hex
        {
            get => HexEncoding.ToString(Bytes);

            set => Bytes = HexEncoding.ToBytes(value);
        }

        /// <summary>
        /// Gets a value indicating whether this instance is empty.
        /// </summary>
        /// <value><c>true</c> if no data is present; otherwise, <c>false</c>.</value>
        public bool IsEmpty
        {
            get
            {
                if (Bytes == null)
                {
                    return true;
                }

                return Bytes.Length == 0;
            }
        }

        /// <summary>
        /// Gets or sets the maximum number of bits allowed for this data.
        /// </summary>
        /// <value>The he maximum number of bits allowed for this data; if 0, no limit.</value>
        public int MaximumBits
        {
            get => MaximumBytes * 8;

            set => MaximumBytes = value / 8;
        }

        /// <summary>
        /// Gets or sets the maximum number of bytes allowed for this data.
        /// </summary>
        /// <value>The maximum number of bytes allowed for this data; if 0, no limit.</value>
        public int MaximumBytes { get; set; }

        /// <summary>
        /// Gets or sets the minimum number of bits allowed for this data.
        /// </summary>
        /// <value>The he minimum number of bits allowed for this data; if 0, no limit.</value>
        public int MinimumBits
        {
            get => MinimumBytes * 8;

            set => MinimumBytes = value / 8;
        }

        /// <summary>
        /// Gets or sets the minimum number of bytes allowed for this data.
        /// </summary>
        /// <value>The he minimum number of bytes allowed for this data; if 0, no limit.</value>
        public int MinimumBytes { get; set; }

        /// <summary>
        /// Gets or sets the text representation of bytes using the text encoding assigned to this instance.
        /// </summary>
        /// <value>The text representation of bytes.</value>
        public string Text
        {
            get
            {
                if (Bytes == null)
                {
                    return String.Empty;
                }

                // Need to handle nulls here; oddly, C# will happily convert nulls into the string
                // whereas VB stops converting at the first null.
                var i = Array.IndexOf(Bytes, Convert.ToByte(0));

                return i >= 0 ? EncodingToUse.GetString(Bytes, 0, i) : EncodingToUse.GetString(Bytes);
            }

            set => Bytes = EncodingToUse.GetBytes(value);
        }

        /// <summary>
        /// Determines if two instances of EncryptionData instances are not equal.
        /// </summary>
        /// <param name="left">The left <see cref="EncryptionData"/> instance.</param>
        /// <param name="right">The right <see cref="EncryptionData"/> instance.</param>
        /// <returns><c>true</c> if the two instances are not equal.</returns>
        public static bool operator !=(EncryptionData left, EncryptionData right) => !Equals(left, right);

        /// <summary>
        /// Determines if two instances of EncryptionData instances are equal.
        /// </summary>
        /// <param name="left">The left <see cref="EncryptionData"/> instance.</param>
        /// <param name="right">The right <see cref="EncryptionData"/> instance.</param>
        /// <returns><c>true</c> if the two instances are equal.</returns>
        public static bool operator ==(EncryptionData left, EncryptionData right) => Equals(left, right);

        /// <inheritdoc/>
        public bool Equals(EncryptionData other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            return ReferenceEquals(this, other) || Equals(_byteData, other._byteData);
        }

        /// <inheritdoc/>
        public override bool Equals(object obj) => (obj.GetType() == typeof(EncryptionData)) && Equals((EncryptionData)obj);

        /// <inheritdoc/>
        [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode", Justification = "It's ok to have a non-read only member in this case.")]
#pragma warning disable S2328 // "GetHashCode" should not reference mutable fields
        public override int GetHashCode() =>
#pragma warning restore S2328 // "GetHashCode" should not reference mutable fields
            _byteData == null
                ? 0
                : (37 * (_byteData[0] + _byteData.Length)) +
                  _byteData[_byteData.Length - 1] + _byteData.Length;

        /// <summary>
        /// returns Base64 string representation of this data.
        /// </summary>
        /// <returns>a Base64 string representation of this data.</returns>
        public string ToBase64() => Base64;

        /// <summary>
        /// returns Hex string representation of this data.
        /// </summary>
        /// <returns>a hex string representation of this data.</returns>
        public string ToHex() => Hex;

        /// <summary>
        /// Returns text representation of bytes using the default text encoding
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public override string ToString() => Text;

        private void Initialize()
        {
            MinimumBytes = 0;
            MaximumBytes = 0;
            EncodingToUse = Encoding.UTF8;
        }
    }
}
