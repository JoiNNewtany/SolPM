using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using SolPM.Core.Cryptography;
using SolPM.Core.Models;
using System;
using System.Diagnostics;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace SolPM.Core.ViewModels
{
    public class DatabaseViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigationService;

        public DatabaseViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;

            // Commands

            NavigateVaultView = new MvxAsyncCommand(() => _navigationService.Navigate<VaultViewModel>());
            CreateVaultCommand = new MvxCommand<string>((s) => CreateVault(s));
        }

        public override async Task Initialize()
        {
            Debug.WriteLine($"DatabaseViewModel: Initialize called.");

            Vault vault = Vault.GetInstance();

            SecureString testPassword = new SecureString();
            testPassword.AppendChar('S');
            testPassword.AppendChar('i');
            testPassword.AppendChar('1');
            testPassword.AppendChar('1');
            testPassword.AppendChar('y');

            vault.EncryptionInfo.SelectedAlgorithm = Algorithm.Twofish_256;
            vault.SetupEncryption(testPassword);
            //_ = CryptoUtilities.DeriveKey(CryptoUtilities.SecStrBytes(testPassword), vault.EncryptionInfo.Salt, 64).Take(32).ToArray();
            vault.EncryptToFile("E:\\Downloads\\TestVault1.solpv", testPassword);
            vault.EncryptToFile("E:\\Downloads\\TestVault2.solpv", vault.EncryptionInfo.ProtectedKey);

            // Reset vault to empty
            Vault.Delete();
            vault = Vault.GetInstance();

            Debug.WriteLine($"Vault exists: {Vault.Exists()}");
            vault.DecryptFromFile("E:\\Downloads\\TestVault1.solpv", testPassword);
            Debug.WriteLine($"Opened vault: {vault.Name}");

            // Reset vault to empty
            Vault.Delete();
            vault = Vault.GetInstance();

            Debug.WriteLine($"Vault exists: {Vault.Exists()}");
            vault.DecryptFromFile("E:\\Downloads\\TestVault2.solpv", testPassword);
            Debug.WriteLine($"Opened vault: {vault.Name}");
        }

        #region Commands

        public IMvxAsyncCommand NavigateVaultView { get; private set; }

        public IMvxCommand<string> CreateVaultCommand { get; private set; }

        #endregion Commands

        private void CreateVault(string filename)
        {

        }
    }
}