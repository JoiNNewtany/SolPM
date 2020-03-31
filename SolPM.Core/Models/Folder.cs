using MvvmCross.ViewModels;
using SolPM.Core.Helpers;
using System.Windows.Media;
using System.Xml;
using System.Xml.Serialization;

namespace SolPM.Core.Models
{
    [XmlRoot("Folder")]
    public class Folder
    {
        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlIgnore]
        public Color Color { get; set; }

        [XmlElement("Color")]
        public XmlColor ColorBuffer
        {
            get
            {
                if (Color != null)
                {
                    return Color;
                }
                else
                {
                    return Color.FromRgb(0, 0, 0);
                }
            }
            set
            {
                if (value == null)
                {
                    Color = Color.FromRgb(0, 0, 0);
                }
                else
                {
                    Color = value.ToColor();
                }
            }
        }

        [XmlArray("EntryList")]
        [XmlArrayItem("Entry")]
        public MvxObservableCollection<Entry> EntryList { get; set; }
    }
}