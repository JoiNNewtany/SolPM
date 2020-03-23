using System.Drawing;
using System.Xml;
using System.Xml.Serialization;

namespace SolPM.Core.Helpers
{
    public class XmlColor
    {
        private Color _color = Color.Black;

        public XmlColor()
        {
        }

        public XmlColor(Color c)
        {
            _color = c;
        }

        public Color ToColor()
        {
            return _color;
        }

        public void FromColor(Color c)
        {
            _color = c;
        }

        public static implicit operator Color(XmlColor x)
        {
            return x.ToColor();
        }

        public static implicit operator XmlColor(Color c)
        {
            return new XmlColor(c);
        }

        [XmlElement]
        public byte Alpha
        {
            get { return _color.A; }
            set
            {
                if (value != _color.A)
                    _color = Color.FromArgb(value, _color);
            }
        }

        [XmlElement]
        public byte Red
        {
            get { return _color.R; }
            set
            {
                if (value != _color.R)
                    _color = Color.FromArgb(_color.A, value, _color.G, _color.B);
            }
        }

        [XmlElement]
        public byte Green
        {
            get { return _color.G; }
            set
            {
                if (value != _color.G)
                    _color = Color.FromArgb(_color.A, _color.R, value, _color.B);
            }
        }

        [XmlElement]
        public byte Blue
        {
            get { return _color.B; }
            set
            {
                if (value != _color.B)
                    _color = Color.FromArgb(_color.A, _color.R, _color.G, value);
            }
        }
    }
}