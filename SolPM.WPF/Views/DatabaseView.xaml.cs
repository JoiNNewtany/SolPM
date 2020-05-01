using Microsoft.Win32;
using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.ViewModels;
using SolPM.Core.Models;
using SolPM.Core.ViewModels;
using SolPM.Core.ViewModels.Parameters;
using System.Diagnostics;
using System.Security;

namespace SolPM.WPF.Views
{
    /// <summary>
    /// Interaction logic for DatabaseView.xaml
    /// </summary>
    [MvxViewFor(typeof(DatabaseViewModel))]
    public partial class DatabaseView : MvxWpfView
    {
        public DatabaseView()
        {
            InitializeComponent();
            Parameters = new VaultParams();
            EncryptionInfo = new EncryptionInfo
            {
                DeriviationRounds = 10000,
                SelectedAlgorithm = Algorithm.AES_256,
            };
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

                var viewModel = (DatabaseViewModel)DataContext;
                viewModel.CreateVaultCommand.RaiseCanExecuteChanged();
            }
        }

        public string VaultName
        {
            get
            {
                return Parameters.Name;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    Debug.WriteLine("BIG REEE ITS NULL!!!");
                }

                Parameters.Name = value;

                Debug.WriteLine($"VaultName = {value}");

                var viewModel = (DatabaseViewModel)DataContext;
                viewModel.CreateVaultCommand.RaiseCanExecuteChanged();
            }
        }

        public string Description
        {
            get
            {
                return Parameters.Description;
            }
            set
            {
                Parameters.Description = value;

                Debug.WriteLine($"Description = {value}");
            }
        }

        public EncryptionInfo EncryptionInfo
        {
            get
            {
                return Parameters.EncryptionInfo;
            }
            set
            {
                Parameters.EncryptionInfo = value;
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

                var viewModel = (DatabaseViewModel)DataContext;
                viewModel.CreateVaultCommand.RaiseCanExecuteChanged();
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

                Debug.WriteLine($"ValidationPassword = {value}");

                var viewModel = (DatabaseViewModel)DataContext;
                viewModel.CreateVaultCommand.RaiseCanExecuteChanged();
            }
        }

        # endregion Parameters Properties

        private void VaultLocation_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog
            {
                DefaultExt = ".solpv",
                Filter = "Solar Password Vault (*.solpv)|*.solpv",
            };

            if (dialog.ShowDialog() == true)
            {
                VaultLocationTextBox.Text = dialog.FileName;
            }
        }

        private void MvxWpfView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            // Workaround to change the index to 0
            // because it's -1 by default and nothing else worked
            AlgorithmComboBox.SelectedIndex = 0;
        }

        private void VaultPasswordBox_PasswordChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            // Workaround to set the VerificationPassword property
            // of the PasswordRule since it cannot be bound
            //PasswordValidation.ValidationPassword = ((PasswordBox)sender).SecurePassword;
        }
    }
}