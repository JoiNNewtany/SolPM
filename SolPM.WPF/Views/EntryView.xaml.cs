using Microsoft.Win32;
using MvvmCross.Base;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.ViewModels;
using SolPM.Core.Cryptography;
using SolPM.Core.Interactions;
using SolPM.Core.Models;
using SolPM.Core.ViewModels;
using System.IO;
using System.Security;
using System.Text;
using System.Windows;
using System.Windows.Controls;

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

            var set = this.CreateBindingSet<EntryView, EntryViewModel>();
            set.Bind(this).For(view => view.Interaction).To(viewModel => viewModel.MessageInteraction).OneWay();
            set.Apply();
        }

        private IMvxInteraction<MessageInteraction> _interaction = new MvxInteraction<MessageInteraction>();
        public IMvxInteraction<MessageInteraction> Interaction
        {
            get => _interaction;
            set
            {
                if (null != _interaction)
                    _interaction.Requested -= OnInteractionRequested;

                if (null != value)
                {
                    _interaction = value;
                    _interaction.Requested += OnInteractionRequested;
                }
            }
        }

        private void OnInteractionRequested(object sender, MvxValueEventArgs<MessageInteraction> eventArgs)
        {
            var message = eventArgs.Value;
            // show message
            EntrySnackbar.MessageQueue.Enqueue(message.Message);
        }

        private void SelectIconBtn_Click(object sender, RoutedEventArgs e)
        {
            // Get current ViewModel
            var viewModel = (EntryViewModel)DataContext;

            var dialog = new OpenFileDialog
            {
                CheckFileExists = true,
                Multiselect = false,
                DefaultExt = ".png",
                Filter = "Image Files (*.png, *.jpg, *.bmp, *.ico)|*.png;*.jpeg;*.jpg;*.bmp;*.ico|PNG Files (*.png)|*.png|JPEG Files (*.jpg)|*.jpeg;*.jpg|Bitmap Files (*.bmp)|*.bmp|ICO Files (*.ico)|*.ico"
            };

            // Call command
            if (dialog.ShowDialog() == true &&
                viewModel.SetEntryIconCommand.CanExecute(dialog.FileName))
            {
                viewModel.SetEntryIconCommand.Execute(dialog.FileName);
            }
        }

        private void SelectImageBtn_Click(object sender, RoutedEventArgs e)
        {
            // Get current ViewModel
            var viewModel = (EntryViewModel)DataContext;

            var dialog = new OpenFileDialog
            {
                CheckFileExists = true,
                Multiselect = false,
                DefaultExt = ".png",
                Filter = "Image Files (*.png, *.jpg, *.bmp, *.ico)|*.png;*.jpeg;*.jpg;*.bmp;*.ico|PNG Files (*.png)|*.png|JPEG Files (*.jpg)|*.jpeg;*.jpg|Bitmap Files (*.bmp)|*.bmp|ICO Files (*.ico)|*.ico"
            };

            // Call command
            if (dialog.ShowDialog() == true &&
                viewModel.SetEntryImageCommand.CanExecute(dialog.FileName))
            {
                viewModel.SetEntryImageCommand.Execute(dialog.FileName);
            }
        }

        private void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                CheckFileExists = true,
                CheckPathExists = true,
                Multiselect = false,
                Filter = "All Files (*.*)|*.*",
            };

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    var content = File.ReadAllBytes(dialog.FileName);
                    ((sender as Button).DataContext as Field).Value = content;
                    ((sender as Button).DataContext as Field).FileName = dialog.SafeFileName;
                }
                catch (System.Exception)
                {
                    throw;
                }
            }
        }

        private void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            var content = ((sender as Button).DataContext as Field).Value as byte[];
            var fileName = ((sender as Button).DataContext as Field).FileName;

            if (null == content)
            {
                EntrySnackbar.MessageQueue.Enqueue("This field does not contain any files.");
                return;
            }

            var dialog = new SaveFileDialog
            {
                FileName = fileName,
                DefaultExt = Path.GetExtension(fileName),
                Filter = "All Files (*.*)|*.*",
            };

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    File.WriteAllBytes(dialog.FileName, content);
                }
                catch (System.Exception)
                {
                    throw;
                }
            }
        }

        private void CopyValue_Click(object sender, RoutedEventArgs e)
        {
            var value = ((sender as Button).DataContext as Field).Value;

            if (null == value)
            {
                return;
            }

            if (value is string str)
            {
                Clipboard.SetText(str);
            }

            if (value is SecureString sstr && 0 < sstr.Length)
            {
                Clipboard.SetText(Encoding.UTF8.GetString(CryptoUtilities.SecStrBytes(sstr)));
            }
        }
    }
}