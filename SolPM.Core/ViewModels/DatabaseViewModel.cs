using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using SolPM.Core.Helpers;
using SolPM.Core.Models;
using SolPM.Core.ViewModels.Parameters;
using System.Windows.Media;
using System.Threading.Tasks;

namespace SolPM.Core.ViewModels
{
    public class DatabaseViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigationService;

        public DatabaseViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;

            // Commands

            NavigateVaultView = new MvxAsyncCommand(() => _navigationService.Navigate<VaultViewModel>());
            CreateVaultCommand = new MvxCommand<VaultParams>((s) => CreateVault(s), (s) => CanCreateVault(s));
        }

        public override async Task Initialize()
        {
        }

        #region Commands

        public IMvxAsyncCommand NavigateVaultView { get; private set; }
        public IMvxCommand CreateVaultCommand { get; private set; }

        #endregion Commands

        private void CreateVault(VaultParams vaultParams)
        {
            if (Vault.Exists())
            {
                // TODO: Rewrite this
                var _vault = Vault.GetInstance();
                _vault.EncryptToFile(_vault.Location, _vault.EncryptionInfo.ProtectedKey);
                Vault.Delete();
            }

            var vault = Vault.GetInstance();

            vault.Location = vaultParams.FilePath;
            vault.Name = vaultParams.Name;
            vault.Description = vaultParams.Description;
            vault.EncryptionInfo = vaultParams.EncryptionInfo;

            // Create example entries
            vault.FolderList = new MvxObservableCollection<Folder>()
            {
                new Folder()
                {
                    Name = "Example Folder",
                    Color = new XmlColor(Color.FromArgb(255, 255, 132, 0)),

                    EntryList = new MvxObservableCollection<Entry>()
                    {
                        new Entry()
                        {
                            Name = "Example Entry",
                            Color = new XmlColor(Color.FromArgb(255, 255, 132, 0)),

                            FieldList = new MvxObservableCollection<Field>()
                            {
                                new Field()
                                {
                                    Type = FieldTypes.Note,
                                    Name = "Welcome to SolPM",
                                    Value = "Thank you for trying out Solar Password Manager!\n\nGet started by making a new field for this entry. To do that you can press the \"+\" button on the bottom right side of the screen and choose the type of the field you wish to add.\nAfter filling the field you may want to save this entry by pressing the respective button in the right upper corner of the screen or simply hitting Ctrl+S on your keyboard.\nWhen outside of entry editing mode, you may add new entries or folders to your Vault from the menu on the upper side of the window or by using the keyboard shortcuts displayed next to the functions in the menu.",
                                }
                            }
                        }
                    }
                }
            };

            vault.SetupEncryption(vaultParams.Password);
            vaultParams.Password.Dispose();

            _navigationService.Navigate<VaultViewModel>();
        }

        public bool CanCreateVault(VaultParams vaultParams)
        {
            if (null == vaultParams)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(vaultParams.FilePath))
            {
                return false;
            }

            //if (!Uri.IsWellFormedUriString("file:///" + vaultParams.FilePath.Replace("\\", "/"), UriKind.Absolute))
            //{
            //    return false;
            //}

            if (string.IsNullOrWhiteSpace(vaultParams.Name))
            {
                return false;
            }

            if (null == vaultParams.Password || null == vaultParams.ValidationPassword)
            {
                return false;
            }

            if (!SecureStringExtension.Equals(vaultParams.Password, vaultParams.ValidationPassword))
            {
                return false;
            }

            return true;
        }
    }
}