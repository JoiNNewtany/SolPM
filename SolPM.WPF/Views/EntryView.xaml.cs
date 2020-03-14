using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.ViewModels;
using SolPM.Core.ViewModels;

namespace SolPM.WPF.Views
{
    /// <summary>
    /// Interaction logic for EntryView.xaml
    /// </summary>
    [MvxViewFor(typeof(EntryViewModel))]
    public partial class EntryView : MvxWpfView
    {
        public EntryView()
        {
            InitializeComponent();
        }

        private void DialogHost_DialogClosing(object sender, MaterialDesignThemes.Wpf.DialogClosingEventArgs eventArgs)
        {
            throw new System.NotImplementedException("\n\nDialog closed: " + eventArgs.ToString());
        }
    }
}
