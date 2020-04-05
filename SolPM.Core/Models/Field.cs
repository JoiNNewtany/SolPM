using System;
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

        [XmlIgnore]
        public FieldTypes Type { get; set; }

        [XmlElement("Value")]
        public object Value { get; set; }
    }
}