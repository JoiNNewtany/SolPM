using MaterialDesignThemes.Wpf;
using MvvmCross.Base;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.ViewModels;
using SolPM.Core.Interactions;
using SolPM.Core.ViewModels;
using SolPM.Core.ViewModels.Parameters;
using SolPM.WPF.Dialogs;
using SolPM.WPF.Properties;
using System;
using System.Security;
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

            var set = this.CreateBindingSet<OptionsView, OptionsViewModel>();
            set.Bind(this).For(view => view.Interaction).To(viewModel => viewModel.MessageInteraction).OneWay();
            set.Apply();
        }

        public VaultParams Parameters { get; set; } = new VaultParams() 
        {
            Password = new SecureString(),
            ValidationPassword = new SecureString(),
        };

        // Exposing parameters as properties to force re-evaluation of CanExecute
        public SecureString Password
        {
            get
            {
                return Parameters.Password;
            }
            set
            {
                Parameters.Password = value;

                var viewModel = (OptionsViewModel)DataContext;
                viewModel.ChangePasswordCommand.RaiseCanExecuteChanged();
            }
        }

        public SecureString ValidationPassword
        {
            get
            {
                return Parameters.ValidationPassword;
            }
            set
            {
                Parameters.ValidationPassword = value;

                var viewModel = (OptionsViewModel)DataContext;
                viewModel.ChangePasswordCommand.RaiseCanExecuteChanged();
            }
        }

        private IMvxInteraction<MessageInteraction> _interaction = new MvxInteraction<MessageInteraction>();
        public IMvxInteraction<MessageInteraction> Interaction
        {
            get => _interaction;
            set
            {
                if (null != _interaction)
                    _interaction.Requested -= OnInteractionRequested;

                if (null != value)
                {
                    _interaction = value;
                    _interaction.Requested += OnInteractionRequested;
                }
            }
        }

        private void OnInteractionRequested(object sender, MvxValueEventArgs<MessageInteraction> eventArgs)
        {
            var message = eventArgs.Value;
            // show message
            OptionsSnackbar.MessageQueue.Enqueue(message.Message);
        }

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