using MvvmCross.ViewModels;
using SolPM.Core.Helpers;
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

        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlArray("FolderList")]
        [XmlArrayItem("Folder")]
        public MvxObservableCollection<Folder> FolderList { get; set; }
    }
}