using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;

namespace ToolKit.Cryptography
{
    /// <summary>
    /// Provides access to the Date Protection API Win32 subsystem.
    /// </summary>
    [SuppressMessage(
        "Design",
        "CA1060:Move pinvokes to native methods class",
        Justification = "This is a native methods class.")]
    public class DataProtectionApi
    {
        [Flags]
        private enum CryptProtect
        {
            None = 0x0,

            UiForbidden = 0x1,

            LocalMachine = 0x4
        }

        /// <summary>
        /// Gets or sets the key used in the encryption and decryption.
        /// </summary>
        /// <value>The key used in the encryption and decryption.</value>
        public EncryptionData Key { get; set; } = new EncryptionData(
                        SHA256Hash.Create().ComputeToBytes(Encoding.Unicode.GetBytes(Environment.MachineName)));

        /// <summary>
        /// Gets or sets the type of the key.
        /// </summary>
        /// <value>The type of the key.</value>
        public DataProtectionKeyType KeyType { get; set; } = DataProtectionKeyType.UserKey;

        /// <summary>
        /// Decrypts the specified data.
        /// </summary>
        /// <param name="cipherText">A string containing the encrypted data.</param>
        /// <returns>A string containing the decrypted data.</returns>
        public string Decrypt(string cipherText) => Encoding.Unicode.GetString(Decrypt(Convert.FromBase64String(cipherText)));

        /// <summary>
        /// Decrypts the specified data.
        /// </summary>
        /// <param name="cipherTextBytes">A byte array containing the encrypted data.</param>
        /// <returns>A byte array containing the decrypted data.</returns>
        public byte[] Decrypt(byte[] cipherTextBytes)
        {
            var plainTextBlob = default(DataBlob);
            var cipherTextBlob = new DataBlob(cipherTextBytes);
            var entropyBlob = new DataBlob(Key.Bytes);

            var description = string.Empty;

            try
            {
                CryptUnprotectData(
                    ref cipherTextBlob,
                    ref description,
                    ref entropyBlob,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    CryptProtect.UiForbidden,
                    ref plainTextBlob);

                var plainTextBytes = new byte[plainTextBlob.DataLength];

                Marshal.Copy(plainTextBlob.DataBuffer, plainTextBytes, 0, plainTextBlob.DataLength);
                return plainTextBytes;
            }
            finally
            {
                if (plainTextBlob.DataBuffer != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(plainTextBlob.DataBuffer);
                }

                if (cipherTextBlob.DataBuffer != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(cipherTextBlob.DataBuffer);
                }

                if (entropyBlob.DataBuffer != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(entropyBlob.DataBuffer);
                }
            }
        }

        /// <summary>
        /// Encrypts the specified data.
        /// </summary>
        /// <param name="plainText">A string containing the data to protect.</param>
        /// <returns>A string containing the encrypted data.</returns>
        public string Encrypt(string plainText) => Convert.ToBase64String(Encrypt(Encoding.Unicode.GetBytes(plainText)));

        /// <summary>
        /// Encrypts the specified data.
        /// </summary>
        /// <param name="plainTextBytes">A byte array containing data to protect.</param>
        /// <returns>A byte array representing the encrypted data.</returns>
        public byte[] Encrypt(byte[] plainTextBytes)
        {
            var description = string.Empty;

            var plainTextBlob = new DataBlob(plainTextBytes);
            var cipherTextBlob = new DataBlob(null);
            var entropyBlob = new DataBlob(Key.Bytes);

            try
            {
                var flags = CryptProtect.UiForbidden;

                if (KeyType == DataProtectionKeyType.MachineKey)
                {
                    flags |= CryptProtect.LocalMachine;
                }

                CryptProtectData(
                    ref plainTextBlob,
                    description,
                    ref entropyBlob,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    flags,
                    ref cipherTextBlob);

                var cipherTextBytes = new byte[cipherTextBlob.DataLength];

                Marshal.Copy(cipherTextBlob.DataBuffer, cipherTextBytes, 0, cipherTextBlob.DataLength);

                return cipherTextBytes;
            }
            finally
            {
                if (plainTextBlob.DataBuffer != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(plainTextBlob.DataBuffer);
                }

                if (cipherTextBlob.DataBuffer != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(cipherTextBlob.DataBuffer);
                }

                if (entropyBlob.DataBuffer != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(entropyBlob.DataBuffer);
                }
            }
        }

        [ExcludeFromCodeCoverage]
        [DllImport("crypt32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern bool CryptProtectData(
            ref DataBlob plainText,
            string description,
            ref DataBlob entropy,
            IntPtr reserved,
            IntPtr prompt,
            CryptProtect flags,
            ref DataBlob cipherText);

        [ExcludeFromCodeCoverage]
        [DllImport("crypt32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern bool CryptUnprotectData(
            ref DataBlob cipherText,
            ref string description,
            ref DataBlob entropy,
            IntPtr reserved,
            IntPtr prompt,
            CryptProtect flags,
            ref DataBlob plainText);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct DataBlob
        {
            public DataBlob(byte[] data = null)
            {
                if (data == null)
                {
                    data = Array.Empty<byte>();
                }

                DataBuffer = Marshal.AllocHGlobal(data.Length);

                DataLength = data.Length;

                Marshal.Copy(data, 0, DataBuffer, data.Length);
            }

            internal int DataLength { get; set; }

            internal IntPtr DataBuffer { get; set; }
        }
    }
}
