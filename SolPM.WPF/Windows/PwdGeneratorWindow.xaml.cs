using System.ComponentModel;
using System.Windows;
using SolPM.Core.ViewModels;
using SolPM.Core.ViewModels.Parameters;

namespace SolPM.WPF.Windows
{
    /// <summary>
    /// Interaction logic for PwdGeneratorWindow.xaml
    /// </summary>
    public partial class PwdGeneratorWindow : Window
    {
        public PwdGeneratorWindow()
        {
            InitializeComponent();

            if (null == Properties.Settings.Default.PwdGenParams)
            {
                PwdGenParameters = new PwdGenParams();

                PwdGenParameters.Length = 16;
                PwdGenParameters.Consecutive = false;
                PwdGenParameters.LowerCase = true;
                PwdGenParameters.UpperCase = true;
                PwdGenParameters.Numbers = true;
                PwdGenParameters.Symbols = false;
                PwdGenParameters.Excluded = string.Empty;
            }
            else
            {
                PwdGenParameters = Properties.Settings.Default.PwdGenParams;
            }
        }

        public PwdGenParams PwdGenParameters { get; set; }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            Properties.Settings.Default.PwdGenParams = PwdGenParameters;
        }

        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(PwdField.Text);
        }
    }
}