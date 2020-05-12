using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using SolPM.Core.Interactions;
using SolPM.Core.Models;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SolPM.Core.ViewModels
{
    public class VaultViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigationService;

        private MvxInteraction<YesNoInteraction> _yesNoInteraction = new MvxInteraction<YesNoInteraction>();
        public IMvxInteraction<YesNoInteraction> YNInteraction => _yesNoInteraction;

        private MvxInteraction<MessageInteraction> _messageInteraction = new MvxInteraction<MessageInteraction>();
        public IMvxInteraction<MessageInteraction> MessageInteraction => _messageInteraction;

        public VaultViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;

            // Commands

            NavigateDatabaseView = new MvxAsyncCommand(() => _navigationService.Navigate<DatabaseViewModel>());
            NavigateEntryView = new MvxAsyncCommand(() => _navigationService.Navigate<EntryViewModel>());
            NavigateOpenVaultView = new MvxAsyncCommand(() => _navigationService.Navigate<OpenVaultViewModel>());
            AddEntryCommand = new MvxAsyncCommand(AddEntry);
            EditEntryCommand = new MvxAsyncCommand(EditEntry);
            RemoveEntryCommand = new MvxCommand(RemoveEntry);
            AddFolderCommand = new MvxCommand<Folder>((s) => AddFolder(s));
            RemoveFolderCommand = new MvxCommand(RemoveFolder);
            SaveVaultCommand = new MvxCommand(SaveVault);
            SaveVaultAsCommand = new MvxCommand<string>((s) => SaveVaultAs(s));
            CloseVaultCommand = new MvxCommand(CloseVault);
            OpenGithubCommand = new MvxCommand(() => Process.Start("https://github.com/JoiNNewtany/SolPM"));
            OpenWikiCommand = new MvxCommand(() => Process.Start("https://github.com/JoiNNewtany/SolPM/wiki"));
            ExitApplicationCommand = new MvxCommand(ExitApplication);
        }

        public override async Task Initialize()
        {
            // TODO: Do all the TODO's in xaml files
            if (Vault.Exists())
            {
                Vault = Vault.GetInstance();
            }
        }

        #region Commands

        public IMvxAsyncCommand NavigateDatabaseView { get; private set; }
        public IMvxAsyncCommand NavigateEntryView { get; private set; }
        public IMvxAsyncCommand NavigateOpenVaultView { get; private set; }

        public IMvxAsyncCommand AddEntryCommand { get; private set; }
        public IMvxAsyncCommand EditEntryCommand { get; private set; }
        public IMvxCommand RemoveEntryCommand { get; private set; }

        public IMvxCommand AddFolderCommand { get; private set; }
        public IMvxCommand RemoveFolderCommand { get; private set; }

        public IMvxCommand SaveVaultCommand { get; private set; }
        public IMvxCommand SaveVaultAsCommand { get; private set; }
        public IMvxCommand CloseVaultCommand { get; private set; }

        public IMvxCommand OpenGithubCommand { get; set; }
        public IMvxCommand OpenWikiCommand { get; set; }
        public IMvxCommand ExitApplicationCommand { get; set; }

        #endregion Commands

        #region Properties

        private Vault vault;

        public Vault Vault
        {
            get
            {
                return vault;
            }

            set
            {
                vault = value;
                // Causing UI buttons to enable/disable
                RaisePropertyChanged(() => IsVaultOpen);
            }
        }

        private Entry selectedEntry;

        public Entry SelectedEntry
        {
            get
            {
                return selectedEntry;
            }

            set
            {
                selectedEntry = value;
                // Causing UI buttons to enable/disable
                RaisePropertyChanged(() => IsEntrySelected);
            }
        }

        public bool IsVaultOpen
        {
            get
            {
                return Vault.Exists();
            }
        }

        public bool IsFolderOpen
        {
            get
            {
                return null != SelectedFolder;
            }
        }

        public bool IsEntrySelected
        {
            get
            {
                return null != SelectedEntry;
            }
        }

        private Folder selectedFolder;

        public Folder SelectedFolder
        {
            get
            {
                return selectedFolder;
            }

            set
            {
                selectedFolder = value;
                RaisePropertyChanged(() => SelectedFolder);
                // Causing FolderEntries to update on UI
                RaisePropertyChanged(() => FolderEntries);
                // Causing UI buttons to enable/disable
                RaisePropertyChanged(() => IsFolderOpen);
            }
        }

        public MvxObservableCollection<Folder> VaultFolders
        {
            get
            {
                if (Vault.Exists())
                {
                    return Vault.FolderList;
                }
                else
                {
                    return null;
                }
            }

            set
            {
                if (Vault.Exists())
                {
                    Vault.FolderList = value;
                    RaisePropertyChanged(() => VaultFolders);
                }
            }
        }

        public MvxObservableCollection<Entry> FolderEntries
        {
            get
            {
                if (SelectedFolder != null)
                {
                    return SelectedFolder.EntryList;
                }
                else
                {
                    return null;
                }
            }

            set
            {
                if (SelectedFolder != null)
                {
                    SelectedFolder.EntryList = value;
                    RaisePropertyChanged(() => FolderEntries);
                }
            }
        }

        #endregion Properties

        private async Task AddEntry()
        {
            if (null != SelectedFolder)
            {
                var newEntry = await _navigationService.Navigate<EntryViewModel, Entry, Entry>(new Entry());
                if (newEntry != null)
                {
                    Debug.WriteLine($"VaultViewModel: Recieved {newEntry.Name}.");
                    FolderEntries.Add(newEntry);
                }
                else
                {
                    Debug.WriteLine($"EntryViewModel: Recieved no entries.");
                }
            }
        }

        // TODO: Rewrite edit command
        private async Task EditEntry()
        {
            if (null != SelectedEntry)
            {
                // HACK: Find a way to fix binding issues
                Entry tempEntry = new Entry()
                {
                    Name = SelectedEntry.Name,
                    FieldList = new MvxObservableCollection<Field>(),
                    Color = SelectedEntry.Color,
                    Icon = SelectedEntry.Icon,
                    Image = SelectedEntry.Image,
                    Created = SelectedEntry.Created,
                    Accessed = SelectedEntry.Accessed,
                    Modified = SelectedEntry.Modified,
                };

                tempEntry.FieldList.ReplaceWith(SelectedEntry.FieldList);

                Entry editedEntry = await _navigationService.Navigate<EntryViewModel, Entry, Entry>(tempEntry);
                if (editedEntry != null)
                {
                    // Replacing original entry with the edited one
                    var replacedEntry = Vault.FolderList.Single(s => s == SelectedFolder).EntryList.Where(
                        s => s == SelectedEntry).First();
                    var index = Vault.FolderList.Single(s => s == SelectedFolder).EntryList.IndexOf(replacedEntry);

                    if (index != -1)
                        Vault.FolderList.Single(s => s == SelectedFolder).EntryList[index] = editedEntry;

                    await RaisePropertyChanged(() => SelectedEntry);
                }
                else
                {
                    tempEntry = null;
                }
            }
        }

        private void RemoveEntry()
        {
            // TODO: Error handling
            if (null != SelectedEntry)
            {
                FolderEntries.Remove(FolderEntries.Single(s => s == SelectedEntry));
            }
        }

        private void AddFolder(Folder folder)
        {
            if (null != folder)
            {
                VaultFolders.Add(folder);
                RaisePropertyChanged(() => VaultFolders);
            }
        }

        private void RemoveFolder()
        {
            if (null != SelectedFolder)
            {
                VaultFolders.Remove(VaultFolders.Single(s => s == SelectedFolder));
            }
        }

        //private void DisplayDialog(Action action, string message)
        //{
        //    var request = new YesNoInteraction
        //    {
        //        YesNoCallback = (accepted) =>
        //        {
        //            if (accepted)
        //            {
        //                action();
        //            }
        //        },
        //        Message = message,
        //    };
        //    _yesNoInteraction.Raise(request);
        //}

        private void DisplayMessage(string message)
        {
            _messageInteraction.Raise(new MessageInteraction(message));
        }

        private void SaveVault()
        {
            if (!Vault.Exists())
            {
                return;
            }

            var vault = Vault.GetInstance();

            if (string.IsNullOrWhiteSpace(vault.Location))
            {
                throw new NotImplementedException("I don't know what to do with this ;-;");
            }

            try
            {
                vault.EncryptToFile(vault.Location, vault.EncryptionInfo.ProtectedKey);
                DisplayMessage("Vault saved.");
            }
            catch (Exception e)
            {
                DisplayMessage($"Unable to save: {e.Message}");
            }
        }

        private void SaveVaultAs(string filePath)
        {
            if (!Vault.Exists())
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentNullException("filePath", "Path to file is either null or empty.");
            }

            var vault = Vault.GetInstance();

            try
            {
                vault.EncryptToFile(filePath, vault.EncryptionInfo.ProtectedKey);
                DisplayMessage("Vault saved.");
            }
            catch (Exception e)
            {
                DisplayMessage($"Unable to save: {e.Message}");
            }
        }

        private void CloseVault()
        {
            // TODO: Implement unsaved changes dialog
            if (!Vault.Exists())
            {
                return;
            }

            // TODO: Add IsSaved so the dialog isn't displayed every time
            // Display save vault dialog
            // DisplayDialog(SaveVault, "Save changes in the vault?");

            Vault.Delete();
            Vault = null;
            SelectedEntry = null;
            SelectedFolder = null;
            FolderEntries = null;
            VaultFolders = null;
            RaisePropertyChanged(() => Vault);
            RaisePropertyChanged(() => SelectedEntry);
            RaisePropertyChanged(() => SelectedFolder);
            RaisePropertyChanged(() => FolderEntries);
            RaisePropertyChanged(() => VaultFolders);
            DisplayMessage("Vault closed.");
        }

        private void ExitApplication()
        {
            CloseVault();

            // TODO: Implement proper application shutdown
            Environment.Exit(0);
        }
    }
}