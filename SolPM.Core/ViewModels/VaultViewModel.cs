using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
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

        #endregion Commands

        #region Properties

        public Vault Vault { get; set; }
        public Entry SelectedEntry { get; set; }

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
            }
        }

        public MvxObservableCollection<Folder> VaultFolders
        {
            get
            {
                if (Vault.Exists())
                {
                    //return Vault.GetInstance().FolderList;
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
                    //Vault.GetInstance().FolderList = value;
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

            vault.EncryptToFile(vault.Location, vault.EncryptionInfo.ProtectedKey);
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

            vault.EncryptToFile(filePath, vault.EncryptionInfo.ProtectedKey);
        }

        private void CloseVault()
        {
            // TODO: Implement unsaved changes dialog
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
        }
    }
}