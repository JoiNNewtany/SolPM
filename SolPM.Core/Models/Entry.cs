using MvvmCross.ViewModels;
using SolPM.Core.Helpers;
using System;
using System.IO;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;
using System.Xml.Serialization;

namespace SolPM.Core.Models
{
    [XmlRoot("Entry")]
    public class Entry
    {
        public Entry()
        {
            Id = Guid.NewGuid();
            FieldList = new MvxObservableCollection<Field>();
        }

        [XmlElement("Id")]
        public Guid Id { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Created")]
        public DateTime Created { get; set; }

        [XmlElement("Modified")]
        public DateTime Modified { get; set; }

        [XmlElement("Accessed")]
        public DateTime Accessed { get; set; }

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

        [XmlIgnore]
        public BitmapSource Icon { get; set; }

        [XmlElement("Icon")]
        public byte[] IconBuffer
        {
            get
            {
                byte[] iconBuffer = null;

                if (Icon != null)
                {
                    using (var stream = new MemoryStream())
                    {
                        var encoder = new PngBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create(Icon));
                        encoder.Save(stream);
                        iconBuffer = stream.ToArray();
                    }
                }

                return iconBuffer;
            }
            set
            {
                if (value == null)
                {
                    Icon = null;
                }
                else
                {
                    using (var stream = new MemoryStream(value))
                    {
                        var decoder = BitmapDecoder.Create(stream,
                            BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                        Icon = decoder.Frames[0];
                    }
                }
            }
        }

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