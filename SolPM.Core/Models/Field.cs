using System.Xml;
using System.Xml.Serialization;

namespace SolPM.Core.Models
{
    [XmlRoot("Field")]
    public class Field
    {
        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Value")]
        public object Value { get; set; }
    }
}