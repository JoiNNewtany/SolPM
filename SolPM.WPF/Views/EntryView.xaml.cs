using Microsoft.Win32;
using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.ViewModels;
using SolPM.Core.ViewModels;
using System;

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

        private void SelectIconBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // Get current ViewModel
            var viewModel = (EntryViewModel)DataContext;

            var dialog = new OpenFileDialog
            {
                CheckFileExists = true,
                Multiselect = false,
                DefaultExt = ".png",
                Filter = @"Image Files (*.png, *.jpg, *.bmp, *.ico)|*.png;*.jpeg;*.jpg;*.bmp;*.ico|
                              PNG Files (*.png)|*.png|
                              JPEG Files (*.jpg)|*.jpeg;*.jpg|
                              Bitmap Files (*.bmp)|*.bmp|
                              ICO Files (*.ico)|*.ico"
            };

            // Call command
            if (dialog.ShowDialog() == true &&
                viewModel.SetEntryIconCommand.CanExecute(dialog.FileName))
            {
                viewModel.SetEntryIconCommand.Execute(dialog.FileName);
            }
        }

        private void SelectImageBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // Get current ViewModel
            var viewModel = (EntryViewModel)DataContext;

            var dialog = new OpenFileDialog
            {
                CheckFileExists = true,
                Multiselect = false,
                DefaultExt = ".png",
                Filter = @"Image Files (*.png, *.jpg, *.bmp, *.ico)|*.png;*.jpeg;*.jpg;*.bmp;*.ico|
                              PNG Files (*.png)|*.png|
                              JPEG Files (*.jpg)|*.jpeg;*.jpg|
                              Bitmap Files (*.bmp)|*.bmp|
                              ICO Files (*.ico)|*.ico"
            };

            // Call command
            if (dialog.ShowDialog() == true &&
                viewModel.SetEntryImageCommand.CanExecute(dialog.FileName))
            {
                viewModel.SetEntryImageCommand.Execute(dialog.FileName);
            }
        }
    }
}