using System;
using System.Runtime.InteropServices;
using System.Text;
using Common.Logging;

namespace ToolKit.Cryptography
{
    /// <summary>
    /// Provides access to the Date Protection API Win32 subsystem
    /// </summary>
    public class DataProtectionApi
    {
        private static ILog _log = LogManager.GetLogger<DataProtectionApi>();

        private EncryptionData _entropy;

        [Flags]
        private enum CryptProtectFlags
        {
            UiForbidden = 0x1,
            LocalMachine = 0x4
        }

        /// <summary>
        /// Gets or sets the key used in the encryption and decryption.
        /// </summary>
        /// <value>The key used in the encryption and decryption.</value>
        public EncryptionData Key
        {
            get
            {
                return _entropy ?? (_entropy =
                           new EncryptionData(
                               SHA256Hash.Create().ComputeToBytes(Encoding.Unicode.GetBytes(Environment.MachineName))));
            }

            set
            {
                _entropy = value;
            }
        }

        /// <summary>
        /// Gets or sets the type of the key.
        /// </summary>
        /// <value>The type of the key.</value>
        public DataProtectionKeyType KeyType { get; set; } = DataProtectionKeyType.UserKey;

        /// <summary>
        /// Decrypts the specified data
        /// </summary>
        /// <param name="cipherText">A string containing the encrypted data.</param>
        /// <returns>A string containing the decrypted data</returns>
        public string Decrypt(string cipherText)
        {
            return Encoding.Unicode.GetString(Decrypt(Convert.FromBase64String(cipherText)));
        }

        /// <summary>
        /// Decrypts the specified data
        /// </summary>
        /// <param name="cipherTextBytes">A byte array containing the encrypted data</param>
        /// <returns>A byte array containing the decrypted data</returns>
        public byte[] Decrypt(byte[] cipherTextBytes)
        {
            var plainTextBlob = new DataBlob();
            var cipherTextBlob = new DataBlob(cipherTextBytes);
            var entropyBlob = new DataBlob(Key.Bytes);

            var description = String.Empty;

            try
            {
                var flags = CryptProtectFlags.UiForbidden;

                if (!CryptUnprotectData(
                    ref cipherTextBlob,
                    ref description,
                    ref entropyBlob,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    flags,
                    ref plainTextBlob))
                {
                    throw new COMException("CryptUnprotectData failed. ", Marshal.GetLastWin32Error());
                }

                var plainTextBytes = new byte[plainTextBlob.DataLength];

                Marshal.Copy(plainTextBlob.DataBuffer, plainTextBytes, 0, plainTextBlob.DataLength);
                return plainTextBytes;
            }
            catch (Exception ex)
            {
                throw new Exception("The Data Protection API was unable to decrypt data.", ex);
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
        /// Encrypts the specified data
        /// </summary>
        /// <param name="plainText">A string containing the data to protect.</param>
        /// <returns>A string containing the encrypted data</returns>
        public string Encrypt(string plainText)
        {
            return Convert.ToBase64String(Encrypt(Encoding.Unicode.GetBytes(plainText)));
        }

        /// <summary>
        /// Encrypts the specified data
        /// </summary>
        /// <param name="plainTextBytes">A byte array containing data to protect</param>
        /// <returns>A byte array representing the encrypted data.</returns>
        public byte[] Encrypt(byte[] plainTextBytes)
        {
            var description = String.Empty;

            var plainTextBlob = new DataBlob(plainTextBytes);
            var cipherTextBlob = new DataBlob();
            var entropyBlob = new DataBlob(Key.Bytes);

            try
            {
                var flags = CryptProtectFlags.UiForbidden;

                if (KeyType == DataProtectionKeyType.MachineKey)
                {
                    flags |= CryptProtectFlags.LocalMachine;
                }

                if (!CryptProtectData(
                    ref plainTextBlob,
                    description,
                    ref entropyBlob,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    flags,
                    ref cipherTextBlob))
                {
                    throw new COMException("CryptProtectData failed." + Marshal.GetLastWin32Error());
                }

                var cipherTextBytes = new byte[cipherTextBlob.DataLength];

                Marshal.Copy(cipherTextBlob.DataBuffer, cipherTextBytes, 0, cipherTextBlob.DataLength);

                return cipherTextBytes;
            }
            catch (Exception ex)
            {
                throw new Exception("The Data Protection API was unable to encrypt data.", ex);
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

        [DllImport("crypt32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool CryptProtectData(
            ref DataBlob plainText,
            string description,
            ref DataBlob entropy,
            IntPtr reserved,
            IntPtr prompt,
            CryptProtectFlags flags,
            ref DataBlob cipherText);

        [DllImport("crypt32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool CryptUnprotectData(
            ref DataBlob cipherText,
            ref string description,
            ref DataBlob entropy,
            IntPtr reserved,
            IntPtr prompt,
            CryptProtectFlags flags,
            ref DataBlob plainText);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct DataBlob
        {
            internal int DataLength;
            internal IntPtr DataBuffer;

            public DataBlob(byte[] data)
            {
                if (data == null)
                {
                    data = new byte[0];
                }

                DataBuffer = Marshal.AllocHGlobal(data.Length);

                if (DataBuffer == IntPtr.Zero)
                {
                    throw new Exception("Unable to allocate data buffer for BLOB structure.");
                }

                DataLength = data.Length;

                Marshal.Copy(data, 0, DataBuffer, data.Length);
            }
        }
    }
}
