using SolPM.Core.Cryptography;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Xml.Serialization;

namespace SolPM.Core.Models
{
    // Some public info about vault encryption
    [XmlRoot("EncryptionInfo")]
    public class EncryptionInfo
    {
        [XmlElement("SelectedAlgorythm")]
        public Algorithm SelectedAlgorithm { get; set; }

        [XmlElement("DeriviationRounds")]
        public int DeriviationRounds { get; set; }

        [XmlElement("EncryptionKey")]
        public byte[] EncryptionKey { get; set; } // k1

        [XmlElement("ValidationKey")]
        public byte[] ValidationKey { get; set; } // k3

        [XmlElement("Salt")]
        public byte[] Salt { get; set; }

        [XmlElement("IV")]
        public byte[] IV { get; set; }

        // HACK: Do not store this if possible
        // Key used to encrypt EncryptionKey
        [XmlIgnore]
        private byte[] protectedKey;

        [XmlIgnore]
        public byte[] ProtectedKey
        {
            get
            {
                // Get appended entropy from data
                byte[] entropy = protectedKey.Skip(protectedKey.Length - 16).ToArray();

                // Get proteccted data
                byte[] proteccted = protectedKey.Take(protectedKey.Length - 16).ToArray();

                return ProtectedData.Unprotect(proteccted, entropy, DataProtectionScope.CurrentUser);
            }

            set
            {
                byte[] entropy = CryptoUtilities.RandomBytes(16);
                byte[] protecc = ProtectedData.Protect(value, entropy, DataProtectionScope.CurrentUser);
                byte[] result = new byte[protecc.Length + 16];

                // Append entropy to proteccted data
                Buffer.BlockCopy(entropy, 0, result, protecc.Length, entropy.Length);
                Buffer.BlockCopy(protecc, 0, result, 0, protecc.Length);

                protectedKey = result;
            }
        }
    }
}