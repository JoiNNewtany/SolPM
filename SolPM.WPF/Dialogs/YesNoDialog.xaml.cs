using System.Windows.Controls;

namespace SolPM.WPF.Dialogs
{
    /// <summary>
    /// Interaction logic for YesNoDialog.xaml
    /// </summary>
    public partial class YesNoDialog : Grid
    {
        public YesNoDialog()
        {
            InitializeComponent();
        }

        public string Message
        {
            get { return MessageBlock.Text; }
            set { MessageBlock.Text = value; }
        }
    }
}