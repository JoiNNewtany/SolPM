using SolPM.Core.Cryptography.Cipher;
using SolPM.Core.Models;
using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;

namespace SolPM.Core.Cryptography
{
    // TODO: Rethink encryption implementation

    public class CryptoUtilities : IDisposable
    {
        private readonly SymmetricAlgorithm algorithm;

        public CryptoUtilities(Algorithm algorithm)
        {
            //// If given type does not derive from SymmetricAlgorythm
            //if (!typeof(SymmetricAlgorithm).IsAssignableFrom(algorithm))
            //{
            //    throw new ArgumentException("Given type does not derive from SymmetricAlgorithm", "algorithm");
            //}

            //this.algorithm = (SymmetricAlgorithm)Activator.CreateInstance(algorithm);

            switch (algorithm)
            {
                case Algorithm.AES_256:
                    this.algorithm = new AesManaged();
                    break;

                case Algorithm.Twofish_256:
                    this.algorithm = new TwofishManaged();
                    break;

                default:
                    throw new ArgumentException("algorithm");
            }
        }

        public byte[] Encrypt(byte[] data, byte[] key, byte[] iv)
        {
            using (var alg = algorithm)
            {
                alg.KeySize = 256;
                alg.BlockSize = 128;
                alg.Padding = PaddingMode.Zeros;

                alg.Key = key;
                alg.IV = iv;

                using (var encryptor = alg.CreateEncryptor(alg.Key, alg.IV))
                {
                    return PerformCryptography(data, encryptor);
                }
            }
        }

        public byte[] Decrypt(byte[] data, byte[] key, byte[] iv)
        {
            using (var alg = algorithm)
            {
                alg.KeySize = 256;
                alg.BlockSize = 128;
                alg.Padding = PaddingMode.Zeros;

                alg.Key = key;
                alg.IV = iv;

                using (var decryptor = alg.CreateDecryptor(alg.Key, alg.IV))
                {
                    return PerformCryptography(data, decryptor);
                }
            }
        }

        private static byte[] PerformCryptography(byte[] data, ICryptoTransform cryptoTransform)
        {
            using (var ms = new MemoryStream())
            using (var cryptoStream = new CryptoStream(ms, cryptoTransform, CryptoStreamMode.Write))
            {
                cryptoStream.Write(data, 0, data.Length);
                cryptoStream.FlushFinalBlock();

                return ms.ToArray();
            }
        }

        /*

        1) Generate a random 128-bit key (k1), a random 128-bit IV, and a random salt (64 bits is probably sufficient).
        2) Use PBKDF2 to generate a 256-bit key from your password and the salt, then split that into two 128-bit keys (k2, k3).
        3) Use k2 to AES encrypt k1 using the random IV.
        4) Save the encrypted key, k3, the salt and the IV to a file somewhere.

        */

        /// <summary>
        /// Allows to retrieve a decrypted version of the key
        /// used to encrypt the data.
        /// </summary>
        public byte[] UnprotectEncryptionKey(SecureString password, byte[] protectedKey, byte[] salt, byte[] iv)
        {
            // Decrypt Key with first 256 bits of derived key
            return Decrypt(protectedKey, DeriveKey(SecStrBytes(password), salt, 64).Take(32).ToArray(), iv);
        }

        /// <summary>
        /// Allows to retrieve an encrypted version of the key
        /// used to encrypt the data.
        /// </summary>
        public byte[] ProtectEncryptionKey(SecureString password, byte[] unprotectedKey, byte[] salt, byte[] iv)
        {
            // Encrypt Key with first 256 bits of derived key
            return Encrypt(unprotectedKey, DeriveKey(SecStrBytes(password), salt, 64).Take(32).ToArray(), iv);
        }

        /// <summary>
        /// The validation key is the last 256 bits of the derived key.
        /// </summary>
        public static byte[] GetValidationKey(SecureString password, byte[] salt)
        {
            return DeriveKey(SecStrBytes(password), salt, 64).Skip(32).ToArray();
        }

        public static bool ValidatePassword(SecureString password, byte[] validationKey, byte[] salt)
        {
            byte[] passwordKey = GetValidationKey(password, salt);

            if (passwordKey.Length == validationKey.Length)
            {
                for (int i = 0; i < validationKey.Length; i++)
                {
                    if (passwordKey[i] != validationKey[i])
                    {
                        return false;
                    }
                }

                return true;
            }
            else return false;
        }

        /// <summary>
        /// Allows to retrieve this secure string instance as a byte array.
        /// Note that this is still visible plainly in memory and should be used as quickly as possible,
        /// then the contents 'zero-ed' so that they cannot be viewed.
        /// </summary>
        public static byte[] SecStrBytes(SecureString secureString)
        {
            char[] bytes = new char[secureString.Length];
            IntPtr ptr = IntPtr.Zero;

            try
            {
                ptr = Marshal.SecureStringToBSTR(secureString);
                bytes = new char[secureString.Length];
                Marshal.Copy(ptr, bytes, 0, secureString.Length);
            }
            finally
            {
                if (ptr != IntPtr.Zero)
                    Marshal.ZeroFreeBSTR(ptr);
            }
            return bytes.Select(c => (byte)c).ToArray();
        }

        //TODO: Implement a more secure way of generating random bytes
        public static byte[] RandomBytes(int size)
        {
            var rng = new byte[size];
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetBytes(rng);
            }
            return rng;
        }

        // TODO: Allow the use of different key deriviation functions
        public static byte[] DeriveKey(byte[] password, byte[] salt, int size = 32, int iterations = 10000)
        {
            using (var kdf = new Rfc2898DeriveBytes(password, salt, iterations))
            {
                return kdf.GetBytes(size);
            }
        }

        public void Dispose()
        {
            algorithm.Clear();
        }
    }
}