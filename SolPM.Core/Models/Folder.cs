using MvvmCross.ViewModels;
using SolPM.Core.Helpers;
using System.Xml;
using System.Xml.Serialization;

namespace SolPM.Core.Models
{
    [XmlRoot("Folder")]
    public class Folder
    {
        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Color")]
        public XmlColor Color { get; set; }

        [XmlArray("EntryList")]
        [XmlArrayItem("Entry")]
        public MvxObservableCollection<Entry> EntryList { get; set; }
    }
}