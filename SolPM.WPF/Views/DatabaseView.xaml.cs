using Microsoft.Win32;
using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.ViewModels;
using SolPM.Core.Models;
using SolPM.Core.ViewModels;
using SolPM.Core.ViewModels.Parameters;
using System;

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
        }

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

        private void CreateVaultButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // Doing this in code-behind since PasswordBox
            // doesn't work with MultiValueConverters
            // despite SecurePassword being bindable

            VaultParams vaultParams = new VaultParams();
            vaultParams.EncryptionInfo = new EncryptionInfo();

            vaultParams.FilePath = VaultLocationTextBox.Text;
            vaultParams.Name = VaultNameTextBox.Text;
            vaultParams.Description = VaultDescriptionTextBox.Text;
            vaultParams.EncryptionInfo.DeriviationRounds = Convert.ToInt32(Math.Floor(RoundsSlider.Value));

            // Algorithm combobox index
            switch (AlgorithmComboBox.SelectedIndex)
            {
                case 0:
                    vaultParams.EncryptionInfo.SelectedAlgorithm = Algorithm.AES_256;
                    break;

                case 1:
                    vaultParams.EncryptionInfo.SelectedAlgorithm = Algorithm.Twofish_256;
                    break;

                default:
                    break;
            }

            vaultParams.Password = VaultPasswordBox.SecurePassword;

            // Get current ViewModel
            var viewModel = (DatabaseViewModel)DataContext;

            // Execute command
            if (viewModel.CreateVaultCommand.CanExecute(vaultParams))
            {
                viewModel.CreateVaultCommand.Execute(vaultParams);
            }
        }
    }
}