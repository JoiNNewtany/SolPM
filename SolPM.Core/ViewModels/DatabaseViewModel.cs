using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

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

        #region Commands

        public IMvxAsyncCommand NavigateVaultView { get; private set; }

        public IMvxCommand<string> CreateVaultCommand { get; private set; }

        private void CreateVault(string filename)
        {
        }

        #endregion Commands
    }
}