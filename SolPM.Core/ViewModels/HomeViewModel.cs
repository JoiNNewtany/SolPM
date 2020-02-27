using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace SolPM.Core.ViewModels
{
    public class HomeViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigationService;

        public HomeViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;

            // Commands
        }
    }
}