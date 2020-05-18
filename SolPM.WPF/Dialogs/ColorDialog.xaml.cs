using System.Windows.Controls;
using System.Windows.Media;

namespace SolPM.WPF.Dialogs
{
    /// <summary>
    /// Interaction logic for YesNoDialog.xaml
    /// </summary>
    public partial class ColorDialog : Grid
    {
        public ColorDialog()
        {
            InitializeComponent();
        }

        public Color SelectedColor
        {
            get { return ColorPicker.Color; }
        }
    }
}