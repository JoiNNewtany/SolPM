using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using SolPM.Core.Helpers;
using SolPM.Core.Interactions;
using SolPM.Core.Models;
using SolPM.Core.ViewModels.Parameters;

namespace SolPM.Core.ViewModels
{
    public class OptionsViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigationService;

        private MvxInteraction<MessageInteraction> _messageInteraction = new MvxInteraction<MessageInteraction>();
        public IMvxInteraction<MessageInteraction> MessageInteraction => _messageInteraction;

        public OptionsViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
            if (Vault.Exists())
            {
                Vault = Vault.GetInstance();
            }

            // Commands

            NavigateVaultView = new MvxAsyncCommand(() =>
                _navigationService.Navigate<VaultViewModel>(), () => CanNavigate());
            ChangePasswordCommand = new MvxCommand<VaultParams>((s) => ChangePassword(s), (s) => CanChangePassword(s));
        }

        public IMvxAsyncCommand NavigateVaultView { get; private set; }
        public IMvxCommand ChangePasswordCommand { get; private set; }

        public Vault Vault { get; set; }

        public bool VaultOpen 
        {
            get
            {
                return Vault.Exists();
            } 
        }

        private bool CanNavigate()
        {
            if (!Vault.Exists())
            {
                return true;
            }

            if (string.IsNullOrWhiteSpace(Vault.Name))
            {
                return false;
            }

            return true;
        }

        private void ChangePassword(VaultParams vaultParams)
        {
            try
            {
                Vault.ChangePassword(vaultParams.Password);
                _messageInteraction.Raise(new MessageInteraction("Password changed."));
            }
            catch (System.Exception e)
            {
                _messageInteraction.Raise(new MessageInteraction(e.Message));
            }
        }

        public bool CanChangePassword(VaultParams vaultParams)
        {
            if (!Vault.Exists())
            {
                return false;
            }

            if (null == vaultParams.Password ||
                vaultParams.Password.Length <= 0)
            {
                return false;
            }

            if (null == vaultParams.ValidationPassword ||
                vaultParams.ValidationPassword.Length <= 0)
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