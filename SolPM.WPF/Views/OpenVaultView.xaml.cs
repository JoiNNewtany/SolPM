using Microsoft.Win32;
using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.ViewModels;
using SolPM.Core.Models;
using SolPM.Core.ViewModels;
using SolPM.Core.ViewModels.Parameters;
using System.Diagnostics;
using System.Security;
using System.Windows.Input;

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
            Parameters = new VaultParams();
        }

        public VaultParams Parameters { get; set; }

        // Exposing parameters as properties to force re-evaluation of CanExecute
        #region Parameters Properties

        public string FilePath
        {
            get
            {
                return Parameters.FilePath;
            }
            set
            {
                Parameters.FilePath = value;

                Debug.WriteLine($"FilePath = {value}");

                var viewModel = (OpenVaultViewModel)DataContext;
                viewModel.OpenVaultCommand.RaiseCanExecuteChanged();
            }
        }

        public SecureString Password
        {
            get
            {
                return Parameters.Password;
            }
            set
            {
                Parameters.Password = value;

                Debug.WriteLine($"Password = {value}");

                var viewModel = (OpenVaultViewModel)DataContext;
                viewModel.OpenVaultCommand.RaiseCanExecuteChanged();
            }
        }

        # endregion Parameters Properties

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
    }
}