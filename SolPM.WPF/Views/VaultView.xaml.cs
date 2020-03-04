using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.ViewModels;
using SolPM.Core.ViewModels;

namespace SolPM.WPF.Views
{
    /// <summary>
    /// Interaction logic for VaultView.xaml
    /// </summary>
    [MvxViewFor(typeof(VaultViewModel))]
    public partial class VaultView : MvxWpfView
    {
        public VaultView()
        {
            InitializeComponent();
        }
    }
}
