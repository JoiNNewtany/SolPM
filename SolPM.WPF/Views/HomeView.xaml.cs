using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.ViewModels;
using SolPM.Core.ViewModels;

namespace SolPM.WPF.Views
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    [MvxViewFor(typeof(HomeViewModel))]
    public partial class HomeView : MvxWpfView
    {
        public HomeView()
        {
            InitializeComponent();
        }
    }
}