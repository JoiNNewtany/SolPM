using Microsoft.Win32;
using MvvmCross.Binding.Extensions;
using MvvmCross.ViewModels;
using SolPM.Core.Cryptography;
using SolPM.Core.Models;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace SolPM.WPF.Windows
{
    /// <summary>
    /// Interaction logic for EntryViewerWindow.xaml
    /// </summary>
    public partial class EntryViewerWindow : Window
    {
        public EntryViewerWindow()
        {
            InitializeComponent();
        }

        public MvxObservableCollection<Field> PrimaryFields
        {
            get 
            {
                var entry = DataContext as Entry;
                return entry.FieldList.Where(
                    (e) => e.Type == FieldTypes.Username ||
                    e.Type == FieldTypes.Password)
                    as MvxObservableCollection<Field>;
            }
        }

        public MvxObservableCollection<Field> SecondaryFields
        {
            get
            {
                var entry = DataContext as Entry;
                return entry.FieldList.Where(
                    f => PrimaryFields.All(f2 => f2.Type != f.Type))
                    as MvxObservableCollection<Field>;
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

        private void Download_Click(object sender, RoutedEventArgs e)
        {
            var content = ((sender as Button).DataContext as Field).Value as byte[];
            var fileName = ((sender as Button).DataContext as Field).FileName;

            if (null == content)
            {
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
    }
}