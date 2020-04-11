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

        private void VaultViewDialogRoot_DialogClosing(object sender, MaterialDesignThemes.Wpf.DialogClosingEventArgs eventArgs)
        {
            if (!Equals(eventArgs.Parameter, true)) return;

            // Get current ViewModel
            var viewModel = (VaultViewModel)DataContext;

            // Call command
            viewModel.AddFolderCommand.Execute(new object[] { ColorPicker.Color, FolderNameBox.Text });
            
            // Clear fields
            ColorPicker.Color = System.Windows.Media.Color.FromArgb(0, 0, 0, 0);
            FolderNameBox.Text = string.Empty;
        }
    }
}