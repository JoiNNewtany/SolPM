using System;
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
    }
}
