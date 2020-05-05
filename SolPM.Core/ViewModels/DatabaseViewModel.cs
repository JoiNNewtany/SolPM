using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using SolPM.Core.Helpers;
using SolPM.Core.Models;
using SolPM.Core.ViewModels.Parameters;
using System;
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
            CreateVaultCommand = new MvxCommand<VaultParams>((s) => CreateVault(s), (s) => CanCreateVault(s));
        }

        public override async Task Initialize()
        {
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

        public bool CanCreateVault(VaultParams vaultParams)
        {
            if (null == vaultParams)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(vaultParams.FilePath))
            {
                return false;
            }

            //if (!Uri.IsWellFormedUriString("file:///" + vaultParams.FilePath.Replace("\\", "/"), UriKind.Absolute))
            //{
            //    return false;
            //}

            if (string.IsNullOrWhiteSpace(vaultParams.Name))
            {
                return false;
            }

            if (null == vaultParams.Password || null == vaultParams.ValidationPassword)
            {
                return false;
            }

            if (!SecureStringExtension.Equals(vaultParams.Password, vaultParams.ValidationPassword))
            {
                return false;
            }

            return true;
        }
    }
}