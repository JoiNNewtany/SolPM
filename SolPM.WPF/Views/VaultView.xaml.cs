using MaterialDesignThemes.Wpf;
using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.ViewModels;
using SolPM.Core.Models;
using SolPM.Core.ViewModels;
using System.Threading.Tasks;

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

        private void EditFolder_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // Get current ViewModel
            var viewModel = (VaultViewModel)DataContext;

            if (null != viewModel.SelectedFolder)
            {
                RootDialog.IsOpen = true;
            }
        }

        private void AddFolder_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // Get current ViewModel
            var viewModel = (VaultViewModel)DataContext;

            // Creating new folder and selecting it
            Folder folder = new Folder()
            {
                EntryList = new MvxObservableCollection<Entry>(),
            };

            if (viewModel.AddFolderCommand.CanExecute(folder))
            {
                viewModel.AddFolderCommand.Execute(folder);
            }

            FolderList.SelectedItem = folder;

            RootDialog.IsOpen = true;
        }
    }
}