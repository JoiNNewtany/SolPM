using MvvmCross.Platforms.Wpf.Views;
using SolPM.WPF.Properties;

namespace SolPM.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MvxWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_Closed(object sender, System.EventArgs e)
        {
            // Save window size

            if (Settings.Default.WindowHeight != this.Height ||
                Settings.Default.WindowWidth != this.Width)
            {
                Settings.Default.WindowHeight = this.Height;
                Settings.Default.WindowWidth = this.Width;
                Settings.Default.Save();
            }
        }
    }
}