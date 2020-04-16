using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using SolPM.Core.Helpers;
using SolPM.Core.Models;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Xml;
using System.Xml.Serialization;

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
            AddEntryCommand = new MvxAsyncCommand(AddEntry);
            EditEntryCommand = new MvxAsyncCommand(EditEntry);
            RemoveEntryCommand = new MvxCommand(RemoveEntry);
            AddFolderCommand = new MvxCommand<Folder>((s) => AddFolder(s));
            RemoveFolderCommand = new MvxCommand(RemoveFolder);
        }

        public override async Task Initialize()
        {
            // TODO: Get rid of test actions AND MAKE VAULT A PROPERTY!!!
            // TODO: Do all the TODO's in xaml files

            // Test Singleton Vault

            //Vault vault;

            if (!Vault.Exists())
            {
                Vault = Vault.GetInstance();
                Vault.Name = "My Test Vault";

                Vault.FolderList = new MvxObservableCollection<Folder>()
                {
                    new Folder()
                    {
                        Name = "Folder 1",
                        Color = new XmlColor(System.Windows.Media.Color.FromRgb(0, 255, 0)),

                        EntryList = new MvxObservableCollection<Entry>()
                        {
                            new Entry()
                            {
                                Name = "Google",
                                Icon = new BitmapImage(new Uri("E:\\Downloads\\google-logo.png")),
                                Color = new XmlColor(System.Windows.Media.Color.FromRgb(255, 255, 255)),
                                Created = DateTime.Now,
                                Modified = DateTime.Now,
                                Accessed = DateTime.Now,

                                FieldList = new MvxObservableCollection<Field>()
                                {
                                    new Field()
                                    {
                                        Type = FieldTypes.Username,
                                        Name = "Username",
                                        Value = "Solar",
                                    },

                                    new Field()
                                    {
                                        Type = FieldTypes.Password,
                                        Name = "Password",
                                        Value = new byte[] { 1, 1, 1, 0, 0 },
                                    },

                                    new Field()
                                    {
                                        Type = FieldTypes.Note,
                                        Name = "URL",
                                        Value = "www.google.com",
                                    },
                                }
                            },

                            new Entry()
                            {
                                Name = "Secret",
                                Image = new BitmapImage(new Uri("E:\\Downloads\\ShareX-ScreenRecordings\\ramiras.png")),
                                Created = DateTime.Now,
                                Modified = DateTime.Now,
                                Accessed = DateTime.Now,

                                FieldList = new MvxObservableCollection<Field>()
                                {
                                    new Field()
                                    {
                                        Type = FieldTypes.Username,
                                        Name = "Username",
                                        Value = "Solar",
                                    },

                                    new Field()
                                    {
                                        Type = FieldTypes.Password,
                                        Name = "Password",
                                        Value = new byte[] { 1, 0, 1, 1, 0 },
                                    },

                                    new Field()
                                    {
                                        Type = FieldTypes.Note,
                                        Name = "URL",
                                        Value = "www.google.com",
                                    },
                                }
                            },
                        }
                    },

                    new Folder()
                    {
                        Name = "Super Folder 2",
                        Color = new XmlColor(System.Windows.Media.Color.FromRgb(0, 0, 255)),

                        EntryList = new MvxObservableCollection<Entry>()
                        {
                            new Entry()
                            {
                                Name = "Microsoft",
                                Icon = new BitmapImage(new Uri("E:\\Downloads\\microsoft-logo.png")),
                                Color = new XmlColor(System.Windows.Media.Color.FromRgb(0, 64, 131)),
                                Created = DateTime.Now,
                                Modified = DateTime.Now,
                                Accessed = DateTime.Now,

                                FieldList = new MvxObservableCollection<Field>()
                                {
                                    new Field()
                                    {
                                        Type = FieldTypes.Username,
                                        Name = "Username",
                                        Value = "Solar Flaer",
                                    },

                                    new Field()
                                    {
                                        Type = FieldTypes.Password,
                                        Name = "Password",
                                        Value = new byte[] { 0, 0, 1, 0, 1 },
                                    },

                                    new Field()
                                    {
                                        Type = FieldTypes.Note,
                                        Name = "URL",
                                        Value = "www.microsoft.com",
                                    },
                                }
                            },

                            new Entry()
                            {
                                Name = "Ducc",
                                Image = new BitmapImage(new Uri("E:\\Downloads\\duck-image.jpg")),
                                Created = DateTime.Now,
                                Modified = DateTime.Now,
                                Accessed = DateTime.Now,

                                FieldList = new MvxObservableCollection<Field>()
                                {
                                    new Field()
                                    {
                                        Type = FieldTypes.Username,
                                        Name = "Username",
                                        Value = "Sauce Fire",
                                    },

                                    new Field()
                                    {
                                        Type = FieldTypes.Password,
                                        Name = "Password",
                                        Value = new byte[] { 0, 0, 0, 1, 0 },
                                    },

                                    new Field()
                                    {
                                        Type = FieldTypes.Note,
                                        Name = "URL",
                                        Value = "www.duckduckgo.com",
                                    },

                                    new Field()
                                    {
                                        Type = FieldTypes.Note,
                                        Name = "Note",
                                        Value = "Ducc is cute\nDucc needs pat",
                                    },
                                }
                            },

                            new Entry()
                            {
                                Name = "Duce Nuce",
                                Icon = new BitmapImage(new Uri("E:\\Downloads\\minecraft-logo-big.png")),
                                Created = DateTime.Now,
                                Modified = DateTime.Now,
                                Accessed = DateTime.Now,

                                FieldList = new MvxObservableCollection<Field>()
                                {
                                    new Field()
                                    {
                                        Type = FieldTypes.Username,
                                        Name = "Username",
                                        Value = "Sauce Fire",
                                    },

                                    new Field()
                                    {
                                        Type = FieldTypes.Password,
                                        Name = "Password",
                                        Value = new byte[] { 1, 1, 0, 1, 1 },
                                    },
                                }
                            },
                        }
                    },
                };
            }
            else
            {
                Vault = Vault.GetInstance();
            }

            // Test export

            XmlSerializer xsSubmit = new XmlSerializer(typeof(Vault));
            var xml = "";

            using (var sww = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(sww))
                {
                    xsSubmit.Serialize(writer, Vault);
                    xml = sww.ToString();
                }
            }

            //File.WriteAllText("E:\\Downloads\\DEBUG.txt", xml);
        }

        #region Commands

        public IMvxAsyncCommand NavigateDatabaseView { get; private set; }
        public IMvxAsyncCommand NavigateEntryView { get; private set; }

        public IMvxAsyncCommand AddEntryCommand { get; private set; }
        public IMvxAsyncCommand EditEntryCommand { get; private set; }
        public IMvxCommand RemoveEntryCommand { get; private set; }

        public IMvxCommand AddFolderCommand { get; private set; }
        public IMvxCommand RemoveFolderCommand { get; private set; }

        #endregion Commands

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
    }
}