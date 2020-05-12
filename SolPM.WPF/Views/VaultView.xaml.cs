using Microsoft.Win32;
using MvvmCross.Base;
using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.ViewModels;
using SolPM.Core.Models;
using SolPM.Core.ViewModels;
using SolPM.WPF.Dialogs;
using SolPM.WPF.Windows;
using MaterialDesignThemes.Wpf;
using MvvmCross.Binding.BindingContext;
using SolPM.Core.Interactions;
using System.Windows;
using System.Windows.Controls;

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

            var set = this.CreateBindingSet<VaultView, VaultViewModel>();
            set.Bind(this).For(view => view.YNInteraction).To(viewModel => viewModel.YNInteraction).OneWay();
            set.Bind(this).For(view => view.MessageInteraction).To(viewModel => viewModel.MessageInteraction).OneWay();
            set.Apply();
        }

        private IMvxInteraction<YesNoInteraction> _yesNoInteraction;
        public IMvxInteraction<YesNoInteraction> YNInteraction
        {
            get => _yesNoInteraction;
            set
            {
                if (null != _yesNoInteraction)
                    _yesNoInteraction.Requested -= OnYNInteractionRequested;

                if (null != value)
                {
                    _yesNoInteraction = value;
                    _yesNoInteraction.Requested += OnYNInteractionRequested;
                }
            }
        }

        private IMvxInteraction<MessageInteraction> _messageInteraction;
        public IMvxInteraction<MessageInteraction> MessageInteraction
        {
            get => _messageInteraction;
            set
            {
                if (null != _messageInteraction)
                    _messageInteraction.Requested -= OnMessageInteractionRequested;

                if (null != value)
                {
                    _messageInteraction = value;
                    _messageInteraction.Requested += OnMessageInteractionRequested;
                }
            }
        }

        private async void OnYNInteractionRequested(object sender, MvxValueEventArgs<YesNoInteraction> eventArgs)
        {
            // If the dialog doesn't display during debug, kill VS' designer process

            var yesNoInteraction = eventArgs.Value;
            var dialog = new YesNoDialog()
            {
                Message = yesNoInteraction.Message,
            };
            var result = await DialogHost.Show(dialog);
            yesNoInteraction.YesNoCallback((bool)result);
        }

        private void OnMessageInteractionRequested(object sender, MvxValueEventArgs<MessageInteraction> eventArgs)
        {
            var messageInteraction = eventArgs.Value;
            // show dialog
            VaultSnackbar.MessageQueue.Enqueue(messageInteraction.Message);
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

        private void SaveVaultAs_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog
            {
                DefaultExt = ".solpv",
                Filter = "Solar Password Vault (*.solpv)|*.solpv"
            };

            if (dialog.ShowDialog() == true)
            {
                // Get current ViewModel
                var viewModel = (VaultViewModel)DataContext;

                if (viewModel.SaveVaultAsCommand.CanExecute())
                {
                    viewModel.SaveVaultAsCommand.Execute(dialog.FileName);
                }
            }
        }

        private void PwdGenerator_Click(object sender, RoutedEventArgs e)
        {
            var window = new PwdGeneratorWindow();
            window.DataContext = new PwdGeneratorViewModel();
            window.Show();
        }

        private void PopOut_Click(object sender, RoutedEventArgs e)
        {
            // Get bound data context
            var dc = (Entry)(sender as Button).DataContext;

            var window = new EntryViewerWindow();
            window.DataContext = dc;
            window.Show();
        }
    }
}