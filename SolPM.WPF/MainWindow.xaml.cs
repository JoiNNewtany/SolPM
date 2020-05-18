using MaterialDesignThemes.Wpf;
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
            }

            // Save application settings

            Settings.Default.Save();
        }

        private void MainWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            // Load theme and colors

            Helpers.PaletteHelper.ModifyTheme(theme => theme.SetBaseTheme(
                Settings.Default.IsThemeDark ? Theme.Dark : Theme.Light));

            Helpers.PaletteHelper.ModifyTheme(theme => theme.SetPrimaryColor(Settings.Default.AppPrimaryColor));
            Helpers.PaletteHelper.ModifyTheme(theme => theme.SetSecondaryColor(Settings.Default.AppSecondaryColor));
        }
    }
}