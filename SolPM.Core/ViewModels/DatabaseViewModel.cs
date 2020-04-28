using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using SolPM.Core.Models;
using SolPM.Core.ViewModels.Parameters;
using System.Diagnostics;
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
            CreateVaultCommand = new MvxCommand<VaultParams>((s) => CreateVault(s));
        }

        public override async Task Initialize()
        {
            Debug.WriteLine($"DatabaseViewModel: Initialize called.");

            //Vault vault = Vault.GetInstance();

            //SecureString testPassword = new SecureString();
            //testPassword.AppendChar('t');
            //testPassword.AppendChar('e');
            //testPassword.AppendChar('s');
            //testPassword.AppendChar('t');

            //vault.Location = "E:\\Downloads\\TestVault1.solpv";

            //vault.EncryptionInfo.SelectedAlgorithm = Algorithm.Twofish_256;
            //vault.SetupEncryption(testPassword);
            /*
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
            */
            //vault.DecryptFromFile("E:\\Downloads\\test.solpv", testPassword);
            //Debug.WriteLine($"Opened vault: {vault.Name}");

            // TODO: PASSWORD IS BREAKING FOR SOME REASON?
        }

        #region Commands

        public IMvxAsyncCommand NavigateVaultView { get; private set; }
        public IMvxCommand CreateVaultCommand { get; private set; }

        #endregion Commands

        private void CreateVault(VaultParams vaultParams)
        {
            if (Vault.Exists())
            {
                // TODO: Rewrite this
                //throw new NotImplementedException("Vault already exists but idk what to do with it yet");
                var _vault = Vault.GetInstance();
                _vault.EncryptToFile(_vault.Location, _vault.EncryptionInfo.ProtectedKey);
                Vault.Delete();
            }

            var vault = Vault.GetInstance();

            vault.Location = vaultParams.FilePath;
            vault.Name = vaultParams.Name;
            vault.Description = vaultParams.Description;
            vault.EncryptionInfo = vaultParams.EncryptionInfo;

            vault.FolderList = new MvxObservableCollection<Folder>();

            vault.SetupEncryption(vaultParams.Password);
            vaultParams.Password.Dispose();

            _navigationService.Navigate<VaultViewModel>();
        }
    }
}