using Microsoft.Win32;
using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.ViewModels;
using SolPM.Core.ViewModels;
using SolPM.Core.ViewModels.Parameters;

namespace SolPM.WPF.Views
{
    /// <summary>
    /// Interaction logic for OpenVaultView.xaml
    /// </summary>
    [MvxViewFor(typeof(OpenVaultViewModel))]
    public partial class OpenVaultView : MvxWpfView
    {
        public OpenVaultView()
        {
            InitializeComponent();
        }

        private void VaultLocationButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                DefaultExt = ".solpv",
                CheckFileExists = true,
                CheckPathExists = true,
                Multiselect = false,
                Filter = "Solar Password Vault (*.solpv)|*.solpv",
            };

            if (dialog.ShowDialog() == true)
            {
                VaultLocationTextBox.Text = dialog.FileName;
            }
        }

        private void OpenVaultButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // Why write MultiValueConverters when you can
            // just use code-behind and it actually works, right?

            VaultParams vaultParams = new VaultParams();

            vaultParams.FilePath = VaultLocationTextBox.Text;
            vaultParams.Password = VaultPasswordBox.SecurePassword;

            // Get current ViewModel
            var viewModel = (OpenVaultViewModel)DataContext;

            // Execute command
            if (viewModel.OpenVaultCommand.CanExecute(vaultParams))
            {
                viewModel.OpenVaultCommand.Execute(vaultParams);
            }
        }
    }
}