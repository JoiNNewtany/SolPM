using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace SolPM.Core.ViewModels
{
    public class EntryViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigationService;

        public EntryViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;

            // CHECK IT OUT AND DO IT
            // https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.controls.passwordbox.passwordrevealmode
            // https://stackoverflow.com/questions/5018613/wpf-switching-usercontrols-depending-on-corresponding-viewmodels-mvvm

            // Commands

            NavigateVaultView = new MvxAsyncCommand(() => _navigationService.Navigate<VaultViewModel>());
        }

        #region Commands

        public IMvxAsyncCommand NavigateVaultView { get; private set; }

        #endregion Commands
    }
}