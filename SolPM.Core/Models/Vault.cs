using MvvmCross.ViewModels;
using SolPM.Core.Cryptography;
using SolPM.Core.Helpers;
using System;
using System.IO;
using System.Security;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace SolPM.Core.Models
{
    // TODO: Reconsider pattern usage

    [XmlInclude(typeof(XmlColor))]
    [XmlRoot("Vault")]
    public class Vault
    {
        private Vault()
        {
            FolderList = new MvxObservableCollection<Folder>();
            EncryptionInfo = new EncryptionInfo();
        }

        // Implementation of the Singleton pattern

        private static Vault _instance;

        // Lock object that is used to synchronize threads
        // during first access to the Vault
        private static readonly object _lock = new object();

        public static Vault GetInstance()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new Vault();
                    }
                }
            }
            return _instance;
        }

        public static bool Exists()
        {
            return _instance != null;
        }

        public static void Delete()
        {
            _instance = null;
        }

        [XmlElement("EncryptionInfo")]
        public EncryptionInfo EncryptionInfo { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Description")]
        public string Description { get; set; }

        /// <summary>
        /// Stores the path to the vault when it is opened.
        /// </summary>
        [XmlIgnore]
        public string Location { get; set; }

        //[XmlArray("FolderList")]
        //[XmlArrayItem("Folder")]
        [XmlIgnore]
        public MvxObservableCollection<Folder> FolderList { get; set; }

        [XmlElement("Data")]
        public byte[] Data { get; set; }

        // TODO: Exception handling

        /// <summary>
        /// Fills <c>EncryptionInfo</c> structure and prepares vault
        /// for encryption.
        /// </summary>
        public void SetupEncryption(SecureString password)
        {
            if (null == password)
            {
                throw new ArgumentNullException("password");
            }

            EncryptionInfo.Salt = CryptoUtilities.RandomBytes(16);
            EncryptionInfo.IV = CryptoUtilities.RandomBytes(16);
            EncryptionInfo.ValidationKey = CryptoUtilities.GetValidationKey(password, EncryptionInfo.Salt);
            EncryptionInfo.ProtectedKey = CryptoUtilities.GetEncryptionProtectionKey(password, EncryptionInfo.Salt);

            // Protecting encryption key using chosen encryption algorythm
            using (var cu = new CryptoUtilities(EncryptionInfo.SelectedAlgorithm))
            {
                EncryptionInfo.EncryptionKey = cu.ProtectEncryptionKey(password,
                    CryptoUtilities.RandomBytes(16), EncryptionInfo.Salt, EncryptionInfo.IV);
            }
        }

        public void EncryptToFile(string filepath, SecureString password)
        {
            if (null == filepath)
            {
                throw new ArgumentNullException("filepath", "Filepath can't be empty");
            }

            if (null == EncryptionInfo)
            {
                throw new NullReferenceException("EncryptionInfo can't be empty");
            }

            if (null == EncryptionInfo.EncryptionKey)
            {
                throw new NullReferenceException("EncryptionKey can't be empty");
            }

            if (null == EncryptionInfo.ValidationKey)
            {
                throw new NullReferenceException("ValidationKey can't be empty");
            }

            if (null == EncryptionInfo.IV)
            {
                throw new NullReferenceException("IV can't be empty");
            }

            if (null == EncryptionInfo.Salt)
            {
                throw new NullReferenceException("Salt can't be empty");
            }

            try
            {
                using (var cu = new CryptoUtilities(EncryptionInfo.SelectedAlgorithm))
                {
                    if (!CryptoUtilities.ValidatePassword(password, EncryptionInfo.ValidationKey, EncryptionInfo.Salt))
                    {
                        throw new ArgumentException("Incorrect password", "password");
                    }

                    // Serialize and encrypt folder list

                    XmlSerializer xsSubmit = new XmlSerializer(typeof(MvxObservableCollection<Folder>));
                    var xml = string.Empty;

                    using (var sww = new StringWriter())
                    using (XmlWriter writer = XmlWriter.Create(sww))
                    {
                        xsSubmit.Serialize(writer, FolderList);
                        xml = sww.ToString(); // Serialized XML
                    }

                    // Unprotect the key and encrypt data

                    Data = cu.Encrypt(Encoding.UTF8.GetBytes(xml),
                        cu.UnprotectEncryptionKey(password,
                        EncryptionInfo.EncryptionKey, EncryptionInfo.Salt, EncryptionInfo.IV),
                        EncryptionInfo.IV);

                    // Serialize vault and save to file

                    xsSubmit = new XmlSerializer(typeof(Vault));
                    xml = string.Empty;

                    using (var sww = new StringWriter())
                    using (XmlWriter writer = XmlWriter.Create(sww))
                    {
                        xsSubmit.Serialize(writer, GetInstance());
                        xml = sww.ToString(); // Serialized XML
                    }

                    File.WriteAllText(filepath, xml);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        // To encrypt using stored copy of the key
        public void EncryptToFile(string filepath, byte[] protectedKey)
        {
            if (null == filepath)
            {
                throw new ArgumentNullException("filepath", "Filepath can't be empty");
            }

            if (null == EncryptionInfo)
            {
                throw new NullReferenceException("EncryptionInfo can't be empty");
            }

            if (null == EncryptionInfo.EncryptionKey)
            {
                throw new NullReferenceException("EncryptionKey can't be empty");
            }

            if (null == EncryptionInfo.ValidationKey)
            {
                throw new NullReferenceException("ValidationKey can't be empty");
            }

            if (null == EncryptionInfo.IV)
            {
                throw new NullReferenceException("IV can't be empty");
            }

            if (null == EncryptionInfo.Salt)
            {
                throw new NullReferenceException("Salt can't be empty");
            }

            try
            {
                using (var cu = new CryptoUtilities(EncryptionInfo.SelectedAlgorithm))
                {
                    // Serialize and encrypt folder list

                    XmlSerializer xsSubmit = new XmlSerializer(typeof(MvxObservableCollection<Folder>));
                    var xml = string.Empty;

                    using (var sww = new StringWriter())
                    using (XmlWriter writer = XmlWriter.Create(sww))
                    {
                        xsSubmit.Serialize(writer, FolderList);
                        xml = sww.ToString(); // Serialized XML
                    }

                    // Unprotect the key and encrypt data

                    Data = cu.Encrypt(Encoding.UTF8.GetBytes(xml),
                        cu.UnprotectEncryptionKey(protectedKey,
                            EncryptionInfo.EncryptionKey, EncryptionInfo.IV),
                        EncryptionInfo.IV);

                    // Serialize vault and save to file

                    xsSubmit = new XmlSerializer(typeof(Vault));
                    xml = string.Empty;

                    using (var sww = new StringWriter())
                    using (XmlWriter writer = XmlWriter.Create(sww))
                    {
                        xsSubmit.Serialize(writer, GetInstance());
                        xml = sww.ToString(); // Serialized XML
                    }

                    File.WriteAllText(filepath, xml);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DecryptFromFile(string filePath, SecureString password)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentNullException("filePath", "File path is empty or null");
            }

            //if (Vault.Exists())
            //{
            //    // TODO: Figure out what to do with this
            //    //throw new NotImplementedException("Vault already exists but idk what to do with it yet");
            //    var _vault = Vault.GetInstance();
            //    _vault.EncryptToFile(_vault.Location, _vault.EncryptionInfo.ProtectedKey);
            //    Vault.Delete();
            //}

            try
            {
                // Deserializing vault

                var xml = File.ReadAllText(filePath);

                XmlSerializer serializer = new XmlSerializer(typeof(Vault));
                using (TextReader reader = new StringReader(xml))
                {
                    var vault = (Vault)serializer.Deserialize(reader);
                    EncryptionInfo = vault.EncryptionInfo;
                    Data = vault.Data;
                    Name = vault.Name;
                }

                using (var cu = new CryptoUtilities(EncryptionInfo.SelectedAlgorithm))
                {
                    if (!CryptoUtilities.ValidatePassword(password, EncryptionInfo.ValidationKey, EncryptionInfo.Salt))
                    {
                        throw new ArgumentException("Incorrect password", "password");
                    }

                    // Unprotect the key and decrypt data

                    var plainData = cu.Decrypt(Data, cu.UnprotectEncryptionKey(password,
                        EncryptionInfo.EncryptionKey, EncryptionInfo.Salt, EncryptionInfo.IV),
                        EncryptionInfo.IV);

                    // Deserialize folder list

                    serializer = new XmlSerializer(typeof(MvxObservableCollection<Folder>));
                    using (TextReader reader = new StringReader(Encoding.UTF8.GetString(plainData)))
                    {
                        FolderList = (MvxObservableCollection<Folder>)serializer.Deserialize(reader);
                    }

                    // Set vault's Location
                    Location = filePath;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}