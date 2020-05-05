using Microsoft.Win32;
using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.ViewModels;
using SolPM.Core.ViewModels;
using SolPM.Core.ViewModels.Parameters;
using SolPM.Core.Interactions;
using System.Security;
using MvvmCross.Base;
using MvvmCross.Binding.BindingContext;
using System.Data.SqlTypes;

namespace SolPM.WPF.Views
{
    /// <summary>
    /// Interaction logic for OpenVaultView.xaml
    /// </summary>
    [MvxViewFor(typeof(OpenVaultViewModel))]
    public partial class OpenVaultView : MvxWpfView
    {
        public OpenVaultView()
        {
            InitializeComponent();
            Parameters = new VaultParams();

            var set = this.CreateBindingSet<OpenVaultView, OpenVaultViewModel>();
            set.Bind(this).For(view => view.Interaction).To(viewModel => viewModel.MessageInteraction).OneWay();
            set.Apply();
        }

        public VaultParams Parameters { get; set; }

        // Exposing parameters as properties to force re-evaluation of CanExecute
        #region Parameters Properties

        public string FilePath
        {
            get
            {
                return Parameters.FilePath;
            }
            set
            {
                Parameters.FilePath = value;

                var viewModel = (OpenVaultViewModel)DataContext;
                viewModel.OpenVaultCommand.RaiseCanExecuteChanged();
            }
        }

        public SecureString Password
        {
            get
            {
                return Parameters.Password;
            }
            set
            {
                Parameters.Password = value;

                var viewModel = (OpenVaultViewModel)DataContext;
                viewModel.OpenVaultCommand.RaiseCanExecuteChanged();
            }
        }

        # endregion Parameters Properties

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
            // show dialog
            OpenVaultSnackbar.MessageQueue.Enqueue(message.Message);
        }


        private void VaultLocationButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                DefaultExt = ".solpv",
                CheckFileExists = true,
                CheckPathExists = true,
                Multiselect = false,
                Filter = "Solar Password Vault (*.solpv)|*.solpv",
            };

            if (dialog.ShowDialog() == true)
            {
                VaultLocationTextBox.Text = dialog.FileName;
            }
        }
    }
}