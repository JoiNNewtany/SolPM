using MvvmCross.ViewModels;
using SolPM.Core.Helpers;
using System;
using System.IO;
using System.Windows.Media.Imaging;
using System.Xml;
using System.Xml.Serialization;

namespace SolPM.Core.Models
{
    [XmlRoot("Entry")]
    public class Entry
    {
        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("LastModified")]
        public DateTime LastModified { get; set; }

        [XmlElement("Color")]
        public XmlColor Color { get; set; }

        [XmlIgnore]
        public BitmapSource Image { get; set; }

        [XmlElement("Image")]
        public byte[] ImageBuffer
        {
            get
            {
                byte[] imageBuffer = null;

                if (Image != null)
                {
                    using (var stream = new MemoryStream())
                    {
                        var encoder = new PngBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create(Image));
                        encoder.Save(stream);
                        imageBuffer = stream.ToArray();
                    }
                }

                return imageBuffer;
            }
            set
            {
                if (value == null)
                {
                    Image = null;
                }
                else
                {
                    using (var stream = new MemoryStream(value))
                    {
                        var decoder = BitmapDecoder.Create(stream,
                            BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                        Image = decoder.Frames[0];
                    }
                }
            }
        }

        [XmlArray("FieldList")]
        [XmlArrayItem("Field")]
        public MvxObservableCollection<Field> FieldList { get; set; }
    }
}