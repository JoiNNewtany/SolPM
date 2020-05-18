using MaterialDesignThemes.Wpf;
using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.ViewModels;
using SolPM.Core.ViewModels;
using SolPM.WPF.Dialogs;
using SolPM.WPF.Properties;
using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace SolPM.WPF.Views
{
    /// <summary>
    /// Interaction logic for OptionsView.xaml
    /// </summary>
    [MvxViewFor(typeof(OptionsViewModel))]
    public partial class OptionsView : MvxWpfView
    {
        public OptionsView()
        {
            InitializeComponent();
        }

        //private static void ModifyTheme(Action<ITheme> modificationAction)
        //{
        //    PaletteHelper paletteHelper = new PaletteHelper();
        //    ITheme theme = paletteHelper.GetTheme();

        //    modificationAction?.Invoke(theme);

        //    paletteHelper.SetTheme(theme);
        //}

        private void Theme_Checked(object sender, RoutedEventArgs e)
        {
            var isDark = (bool)(sender as ToggleButton).IsChecked;
            Helpers.PaletteHelper.ModifyTheme(theme => theme.SetBaseTheme(isDark ? Theme.Dark : Theme.Light));
            Settings.Default.IsThemeDark = isDark;
        }

        private async void PrimaryColor_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ColorDialog();
            var result = await DialogHost.Show(dialog);
            if (result is bool accept && accept)
            {
                Helpers.PaletteHelper.ModifyTheme(theme => theme.SetPrimaryColor(dialog.SelectedColor));
                Settings.Default.AppPrimaryColor = dialog.SelectedColor;
            }
        }

        private async void AccentColor_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ColorDialog();
            var result = await DialogHost.Show(dialog);
            if (result is bool accept && accept)
            {
                Helpers.PaletteHelper.ModifyTheme(theme => theme.SetSecondaryColor(dialog.SelectedColor));
                Settings.Default.AppSecondaryColor = dialog.SelectedColor;
            }
        }
    }
}