using MvvmCross.ViewModels;
using MvvmCross.Commands;
using MvvmCross.Navigation;

namespace SolPM.Core.ViewModels
{
    public class EntryViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigationService;

        public EntryViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;

            // Commands

            NavigateVaultView = new MvxAsyncCommand(() => _navigationService.Navigate<VaultViewModel>());
        }

        #region Commands

        public IMvxAsyncCommand NavigateVaultView { get; private set; }

        #endregion
    }
}
