using System.Diagnostics;
using System.Reflection;
using System.Windows.Controls;

namespace SolPM.WPF.Dialogs
{
    /// <summary>
    /// Interaction logic for YesNoDialog.xaml
    /// </summary>
    public partial class AppInfoDialog : Grid
    {
        public AppInfoDialog()
        {
            InitializeComponent();
        }

        public string Version
        {
            get 
            { 
                return Assembly.GetExecutingAssembly().GetName().Version.ToString(); 
            }
        }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Process.Start(e.Uri.ToString());
        }
    }
}