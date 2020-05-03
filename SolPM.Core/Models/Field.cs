using SolPM.Core.Cryptography;
using System;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace SolPM.Core.Models
{
    public enum FieldTypes
    {
        Username,
        Password,
        Note,
        File,
    }

    [XmlRoot("Field")]
    public class Field
    {
        public Field()
        {
            Id = Guid.NewGuid();
        }

        [XmlElement("Id")]
        public Guid Id { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Type")]
        public FieldTypes Type { get; set; }

        // TODO: Make each field type into own class
        [XmlElement("FileName")]
        public string FileName { get; set; }

        [XmlElement("ProtectedValue")]
        public byte[] ProtectedValue;

        [XmlIgnore]
        public object Value
        {
            get
            {
                if (null == ProtectedValue)
                {
                    return null;
                }

                // Get appended entropy from data
                byte[] entropy = ProtectedValue.Skip(ProtectedValue.Length - 16).ToArray();

                // Get proteccted data
                byte[] proteccted = ProtectedValue.Take(ProtectedValue.Length - 16).ToArray();

                byte[] unprotecc = ProtectedData.Unprotect(proteccted, entropy, DataProtectionScope.CurrentUser);

                switch (Type)
                {
                    case FieldTypes.Username:
                        return Encoding.UTF8.GetString(unprotecc);

                    case FieldTypes.Password:

                        var secure = new SecureString();

                        foreach (var character in Encoding.UTF8.GetString(unprotecc))
                        {
                            secure.AppendChar(character);
                        }

                        return secure;

                    case FieldTypes.Note:
                        return Encoding.UTF8.GetString(unprotecc);

                    default:
                        return unprotecc;
                }
            }

            set
            {
                byte[] protecc = null;

                if (value is byte[])
                {
                    protecc = value as byte[];
                }

                if (value is string)
                {
                    protecc = Encoding.UTF8.GetBytes(value as string);
                }

                if (value is SecureString)
                {
                    protecc = CryptoUtilities.SecStrBytes(value as SecureString);
                }

                if (null != protecc)
                {
                    byte[] entropy = CryptoUtilities.RandomBytes(16);
                    byte[] proteccted = ProtectedData.Protect(protecc, entropy, DataProtectionScope.CurrentUser);

                    // Clear out sensitive data
                    Array.Clear(protecc, 0, protecc.Length);

                    byte[] result = new byte[proteccted.Length + 16];

                    // Append entropy to proteccted data
                    Buffer.BlockCopy(entropy, 0, result, proteccted.Length, entropy.Length);
                    Buffer.BlockCopy(proteccted, 0, result, 0, proteccted.Length);

                    ProtectedValue = result;
                }
            }
        }
    }
}