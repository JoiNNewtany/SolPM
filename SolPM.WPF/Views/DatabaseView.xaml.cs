using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.ViewModels;
using SolPM.Core.ViewModels;

namespace SolPM.WPF.Views
{
    /// <summary>
    /// Interaction logic for DatabaseView.xaml
    /// </summary>
    [MvxViewFor(typeof(DatabaseViewModel))]
    public partial class DatabaseView : MvxWpfView
    {
        public DatabaseView()
        {
            InitializeComponent();
        }
    }
}